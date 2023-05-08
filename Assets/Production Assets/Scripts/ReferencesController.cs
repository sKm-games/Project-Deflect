using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencesController : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    public static CameraController GetCameraController;

    [SerializeField] private GameController gameController;
    public static GameController GetGameController;

    [SerializeField] private BlockerController blockerController;
    public static BlockerController GetBlockerController;

    [SerializeField] private ScoreController scoreController;
    public static ScoreController GetScoreController;

    [SerializeField] private LevelController levelController;
    public static LevelController GetLevelController;
    
    [SerializeField] private DifficultyController difficultyController;
    public static DifficultyController GetDifficultyController;

    [SerializeField] private ProjectileController projectileController;
    public static ProjectileController GetProjectileController;

    [SerializeField] private UIController uiController;
    public static UIController GetUIController;

    [SerializeField] private SettingsController settingController;
    public static SettingsController GetSettingsController;    

    [SerializeField] private LoadingScreenController loadingScreenController;
    public static LoadingScreenController GetLoadingScreenController;

    [SerializeField] private SoundController soundController;
    public static SoundController GetSoundController;

    [SerializeField] private SaveManager saveManager;
    public static SaveManager GetSaveManager;

    [SerializeField] private GooglePlayController googlePlayController;
    public static GooglePlayController GetGooglePlayController;

    [SerializeField] private LeaderboardController leaderboardController;
    public static LeaderboardController GetLeaderboardController;

    [SerializeField] private AchievementController achievementController;
    public static AchievementController GetAchievementController;

    [SerializeField] private BannerADController bannerADController;
    public static BannerADController GetBannerADController;

    [SerializeField] private VideoADController videoADController;
    public static VideoADController GetVideoADController;

    private void Awake()
    {
        GetCameraController = cameraController;        
        GetGameController = gameController;
        GetBlockerController = blockerController;

        GetScoreController = scoreController;
        GetLevelController = levelController;
        GetDifficultyController = difficultyController;

        GetProjectileController = projectileController;
        GetUIController = uiController;
        GetSettingsController = settingController;

        GetLoadingScreenController = loadingScreenController;
        GetSoundController = soundController;
        GetSaveManager = saveManager;

        GetGooglePlayController = googlePlayController;
        GetLeaderboardController = leaderboardController;
        GetBannerADController = bannerADController;

        GetVideoADController = videoADController;
        GetAchievementController = achievementController;
    }
}
