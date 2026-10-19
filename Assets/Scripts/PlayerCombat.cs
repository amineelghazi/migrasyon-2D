//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerCombat : MonoBehaviour
//{
//    [SerializeField] private float damage = 20f;
//    [SerializeField] private float attackRange = 1.2f;
//    [SerializeField] private float attackRadius = 0.7f;
//    [SerializeField] private float cooldown = 0.5f;
//    [SerializeField] private float attackDuration = 0.4f;  // Duration of attack clip
//    [SerializeField] private float hitDelay = 0.15f;       // Moment of impact after attack starts

//    private PlayerMovement movement;
//    private float nextAttack;

//    private void Awake()
//    {
//        movement = GetComponent<PlayerMovement>();
//    }

//    private void Update()
//    {
//        if (!movement.enabled || movement.IsDefending || movement.IsAttacking) return;

//        var kb = Keyboard.current;
//        if (kb == null || Time.time < nextAttack) return;

//        if (kb.jKey.wasPressedThisFrame)
//        {
//            nextAttack = Time.time + cooldown;
//            movement.PlayAttack(attackDuration);
//            Invoke(nameof(DealDamage), hitDelay);
//        }
//    }

//    private void DealDamage()
//    {
//        float facing = Mathf.Sign(transform.localScale.x);
//        Vector2 point = (Vector2)transform.position + new Vector2(facing * attackRange, 0f);

//        foreach (var hit in Physics2D.OverlapCircleAll(point, attackRadius))
//        {
//            if (hit.GetComponent<EnemyWorker>() == null) continue;
//            var h = hit.GetComponent<Health>();
//            if (h != null) h.TakeDamage(damage);
//        }
//    }

//    private void OnDrawGizmosSelected()
//    {
//        float facing = Mathf.Sign(transform.localScale.x == 0 ? 1 : transform.localScale.x);
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position + new Vector3(facing * attackRange, 0f, 0f), attackRadius);
//    }
//}