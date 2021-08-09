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
    public override void LoadValues(Keyframe keyframe)
    {
        timingInput.text = keyframe.timing.ToString();
        R.value = keyframe.values[0];
        B.value = keyframe.values[1];
        G.value = keyframe.values[2];
        SetFade(keyframe.fade);
    }

    public override List<int> GetCorrectValues()
    {
        List<int> temp = new List<int>();
        temp.Add((int)R.value);
        temp.Add((int)G.value);
        temp.Add((int)B.value);
        return temp;
    }
    public override float GetDuration()
    {
        return 0;
    }
}
