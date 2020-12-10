using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControllerActuator : PlayController
{
    public override ArrayList CreateList()
    {
        return GetComponent<SingleElementPlay>().BuildAllFade();
    }
}
