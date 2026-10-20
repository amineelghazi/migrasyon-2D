using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Collider2D zone;
    [SerializeField] private float smoothTime = 0.15f;
    [SerializeField] private Vector2 offset = new Vector2(0f, 1f);

    private Camera cam;
    private Vector3 velocity;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z);

        if (zone != null)
        {
            Bounds b = zone.bounds;
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;

            // if the zone is smaller than the view, just center on it
            desired.x = b.size.x <= halfW * 2f
                ? b.center.x
                : Mathf.Clamp(desired.x, b.min.x + halfW, b.max.x - halfW);

            desired.y = b.size.y <= halfH * 2f
                ? b.center.y
                : Mathf.Clamp(desired.y, b.min.y + halfH, b.max.y - halfH);
        }

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }
}