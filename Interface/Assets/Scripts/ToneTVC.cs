using System.Collections.Generic;
using TMPro;

public class ToneTVC : TimeValuesControll
{
    Tones tones = null;

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
        tones = new Tones();
        foreach (Tone tone in tones.tones)
            notesDropdown.AddOptions(new List<string>() { tone.name });
    }

    public override string PassString()
    {
        int index = notesDropdown.value;
        int temp = int.Parse(valueInput.text);
        return temp.ToString() + " " + ((Tone)tones.tones[index]).value.ToString() + " ";
    }

    public override string GetValue()
    {
        return valueInput.text + " " + notesDropdown.captionText.text;
    }

    public override void SetValue(string[] values)
    {
        base.SetValue(values);
        LoadTones();
        for (int i = 0; i < notesDropdown.options.Count; i++)
            if (values[2] == notesDropdown.options[i].text)
            {
                notesDropdown.value = i;
                notesDropdown.captionText.text = values[2];
                break;
            }
        setup = true;
    }
}
