using System.Collections;
using UnityEngine;
using TMPro;
public class PlayControllerDivisor : PlayController
{
    public TMP_InputField divisorName;
    public override ArrayList CreateList()
    {
        scroll = transform.parent;
        ArrayList list = new ArrayList();
        for (int i = transform.GetSiblingIndex() + 1; i < scroll.childCount; i++)
        {
            Transform actuator = scroll.GetChild(i);
            if (actuator.tag == "Actuator")
            {
                ArrayList lista = actuator.GetComponent<SingleElementPlay>().BuildAllFade();
                list = AddInOrder(list, lista);
            }
            else if(actuator.tag == "Divisor")
                return list;
        }
        return list;
    }
    public Actuator BuildDivisor()
    {
        Actuator divisor = new Actuator();
        divisor.name = divisorName.text;
        divisor.typeComponent = Constants.Divisor;
        return divisor;
    }
    public void LoadDivisor(Actuator divisor)
    {
        divisorName.text = divisor.name;
    }
}