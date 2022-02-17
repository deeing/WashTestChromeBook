using System.Collections.Generic;
using System;
using System.Text;

[Serializable]
public class SurveyData 
{
    public string gender;
    public string age;
    public string timeStarted;
    public List<SurveySongData> songData;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Gender:" + gender);
        sb.AppendLine("Age: " + age);
        sb.AppendLine("TimeStarted: " + timeStarted);

        return sb.ToString();
    }
}
