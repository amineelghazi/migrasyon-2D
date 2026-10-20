using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ZoneLimit : MonoBehaviour
{
    [SerializeField] private Collider2D zone;
    [SerializeField] private bool limitHorizontal = true;
    [SerializeField] private bool limitVertical = false;   

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (zone == null) return;

        Bounds z = zone.bounds;
        Bounds b = col.bounds;
        Vector2 delta = Vector2.zero;

        if (limitHorizontal)
        {
            float x = Mathf.Clamp(b.center.x, z.min.x + b.extents.x, z.max.x - b.extents.x);
            delta.x = x - b.center.x;
        }

        if (limitVertical)
        {
            float y = Mathf.Clamp(b.center.y, z.min.y + b.extents.y, z.max.y - b.extents.y);
            delta.y = y - b.center.y;
        }

        if (delta == Vector2.zero) return;

        rb.position += delta;

        // stop pushing against the edge
        Vector2 vel = rb.linearVelocity;
        if (delta.x != 0f && Mathf.Sign(vel.x) != Mathf.Sign(delta.x)) vel.x = 0f;
        if (delta.y != 0f && Mathf.Sign(vel.y) != Mathf.Sign(delta.y)) vel.y = 0f;
        rb.linearVelocity = vel;
    }
}