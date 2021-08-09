using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Globalization;

public class SliderTextValue : MonoBehaviour
{
    public TMP_InputField inputField;
    public Slider slider;

    private void Start() {
        ModifiedSlider();
    }
    public void ModifiedSlider()
    {
        inputField.text = slider.value.ToString();
    }
    public void ModifiedInputField()
    {
        if (inputField.text == "")
            inputField.text = "1";
        slider.value = float.Parse(inputField.text, CultureInfo.InvariantCulture);
    }
}
