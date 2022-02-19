using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Wash.Utilities;

public class SurveyManager : SingletonMonoBehaviour<SurveyManager>
{
    public float minAge;
    public float maxAge;

    public SurveyData currentSurveyData { private set; get; }

    private const string surveyKey = "Game Survey Data";
    private const string surveyFile = "gameSurveyData.json";

    protected override void Awake()
    {
        if (!InitializeSingleton(this))
        {
            return;
        }
        currentSurveyData = new SurveyData();
    }

    public void SetBoy()
    {
        currentSurveyData.gender = Gender.Boy.GetDescription();
    }

    public void SetGirl()
    {
        currentSurveyData.gender = Gender.Girl.GetDescription();
    }

    public void SetOther()
    {
        currentSurveyData.gender = Gender.Other.GetDescription();
    }

    public void SetAge(string chosenAge)
    {
        currentSurveyData.age = chosenAge;
    }

    public void SetTimeStarted()
    {
        currentSurveyData.timeStarted = System.DateTime.Now.ToString("HH:mm dd MMMM, yyyy");
    }

    public void AddSongData(SurveySongData songData)
    {
        Debug.Log("Adding song data");
        currentSurveyData.songData = songData.ToString();
    }


    /*public void SaveCurrentSurveyData()
    {
        List<SurveyData> previousSurveyData;

        try
        {
            previousSurveyData = (List<SurveyData>)ES3.Load(surveyKey, surveyFile);
        }
        catch (System.IO.FileNotFoundException e)
        {
            previousSurveyData = null;
        }

        if (previousSurveyData != null)
        {
            Debug.Log("previous survey file detected");
            previousSurveyData.Add(currentSurveyData);
            ES3.Save(surveyKey, previousSurveyData, surveyFile);
        }
        else
        {
            Debug.Log("no previous survey file detected");
            List<SurveyData> newSurveyData = new List<SurveyData>();
            newSurveyData.Add(currentSurveyData);
            ES3.Save(surveyKey, newSurveyData, surveyFile);
        }
    }*/

    private void SanitizeSurveyData()
    {

        if (currentSurveyData.gender == null)
        {
            currentSurveyData.gender = Gender.Other.GetDescription();
        }

        if (currentSurveyData.age == null)
        {
            currentSurveyData.age = minAge.ToString();
        }

        if (currentSurveyData.buildNumber == null)
        {
            currentSurveyData.buildNumber = "No build found";
        }

        if (currentSurveyData.timeStarted == null)
        {
            currentSurveyData.timeStarted = "No timestamp found";
        }
    }

    public void SendDataToServer()
    {
        currentSurveyData.buildNumber = AllScenes.instance.build;
        SanitizeSurveyData();

        WWWForm form = new WWWForm();
        form.AddField("gender", currentSurveyData.gender);
        form.AddField("age", currentSurveyData.age);
        form.AddField("timestamp", currentSurveyData.timeStarted);
        form.AddField("build", currentSurveyData.buildNumber);
        if (currentSurveyData.songData != null)
        {
            form.AddField("wash_results", currentSurveyData.songData.ToString());
        }
        else
        {
            form.AddField("wash_results", "");
        }

        UnityWebRequest uwr = UnityWebRequest.Post("https://eou6x1wkkjjh9fu.m.pipedream.net", form);
        uwr.SendWebRequest();
    }
}
