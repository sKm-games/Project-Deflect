using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class MainMenuUIClass
{
    public GameObject BackgroundImage;
    public GameObject StartScreen;
    [Space]
    public GameObject ModeSelectScreen;
    public List<Button> ModeSelectButtons;    
    public List<Button> ModeLeaderboardButtons;
    [Space]
    public GameObject SettingsScreen;
    public GameObject GameInfoScreen;
    public GameObject CreditsScreen;
}

[System.Serializable]
public class GameOverUIClass
{
    public GameObject GameOverScreen;
    public TextMeshProUGUI TitleText;
    public TextMeshProUGUI ScoreText;
    public Button LeaderboardButton;
    public Color HighScoreTextColor;    
}

[System.Serializable]
public class GameplayUIClass
{   
    public GameObject GameplayScreen;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI ScoreText;
    [Space]
    public GameObject PauseScreen;
    [Space]
    public TextMeshProUGUI CountdownText;
}
