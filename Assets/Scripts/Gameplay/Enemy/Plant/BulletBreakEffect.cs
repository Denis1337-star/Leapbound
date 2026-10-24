using UnityEngine;

public sealed class BulletBreakEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    public bool IsBusy => _particleSystem.isPlaying;
    public void Play(Vector3 worlPosition)
    {
        transform.SetParent(null, true);
        transform.position = worlPosition;
        gameObject.SetActive(true);
        _particleSystem.Clear(true);
        _particleSystem.Play(true);
    }
}
