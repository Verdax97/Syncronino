using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SectionController : MonoBehaviour
{
    public GameObject actuator;
    public GameObject divisor;
    public Transform parent;
    #region Singleton
    public static SectionController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }
    #endregion
    public void AddActuator()
    {
        AddNewActuator().ChangeType();
    }

    public SingleElementPlay AddNewActuator()
    {
        SingleElementPlay singleElement = Instantiate(actuator, parent).GetComponent<SingleElementPlay>();
        return singleElement;
    }

    public void AddDivisor()
    {
        AddNewDivisor();
    }

    public PlayControllerDivisor AddNewDivisor()
    {
        return Instantiate(divisor, parent).GetComponent<PlayControllerDivisor>();
    }
}
