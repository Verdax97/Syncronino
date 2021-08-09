using System;
using System.Collections.Generic;
using System.Collections;
[Serializable]
public class Actuator : BaseElement
{
    public Actuator()
    {
        this.typeComponent = Constants.ACTUATOR;
    }
    public string typeActuator = Constants.DIGITAL_TYPE_SHORT;
    public List<int> pins = new List<int>();
    public int maxValue = 1;
    public int rate = 10;
    public List<Keyframe> keyframes = new List<Keyframe>();
}
[Serializable]
public class ActuatorList
{
    public List<Actuator> actuators = new List<Actuator>();
}