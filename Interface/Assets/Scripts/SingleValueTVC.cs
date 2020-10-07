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
        float val = int.Parse(valueInput.text);
        if (val > slider.maxValue)
            val = slider.maxValue;
        slider.value = val;
    }

    public override string PassString()
    {
        return slider.value.ToString() + " ";
    }

    public void ModifiedSlider()
    {
        valueInput.text = slider.value.ToString();
    }

    public override string GetValue()
    {
        return valueInput.text;
    }

    public void ModifyIntesifies()
    {
        fadeIntensity = float.Parse(fadeIntensityInput.text);
    }
}
