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
}
