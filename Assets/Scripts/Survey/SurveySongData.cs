using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SurveySongData
{
    public string songName;
    public float totalPoints;
    public Dictionary<string, float> scrubResults;
}
