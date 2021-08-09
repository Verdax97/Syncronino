using System.IO.Ports;
using UnityEngine;
using TMPro;

public class ComunicationsController : MonoBehaviour
{
    #region Singleton
    public static ComunicationsController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }
    #endregion

    public SerialPort arduino;
    public TMP_Dropdown portDropdown;
    public bool debug;
    //used to connect the selected port
    public void ConnectToPort()
    {
        string port;
        //if the port is still open close it
        if (arduino!= null && arduino.IsOpen)
            arduino.Close();
        if (portDropdown.options[portDropdown.value].text == "")
        {
            Debug.LogError("Select a valid Port");
            PopUpMessageController.instance.WritePopUp("Select a valid Port");
            return;
        }
        port = portDropdown.options[portDropdown.value].text;
        //instantiate the port
        arduino = new SerialPort(port, 19200);
        //open the port
        arduino.Open();
        Debug.Log("connected to port " + port);
            PopUpMessageController.instance.WritePopUp("connected to port " + port);        
    }

    public void SendMessageToArduino(string stringa)
    {
        //if port is not open do nothing
        if (arduino == null || !arduino.IsOpen)
        {
            Debug.LogWarning("Open the port");
            PopUpMessageController.instance.WritePopUp("Open the port or select one");    
            return;
        }
        //write the message passed
        arduino.Write(stringa);
        if (debug)
            Debug.Log("stringa inviata: " + stringa);
    }
}
