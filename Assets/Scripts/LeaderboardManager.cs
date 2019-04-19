using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* link to Leaderboard http://dreamlo.com/lb/y20euAK17kW263WHBMHfugDBx8lsbNY0qYlhUD2_sIuQ */

public class LeaderboardManager : MonoBehaviour {

    const string privateCode = "y20euAK17kW263WHBMHfugDBx8lsbNY0qYlhUD2_sIuQ";
    const string publicCode = "5b282d0d191a8a0bcc8ba9d5";
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;

    private void Start()
    {
        DownloadHighscores();
    }

    public bool UsernameTaken(string username)
    {
        bool usernameTaken = false;

        for (int i = 0; i < highscoresList.Length; i++)
        {
            if (highscoresList[i].username == username)
            {
                usernameTaken = true;
            }
        }

        return usernameTaken;
    }

    public void AddNewHighscore(string username, int score)
    {
        StartCoroutine(UploadNewHighscore(username, score));
    }

    IEnumerator UploadNewHighscore(string username, int score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            print("Upload Successful");
        else
        {
            print("Error uploading: " + www.error);
        }
    }

    public void DownloadHighscores()
    {
        StartCoroutine("DownloadHighscoresFromDatabase");
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
            FormatHighscores(www.text);
        else
        {
            print("Error Downloading: " + www.error);
        }
    }

    void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoresList = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highscoresList[i] = new Highscore(username, score);
            print(highscoresList[i].username + ": " + highscoresList[i].score);
        }

        GameManager.Instance.UpdateLeader();
    }
}

public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }

}