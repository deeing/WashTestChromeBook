using System.Collections.Generic;
using System;
using System.Text;

[Serializable]
public class SurveyData 
{
    public string gender;
    public string age;
    public string timeStarted;
    public string timeTaken;
    public string buildNumber;
    public Dictionary<string, float> scrubResults;
    public string deviceId;
    public string songName;
    public string totalPoints;
}
