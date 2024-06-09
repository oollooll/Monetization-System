using System.Collections;
using Area;
using Plane;
using Plane.Bullet;
using Plane.Enemy;
using Services;
using Services.GameCenter;
using Services.ProjectUpdater;
using Services.SDK.Ads;
using Services.SoundManager;
using Services.VibrationManagement;
using TMPro;
using UnityEngine;

public class EntryPoint:MonoBehaviour
{
    [SerializeField] private PlayerPlane _playerPlane;
    [SerializeField] private EnemyPlaneFactory _enemyPlaneFactory;
    [SerializeField] private BulletFactory _bulletFactory;
    [SerializeField] private GameOverManager _gameOverManager;
    [SerializeField] private TextMeshProUGUI _textScore;
    [SerializeField] private TextMeshProUGUI _coinsScore;
    [SerializeField] private AreaDataStorage _areaDataStorage;
    [SerializeField] private SpriteRenderer _backGround;
    [SerializeField] private InterstitialAd _interstitialAd;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private VibrationManager _vibrationManager;
    [SerializeField] private EnemyCrash _enemyCrash;
    [SerializeField] private SkillController _skillController;
    [SerializeField] private GameCenterManager _gameCenterManager;

    private ProjectUpdater _projectUpdater;
    private ScoreUpdater _scoreUpdater;
    private CoinsUpdater _coinsUpdater;

    private void Start()
    {
        var index = PlayerPrefs.GetInt("CurrentArea");

        var areaData = _areaDataStorage.AreasList[index];
        _backGround.sprite = areaData.AreaSprite;

        if (ProjectUpdater.Instance == null)
            _projectUpdater = new GameObject().AddComponent<ProjectUpdater>();
        else
            _projectUpdater = ProjectUpdater.Instance as ProjectUpdater;
        _projectUpdater.IsPaused = false;

        _coinsUpdater = new CoinsUpdater(_coinsScore,areaData.LevelModif);
        _scoreUpdater = new ScoreUpdater(_coinsUpdater,_textScore, index,areaData.ScoreToPass);
       
        _playerPlane.Initialize(3);
        _gameOverManager.Initialize(_coinsUpdater,_scoreUpdater,index,_projectUpdater,_interstitialAd,_gameCenterManager);
        _enemyPlaneFactory.Initialize(_gameOverManager,_soundManager,_vibrationManager);
        _bulletFactory.Initialize(_scoreUpdater,_gameOverManager);
        _soundManager.Initialize();
        _skillController.Initialize();
        PlayMusic();

        _enemyCrash.ScoreUpdated += _scoreUpdater.UpdateScore;
        _playerPlane.GameLoss += _gameOverManager.ShowLossPAnel;
        _scoreUpdater.GameWon += _gameOverManager.ShowWinPAnel;
    }

    public void Pause()
    {
        _projectUpdater.IsPaused = !_projectUpdater.IsPaused;
    }
    
    private void PlayMusic()
    {
        StartCoroutine(MusicDelay());
    }

    private IEnumerator MusicDelay()
    {
        yield return new WaitForSeconds(0.1f);
        _soundManager.PlayMusic(0);
    }

    private void OnDestroy()
    {
        _enemyCrash.ScoreUpdated -= _scoreUpdater.UpdateScore;
        _playerPlane.GameLoss -= _gameOverManager.ShowLossPAnel;
        _scoreUpdater.GameWon -= _gameOverManager.ShowWinPAnel;
    }
}