using UnityEngine;

public class PlayerPlatformHandler : MonoBehaviour
{
    public Vector2 PlatformVelocity { get; private set; }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Platform platform = collision.collider.GetComponent<Platform>();
        if (platform == null) return;

        // проверяем, что стоим СВЕРХУ
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                PlatformVelocity = platform.PlatformVelocity;
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Platform>() != null)
        {
            PlatformVelocity = Vector2.zero;
        }
    }
}
