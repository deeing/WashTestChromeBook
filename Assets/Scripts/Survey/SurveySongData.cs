using System.Collections.Generic;
using System;
using System.Text;

[Serializable]
public class SurveySongData
{
    public string songName;
    public float totalPoints;
    public Dictionary<string, float> scrubResults;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("SongName:" + songName);
        sb.AppendLine("Total: " + totalPoints);
        sb.AppendLine("Scrub results: ");
        foreach (KeyValuePair<string, float> scrubPair in scrubResults)
        {
            sb.AppendLine(scrubPair.Key + ": " + scrubPair.Value);
        }

        return sb.ToString();
    }
}
