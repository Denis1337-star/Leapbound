using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private int _targetScore;
    [SerializeField] private DustOneShot _dustOneShotPrefab;
    [SerializeField] private CoinOneShot _coinOneShotPrefab;
    [SerializeField] private CoinFlyFx _coinFlyFxPrefab;
    [SerializeField] private Canvas _fxCanvas;
    [SerializeField] private ScoreDisplay _scoreDisplay;
    [SerializeField] private Transform _playerSpawn;
    [SerializeField] private Player _knightPrefab;
    [SerializeField] private Player _roguePrefab;
    [SerializeField] private CinemachineCamera _followCamera;
    private LevelRunContext _levelRunContext;
    private Player _spawnedPlayer;

    [Inject]
    public void Construct(LevelRunContext levelRunContext)
    {
        _levelRunContext = levelRunContext;
    }
    public override void InstallBindings()
    {
        PlayerInstaller();
        LevelInstaller();
        FxInstaller();
        Container.QueueForInject(_spawnedPlayer);
        Container.QueueForInject(_spawnedPlayer.HitStopPulse);
    }
    private void PlayerInstaller()
    {
        _spawnedPlayer = SpawnPlayer();
        Container.Bind<Player>().FromInstance(_spawnedPlayer).AsSingle();
        Container.Bind<PlayerConfig>().FromInstance(_spawnedPlayer.PlayerConfig).AsSingle();
        Container.Bind<PlayerLives>().AsSingle().WithArguments(2);
        Container.Bind<PlayerHealth>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMotor>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAttack>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAnimation>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAudio>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerDamageFeedback>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerDeathEffect>().AsSingle();
        Container.Bind<PlayerRespawn>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerWinEffect>().AsSingle();
    }
    private void LevelInstaller()
    {
        Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle().NonLazy();
        Container.Bind<CheckpointService>().AsSingle().NonLazy();
        Container.Bind<LevelStarCalculator>().AsSingle().WithArguments(_targetScore);
        Container.BindInterfacesAndSelfTo<LevelTimer>().AsSingle();
    }
    private void FxInstaller()
    {
        Container.Bind<HealthOneShot>().FromInstance(_spawnedPlayer.HealthOneShot).AsSingle();
        Container.Bind<PlayerDust>().AsSingle()
            .WithArguments(_spawnedPlayer.DustRun, _spawnedPlayer.GroundCheckAnchor);

        Container.Bind<DustOneShotPool>().AsSingle().WithArguments(_dustOneShotPrefab, 4);
        Container.Bind<CoinOneShotPool>().AsSingle().WithArguments(_coinOneShotPrefab, 8);
        Container.Bind<CoinFlyFxPool>().AsSingle().WithArguments(_coinFlyFxPrefab, 8, _fxCanvas, _scoreDisplay);
    }
    private Player SpawnPlayer()
    {
        Player prefab = _knightPrefab;
        if (_levelRunContext.SelectedHero == HeroId.Rogue)
            prefab = _roguePrefab;

        Player player = Instantiate(prefab, _playerSpawn.position, _playerSpawn.rotation);

        CameraTarget target = _followCamera.Target;
        target.TrackingTarget = player.transform;
        _followCamera.Target = target;

        return player;
    }
}
