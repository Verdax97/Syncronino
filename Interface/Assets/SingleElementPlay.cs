using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleElementPlay : MonoBehaviour
{
    //reference to the comunication controller script
    public ComunicationsController comunicationsController;

    public TMP_Dropdown dropdown;
    public GameObject scrollView;
    public TMP_InputField input;
    public TMP_InputField maxValue;

    public GameObject singleValueParent;
    public GameObject singleDefaultPrefab;
    public GameObject singleTonePrefab;

    public TimeValuesControll singleValue;

    public GameObject defaultKeyframe;
    public GameObject toneKeyframe;

    private void Start()
    {
        ChangeType();
    }
    public void Coso()
    {
        StartCoroutine(Play());
    }

    //Play() needed to be IEnumerator in order to do the yeld
    public IEnumerator Play()
    {
        float timing = 0;
        //if one imput is not inserted do nothing
        if (!(input.text == ""))
        {
            //foreach framekey
            foreach (Transform keyframe in scrollView.transform.GetChild(0).GetChild(0))
            {
                TimeValuesControll TVC = keyframe.GetComponent<TimeValuesControll>();
                //if keyframe is active
                if (TVC.active)
                {
                    //build the string
                    string str = BuildString(TVC.PassString());
                    //wait to reach the new keyframe
                    yield return new WaitForSeconds(float.Parse(TVC.timingInput.text) - timing);
                    //update the timing variable
                    timing = float.Parse(TVC.timingInput.text);
                    //send message thru script
                    comunicationsController.SendMessageToArduino(str);
                }
                //wait to reach the new keyframe
                //yield return new WaitForSeconds(float.Parse(framePause.text));
            }
        }
    }

    public void PlaySingle()
    {
        //send message thru script
        comunicationsController.SendMessageToArduino(BuildString(singleValue.PassString()));
    }

    public string BuildString(string val)
    {
        //set initial string to select
        string str = "s";
        //if you need to add new elements modify the switch statement
        switch (dropdown.captionText.text)
        {
            //for digital pin
            case "Digital":
                str += "d";
                break;
            //for analog pin
            case "Analog":
                str += "a";
                break;
            //for servo motors
            case "Servo":
                str += "s";
                break;
            //for Buzzers
            case "Buzzer":
                str += "b";
                break;
            //should never enter the default branch
            default:
                Debug.LogError("Entered in the default branch because the value in the dropdown is not contemplated in the switch case");
                break;
        }
        //add pin and value
        str += input.text + " " + val;
        return str;
    }

    //called when modified the max value input
    public void ChangeMaxValue()
    {
        if (maxValue.text == "")
            return;
        //parse the text as int
        int max = int.Parse(maxValue.text);
        //search in each keyframe the value to modify
        foreach (Transform keyframe in scrollView.transform.GetChild(0).GetChild(0))
        {
            //modify the max value for the slider
            keyframe.GetChild(0).GetComponent<TimeValuesControll>().ModifyMaxValue(max);
        }
        singleValue.ModifyMaxValue(max);
    }

    /*
    //called when modify the time interval input
    public void ChangeTimeInterval()
    {
        //parse the text as float
        float deltaTime = float.Parse(framePause.text);
        //initialize the time variable
        float time = 0;
        //search in each keyframe the text to modify
        foreach (Transform keyframe in scrollView.transform.GetChild(0).GetChild(0))
        {
            //modify the text in the time text of the keyframe
            keyframe.GetChild(3).GetComponent<TMP_Text>().text = time.ToString();
            //update the time value to write 
            time += deltaTime;
        }
    }*/

    public void AddKeyframe()
    {
        GameObject toInstantiate;
        if (dropdown.captionText.text == "Buzzer")
        {
            toInstantiate = toneKeyframe;
        }
        else
        {
            toInstantiate = defaultKeyframe;
        }
        Transform baseParent = scrollView.transform.GetChild(0).GetChild(0);
        Instantiate(toInstantiate, baseParent);
    }

    public void ChangeType()
    {
        foreach (Transform child in scrollView.transform.GetChild(0).GetChild(0))
        {
            Destroy(child.gameObject);
        }
        if (singleValue != null)
            Destroy(singleValue.gameObject);
        if (dropdown.captionText.text == "Buzzer")
        {
            singleValue = Instantiate(singleTonePrefab, singleValueParent.transform).GetComponent<TimeValuesControll>();
        }
        else
        {
            singleValue = Instantiate(singleDefaultPrefab, singleValueParent.transform).GetComponent<TimeValuesControll>();
        }
        singleValue.comunications = this;
    }
}
