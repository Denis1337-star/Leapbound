using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private AudioManager _audioManager;

    public override void InstallBindings()
    {
        Container.Bind<IAudioService>()
                   .FromInstance(_audioManager)
                   .AsSingle();

        Container.Bind<ISettingsService>()
            .To<SettingsService>()
            .AsSingle();

        Container.Bind<ISceneFlowService>()
            .To<SceneFlowService>()
            .AsSingle();

        Container.Bind<IScoreService>()
            .To<ScoreService>()
            .AsSingle();

        Container.Bind<IGameStateService>()
          .To<GameStateService>()
          .AsSingle();

        Container.BindInterfacesTo<KeyboardInputService>()
         .AsSingle();
    }
}
