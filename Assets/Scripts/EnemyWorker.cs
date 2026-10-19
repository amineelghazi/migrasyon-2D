//using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
//public class EnemyWorker : MonoBehaviour
//{
//    [SerializeField] private float speed = 2f;
//    [SerializeField] private float attackRange = 1.2f;
//    [SerializeField] private float damage = 10f;
//    [SerializeField] private float cooldown = 1.2f;
//    [SerializeField] private float attackDuration = 0.5f;
//    [SerializeField] private float hitDelay = 0.2f;

//    [Header("Animator State Names")]
//    [SerializeField] private string idleState = "Idle";
//    [SerializeField] private string walkState = "Walk";
//    [SerializeField] private string attackState = "Attack";

//    private Rigidbody2D rb;
//    private Animator anim;
//    private Transform target;
//    private Health targetHealth;
//    private float nextAttack;
//    private float attackUntil;
//    private string currentState = "";

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        anim = GetComponentInChildren<Animator>();
//    }

//    private void Start()
//    {
//        var p = GameObject.FindWithTag("Player");
//        if (p == null) return;
//        target = p.transform;
//        targetHealth = p.GetComponent<Health>();
//    }

//    private void FixedUpdate()
//    {
//        if (target == null) return;

//        float dx = target.position.x - transform.position.x;
//        float dist = Mathf.Abs(dx);
//        bool attacking = Time.time < attackUntil;

//        if (dist > 0.05f && !attacking)
//        {
//            var s = transform.localScale;
//            s.x = Mathf.Abs(s.x) * Mathf.Sign(dx);   // if enemy is looking at the wrong direction, we add "-" sign to Mathf.abs(s.x) to flip it
//            transform.localScale = s;
//        }

//        if (attacking)
//        {
//            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
//            SetState(attackState);
//        }
//        else if (dist > attackRange)
//        {
//            rb.linearVelocity = new Vector2(Mathf.Sign(dx) * speed, rb.linearVelocity.y);
//            SetState(walkState);
//        }
//        else
//        {
//            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
//            if (Time.time >= nextAttack)
//            {
//                nextAttack = Time.time + cooldown;
//                attackUntil = Time.time + attackDuration;
//                Invoke(nameof(DealDamage), hitDelay);
//                SetState(attackState);
//            }
//            else
//            {
//                SetState(idleState);
//            }
//        }
//    }

//    private void DealDamage()
//    {
//        if (target == null || targetHealth == null) return;
//        if (Mathf.Abs(target.position.x - transform.position.x) <= attackRange + 0.3f)
//            targetHealth.TakeDamage(damage);
//    }

//    private void SetState(string next)
//    {
//        if (anim == null || next == currentState) return;
//        if (!anim.HasState(0, Animator.StringToHash(next))) return;
//        anim.Play(next);
//        currentState = next;
//    }
//}