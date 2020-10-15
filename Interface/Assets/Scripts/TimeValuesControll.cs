using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeValuesControll : MonoBehaviour
{
    public Button button;
    public TMP_InputField timingInput;
    public TMP_InputField valueInput;

    public GameObject fadePanel;
    public TMP_Dropdown fadeDropdown;

    public SingleElementPlay comunications;

    public bool active = false;
    public bool single = false;

    public float fadeIntensity = 5;
    public void ModifiedTiming()
    {
        comunications.ControlTiming();
    }

    //update the slider value
    public virtual void ModifiedValue()
    {
    }

    public virtual void SetValue(string[] values)
    {
        timingInput.text = values[0];
        valueInput.text = values[1];
        ModifiedValue();
    }

    public void ButtonPress()
    {
        if (!single)
        {
            ToggleButton();
            return;
        }
        comunications.PlaySingle();
    }

    //toggle the button color and active
    private void ToggleButton()
    {
        //invert bool
        active = !active;

        if (active)
        {
            //change color to green
            ColorBlock colors = button.colors;
            colors.normalColor = Color.green;
            colors.selectedColor = Color.green;
            button.colors = colors;
        }
        else
        {
            //change color to red
            ColorBlock colors = button.colors;
            colors.selectedColor = Color.red;
            colors.normalColor = Color.red;
            button.colors = colors;
        }
    }

    public virtual string PassString()
    {
        return " ";
    }

    public virtual void ModifyMaxValue(int maxValue)
    {
    }

    public virtual List<string> GetValue()
    {
        return null;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public void ShowFade()
    {
        fadePanel.SetActive(!fadePanel.activeSelf);
    }

    public string FadeType()
    {
        switch (fadeDropdown.value)
        {
            case 0://none
                return "n";
            case 1://linear
                return "l";
            case 2://exponential
                return "e";
            case 3://logaritmic
                return "g";
            default://none
                return "n";
        }
    }
}

public class Tones
{
    public ArrayList tones = new ArrayList();
    public Tones()
    {
        tones.Add(new Tone("NOTE_B0", 31));
        tones.Add(new Tone("NOTE_C1", 33));
        tones.Add(new Tone("NOTE_CS1", 35));
        tones.Add(new Tone("NOTE_D1", 37));
        tones.Add(new Tone("NOTE_DS1", 39));
        tones.Add(new Tone("NOTE_E1", 41));
        tones.Add(new Tone("NOTE_F1", 44));
        tones.Add(new Tone("NOTE_FS1", 46));
        tones.Add(new Tone("NOTE_G1", 49));
        tones.Add(new Tone("NOTE_GS1", 52));
        tones.Add(new Tone("NOTE_A1", 55));
        tones.Add(new Tone("NOTE_AS1", 58));
        tones.Add(new Tone("NOTE_B1", 62));
        tones.Add(new Tone("NOTE_C2", 65));
        tones.Add(new Tone("NOTE_CS2", 69));
        tones.Add(new Tone("NOTE_D2", 73));
        tones.Add(new Tone("NOTE_DS2", 78));
        tones.Add(new Tone("NOTE_E2", 82));
        tones.Add(new Tone("NOTE_F2", 87));
        tones.Add(new Tone("NOTE_FS2", 93));
        tones.Add(new Tone("NOTE_G2", 98));
        tones.Add(new Tone("NOTE_GS2", 104));
        tones.Add(new Tone("NOTE_A2", 110));
        tones.Add(new Tone("NOTE_AS2", 117));
        tones.Add(new Tone("NOTE_B2", 123));
        tones.Add(new Tone("NOTE_C3", 131));
        tones.Add(new Tone("NOTE_CS3", 139));
        tones.Add(new Tone("NOTE_D3", 147));
        tones.Add(new Tone("NOTE_DS3", 156));
        tones.Add(new Tone("NOTE_E3", 165));
        tones.Add(new Tone("NOTE_F3", 175));
        tones.Add(new Tone("NOTE_FS3", 185));
        tones.Add(new Tone("NOTE_G3", 196));
        tones.Add(new Tone("NOTE_GS3", 208));
        tones.Add(new Tone("NOTE_A3", 220));
        tones.Add(new Tone("NOTE_AS3", 233));
        tones.Add(new Tone("NOTE_B3", 247));
        tones.Add(new Tone("NOTE_C4", 262));
        tones.Add(new Tone("NOTE_CS4", 277));
        tones.Add(new Tone("NOTE_D4", 294));
        tones.Add(new Tone("NOTE_DS4", 311));
        tones.Add(new Tone("NOTE_E4", 330));
        tones.Add(new Tone("NOTE_F4", 349));
        tones.Add(new Tone("NOTE_FS4", 370));
        tones.Add(new Tone("NOTE_G4", 392));
        tones.Add(new Tone("NOTE_GS4", 415));
        tones.Add(new Tone("NOTE_A4", 440));
        tones.Add(new Tone("NOTE_AS4", 466));
        tones.Add(new Tone("NOTE_B4", 494));
        tones.Add(new Tone("NOTE_C5", 523));
        tones.Add(new Tone("NOTE_CS5", 554));
        tones.Add(new Tone("NOTE_D5", 587));
        tones.Add(new Tone("NOTE_DS5", 622));
        tones.Add(new Tone("NOTE_E5", 659));
        tones.Add(new Tone("NOTE_F5", 698));
        tones.Add(new Tone("NOTE_FS5", 740));
        tones.Add(new Tone("NOTE_G5", 784));
        tones.Add(new Tone("NOTE_GS5", 831));
        tones.Add(new Tone("NOTE_A5", 880));
        tones.Add(new Tone("NOTE_AS5", 932));
        tones.Add(new Tone("NOTE_B5", 988));
        tones.Add(new Tone("NOTE_C6", 1047));
        tones.Add(new Tone("NOTE_CS6", 1109));
        tones.Add(new Tone("NOTE_D6", 1175));
        tones.Add(new Tone("NOTE_DS6", 1245));
        tones.Add(new Tone("NOTE_E6", 1319));
        tones.Add(new Tone("NOTE_F6", 1397));
        tones.Add(new Tone("NOTE_FS6", 1480));
        tones.Add(new Tone("NOTE_G6", 1568));
        tones.Add(new Tone("NOTE_GS6", 1661));
        tones.Add(new Tone("NOTE_A6", 1760));
        tones.Add(new Tone("NOTE_AS6", 1865));
        tones.Add(new Tone("NOTE_B6", 1976));
        tones.Add(new Tone("NOTE_C7", 2093));
        tones.Add(new Tone("NOTE_CS7", 2217));
        tones.Add(new Tone("NOTE_D7", 2349));
        tones.Add(new Tone("NOTE_DS7", 2489));
        tones.Add(new Tone("NOTE_E7", 2637));
        tones.Add(new Tone("NOTE_F7", 2794));
        tones.Add(new Tone("NOTE_FS7", 2960));
        tones.Add(new Tone("NOTE_G7", 3136));
        tones.Add(new Tone("NOTE_GS7", 3322));
        tones.Add(new Tone("NOTE_A7", 3520));
        tones.Add(new Tone("NOTE_AS7", 3729));
        tones.Add(new Tone("NOTE_B7", 3951));
        tones.Add(new Tone("NOTE_C8", 4186));
        tones.Add(new Tone("NOTE_CS8", 4435));
        tones.Add(new Tone("NOTE_D8", 4699));
        tones.Add(new Tone("NOTE_DS8", 4978));
    }
}

public class Tone
{
    public string name;
    public int value;
    public Tone(string name, int value)
    {
        this.name = name;
        this.value = value;
    }
}