using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlanetInteractor : MonoBehaviour
{
    public static PlanetInteractor Active { get; private set; }

    [Header("Planet Info")]
    public string planetName;
    [TextArea] public string planetDescription;

    [Header("UI References")]
    public GameObject infoPanelUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image planetUIImage;
    public Sprite planetSprite;
    public GameObject raycastBlockerPanel;

    [Header("Camera & Motion")]
    public Transform cameraTransform; // manually assign in inspector for mobile!
    public PlanetMotion planetMotion;
    public Transform planetModel;

    [Header("Audio Description")]
    public AudioClip audioClip;
    private AudioSource audioSource;

    private Vector3 originalCamPos;
    private Quaternion originalCamRot;
    private bool isZoomed = false;
    private bool isMoving = false;

    void Start()
    {
        if (cameraTransform == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
                cameraTransform = cam.transform;
            else
                Debug.LogError("❗ cameraTransform is not assigned and Camera.main is null.");
        }

        originalCamPos = cameraTransform.position;
        originalCamRot = cameraTransform.rotation;

        infoPanelUI?.SetActive(false);
        raycastBlockerPanel?.SetActive(false);

        if (planetMotion == null)
            planetMotion = GetComponent<PlanetMotion>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.playOnAwake = false;
    }

    void OnMouseDown()
    {
        if (InteractionManager.IsFrozen || isZoomed || isMoving || infoPanelUI == null)
            return;

        StartCoroutine(OpenPlanet());
    }

    public IEnumerator OpenPlanet()
    {
        yield return StartCoroutine(SmoothZoomToPlanet());
        ShowInfoPanel();
    }

    void ShowInfoPanel()
    {
        if (infoPanelUI == null) return;

        infoPanelUI.SetActive(true);
        raycastBlockerPanel?.SetActive(true);

        nameText.text = planetName;
        descriptionText.text = planetDescription;

        if (planetUIImage != null && planetSprite != null)
            planetUIImage.sprite = planetSprite;

        planetMotion?.PauseMotion();
        InteractionManager.Freeze();
        Active = this;
    }

    public void CloseInfo()
    {
        if (!isZoomed || isMoving) return;

        infoPanelUI?.SetActive(false);
        raycastBlockerPanel?.SetActive(false);

        StopAudioDescription();
        planetMotion?.ResumeMotion();

        StartCoroutine(SmoothZoomOut());

        isZoomed = false;
        InteractionManager.Unfreeze();
        if (Active == this) Active = null;
    }

    IEnumerator SmoothZoomToPlanet()
    {
        isMoving = true;

        float zoomDistance = 0.4f;
        float sideOffsetAmount = 0.9f;

        Vector3 toPlanet = (transform.position - cameraTransform.position).normalized;
        Vector3 forward = cameraTransform.forward.normalized;
        Vector3 left = -cameraTransform.right.normalized;

        Vector3 targetPos = transform.position - forward * zoomDistance + left * sideOffsetAmount;
        Quaternion targetRot = Quaternion.LookRotation(transform.position - targetPos);

        yield return StartCoroutine(MoveCamera(cameraTransform.position, cameraTransform.rotation, targetPos, targetRot));

        isZoomed = true;
        isMoving = false;
    }

    IEnumerator SmoothZoomOut()
    {
        isMoving = true;
        yield return StartCoroutine(MoveCamera(cameraTransform.position, cameraTransform.rotation, originalCamPos, originalCamRot));
        isMoving = false;
    }

    IEnumerator MoveCamera(Vector3 fromPos, Quaternion fromRot, Vector3 toPos, Quaternion toRot)
    {
        float duration = 0.6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            cameraTransform.position = Vector3.Lerp(fromPos, toPos, t);
            cameraTransform.rotation = Quaternion.Slerp(fromRot, toRot, t);
            yield return null;
        }

        cameraTransform.position = toPos;
        cameraTransform.rotation = toRot;
    }

    public void ToggleAudioDescription()
    {
        if (audioSource == null || audioClip == null) return;

        if (audioSource.isPlaying)
            audioSource.Pause();
        else
            audioSource.Play();
    }

    public void StopAudioDescription()
    {
        if (audioSource != null)
            audioSource.Stop();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (planetModel != null && InteractionManager.IsFrozen && Input.GetMouseButton(0))
        {
            float rotationSpeed = 0.5f;
            float deltaX = Input.GetAxis("Mouse X");
            planetModel.Rotate(Vector3.up, -deltaX * rotationSpeed, Space.World);
        }
#else
        if (planetModel != null && InteractionManager.IsFrozen && Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float rotationSpeed = 0.5f;
                planetModel.Rotate(Vector3.up, -touch.deltaPosition.x * rotationSpeed, Space.World);
            }
        }
#endif
    }
}
