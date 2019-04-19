using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AchievementsManager : MonoBehaviour {

    public GameObject AnnouncementPanel;
    public Text AnnouncementText;

    [Header("Milestones")]
    public int[] ScoreMilestones;
    public int[] TotalPointsMilestones;
    public float[] IdleMilestones;
    public int[] DeathsMilestone;

    [Header("Achievements Titles")]
    public string HighscoreString = "Highscore";
    public string ScoreString = "Score";
    public string TotalPointsString = "TotalPoints";
    public string IdleString = "IdleScore";
    public string BeLeaderString = "BeLeader";
    public string DeathsString = "Deaths";

    public List<string> _announcementsQueued;
    public bool _announcing;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

		if (_announcementsQueued.Count > 0 && !_announcing)
        {
            _announcing = true;

            StartCoroutine(DoSend(_announcementsQueued[0]));
        }

	}

    public void UpdateLeaderAchievement()
    {
        if (PlayerPrefs.GetInt(BeLeaderString) == 0)
        {
            string achivementTitle = BeLeaderString;
            if (PlayerPrefs.GetInt(achivementTitle, 0) != 1)
            {
                PlayerPrefs.SetInt(achivementTitle, 1);
                AnnounceAchievement(achivementTitle);
            }
        }
    }

    public void UpdateIdleAchievements(float newIdleScore)
    {
        PlayerPrefs.SetFloat(IdleString, newIdleScore);

        for (int i = 0; i < IdleMilestones.Length; i++)
        {
            if (newIdleScore > IdleMilestones[i])
            {
                string achivementTitle = IdleString + IdleMilestones[i].ToString();
                if (PlayerPrefs.GetInt(achivementTitle, 0) != 1)
                {
                    PlayerPrefs.SetInt(achivementTitle, 1);
                    AnnounceAchievement(achivementTitle);
                }
            }
        }
    }

    public void UpdateScoreAchievements(int newHighscore)
    {
        PlayerPrefs.SetInt(ScoreString, newHighscore);

        for (int i = 0; i < ScoreMilestones.Length; i++)
        {
            if (newHighscore > ScoreMilestones[i])
            {
                string achivementTitle = ScoreString + ScoreMilestones[i].ToString();
                if (PlayerPrefs.GetInt(achivementTitle, 0) != 1)
                {
                    PlayerPrefs.SetInt(achivementTitle, 1);
                    AnnounceAchievement(achivementTitle);
                }
            }
        }
    }

    public void AnnounceAchievement(string achievementTitle)
    {
        Debug.Log("Achievement Unlocked! : " + achievementTitle);
        _announcementsQueued.Add(achievementTitle);
    }

    IEnumerator DoSend(string achievementTitle)
    {
        AnnouncementText.text = "Achievement Unlocked! : " + achievementTitle;
        AnnouncementPanel.transform.localScale = Vector3.one;
        AnnouncementPanel.SetActive(true);
        AnnouncementPanel.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        yield return new WaitForSeconds(3f);
        AnnouncementPanel.transform.DOScale(Vector3.zero, 0.2f).OnComplete(TurnOffPanel);
    }

    private void TurnOffPanel()
    {
        AnnouncementPanel.SetActive(false);
        _announcing = false;
        _announcementsQueued.RemoveAt(0);
    }

    public void UpdateTotalPointsAchievements(int newScore)
    {
        PlayerPrefs.SetInt(TotalPointsString, PlayerPrefs.GetInt(TotalPointsString, 0) + newScore);

        for (int i = 0; i < TotalPointsMilestones.Length; i++)
        {
            if (PlayerPrefs.GetInt(TotalPointsString, 0) > TotalPointsMilestones[i])
            {
                string achivementTitle = TotalPointsString + TotalPointsMilestones[i].ToString();
                if (PlayerPrefs.GetInt(achivementTitle, 0) != 1)
                {
                    PlayerPrefs.SetInt(achivementTitle, 1);
                    AnnounceAchievement(achivementTitle);
                }
            }
        }
    }

    public void UpdateHighscore(int highscore)
    {
        PlayerPrefs.SetInt(HighscoreString, highscore);
    }
}