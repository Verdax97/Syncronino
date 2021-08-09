using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Keyframe 
{
    public bool active;
    public float timing;
    public float duration = 0;
    public int fade;
    public List<int> values = new List<int>();
}