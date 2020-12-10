using System.Collections;
using UnityEngine;

public class PlayController : MonoBehaviour
{
    public Transform scroll;
    public void Play()
    {
        StartCoroutine(Coso(CreateList()));
    }

    public IEnumerator Coso(ArrayList list)
    {
        float timing = 0;
        Debug.Log(SaverController.instance.SaveAnimationString(list));
        foreach (Lista item in list)
        {
            yield return new WaitForSeconds(item.time - timing);
            ComunicationsController.instance.SendMessageToArduino(item.str);
            timing = item.time;
        }
    }

    public virtual ArrayList CreateList()
    {
        ArrayList list = new ArrayList();
        foreach (Transform actuator in scroll)
        {
            if (actuator.tag == "Actuator")
            {
                ArrayList lista = actuator.GetComponent<SingleElementPlay>().BuildAllFade();
                list = AddInOrder(list, lista);
            }
        }
        return list;
    }

    public ArrayList AddInOrder(ArrayList main, ArrayList toAdd)
    {
        int added = 0;
        for (int i = 0; i < main.Count; i++)
        {
            if (added >= toAdd.Count)
                return main;
            if (((Lista)toAdd[added]).time < ((Lista)main[i]).time)
            {
                main.Insert(i, ((Lista)toAdd[added]));
                added++;
            }
        }
        while (added < toAdd.Count)
        {
            main.Add(((Lista)toAdd[added]));
            added++;
        }
        return main;
    }
}
