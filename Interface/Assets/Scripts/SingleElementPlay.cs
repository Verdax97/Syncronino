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

    //Play() needed to be IEnumerator in order to do the yeld
    public IEnumerator Play()
    {
        float timing = 0;
        //if one imput is not inserted do nothing
        if (!(pin.text == ""))
        {
            //foreach framekey
            foreach (Transform keyframe in scrollView.transform)
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
            }
        }
        comunicationsController.SendMessageToArduino("r");
    }


    public IEnumerator PlayWithFade()
    {
        float timing = 0;
        //if one imput is not inserted do nothing
        if (!(pin.text == ""))
        {
            //foreach framekey
            foreach (Transform keyframe in scrollView.transform)
            {
                TimeValuesControll TVC = keyframe.GetComponent<TimeValuesControll>();
                int index = keyframe.GetSiblingIndex();

                //if keyframe is active
                if (TVC.active)
                {
                    ArrayList lista = BuildFade(index);
                    foreach (Lista item in lista)
                    {
                        yield return new WaitForSeconds(item.time - timing);
                        comunicationsController.SendMessageToArduino(item.str);
                        timing = item.time;
                    }
                }
            }
        }
        comunicationsController.SendMessageToArduino("r");
        //Debug.Log(debug);
    }
    
    float Round(float val)
    {
        return (int)(val * 100.0f) / 100.0f;
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
                int newVal = CalcFade(TVC.FadeType(), int.Parse(startValue[i]), int.Parse(endValue[i]), timing - startTime, endTime - startTime, TVC.fadeIntensity);
                str += newVal.ToString() + " ";
            }
            //add to the list
            lista.Add(new Lista(BuildString(str), Round(timing)));
        }
        return lista;
    }
    int CalcFade(string type, int start, int end, float elapsed, float deltaT, float i)
    {
        switch (type)
        {
            case "l"://linear
                return (int)(start + (end - start) * elapsed/deltaT);
            case "e"://exponential
                float k = ExpInterpolation(start, end, deltaT, i);
                int pos = 1;
                if (end < start)
                    pos = -1;
                int val = (int)(start  + pos*(Mathf.Pow(i, k * elapsed) - 1));
                //Debug.Log(val);
                return val;
            case "g"://logaritmic
                return 2;
            default://none
                return 0;
        }
    }

    float ExpInterpolation(int start, int end, float deltaT, float i)
    {
        float a = Mathf.Abs(end - start);
        if (i == 0)
            i = 2;
        float l = Mathf.Log(a + 1, i);
        float val =  l/ deltaT;
        return val;
    }

    public void PlaySingle()
    {
        //send message thru script
        comunicationsController.SendMessageToArduino(BuildString(singleValue.PassString()));
    }

    public string Type()
    {
        string str = "";

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
            //for RGBs
            case "RGB":
                str += "l";
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
        string str = "s";
        str += Type();
        //add pin and value
        str += pin.text;
        if (pin2.gameObject.activeSelf)
            str += " " + pin2.text;
        if (pin3.gameObject.activeSelf)
            str += " " + pin3.text;
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
        if (dropdown.captionText.text == "Buzzer")
        {
            toInstantiate = toneKeyframe;
        }
        else if (dropdown.captionText.text == "RGB")
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

    public void LoadKeyframe(string[] values)
    {
        TimeValuesControll tvc = AddKeyframe();
        if (values.Length == 3)
        {
            //is a buzzer
            tvc.SetValue(values);
        }
        else
            tvc.SetValue(values);
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
        if (dropdown.captionText.text == "Buzzer")
        {
            singleValue = Instantiate(singleTonePrefab, singleValueParent.transform).GetComponent<TimeValuesControll>();
        }
        else if (dropdown.captionText.text == "RGB")
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

    public virtual void SetPin(string[] pins)
    {
        pin.text = pins[0];
    }

    public void SetType(string type)
    {
        switch (type)
        {
            //for digital pin
            case "d":
                dropdown.value = 0;
                dropdown.captionText.text= "Digital";
                break;
            //for analog pin
            case "a":
                dropdown.value = 1;
                dropdown.captionText.text = "Analog";
                break;
            //for servo motors
            case "s":
                dropdown.value = 2;
                dropdown.captionText.text = "Servo";
                break;
            //for Buzzers
            case "b":
                dropdown.value = 3;
                dropdown.captionText.text = "Buzzer";
                break;
            //for RGBs
            case "l":
                dropdown.value = 4;
                dropdown.captionText.text = "RGB";
                break;
            //should never enter the default branch
            default:
                Debug.LogError("Entered in the default branch because the value in the dropdown is not contemplated in the switch case");
                break;
        }
        ChangeType();
    }

    public void DeleteActuator()
    {
        Destroy(gameObject);
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
}