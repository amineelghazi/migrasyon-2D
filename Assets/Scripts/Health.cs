using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 40f;
    [SerializeField] private bool destroyOnDeath = true;

    public UnityEvent onDeath;

    private float current;
    private bool dead;
    private SpriteRenderer sr;

    private void Awake()
    {
        current = maxHealth;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void TakeDamage(float amount)
    {
        if (dead) return;

        current -= amount;
        Debug.Log(gameObject.name + " health: " + current);
        StartCoroutine(Flash());

        if (current <= 0f)
        {
            dead = true;
            onDeath.Invoke();
            if (destroyOnDeath) Destroy(gameObject, 0.15f);
        }
    }

    private IEnumerator Flash()
    {
        if (sr == null) yield break;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (sr != null) sr.color = Color.white;
    }
}