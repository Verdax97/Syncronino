using UnityEngine;
public class HelpScript : MonoBehaviour
{
    public void Clicked(string msg)
    {
        PopUpMessageController.instance.WritePopUp(msg);
    }
}
