using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LessonManager : MonoBehaviour
{
    [Header("Lesson Order")]
    public List<PlanetInteractor> lessonPlanets;
    private int currentIndex = 0;

    // Don't auto-start lesson here anymore!
    // void Start() { StartLesson(); }

    public void StartLesson()
    {
        if (lessonPlanets == null || lessonPlanets.Count == 0)
        {
            Debug.LogWarning("❗ No planets assigned in LessonManager.");
            return;
        }

        currentIndex = 0;
        StartCoroutine(lessonPlanets[currentIndex].OpenPlanet());
    }

    public void NextStep()
    {
        if (PlanetInteractor.Active != null)
            PlanetInteractor.Active.CloseInfo();

        currentIndex++;

        if (currentIndex < lessonPlanets.Count)
        {
            StartCoroutine(lessonPlanets[currentIndex].OpenPlanet());
        }
        else
        {
            Debug.Log("🎉 Lesson complete!");
        }
    }
}
