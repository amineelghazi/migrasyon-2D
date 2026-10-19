using UnityEngine;
using UnityEngine.SceneManagement;

public class FightManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private string nextScene = "Level2";
    [SerializeField] private float delay = 1.5f;

    private bool over;
    private bool started;

    private void Start()
    {
        playerHealth.onDeath.AddListener(OnPlayerDeath);
        Invoke(nameof(Arm), 0.5f);   // avoids ending instantly before workers are counted
    }

    private void Arm()
    {
        started = true;
    }

    private void Update()
    {
        if (!started || over) return;

        if (FindObjectsByType<EnemyWorker>(FindObjectsSortMode.None).Length == 0)
            Finish(true);
    }

    private void OnPlayerDeath()
    {
        Finish(false);
    }

    private void Finish(bool win)
    {
        if (over) return;
        over = true;

        if (win) Invoke(nameof(LoadNext), delay);
        else Invoke(nameof(Retry), delay);
    }

    private void LoadNext()
    {
        SceneManager.LoadScene(nextScene);
    }

    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}