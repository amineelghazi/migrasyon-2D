using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerCameraClamp : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private bool clampHorizontal = true;
    [SerializeField] private bool clampVertical = true;

    private Rigidbody2D rb;
    private Collider2D col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        if (cam == null) cam = Camera.main;
    }

    private void FixedUpdate()
    {
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        Vector3 c = cam.transform.position;

        Bounds b = col.bounds;
        Vector2 delta = Vector2.zero;

        if (clampHorizontal)
        {
            float minX = c.x - halfW + b.extents.x;
            float maxX = c.x + halfW - b.extents.x;
            delta.x = Mathf.Clamp(b.center.x, minX, maxX) - b.center.x;
        }

        if (clampVertical)
        {
            float minY = c.y - halfH + b.extents.y;
            float maxY = c.y + halfH - b.extents.y;
            delta.y = Mathf.Clamp(b.center.y, minY, maxY) - b.center.y;
        }

        if (delta == Vector2.zero) return;

        rb.position += delta;

        // Cancel velocity pushing against the edge so the player doesn't stick or jitter
        Vector2 vel = rb.linearVelocity;
        if (delta.x != 0f && Mathf.Sign(vel.x) != Mathf.Sign(delta.x)) vel.x = 0f;
        if (delta.y != 0f && Mathf.Sign(vel.y) != Mathf.Sign(delta.y)) vel.y = 0f;
        rb.linearVelocity = vel;
    }
}