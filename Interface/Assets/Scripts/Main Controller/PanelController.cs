using TMPro;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class PanelController : MonoBehaviour
{
    public GameObject actuatorsPanel;
    public GameObject animationsPanel;
    public bool combine = false;

    public void PressSwitchButton()
    {
        combine = !combine;
        actuatorsPanel.SetActive(!combine);
        animationsPanel.SetActive(combine);
        TMP_Dropdown dropdown = animationsPanel.GetComponentInChildren<TMP_Dropdown>();
        dropdown.ClearOptions();
        string[] files = Directory.GetFiles(Application.dataPath + "/Animations/");
        List<string> list = new List<string>();
        foreach (string item in files)
        {
            if(Path.GetExtension(item) != ".meta")
            {
                string s = item.Substring(item.LastIndexOf('/') + 1);
                s = s.Substring(0, s.LastIndexOf(".txt"));
                list.Add(s);
            }
        }
        dropdown.AddOptions(list);
    }
}
