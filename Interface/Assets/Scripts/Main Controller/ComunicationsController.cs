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
    public TMP_InputField portName;
    public bool debug;
    //used to connect the selected port
    public void ConnectToPort()
    {
        //if the port is still open close it
        if (arduino!= null && arduino.IsOpen)
            arduino.Close();
        //if the port name is empty do nothing
        if (portName.text == "")
        {
            Debug.LogError("Enter a valid Port");
            return;
        }
        //instantiate the port
        arduino = new SerialPort(portName.text, 19200);
        //open the port
        arduino.Open();
    }

    public void SendMessageToArduino(string stringa)
    {
        //if port is not open do nothing
        if (arduino == null || !arduino.IsOpen)
        {
            Debug.LogWarning("Open the port");
            return;
        }
        //write the message passed
        arduino.Write(stringa);
        if (debug)
            Debug.Log("stringa inviata: " + stringa);
    }
}
