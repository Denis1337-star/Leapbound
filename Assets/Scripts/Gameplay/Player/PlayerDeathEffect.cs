using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerDeathEffect : MonoBehaviour
{
    private Player _player;
    private ISceneFlowService _sceneFlowService;
    private IGameStateService _gameStateService;

    [Inject]
    public void Construct(Player player,ISceneFlowService sceneFlowService, IGameStateService gameStateService)
    {
        _player = player;
        _sceneFlowService = sceneFlowService;
        _gameStateService = gameStateService;
    }
   
    private void OnEnable()
    {
        GetComponent<PlayerHealth>().OnDeath += Play;
    }

    private void OnDisable()
    {
        GetComponent<PlayerHealth>().OnDeath -= Play;
    }
    public void Play()
    {
        _player.Rigidbody.linearVelocity = Vector2.zero;  
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        _gameStateService.Lose();              
        _player.SpriteRenderer.color = Color.red;
        yield return new WaitForSecondsRealtime(1.2f);
        _sceneFlowService.RestartCurrentLevel();  
    }
}
