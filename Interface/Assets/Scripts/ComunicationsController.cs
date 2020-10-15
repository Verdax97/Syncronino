using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComunicationsController : MonoBehaviour
{

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

    public void PlayALL()
    {
        StartCoroutine(Coso());
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
            str += sep.pin.text + " ";
            if (type != "b")
                str += sep.maxValue.text + " ";

            foreach (Transform keyframe in actuator.GetComponent<SingleElementPlay>().scrollView.transform)
            {
                TimeValuesControll TVC = keyframe.GetComponent<TimeValuesControll>();
                str += TVC.timingInput.text + " ";
                str += TVC.GetValue() + " ";
            }
            str += "\n";
        }
        return str;
    }

    IEnumerator Coso()
    {
        ArrayList lista = CreateList();
        float timing = 0;
        Debug.Log(SaveAnimationString(lista));
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
        singleElement.comunicationsController = this;
        return singleElement;
    }

    public void SaveAnimation()
    {
        string path = Application.dataPath + "/" + fileName.text + ".txt";
        string path1 = Application.dataPath + "/" + fileName.text + "Actuators.txt";
        File.WriteAllText(path, SaveAnimationString(CreateList()));
        File.WriteAllText(path1, SaveActuatorString());
    }

    public void LoadAnimation()
    {
        string path = Application.dataPath + "/" + fileName.text + "Actuators.txt";
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            //create new actuator
            SingleElementPlay actuator = AddNewActuator();
            //split the line
            string[] lineSplitted = line.Split(' ');
            //set the actuator type
            string type = lineSplitted[0];
            actuator.SetType(type);
            //set the actuator pin
            actuator.SetPin(new string[] { lineSplitted[1] });

            int i = 2;
            if (type != "b")
            {
                actuator.maxValue.text = lineSplitted[2];
                actuator.ChangeMaxValue();
                i++;
            }
            //for adding keyframes
            for (; i < lineSplitted.Length-1; i+=2)
            {
                if (type == "b")
                {
                    actuator.LoadKeyframe(new string[] { lineSplitted[i], lineSplitted[i+1], lineSplitted[i+2]});
                    i++;
                }
                else
                {
                    actuator.LoadKeyframe(new string[] { lineSplitted[i], lineSplitted[i + 1]});
                }
            }
        }
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
