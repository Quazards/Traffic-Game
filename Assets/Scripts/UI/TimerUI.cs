using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    public static TimerUI Instance;

    [SerializeField] private Slider timerSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateTimer(float currentTime, float maxTime)
    {
        timerSlider.value = currentTime/maxTime;
    }
}
