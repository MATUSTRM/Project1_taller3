using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public List<Panel> panels;

    public void OpenPanel(Panel panel)
    {
        CloseAllPanels();
        panel.gameObject.SetActive(true);
    }

    public void ClosePanel(Panel panel)
    {
        panel.gameObject.SetActive(false);
    }

    public void CloseAllPanels()
    {
        foreach (Panel panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
    }

    public void TogglePanel(Panel panel)
    {
        if (panel.gameObject.activeSelf)
            ClosePanel(panel);
        else
            OpenPanel(panel);
    }
}