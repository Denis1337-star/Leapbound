using UnityEngine;
using YG;
using Zenject;

public sealed class YandexGameplayMarker : MonoBehaviour
{
    private IGameStateService _gameStateService;
    private bool _isGameplayActive;

    [Inject]
    private void Construct(IGameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }
    private void OnEnable()
    {
        _gameStateService.OnStateChanged += HandleGameStateChanged;
        HandleGameStateChanged(_gameStateService.CurrentState);
    }
    private void OnDisable()
    {
        _gameStateService.OnStateChanged -= HandleGameStateChanged;
        if (!_isGameplayActive)
            return;

        YG2.GameplayStop();
        _isGameplayActive = false;
    }
    private void HandleGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            if (_isGameplayActive)
                return;

            YG2.GameplayStart();
            _isGameplayActive = true;
            return;
        }
        if (!_isGameplayActive)
            return;

        YG2.GameplayStop();
        _isGameplayActive = false;
    }
}
