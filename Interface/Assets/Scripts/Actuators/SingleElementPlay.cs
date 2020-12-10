using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleElementPlay : MonoBehaviour
{
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
    public TMP_Text campionamentoText;

    public void Coso()
    {
        StartCoroutine(PlayWithFade());
    }

    public IEnumerator PlayWithFade()
    {
        float timing = 0;
        //if one imput is not inserted do nothing
        if (!(pin.text == ""))
        {
            //foreach framekey
            ArrayList lista = BuildAllFade();
            foreach (Lista item in lista)
                {
                    yield return new WaitForSeconds(item.time - timing);
                    ComunicationsController.instance.SendMessageToArduino(item.str);
                    timing = item.time;
                }
        }
        ComunicationsController.instance.SendMessageToArduino("r");
        //Debug.Log(debug);
    }
    
    float Round(float val)
    {
        return (int)(val * 100.0f) / 100.0f;
    }
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
        lista.Add(new Lista(BuildString(TVC.PassString()), Round(float.Parse(TVC.timingInput.text))));
        //exit if the are no next keyframe or there is no fade
        if (index == scrollView.transform.childCount - 1 || TVC.FadeType() == "n")
            return lista;
        //get the next TimeValuesControll
        TimeValuesControll nextTvc = scrollView.transform.GetChild(index+1).GetComponent<TimeValuesControll>();
        //get the starting value
        List<string> startValue = TVC.GetValue();
        //get the ending value
        List<string> endValue = nextTvc.GetValue();
        //get the start time
        float startTime = float.Parse(TVC.timingInput.text);
        //get the end time
        float endTime = float.Parse(nextTvc.timingInput.text);
        //variable to wich assign the delta time
        float deltaTime = 1f / campionamento.value;
        //setting the timing variable
        float timing = startTime;
        //while there
        while (timing < endTime)
        {
            //add the delta time to the timing
            timing += deltaTime;
            //if the timing is higher than the end time break the loop
            if (timing >= endTime)
                break;
            string str = "";
            for (int i = 0; i < startValue.Count; i++)
            {
                //calc the new value
                int newVal = CalcFade(TVC.FadeType(), int.Parse(startValue[i]), int.Parse(endValue[i]), timing - startTime, endTime - startTime);
                str += newVal.ToString() + " ";
            }
            //add to the list
            lista.Add(new Lista(BuildString(str), Round(timing)));
        }
        return lista;
    }
    int CalcFade(string type, int start, int end, float elapsed, float deltaT)
    {
        switch (type)
        {
            case "l"://linear
                return (int)(start + (end - start) * elapsed/deltaT);
            case "e"://exponential
                float k = ExpInterpolation(start, end, deltaT);
                int pos = 1;
                if (end < start)
                    pos = -1;
                int val = (int)(start  + pos*(Mathf.Pow(2, k * elapsed) - 1));
                //Debug.Log(val);
                return val;
            case "g"://logaritmic
                return 2;
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
        ComunicationsController.instance.SendMessageToArduino(BuildString(singleValue.PassString()));
    }

    public string Type()
    {
        string str = "";

        //if you need to add new elements modify the switch statement
        switch (dropdown.captionText.text)
        {
            //for digital pin
            case Constants.DIGITAL_TYPE:
                str += Constants.DIGITAL_TYPE_SHORT;
                break;
            //for analog pin
            case Constants.ANALOG_TYPE:
                str += Constants.ANALOG_TYPE_SHORT;
                break;
            //for servo motors
            case Constants.SERVO_TYPE:
                str += Constants.SERVO_TYPE_SHORT;
                break;
            //for Buzzers
            case Constants.BUZZER_TYPE:
                str += Constants.BUZZER_TYPE_SHORT;
                break;
            //for RGBs
            case Constants.RGB_TYPE:
                str += Constants.RGB_TYPE_SHORT;
                break;
            //should never enter the default branch
            default:
                Debug.LogError("Entered in the default branch because the value in the dropdown is not contemplated in the switch case");
                break;
        }
        return str;
    }

    public string BuildString(string val)
    {
        //set initial string to select
        string str = Constants.SELECT_SHORT;
        str += Type();
        //add pin and value
        str += GetPin();
        return str + " " + val;
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
        AddKeyframe();
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

    public void LoadKeyframe(List<string> values)
    {
        TimeValuesControll tvc = AddKeyframe();
        tvc.SetValue(values);
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
    }

    public virtual void SetPin(List<string> pins)
    {
        pin.text = pins[0];
        if (pins.Count >= 2)
            pin2.text = pins[1];
        if (pins.Count >= 3)
            pin3.text = pins[2];
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
                float timing = float.Parse(TVC.timingInput.text);
                float timing1 = float.Parse(scrollView.transform.GetChild(temp).GetComponent<TimeValuesControll>().timingInput.text);
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

    public void ModifiedCampionamento()
    {
        campionamentoText.text = campionamento.value.ToString();
    }

    public string GetPin()
    {
        string str = pin.text;
        if (pin2.gameObject.activeSelf)
            str += " " + pin2.text;
        if (pin3.gameObject.activeSelf)
            str += " " + pin3.text;
        return str;
    }
}