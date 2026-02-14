using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        var player = col.GetComponent<PlayerHealth>();
        if (player != null)
            player.Kill();
    }
}
