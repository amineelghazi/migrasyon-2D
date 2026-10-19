using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TensionManager : MonoBehaviour
{
    public static TensionManager Instance;

    [SerializeField] private Image fill;
    [SerializeField] private float startTension = 0f;
    [SerializeField] private float risePerSecond = 0f;   // 0 once the workers are in
    [SerializeField] private string fightScene = "";     // leave empty until the fight scene exists

    public float Tension { get; private set; }

    private bool triggered;

    private void Awake()
    {
        Instance = this;
        Tension = startTension;
    }

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        if (risePerSecond != 0f) Add(risePerSecond * Time.deltaTime);
    }

    public void Add(float amount)
    {
        if (triggered) return;

        Tension = Mathf.Clamp(Tension + amount, 0f, 100f);
        Refresh();

        if (Tension >= 100f)
        {
            triggered = true;
            Debug.Log("Max tension reached!");
            if (!string.IsNullOrEmpty(fightScene))
                SceneManager.LoadScene(fightScene);
        }
    }

    private void Refresh()
    {
        if (fill != null) fill.fillAmount = Tension / 100f;
    }
}