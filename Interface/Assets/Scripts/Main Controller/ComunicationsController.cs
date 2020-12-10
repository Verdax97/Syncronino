using System.Collections;
using System.IO.Ports;
using System.IO;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

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

    public Transform scroll;
    public SerialPort arduino;
    public TMP_InputField portName;

    public TMP_InputField fileName;
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
        Debug.Log("stringa inviata: " + stringa);
    }
    public string SaveAnimationString(ArrayList lista)
    {
        string stringa = "";
        float timing = -1;
        foreach (Lista item in lista)
        {
            if (timing < item.time)
            {
                stringa += "t" + item.time.ToString() + " ";
                timing = item.time;
            }    
            stringa += item.str;
        }
        return stringa;
    }

    public string SaveActuatorString()
    {
        string str = "";
        foreach (Transform actuator in scroll)
        {
            SingleElementPlay sep = actuator.GetComponent<SingleElementPlay>();
            string type = sep.Type();
            str += type + " ";
            str += sep.GetPin() + " ";
            if (type != Constants.BUZZER_TYPE_SHORT)
                str += sep.maxValue.text + " ";
            foreach (Transform keyframe in actuator.GetComponent<SingleElementPlay>().scrollView.transform)
            {
                TimeValuesControll TVC = keyframe.GetComponent<TimeValuesControll>();
                str += TVC.timingInput.text + " ";
                foreach(string s in TVC.GetValue())
                {
                    str += s + " ";
                }
                str += TVC.FadeTypeValue().ToString() + " ";
            }
            str += "\n";
        }
        return str;
    }

    Actuator ChoseActuator(string type)
    {
        switch (type)
        {
            //for Buzzers
            case Constants.BUZZER_TYPE_SHORT:
                return new Buzzer();
            //for RGBs
            case Constants.RGB_TYPE_SHORT:
                return new RGB();
            //should never enter the default branch
            default:
                return new Actuator();
        }
    }

    IEnumerator Coso()
    {
        ArrayList lista = CreateList();
        float timing = 0;
        //Debug.Log(SaveAnimationString(lista));
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
            foreach (Transform keyframe in actuator.GetComponent<SingleElementPlay>().scrollView.transform)
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

    public void PressAddButton()
    {
        AddNewActuator().ChangeType();
    }

    public SingleElementPlay AddNewActuator()
    {
        SingleElementPlay singleElement = Instantiate(actuator, actuatorParent).GetComponent<SingleElementPlay>();
        return singleElement;
    }

    public void SaveAnimation()
    {
        string path = Application.dataPath + "/" + fileName.text + ".txt";
        string path1 = Application.dataPath + "/" + fileName.text + "Actuators.txt";
        File.WriteAllText(path, SaveAnimationString(CreateList()));
        File.WriteAllText(path1, SaveActuatorString());
    }
}
