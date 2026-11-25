using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private SceneFadeView _sceneFade;
    [SerializeField] private LevelTimeCatalog _levelTimeCatalog;

    public override void InstallBindings()
    {
        Container.Bind<IAudioService>().FromInstance(_audioManager).AsSingle();
        Container.Bind<ISettingsService>().To<SettingsService>().AsSingle();
        Container.BindInterfacesTo<KeyboardInputService>().AsSingle();
        Container.Bind<LevelResultManager>().AsSingle();
        Container.Bind<SceneFadeView>().FromInstance(_sceneFade).AsSingle();
        Container.Bind<ISceneFlowService>().To<SceneFlowService>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameReviewPrompt>().AsSingle().NonLazy();
        Container.Bind<LevelRunContext>().AsSingle();
        Container.BindInstance(_levelTimeCatalog);
        Container.Bind<HeroProgress>().AsSingle();
    }
}
