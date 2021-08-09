using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Text;

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
        if (fileName.text == "")
        {
            Debug.LogWarning("Empty file name!");
            PopUpMessageController.instance.WritePopUp("Empty file name!");    
            return;
        }
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Animations/");
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Animations/Actuators");
        string path = Application.dataPath + "/Animations/" + fileName.text.Replace(' ', '_') + ".txt";
        string path1 = Application.dataPath + "/Animations/Actuators/" + fileName.text.Replace(' ', '_') + "Actuators.json";
        
        File.WriteAllText(path, FormattAnimationString(GetComponent<PlayController>().CreateList()));
        File.WriteAllText(path1, SaveAnimation());
        PopUpMessageController.instance.WritePopUp("Saved file name: " + fileName.text); 
    }
    public string SaveAnimation()
    {
        ActuatorList actuatorList = new ActuatorList();
        foreach (Transform actuator in scroll)
        {
            if (actuator.tag == "Actuator")
                actuatorList.actuators.Add((Actuator)SaveActuator(actuator.GetComponent<SingleElementPlay>()));
            else if (actuator.tag == "Divisor")
                actuatorList.actuators.Add(SaveDivisor(actuator.GetComponent<PlayControllerDivisor>()));
        }
        return JsonUtility.ToJson(actuatorList);
    }
    public string FormattAnimationString(ArrayList lista)
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
    public Actuator SaveActuator(SingleElementPlay singleElementPlay)
    {
        return singleElementPlay.BuildActuator();
    }
    public Actuator SaveDivisor(PlayControllerDivisor playControllerDivisor)
    {
        Actuator divisor = playControllerDivisor.BuildDivisor();
        return divisor;
    }
    public void LoadAnimation()
    {
        string[] lines;
        ActuatorList actuators = new ActuatorList();
        if (fileName.text == "")
        {
            Debug.LogWarning("Empty file name!");
            PopUpMessageController.instance.WritePopUp("Empty file name!");    
            return;
        }

        try
        {
            string path = Application.dataPath + "/Animations/Actuators/" + fileName.text.Replace(' ', '_') + "Actuators.json";
            lines = File.ReadAllLines(path);
            actuators = JsonUtility.FromJson<ActuatorList>(File.ReadAllText(path));
        }
        catch (FileNotFoundException e)
        {
            Debug.LogWarning("File not found!");
            PopUpMessageController.instance.WritePopUp("File not found!\nFile name: " + fileName.text);    
            return;
        }

        if (lines.Length == 0)
        {
            Debug.LogWarning("File is empty!");
            PopUpMessageController.instance.WritePopUp("File is empty!\nFile name: " + fileName.text);    
            return;
        }
        foreach (Actuator actuator in actuators.actuators)
        {
            if (actuator.typeComponent == "Actuator")
            {
                LoadActuator(actuator);
            }
            else if (actuator.typeComponent == "Divisor")
            {
                SectionController.instance.AddNewDivisor().LoadDivisor(actuator);
            }
            
        }
    }

    public void LoadActuator(Actuator actuator)
    {
        //create new actuator
        SectionController.instance.AddNewActuator().LoadActuator(actuator);
    }
}
