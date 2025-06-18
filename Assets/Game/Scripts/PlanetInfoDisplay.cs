using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlanetInfoDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameTMP;
    public TextMeshProUGUI descriptionTMP;
    public Button closeButton;
    public Transform target;

    public void SetInfo(string name, string description)
    {
        if (nameTMP) nameTMP.text = name;
        if (descriptionTMP) descriptionTMP.text = description;
    }

    void Update()
    {
        if (target != null)
            transform.position = target.position + new Vector3(0, 0.5f, 0);
    }
}
