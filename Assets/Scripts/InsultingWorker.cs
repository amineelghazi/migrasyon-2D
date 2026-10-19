using System.Collections;
using TMPro;
using UnityEngine;

public class InsultingWorker : MonoBehaviour
{
    [Header("Bubble")]
    [SerializeField] private GameObject bubble;      // the Bubble canvas
    [SerializeField] private TMP_Text bubbleText;    // the Text inside it

    [Header("Insults")]
    [SerializeField]
    private string[] insults =
    {
        "Plus vite!",
        "Tu es lent!",
        "Retourne travailler!",
        "Quel incapable!"
    };

    [Header("Timing")]
    [SerializeField] private float minDelay = 4f;     // wait between insults
    [SerializeField] private float maxDelay = 9f;
    [SerializeField] private float showTime = 2.5f;

    [Header("Tension")]
    [SerializeField] private float range = 5f;           // player must be this close
    [SerializeField] private float tensionPerInsult = 8f;

    private Transform player;

    private void Awake()
    {
        if (bubble != null) bubble.SetActive(false);
    }

    private void Start()
    {
        var p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
        StartCoroutine(InsultLoop());
    }

    private IEnumerator InsultLoop()
    {
        yield return new WaitForSeconds(Random.Range(0f, maxDelay));   // so workers aren't in sync

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));

            bubbleText.text = insults[Random.Range(0, insults.Length)];
            bubble.SetActive(true);

            bool inRange = player != null &&
                           Vector2.Distance(player.position, transform.position) <= range;
            if (inRange && TensionManager.Instance != null)
                TensionManager.Instance.Add(tensionPerInsult);

            yield return new WaitForSeconds(showTime);
            bubble.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}