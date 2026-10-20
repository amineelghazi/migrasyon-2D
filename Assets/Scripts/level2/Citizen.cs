using System.Collections;
using TMPro;
using UnityEngine;

public enum CitizenType { Gentil, Hostile }

[RequireComponent(typeof(Rigidbody2D))]
public class Citizen : MonoBehaviour
{
    [Header("Talking")]
    [SerializeField] private GameObject bubble;
    [SerializeField] private TMP_Text bubbleText;
    [SerializeField] private CitizenType type = CitizenType.Gentil;
    [SerializeField] private bool randomOrder = true;
    [SerializeField] private float talkRange = 3f;
    [SerializeField] private float showTime = 2.5f;

    [Header("Wandering")]
    [SerializeField] private bool wander = false;
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float wanderDistance = 6f;
    [SerializeField] private Vector2 walkTime = new Vector2(1.5f, 4f);
    [SerializeField] private Vector2 pauseTime = new Vector2(1f, 3f);

    [Header("Facing")]
    [SerializeField] private bool spriteFacesRight = true;

    [Header("Animator State Names (optional)")]
    [SerializeField] private string idleState = "Idle";
    [SerializeField] private string walkState = "Walk";

    // ---------- RÉPLIQUES ----------
    private static readonly string[] friendlyLines =
    {
        "Bienvenue à Montréal!",
        "Tu as besoin d'aide?",
        "Courage, ça va aller.",
        "Tu viens d'arriver?",
        "Il fait froid, hein? Tu t'y feras.",
        "Le métro, c'est par là.",
        "Tu es la bienvenue ici.",
        "Bon courage pour tes démarches!",
        "Passe une belle journée!",
        "Tu vas voir, c'est une belle ville.",
        "Si tu te perds, demande-moi.",
        "Il y a une épicerie plus loin."
    };

    private static readonly string[] hostileLines =
    {
        "Tu bloques le chemin.",
        "Pousse-toi, j'ai pas le temps.",
        "Encore des nouveaux...",
        "On t'a pas demandé de venir.",
        "Parle français, ici!",
        "Je comprends rien à ce que tu dis.",
        "Tu cherches quoi, toi?",
        "Retourne d'où tu viens.",
        "Vous prenez toute la place.",
        "Tu vas rien trouver ici.",
        "Avance, tu me fais perdre mon temps.",
        "Ça se voit que t'es pas d'ici."
    };
    // --------------------------------

    private string[] lines;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform player;
    private float startX;
    private int dir;
    private float timer;
    private bool talking;
    private bool playerWasInRange;
    private string currentState = "";
    private int lastLine = -1;
    private int lineIndex = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        startX = transform.position.x;
        if (bubble != null) bubble.SetActive(false);

        lines = type == CitizenType.Gentil ? friendlyLines : hostileLines;
    }

    private void Start()
    {
        var p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
        timer = Random.Range(pauseTime.x, pauseTime.y);
    }

    private void Update()
    {
        if (player != null)
        {
            bool inRange = Vector2.Distance(player.position, transform.position) <= talkRange;
            if (inRange && !playerWasInRange && !talking)
                StartCoroutine(Talk());
            playerWasInRange = inRange;
        }

        if (wander && !talking)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                if (dir == 0)
                {
                    dir = Random.value < 0.5f ? -1 : 1;
                    timer = Random.Range(walkTime.x, walkTime.y);
                }
                else
                {
                    dir = 0;
                    timer = Random.Range(pauseTime.x, pauseTime.y);
                }
            }

            float offset = transform.position.x - startX;
            if (offset > wanderDistance && dir > 0) dir = -1;
            if (offset < -wanderDistance && dir < 0) dir = 1;

            if (dir != 0) Face(dir);
        }

        SetState(!talking && dir != 0 ? walkState : idleState);
    }

    private void FixedUpdate()
    {
        float vx = (talking || !wander) ? 0f : dir * walkSpeed;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }

    private IEnumerator Talk()
    {
        talking = true;

        if (player != null)
            Face(player.position.x >= transform.position.x ? 1 : -1);

        if (bubbleText != null)
            bubbleText.text = NextLine();
        if (bubble != null) bubble.SetActive(true);

        yield return new WaitForSeconds(showTime);

        if (bubble != null) bubble.SetActive(false);
        talking = false;

        dir = 0;
        timer = Random.Range(pauseTime.x, pauseTime.y);
    }

    private string NextLine()
    {
        if (lines == null || lines.Length == 0) return "";
        if (lines.Length == 1) return lines[0];

        if (randomOrder)
        {
            int i;
            do { i = Random.Range(0, lines.Length); } while (i == lastLine);
            lastLine = i;
            return lines[i];
        }

        string line = lines[lineIndex];
        lineIndex = (lineIndex + 1) % lines.Length;
        return line;
    }

    private void Face(int d)
    {
        float s = d * (spriteFacesRight ? 1f : -1f);

        var scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(s);
        transform.localScale = scale;

        if (bubble != null)
        {
            var bs = bubble.transform.localScale;
            bs.x = Mathf.Abs(bs.x) * Mathf.Sign(s);
            bubble.transform.localScale = bs;
        }
    }

    private void SetState(string next)
    {
        if (anim == null || next == currentState) return;
        if (!anim.HasState(0, Animator.StringToHash(next))) return;
        anim.Play(next);
        currentState = next;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, talkRange);
    }
}