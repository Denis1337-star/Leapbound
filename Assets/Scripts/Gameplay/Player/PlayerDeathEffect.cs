using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerDeathEffect : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Rigidbody2D _rb;

    private ISceneFlowService _sceneFlowService;
    private IScoreService _scoreService;

    [Inject]
    public void Construct(ISceneFlowService sceneFlowService, IScoreService scoreService)
    {
        _sceneFlowService = sceneFlowService;
        _scoreService = scoreService;
    }
    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
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
        _rb.linearVelocity = Vector2.zero;  
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        _scoreService.Lose();                
        _sprite.color = Color.red;
        yield return new WaitForSeconds(1.2f);

        _sceneFlowService.RestartCurrentLevel();  
    }
}
