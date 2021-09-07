using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
public class SingleElementPlay : MonoBehaviour
{
    public Color digital;
    public Color analog;
    public Color servo;
    public Color buzzer;
    public Color rgb;
    public TMP_Dropdown dropdown;
    public GameObject scrollView;
    public TMP_InputField pin;
    public TMP_InputField pin2;
    public TMP_InputField pin3;
    public TMP_InputField maxValue;
    public TMP_InputField actuatorName;
    public GameObject singleValueParent;
    public GameObject singleDefaultPrefab;
    public GameObject singleTonePrefab;
    public GameObject singleRGBPrefab;
    public TimeValuesControll singleValue;
    public GameObject defaultKeyframe;
    public GameObject toneKeyframe;
    public GameObject RGBKeyframe;

    public Slider campionamento;
    //public TMP_Text campionamentoText;
    public ArrayList BuildAllFade()
    {
        if ((pin.text == ""))
            return new ArrayList();
        ArrayList list = new ArrayList();
        //foreach framekey
        foreach (Transform keyframe in scrollView.transform)
        {
            TimeValuesControll TVC = keyframe.GetComponent<TimeValuesControll>();
            int index = keyframe.GetSiblingIndex();
            //if keyframe is active
            if (TVC.active)
            {
                ArrayList lista = BuildFade(index);
                list.AddRange(lista);
            }
        }
        return list;
    }
    public ArrayList BuildFade(int index)
    {
        //create new list
        ArrayList lista = new ArrayList();
        //get the TimeValuesControll
        TimeValuesControll TVC = scrollView.transform.GetChild(index).GetComponent<TimeValuesControll>();
        //add the keyframe value to the list
        FadeKeyframe keyframe = new FadeKeyframe(TVC.BuildKeyframe());
        lista.Add(BuildFadeKeyframe(keyframe));
        //exit if the are no next keyframe or there is no fade
        if (index == scrollView.transform.childCount - 1 || TVC.FadeType() == "n")
            return lista;
        //get the next TimeValuesControll
        TimeValuesControll nextTvc = scrollView.transform.GetChild(index+1).GetComponent<TimeValuesControll>();
        Keyframe nextKeyframe = new FadeKeyframe(nextTvc.BuildKeyframe());
        //variable to wich assign the delta time
        float deltaTime = 1f / campionamento.value;
        //setting the timing variable
        float timing = keyframe.timing;
        //while there
        while (timing < nextKeyframe.timing)
        {
            //add the delta time to the timing
            timing += deltaTime;
            //if the timing is higher than the end time break the loop
            if (timing >= nextKeyframe.timing)
                break;
            FadeKeyframe fadeKeyframe = new FadeKeyframe(keyframe);
            fadeKeyframe.values.Clear();
            //cycle all values
            for (int i = 0; i < keyframe.values.Count; i++)
            {
                //calc the new value
                int newVal = CalcFade(TVC.FadeType(), keyframe.values[i], nextKeyframe.values[i], timing - keyframe.timing, nextKeyframe.timing - keyframe.timing);
                fadeKeyframe.values.Add(newVal);
            }
            fadeKeyframe.timing = (float)Mathf.RoundToInt(timing*100)/100;
            //add to the list
            lista.Add(BuildFadeKeyframe(fadeKeyframe));
        }
        return lista;
    }
    FadeKeyframe BuildFadeKeyframe(FadeKeyframe keyframe)
    {
        keyframe.type = Type()[0];
        //pins
        keyframe.pins.Clear();
        keyframe.pins.Add(int.Parse(pin.text));
        if(pin2.IsActive())
            keyframe.pins.Add(int.Parse(pin2.text));
        if(pin3.IsActive())
            keyframe.pins.Add(int.Parse(pin3.text));
        return keyframe;
    }
    int CalcFade(string type, int start, int end, float elapsed, float deltaT)
    {
        switch (type)
        {
            case "l"://linear
                return Mathf.RoundToInt(start + (end - start) * elapsed/deltaT);
            case "e"://exponential
                float k = ExpInterpolation(start, end, deltaT);
                int pos = 1;
                if (end < start)
                    pos = -1;
                int val = (int)(start  + pos*(Mathf.Pow(2, k * elapsed) - 1));
                //Debug.Log(val);
                return val;
            case "g"://logaritmic
                return (int)(start + (end - start) * elapsed/deltaT);
            default://none
                return 0;
        }
    }
    float ExpInterpolation(int start, int end, float deltaT)
    {
        float a = Mathf.Abs(end - start);
        int i = 2;
        float l = Mathf.Log(a + 1, i);
        float val =  l/ deltaT;
        return val;
    }
    public void PlaySingle()
    {
        //send message thru script
        ComunicationsController.instance.SendMessageToArduino(BuildFadeKeyframe(new FadeKeyframe(singleValue.BuildKeyframe())));
    }
    public string Type()
    {
        string str = "";
        //if you need to add new elements modify the switch statement
        switch (dropdown.captionText.text)
        {
            //for digital pin
            case Constants.DIGITAL_TYPE:
                return Constants.DIGITAL_TYPE_SHORT;
            //for analog pin
            case Constants.ANALOG_TYPE:
                return Constants.ANALOG_TYPE_SHORT;
            //for servo motors
            case Constants.SERVO_TYPE:
                return Constants.SERVO_TYPE_SHORT;
            //for Buzzers
            case Constants.BUZZER_TYPE:
                return Constants.BUZZER_TYPE_SHORT;
            //for RGBs
            case Constants.RGB_TYPE:
                return Constants.RGB_TYPE_SHORT;
            //should never enter the default branch
            default:
                Debug.LogError("Entered in the default branch because the value in the dropdown is not contemplated in the switch case");
                PopUpMessageController.instance.WritePopUp("Entered in the default branch because the value in the dropdown is not contemplated in the switch case");
                break;
        }
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
        foreach (Transform keyframe in scrollView.transform)
        {
            //modify the max value for the slider
            keyframe.GetComponent<TimeValuesControll>().ModifyMaxValue(max);
        }
        singleValue.ModifyMaxValue(max);
    }
    public void ButtonPressAddKeyframe()
    {
        TimeValuesControll tvc = AddKeyframe();
        if(scrollView.transform.childCount == 1)
            return;
        TimeValuesControll precTvc = scrollView.transform.GetChild(scrollView.transform.childCount - 2).GetComponent<TimeValuesControll>();
        float newTiming = precTvc.GetTiming() + 1;
        tvc.timingInput.text = newTiming.ToString();
    }
    public TimeValuesControll AddKeyframe()
    {
        GameObject toInstantiate;
        if (dropdown.captionText.text == Constants.BUZZER_TYPE)
        {
            toInstantiate = toneKeyframe;
        }
        else if (dropdown.captionText.text == Constants.RGB_TYPE)
        {
            toInstantiate = RGBKeyframe;
        }
        else
        {
            toInstantiate = defaultKeyframe;
        }
        Transform baseParent = scrollView.transform;

        TimeValuesControll tvc = Instantiate(toInstantiate, baseParent).GetComponent<TimeValuesControll>();
        tvc.comunications = this;
        //modify the max value for the slider
        if (maxValue.text != "")
            tvc.GetComponent<TimeValuesControll>().ModifyMaxValue(int.Parse(maxValue.text));
        return tvc;

    }
    public void LoadKeyframe(Keyframe keyframe)
    {
        TimeValuesControll tvc = AddKeyframe();
        tvc.LoadValues(keyframe);
        if (keyframe.active)
            tvc.ButtonPress();
    }
    public void ChangeType()
    {
        pin2.gameObject.SetActive(false);
        pin3.gameObject.SetActive(false);

        foreach (Transform child in scrollView.transform)
        {
            Destroy(child.gameObject);
        }

        if (singleValue != null)
            Destroy(singleValue.gameObject);

        if (dropdown.captionText.text == Constants.BUZZER_TYPE)
        {
            singleValue = Instantiate(singleTonePrefab, singleValueParent.transform).GetComponent<TimeValuesControll>();
        }
        else if (dropdown.captionText.text == Constants.RGB_TYPE)
        {
            pin2.gameObject.SetActive(true);
            pin3.gameObject.SetActive(true);
            singleValue = Instantiate(singleRGBPrefab, singleValueParent.transform).GetComponent<TimeValuesControll>();
        }
        else
        {
            singleValue = Instantiate(singleDefaultPrefab, singleValueParent.transform).GetComponent<TimeValuesControll>();
        }
        singleValue.comunications = this;
        if (maxValue.text != "")
            singleValue.ModifyMaxValue(int.Parse(maxValue.text));
        // switch(dropdown.captionText.text)
        // {
        //     case Constants.DIGITAL_TYPE:
        //         GetComponent<Image>().color = digital;
        //         break;
        //     case Constants.ANALOG_TYPE:
        //         GetComponent<Image>().color = analog;
        //         break;
        //     case Constants.SERVO_TYPE:
        //         GetComponent<Image>().color = servo;
        //         break;
        //     case Constants.BUZZER_TYPE:
        //         GetComponent<Image>().color = buzzer;
        //         break;
        //     case Constants.RGB_TYPE:
        //         GetComponent<Image>().color = rgb;
        //         break;
        //     default:
        //         return;
        // }
    }
    public virtual void SetPin(List<int> pins)
    {
        pin.text = pins[0].ToString();
        if (pins.Count >= 2)
            pin2.text = pins[1].ToString();
        if (pins.Count >= 3)
            pin3.text = pins[2].ToString();
    }
    public void SetType(string type)
    {
        switch (type)
        {
            //for digital pin
            case Constants.DIGITAL_TYPE_SHORT:
                dropdown.value = 0;
                dropdown.captionText.text= Constants.DIGITAL_TYPE;
                break;
            //for analog pin
            case Constants.ANALOG_TYPE_SHORT:
                dropdown.value = 1;
                dropdown.captionText.text = Constants.ANALOG_TYPE;
                break;
            //for servo motors
            case Constants.SERVO_TYPE_SHORT:
                dropdown.value = 2;
                dropdown.captionText.text = Constants.SERVO_TYPE;
                break;
            //for Buzzers
            case Constants.BUZZER_TYPE_SHORT:
                dropdown.value = 3;
                dropdown.captionText.text = Constants.BUZZER_TYPE;
                break;
            //for RGBs
            case Constants.RGB_TYPE_SHORT:
                dropdown.value = 4;
                dropdown.captionText.text = Constants.RGB_TYPE;
                break;
            //should never enter the default branch
            default:
                Debug.LogError("Entered in the default branch because the value in the dropdown is not contemplated in the switch case");
                PopUpMessageController.instance.WritePopUp("Entered in the default branch because the value in the dropdown is not contemplated in the switch case");
                break;
        }
        ChangeType();
    }
    public void ControlTiming()
    {
        for (int i = 1; i < scrollView.transform.childCount; i++)
        {
            int temp = i - 1;
            TimeValuesControll TVC = scrollView.transform.GetChild(i).GetComponent<TimeValuesControll>();
            while (temp >= 0)
            {
                float timing = TVC.GetTiming();
                float timing1 = scrollView.transform.GetChild(temp).GetComponent<TimeValuesControll>().GetTiming();
                if (timing < timing1)
                {
                    TVC.transform.SetSiblingIndex(temp);
                    temp--;
                }
                else
                    break;
            }
        }
    }
    //get pins as string separated by spaces
    public Actuator BuildActuator()
    {
        Actuator actuator = new Actuator();
        //name
        actuator.name = actuatorName.text;
        //type
        actuator.typeActuator = Type();
        //pins
        actuator.pins.Clear();
        actuator.pins.Add(int.Parse(pin.text));
        if(pin2.IsActive())
            actuator.pins.Add(int.Parse(pin2.text));
        if(pin3.IsActive())
            actuator.pins.Add(int.Parse(pin3.text));
        //max value
        actuator.maxValue = int.Parse(maxValue.text);
        //rate
        actuator.rate = (int)campionamento.value;
        //keyframes
        actuator.keyframes.Clear();
        foreach (Transform keyframe in scrollView.transform)
        {
            TimeValuesControll TVC = keyframe.GetComponent<TimeValuesControll>();
            actuator.keyframes.Add(TVC.BuildKeyframe());
        }
        return actuator;
    }
    public void LoadActuator(Actuator actuator)
    {
        actuatorName.text = actuator.name;
        SetType(actuator.typeActuator);
        SetPin(actuator.pins);
        maxValue.text = actuator.maxValue.ToString();
        ChangeMaxValue();
        campionamento.value = actuator.rate;
        foreach(Keyframe keyframe in actuator.keyframes)
        {
            LoadKeyframe(keyframe);
        }
    }
}