using UnityEngine;
using Zenject;

public class LevelMusic : ValidatedMonoBehaviour
{
    [SerializeField] private AudioClip _levelMusic;

    private IAudioService _audioService;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }

    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _levelMusic, nameof(_levelMusic));
        return valid;
    }

    private void Start()
    {
        _audioService.PlayMusic(_levelMusic, true);
    }
}
