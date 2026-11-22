using UnityEngine;

public sealed class CheckpointService 
{
    public Vector3 SpawnPosition { get; private set; }
    public CheckpointService(Player player)
    {
        SpawnPosition = player.transform.position;
    }
    public void SetSpawn(Vector3 position)
    {
        SpawnPosition = position;
    }
}
