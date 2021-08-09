using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleValueTVC : TimeValuesControll
{
    public Slider slider;
    public TMP_InputField fadeIntensityInput;

    public override void ModifyMaxValue(int maxValue)
    {
        slider.maxValue = maxValue;
    }

    public override void ModifiedValue()
    {
        // int val = int.Parse(valueInput.text, CultureInfo.InvariantCulture);
        // if (val > slider.maxValue)
        //     val = (int)slider.maxValue;
        // slider.value = val;
    }

    public override string PassString()
    {
        return slider.value.ToString() + " ";
    }

    public override List<string> GetValue()
    {
        List<string> a = new List<string>();
        if(valueInput.text != "")
        {
            a.Add(valueInput.text);
            return a;
        }
        a.Add("0");
        return a;
    }
}
