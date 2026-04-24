using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Action OnTimerFinish;

    [Header("Minigames")]
    public MinigameBase[] minigameBases;

    private List<MinigameBase> shuffledMinigames = new();

    private float baseTimer = 5;
    private bool canCountDownTimer = false;
    private float score;
    private float currentTime;
    private float playedMinigamesCount = 0;

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
        PostMinigameUI.OnPostGameTimerEnd += SetNextMinigame;
    }

    private void OnDisable()
    {
        MinigameBase.OnMinigameWin -= IncrementScore;
        PostMinigameUI.OnPostGameTimerEnd -= SetNextMinigame;
    }

    private void Start()
    {
        RandomizeMinigame();
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

    private void ShuffleMinigames()
    {
        shuffledMinigames = new List<MinigameBase>(minigameBases);

        for(int i = 0; i < shuffledMinigames.Count; i++)
        {
            int random = UnityEngine.Random.Range(i, shuffledMinigames.Count);

            MinigameBase temp = shuffledMinigames[i];
            shuffledMinigames[i] = shuffledMinigames[random];
            shuffledMinigames[random] = temp;
        }
    }

    private void RandomizeMinigame()
    {
        if (shuffledMinigames.Count == 0)
        {
            ShuffleMinigames();
        }

        MinigameBase selectedMinigame = shuffledMinigames[0];
        shuffledMinigames.RemoveAt(0);

        selectedMinigame.SetupMinigame();
        selectedMinigame.SetupUI();
    }

    private void SetNextMinigame()
    {
        ResetTimer();
        RandomizeMinigame();
    }

    public void SetTimerToZero()
    {
        currentTime = 0;
    }
}
