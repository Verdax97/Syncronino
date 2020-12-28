using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RGBTVC : TimeValuesControll
{
    public Slider R;
    public Slider G;
    public Slider B;
    public Image image;
    public TMP_InputField fadeIntensityInput;

    public override void ModifiedValue()
    {
        image.color = new Color(R.value/255, G.value/255, B.value/255);
    }

    public override string PassString()
    {
        return R.value.ToString() + " " + G.value.ToString() + " " + B.value.ToString() + " ";
    }

    public override List<string> GetValue()
    {
        List<string> a = new List<string>
        {
            R.value.ToString(),
            G.value.ToString(),
            B.value.ToString()
        };
        return a;
    }

    public void ModifyIntesifies()
    {
        fadeIntensity = float.Parse(fadeIntensityInput.text);
    }

    public override void SetValue(List<string> values)
    {
        timingInput.text = values[0];
        R.value = int.Parse(values[1]);
        G.value = int.Parse(values[2]);
        B.value = int.Parse(values[3]);
        ModifiedValue();
        SetFade(int.Parse(values[values.Count - 1]));
        ModifiedValue();
    }
}
