using UnityEngine;
using Zenject;

public class LevelMusic : MonoBehaviour
{
    [SerializeField] private AudioClip _levelMusic;

    private IAudioService _audioService;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }

    private void Start()
    {
        _audioService.PlayMusic(_levelMusic, true);
    }
}
