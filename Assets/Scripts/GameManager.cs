using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class GameManager : MonoBehaviour {

    #region
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    #endregion

    private Spawner _spawner;
    public Spawner Spawner { get { if (_spawner == null) { _spawner = FindObjectOfType<Spawner>(); } return _spawner; } }
    private Player _player;
    public Player Player { get { if (_player == null) { _player = FindObjectOfType<Player>(); } return _player; } }
    private Text _scoreText;
    public Text ScoreText { get { if (_scoreText == null) { _scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>(); } return _scoreText; } }
    private Text _highscoreText;
    public Text HighscoreText { get { if (_highscoreText == null) { _highscoreText = GameObject.FindGameObjectWithTag("Highscore").GetComponent<Text>(); } return _highscoreText; } }
    private Text _leaderScore;
    public Text LeaderScore { get { if (_leaderScore == null) { _leaderScore = GameObject.FindGameObjectWithTag("Leader Statement").GetComponent<Text>(); } return _leaderScore; } }

    public GameObject IdWindow;
    public InputField NameField;
    public Text UsernameLabel;
    public int LevelInterval;
    public int Level = 1;
    public bool Paused = true;
    public int Score;
    public float IdleTimer;

    private float _gameTime;
    private string _playerName;
    private LeaderboardManager _leaderboard;
    private AchievementsManager _achievementsManager;
    public float _idleScore;
    public int _highScore;

    // Use this for initialization
    void Start () {
        _leaderboard = GetComponent<LeaderboardManager>();
        _achievementsManager = GetComponent<AchievementsManager>();

        if (PlayerPrefs.GetString("Name").Length > 0)
        {
            GetPlayerPrefs();
            StartGame();
        }
        else
        {
            PanelShow(IdWindow);
            NameField.onEndEdit.AddListener(delegate { CreatePlayer(NameField); });
            Debug.Log("Name in Playerprefs is empty");
        }

        UpdateHighscoreText();
        UpdateScoreText();
	}

    private void PanelShow(GameObject panel)
    {
        panel.transform.localScale = Vector3.one;
        panel.SetActive(true);
        panel.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
    }

    // Update is called once per frame
    void Update () {
        if (Paused) return;

        Score += 1;

        ScoreText.text = Score.ToString();

        _gameTime += Time.deltaTime;

        IdleTimer += Time.deltaTime;

        if (_gameTime/LevelInterval > Level)
        {
            Level++;
        }

        _achievementsManager.UpdateScoreAchievements(Score);

        //Debug tools
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Deleting All PlayerPrefs");
            PlayerPrefs.DeleteAll();
        }
	}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StartGame()
    {
        Paused = false;
    }

    private void GetPlayerPrefs()
    {
        Debug.Log("Importing PlayerPrefs");
        _playerName = PlayerPrefs.GetString("Name");
        _idleScore = PlayerPrefs.GetFloat(_achievementsManager.IdleString, 0);
        _highScore = PlayerPrefs.GetInt(_achievementsManager.HighscoreString, 0);
    }

    public void UpdateIdleScore()
    {
        if (IdleTimer > _idleScore)
        {
            _idleScore = IdleTimer;
        }

        _achievementsManager.UpdateIdleAchievements(_idleScore);

        IdleTimer = 0;
    }

    private void UpdateScoreText()
    {
        ScoreText.text = Score.ToString();
    }

    public void UpdateLeader()
    {
        if (_leaderboard.highscoresList != null)
        {
            if (_leaderboard.highscoresList.Length > 0)
            {
                if (_playerName == _leaderboard.highscoresList[0].username)
                {
                    LeaderScore.text = "YOU DA BEST BOO";

                    _achievementsManager.UpdateLeaderAchievement();
                }
                else
                    LeaderScore.text = "DA BEST BOO BE " + _leaderboard.highscoresList[0].username + " " + _leaderboard.highscoresList[0].score;
            }
            else
            {
                LeaderScore.text = "You are the first player ever lol wut???";
            }
        }
        else
        {
            LeaderScore.text = "You are the first player ever lol wut???";
        }

    }

    public void CreatePlayer(InputField nameField)
    {
        if (nameField.text == string.Empty)
        {
            return;
        }
        else if (_leaderboard.UsernameTaken(nameField.text))
        {
            UsernameLabel.text = "USERNAME TAKEN";
            return;
        }

        _playerName = nameField.text;
        PlayerPrefs.SetString("Name", _playerName);

        IdWindow.SetActive(false);
        Paused = false;
    }

    private void UpdateHighscoreText()
    {
        HighscoreText.text = PlayerPrefs.GetInt(_achievementsManager.HighscoreString, 0).ToString();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

    public IEnumerator Die()
    {
        Paused = true;

        PlayerPrefs.SetInt(_achievementsManager.DeathsString, PlayerPrefs.GetInt(_achievementsManager.DeathsString, 0) + 1);

        UpdateIdleScore();
        UpdateHighscore();

        _achievementsManager.UpdateTotalPointsAchievements(Score);

        foreach (Goomba goomba in Spawner.transform.GetComponentsInChildren<Goomba>())
        {
            goomba.Stop();
        }

        yield return new WaitForSeconds(1);

        Reset();
    }

    void UpdateHighscore()
    {
        if (Score > _highScore)
        {
            _highScore = Score;
            GetComponent<LeaderboardManager>().AddNewHighscore(_playerName, _highScore);
            _leaderboard.DownloadHighscores();
            _achievementsManager.UpdateHighscore(_highScore);
            _achievementsManager.UpdateScoreAchievements(_highScore);
        }
    }

    public void Reset()
    {
        Score = 0;

        _gameTime = 0;

        Level = 1;

        SceneManager.LoadScene(0);
    }
}