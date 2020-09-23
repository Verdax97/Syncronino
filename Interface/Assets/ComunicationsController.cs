using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComunicationsController : MonoBehaviour
{

    public Transform scroll;
    public SerialPort arduino;
    public TMP_InputField portName;

    public Transform actuatorParent;
    public GameObject actuator;

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
        arduino = new SerialPort(portName.text, 9600);
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
        //Debug.Log("stringa inviata" + stringa);
    }

    public void PlayALL()
    {
        StartCoroutine(Coso());
    }

    IEnumerator Coso()
    {
        ArrayList lista = CreateList();
        float timing = 0;
        foreach (Lista item in lista)
        {
            yield return new WaitForSeconds(item.time - timing);
            SendMessageToArduino(item.str);
            timing = item.time;
        }
    }

    ArrayList CreateList()
    {
        ArrayList lista = new ArrayList();
        foreach (Transform actuator in scroll)
        {
            int temp = 0;
            foreach (Transform keyframe in actuator.GetComponent<SingleElementPlay>().scrollView.transform.GetChild(0).GetChild(0))
            {
                TimeValuesControll TVC = keyframe.GetComponent<TimeValuesControll>();
                if (TVC.active)
                {
                    float time = float.Parse(TVC.timingInput.text);
                    if (lista.Count == 0)
                    {
                        lista.Add(new Lista(actuator.GetComponent<SingleElementPlay>().BuildString(TVC.PassString()), time));
                    }
                    else
                    {
                        while (temp < lista.Count && time >= ((Lista)lista[temp]).time)
                            temp++;
                        if (temp <= lista.Count)
                            lista.Insert(temp, new Lista(actuator.GetComponent<SingleElementPlay>().BuildString(TVC.PassString()), time));
                        else
                            lista.Add(new Lista(actuator.GetComponent<SingleElementPlay>().BuildString(TVC.PassString()), time));
                    }
                }
            }
        }
        return lista;
    }

    public void AddNewActuator()
    {
        Instantiate(actuator, actuatorParent).GetComponent<SingleElementPlay>().comunicationsController = this;
    }
}

public class Lista
{
    public string str;
    public float time;
    public Lista(string str, float time)
    {
        this.str = str;
        this.time = time;
    }
}
