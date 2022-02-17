using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SurveyData 
{
    public string gender;
    public string age;
    public string timeStarted;
    public List<SurveySongData> songData;
}
