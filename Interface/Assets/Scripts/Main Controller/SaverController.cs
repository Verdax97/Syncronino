using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
public class SaverController : MonoBehaviour
{
    #region Singleton
    public static SaverController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }
    #endregion
    public Transform scroll;
    public TMP_InputField fileName;

    public void Save()
    {
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Animations/");
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Animations/Actuators");
        string path = Application.dataPath + "/Animations/" + fileName.text.Replace(' ', '_') + ".txt";
        string path1 = Application.dataPath + "/Animations/Actuators/" + fileName.text.Replace(' ', '_') + "Actuators.txt";
        File.WriteAllText(path, SaveAnimationString(GetComponent<PlayController>().CreateList()));
        File.WriteAllText(path1, SaveAnimation());
    }

    public string SaveAnimation()
    {
        string str = "";
        foreach (Transform actuator in scroll)
        {
            if (actuator.tag == "Actuator")
                str += SaveActuator(actuator.GetComponent<SingleElementPlay>());
            else if (actuator.tag == "Divisor")
                str += SaveDivisor(actuator.GetComponent<PlayControllerDivisor>());
            str += "\n";
        }
        return str;
    }
    public string SaveAnimationString(ArrayList lista)
    {
        string stringa = "";
        float timing = -1;
        foreach (Lista item in lista)
        {
            if (timing < item.time)
            {
                stringa += Constants.TIME_SHORT + item.time.ToString() + " ";
                timing = item.time;
            }    
            stringa += item.str;
        }
        return stringa;
    }

    public string SaveActuator(SingleElementPlay singleElementPlay)
    {
        string str = "Actuator " + singleElementPlay.actuatorName.text.Replace(' ', '_') + " ";
        string type = singleElementPlay.Type();
        str += type + " ";
        str += singleElementPlay.GetPin() + " ";
        if (type != Constants.BUZZER_TYPE_SHORT)
            str += singleElementPlay.maxValue.text + " ";
        foreach (Transform keyframe in singleElementPlay.scrollView.transform)
        {
            TimeValuesControll TVC = keyframe.GetComponent<TimeValuesControll>();
            str += TVC.timingInput.text + " ";
            foreach(string s in TVC.GetValue())
            {
                str += s + " ";
            }
            str += TVC.FadeTypeValue().ToString() + " ";
        }
        return str;
    }

    public string SaveDivisor(PlayControllerDivisor playControllerDivisor)
    {
        string str = "Divisor " + playControllerDivisor.DivisorName.text.Replace(' ', '_');
        return str;
    }

    
    public void LoadAnimation()
    {
        if (fileName.text == "")
            return;

        string path = Application.dataPath + "/Animations/Actuators/" + fileName.text.Replace(' ', '_') + "Actuators.txt";
        string[] lines = File.ReadAllLines(path);
        if (lines.Length == 0)
        {
            Debug.LogWarning("File not found!");
            return;
        }
        foreach (string line in lines)
        {
            if (line.Split(' ')[0] == "Actuator")
            {
                LoadActuator(line);
            }
            else if (line.Split(' ')[0] == "Divisor")
            {
                PlayControllerDivisor divisor = SectionController.instance.AddNewDivisor();
                divisor.DivisorName.text = line.Split(' ')[1];
            }
            
        }
    }

    public void LoadActuator(string line)
    {
        int offset = 2;
        //create new actuator
        SingleElementPlay actuator = SectionController.instance.AddNewActuator();
        //set the name
        actuator.actuatorName.text = line.Split(' ')[1];
        //split the line
        string[] lineSplitted = line.Split(' ');
        //set the actuator type
        string type = lineSplitted[0 + offset];
        actuator.SetType(type);

        Actuator act = ChoseActuator(type);

        int i = 0;
        List<string> temp = new List<string>();
        //set the actuator pin
        for (i = 1; i <= act.nPin; i++)
        {
            temp.Add(lineSplitted[i + offset]);
        }
        actuator.SetPin(temp);
        i += offset;
        for (int j = 0; j < act.maxValue; j++)
        {
            actuator.maxValue.text = lineSplitted[i++];
            actuator.ChangeMaxValue();
        }
        //for adding keyframes
        for (; i < lineSplitted.Length-1; )
        {
            List<string> list = new List<string>();
            list.Add(lineSplitted[i]);
            i++;
            for (int j = 0; j < act.nValues; j++)
            {
                list.Add(lineSplitted[i]);
                i++;
            }
            for (int j = 0; j < act.fade; j++)
            {
                list.Add(lineSplitted[i]);
                i++;
            }
            //need to add for fade
            actuator.LoadKeyframe(list);
        }
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
}
