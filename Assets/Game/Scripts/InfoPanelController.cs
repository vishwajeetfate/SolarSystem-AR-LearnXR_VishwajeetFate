using UnityEngine;

public class InfoPanelController : MonoBehaviour
{
    public void CloseActivePanel()
    {
        if (PlanetInteractor.Active != null)
        {
            PlanetInteractor.Active.CloseInfo();
        }
        else
        {
            Debug.Log("No active panel to close.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseActivePanel();
        }
    }

    public void OnCloseButtonPressed()
    {
        CloseActivePanel();
    }

    // ✅ Toggle play/pause with one button
    public void OnAudioToggleButtonPressed()
    {
        if (PlanetInteractor.Active != null)
        {
            PlanetInteractor.Active.ToggleAudioDescription();
        }
    }
}
