using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayControllerActuator : PlayController
{
    //add only all actuator's keyframe
    public override ArrayList CreateList()
    {
        return GetComponent<SingleElementPlay>().BuildAllFade();
    }
}
