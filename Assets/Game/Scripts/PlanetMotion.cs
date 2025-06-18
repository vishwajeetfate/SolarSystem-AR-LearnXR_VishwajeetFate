using UnityEngine;

public class PlanetMotion : MonoBehaviour
{
    public Transform orbitCenter;
    public float rotationSpeed = 10f;
    public float orbitSpeed = 5f;
    public float orbitRadius = 1.0f;

    private float currentAngle;
    private bool isPaused = false;

    public void PauseMotion()
    {
        isPaused = true;
        Debug.Log($"{name}: Motion Paused");
    }

    public void ResumeMotion()
    {
        isPaused = false;
        Debug.Log($"{name}: Motion Resumed");
    }

    void Start()
    {
        if (orbitCenter != null)
        {
            Vector3 direction = (transform.position - orbitCenter.position).normalized;
            transform.position = orbitCenter.position + direction * orbitRadius;
        }
    }

    void Update()
    {
        if (isPaused) return;

        // Spin on own axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.Self);

        // Orbit around center
        if (orbitCenter != null)
        {
            currentAngle += orbitSpeed * Time.deltaTime;
            float radians = currentAngle * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Sin(radians), 0, Mathf.Cos(radians)) * orbitRadius;
            transform.position = orbitCenter.position + offset;
        }
    }
}
