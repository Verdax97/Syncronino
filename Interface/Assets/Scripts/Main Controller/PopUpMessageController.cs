using UnityEngine;
using TMPro;

public class PopUpMessageController : MonoBehaviour
{
    #region Singleton
    public static PopUpMessageController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }
    #endregion
    public GameObject popUpPanel;
    public TMP_Text messageText;

    public void WritePopUp(string msg)
    {
        popUpPanel.SetActive(true);
        messageText.SetText(msg);
    }

    public void ClosePopUp()
    {
        popUpPanel.SetActive(false);
    }
}
