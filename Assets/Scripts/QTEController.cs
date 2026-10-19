using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QTEController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject qtePanel;
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform zone;
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Image progressFill;
    [SerializeField] private GameObject successIcon;
    [SerializeField] private GameObject perfectIcon;
    [SerializeField] private GameObject failIcon;

    [Header("Gameplay")]
    [SerializeField] private float cursorSpeed = 0.8f;
    [Range(0.05f, 0.5f)]
    [SerializeField] private float zoneSize = 0.2f;
    [Range(0.1f, 1f)]
    [SerializeField] private float perfectRatio = 0.35f;
    [SerializeField] private int hitsNeeded = 5;
    [SerializeField] private int maxMisses = 3;
    [SerializeField] private float speedUpPerHit = 0.08f;
    [SerializeField] private float feedbackTime = 0.6f;

    [Header("Test")]
    [SerializeField] private bool testOnStart = true;   // untick once the table works

    private bool running;
    private bool waiting;
    private float cursorPos;
    private float direction = 1f;
    private float zoneCenter;
    private float speed;
    private int hits;
    private int misses;
    private System.Action<bool> onFinished;

    private void Awake()
    {
        qtePanel.SetActive(false);
    }

    private void Start()
    {
        if (testOnStart)
            Begin(success => Debug.Log("QTE finished, success = " + success));
    }

    public void Begin(System.Action<bool> callback)
    {
        if (running) return;

        onFinished = callback;
        hits = 0;
        misses = 0;
        speed = cursorSpeed;
        cursorPos = 0f;
        direction = 1f;
        waiting = false;

        HideIcons();
        UpdateFill();
        qtePanel.SetActive(true);
        NewZone();
        running = true;
    }

    private void Update()
    {
        if (!running || waiting) return;

        cursorPos += direction * speed * Time.deltaTime;
        if (cursorPos >= 1f) { cursorPos = 1f; direction = -1f; }
        if (cursorPos <= 0f) { cursorPos = 0f; direction = 1f; }

        float w = bar.rect.width;
        cursor.anchoredPosition = new Vector2((cursorPos - 0.5f) * w, cursor.anchoredPosition.y);

        var kb = Keyboard.current;
        if (kb != null && kb.spaceKey.wasPressedThisFrame)
            StartCoroutine(Resolve());
    }

    private IEnumerator Resolve()
    {
        waiting = true;

        float dist = Mathf.Abs(cursorPos - zoneCenter);
        float half = zoneSize / 2f;

        if (dist <= half)
        {
            hits++;
            speed += speedUpPerHit;
            if (dist <= half * perfectRatio) perfectIcon.SetActive(true);
            else successIcon.SetActive(true);
        }
        else
        {
            misses++;
            failIcon.SetActive(true);
        }

        UpdateFill();
        yield return new WaitForSeconds(feedbackTime);
        HideIcons();

        if (hits >= hitsNeeded) { End(true); yield break; }
        if (misses >= maxMisses) { End(false); yield break; }

        NewZone();
        waiting = false;
    }

    private void NewZone()
    {
        float margin = 0.05f;
        zoneCenter = Random.Range(zoneSize / 2f + margin, 1f - zoneSize / 2f - margin);

        float w = bar.rect.width;
        zone.sizeDelta = new Vector2(zoneSize * w, zone.sizeDelta.y);
        zone.anchoredPosition = new Vector2((zoneCenter - 0.5f) * w, zone.anchoredPosition.y);
    }

    private void UpdateFill()
    {
        progressFill.fillAmount = (float)hits / hitsNeeded;
    }

    private void HideIcons()
    {
        successIcon.SetActive(false);
        perfectIcon.SetActive(false);
        failIcon.SetActive(false);
    }

    private void End(bool success)
    {
        running = false;
        qtePanel.SetActive(false);
        onFinished?.Invoke(success);
    }
}