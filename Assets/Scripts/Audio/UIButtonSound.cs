using UnityEngine;
using Zenject;

public class UIButtonSound : ValidatedMonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    private IAudioService _audioService;

    [Inject]
    public void Construct(IAudioService audioService)
    {
        _audioService = audioService;
    }
    protected override bool ValidateInternal()
    {
        bool valid = true;
        valid &= ValidationUtility.IsAssigned(this, _clip, nameof(_clip));
        return valid;
    }

    public void Play()
    {
        _audioService.PlaySFX(_clip);
    }
}
