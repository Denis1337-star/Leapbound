using UnityEngine;

public class PlayerGameOver : MonoBehaviour
{
    private PlayerHealth health;

    private void Awake()
    {
        health = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        health.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        health.OnDeath -= OnDeath;
    }

    private void OnDeath()
    {
        GameManager.Instance.GameOver();
    }
}
