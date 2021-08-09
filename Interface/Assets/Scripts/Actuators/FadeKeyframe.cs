using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class FadeKeyframe : Keyframe
{
    public string type;
    public List<int> pins = new List<int>();
    public FadeKeyframe(Keyframe keyframe)
    {
        this.timing = keyframe.timing;
        this.duration = keyframe.duration;
        this.fade = keyframe.fade;
        this.values = new List<int>(keyframe.values);
    }
}
[Serializable]
public class FadeKeyframeList
{
    public List<FadeKeyframe> keyframes = new List<FadeKeyframe>();
}