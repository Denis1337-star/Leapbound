using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private Player _player; 
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle().NonLazy();

        Container.Bind<Player>().FromInstance(_player).AsSingle();
    }
}
