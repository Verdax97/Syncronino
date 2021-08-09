using System.Collections.Generic;
using System.Globalization;
using TMPro;

public class ToneTVC : TimeValuesControll
{
    Tones tones = new Tones();

    private bool setup = false;

    public TMP_Dropdown notesDropdown;
    private void Start()
    {
        if (setup)
            return;
        LoadTones();
    }

    private void LoadTones()
    {
        foreach (Tone tone in tones.tones)
            notesDropdown.AddOptions(new List<string>() { tone.name });
    }

    public override string PassString()
    {
        int index = notesDropdown.value;
        int temp = int.Parse(valueInput.text, CultureInfo.InvariantCulture);
        return temp.ToString() + " " + ((Tone)tones.tones[index]).value.ToString() + " ";
    }

    public override List<string> GetValue()
    {
        List<string> a = new List<string>();
        if(valueInput.text != "")
        {
            a.Add(valueInput.text + " " + notesDropdown.captionText.text);
            return a;
        }
        a.Add("10 " + notesDropdown.captionText.text);
        return a;
    }

    public override void LoadValues(Keyframe keyframe)
    {
        timingInput.text = keyframe.timing.ToString();
        valueInput.text = keyframe.duration.ToString();
        LoadTones();
        for(int i = 0; i < tones.tones.Count; i++)
        {
            Tone tone = (Tone)tones.tones[i];
            if (tone.value == keyframe.values[0])
            {
                notesDropdown.captionText.text = tone.name;
                notesDropdown.value = i;
                break;
            }
        }
        setup = true;
    }

    public override List<int> GetCorrectValues()
    {
        List<int> temp = new List<int>();
        foreach(Tone tone in tones.tones)
        {
            if(tone.name == notesDropdown.captionText.text)
            {
                temp.Add(tone.value);
                return temp;
            }
        }
        return temp;
    }
    public override float GetDuration()
    {
        return int.Parse(valueInput.text);
    }
}
