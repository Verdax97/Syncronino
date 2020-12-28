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

    public override List<string> GetValue()
    {
        List<string> a = new List<string>
        {
            valueInput.text + " " + notesDropdown.captionText.text
        };
        return a;
    }

    public override void SetValue(List<string> values)
    {
        timingInput.text = values[0];
        valueInput.text = values[1];
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
