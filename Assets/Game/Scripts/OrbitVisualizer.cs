using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitVisualizer : MonoBehaviour
{
    public Transform orbitCenter;    // Typically the Sun
    public float orbitRadius = 1f;   // Must match the planet's orbit radius
    public int segments = 100;       // More segments = smoother circle

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = true;
        lineRenderer.positionCount = segments;

        DrawOrbit();
    }

    void DrawOrbit()
    {
        float angle = 0f;
        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * orbitRadius;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * orbitRadius;
            Vector3 point = orbitCenter.position + new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, point);
            angle += 360f / segments;
        }
    }
}
