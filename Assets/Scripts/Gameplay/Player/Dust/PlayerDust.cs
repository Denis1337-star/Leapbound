using UnityEngine;

public sealed class PlayerDust 
{
    private readonly DustOneShotPool _dustOneShotPool;
    private readonly DustRun _dustRun;
    private readonly Transform _feet;
    public PlayerDust(DustOneShotPool dustOneShotPool, DustRun dustRun, Transform feet)
    {
        _dustOneShotPool = dustOneShotPool;
        _dustRun = dustRun;
        _feet = feet;
    }
    public void PlayJump()
    {
        _dustOneShotPool.Play(DustOneShotType.Jump, _feet.position);
    }
    public void PlayLand()
    {
        _dustOneShotPool.Play(DustOneShotType.Land, _feet.position);
    }
    public void SetRunning(bool isRunning, bool flipx)
    {
        _dustRun.SetRunning(isRunning,flipx);
    }
}
