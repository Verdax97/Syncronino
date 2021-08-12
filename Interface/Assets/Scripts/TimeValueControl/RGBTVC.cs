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
    public override void LoadValues(Keyframe keyframe)
    {
        timingInput.text = keyframe.timing.ToString();
        R.value = keyframe.values[0];
        B.value = keyframe.values[1];
        G.value = keyframe.values[2];
        SetFade(keyframe.fade);
    }

    public override List<int> GetValues()
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
