using UnityEngine;
using Zenject;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    private IAudioService _audioService;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }


    public void Play()
    {
        _audioService.PlaySFX(_clip);
    }
}
