using UnityEngine;

public sealed class DustRun : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    public void SetRunning(bool isRunning, bool flipX)
    {
        if (!isRunning)
        {
            if (gameObject.activeSelf)
            {
                _animator.SetBool(IsRunningHash, false);
                gameObject.SetActive(false);
            }
            return;
        }

        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
        _spriteRenderer.flipX = flipX;
        _animator.SetBool(IsRunningHash, true);
    }
}
