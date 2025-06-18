// PlanetSizeManager.cs
using UnityEngine;

public class PlanetSizeManager : MonoBehaviour
{
    [System.Serializable]
    public class PlanetScale
    {
        public string planetName;
        public Transform planetTransform;
        public float visualScale = 1f;
    }

    public PlanetScale[] planets;

    void Start()
    {
        foreach (var p in planets)
        {
            if (p.planetTransform != null)
            {
                p.planetTransform.localScale = Vector3.one * p.visualScale;
                Debug.Log($"Set scale of {p.planetName} to {p.visualScale}");
            }
        }
    }
}
