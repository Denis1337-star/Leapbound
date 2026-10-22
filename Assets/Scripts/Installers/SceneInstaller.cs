using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Player _player;
    [SerializeField] private int _threeStarScore;
    [SerializeField] private DustOneShot _dustOneShotPrefab;
    [SerializeField] private DustRun _dustRun;
    [SerializeField] private CoinOneShot _coinOneShotPrefab;
    [SerializeField] private HealthOneShot _healthOneShot;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(_player).AsSingle();
        Container.Bind<PlayerConfig>().FromInstance(_player.PlayerConfig).AsSingle();
        Container.Bind<HealthOneShot>().FromInstance(_healthOneShot).AsSingle();

        Container.Bind<DustOneShotPool>().AsSingle().WithArguments(_dustOneShotPrefab, 4);
        Container.Bind<CoinOneShotPool>().AsSingle().WithArguments(_coinOneShotPrefab, 8);
        Container.Bind<PlayerDust>().AsSingle().WithArguments(_dustRun, _player.GroundCheckAnchor);


        Container.Bind<PlayerHealth>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMotor>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAnimation>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAudio>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerDamageFeedback>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerDeathEffect>().AsSingle();

        Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle().NonLazy();
        Container.Bind<CheckpointService>().AsSingle().NonLazy();
        Container.Bind<PlayerRespawn>().AsSingle();

        Container.Bind<LevelStarCalculator>().AsSingle().WithArguments(_threeStarScore);

     
    }
}
