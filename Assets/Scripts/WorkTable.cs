using UnityEngine;
using UnityEngine.InputSystem;

public class WorkTable : MonoBehaviour
{
    [SerializeField] private QTEController qte;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject prompt;   // optional: a small "E" above the table

    private bool playerInRange;
    private bool busy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
        if (prompt != null) prompt.SetActive(!busy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if (prompt != null) prompt.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange || busy) return;
        if (!player.IsGrounded) return;   // no interacting mid-jump

        var kb = Keyboard.current;
        if (kb != null && kb.eKey.wasPressedThisFrame)
            StartQTE();
    }

    private void StartQTE()
    {
        busy = true;
        player.enabled = false;           // freezes movement and jumping
        if (prompt != null) prompt.SetActive(false);
        qte.Begin(OnFinished);
    }

    private void OnFinished(bool success)
    {
        busy = false;
        player.enabled = true;
        Debug.Log("QTE success = " + success);

        if (playerInRange && prompt != null) prompt.SetActive(true);
    }
}