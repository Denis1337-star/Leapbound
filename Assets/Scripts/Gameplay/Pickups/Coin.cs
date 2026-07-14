using UnityEngine;
using Zenject;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _value = 10;

    private IScoreService _scoreService;

    [Inject]
    public void Construct(IScoreService scoreService)
    {
    _scoreService = scoreService;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _scoreService.Add(_value);  
        Destroy(gameObject);     
    }
}
