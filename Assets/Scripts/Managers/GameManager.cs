using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Action OnTimerFinish;

    private float baseTimer = 5;
    private bool canCountDownTimer = false;
    private float score;
    private float currentTime;

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

    private void OnEnable()
    {
        MinigameBase.OnMinigameWin += IncrementScore;
        PostMinigameUI.OnPostGameTimerEnd    += ResetTimer;
    }

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        if (!canCountDownTimer) return;

        currentTime -= Time.deltaTime;
        TimerUI.Instance.UpdateTimer(currentTime, baseTimer);

        if(currentTime <= 0)
        {
            OnTimerFinish?.Invoke();
            canCountDownTimer = false;
        }
    }

    private void ResetTimer()
    {
        currentTime = baseTimer;
        canCountDownTimer = true;
    }

    private void IncrementScore(object sender, System.EventArgs e)
    {
        score += 100;
        PostMinigameUI.Instance.UpdateScoreText(score);
    }

    private void EvaluateMinigame(object sender, System.EventArgs e)
    {
        
    }

}
