using UnityEngine;

public sealed class FireTrapHitbox : MonoBehaviour
{
    [SerializeField] private FireTrap _trap;
    private void OnTriggerStay2D(Collider2D other) 
    {
        _trap.TryHit(other); 
    }
}
