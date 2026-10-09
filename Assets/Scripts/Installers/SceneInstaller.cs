using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Player _player; 
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(_player).AsSingle();
        Container.Bind<PlayerConfig>().FromInstance(_player.PlayerConfig).AsSingle();

        Container.Bind<PlayerHealth>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerMotor>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAnimation>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerAudio>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerDamageFeedback>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayerDeathEffect>().AsSingle();

        Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle().NonLazy();

     
    }
}
