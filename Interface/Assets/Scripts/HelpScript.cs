using UnityEngine;
public class HelpScript : MonoBehaviour
{
    public GameObject panel;
    public void Clicked()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
