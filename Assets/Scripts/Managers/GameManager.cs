using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Action OnTimerFinish;
    public static Action OnGameStart;
    public static Action OnTimeModifierIncrease;

    [Header("References")]
    [SerializeField] private GameObject[] Fuels;

    [Header("Minigames")]
    public MinigameBase[] minigameBases;

    private List<MinigameBase> shuffledMinigames = new();

    public float PlayedMinigamesCount { get; private set; } = 0;

    private bool canCountDownTimer = false;
    private bool canRandomizeMinigame = true;
    private bool hasStartedGame = false;
    private bool hasIncreasedTimerModOnetime = false;
    private float score;
    private float currentTime;
    private float baseTimer = 5;
    private float timerModifier = 0;
    private float fuelTimer = 2;
    private float fuelCount = 0;

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
        OnGameStart += SetNextMinigame;
        OnGameStart += ResetScore;
        MinigameBase.OnMinigameWin += IncrementScore;
        PostMinigameUI.OnPostGameTimerEnd += SetNextMinigame;
        MinigameBase.OnMinigameStop += EnableMinigameRandomization;
        PostMinigameUI.OnPostGameHalfWay += IncrementTimerModifier;
    }

    private void OnDisable()
    {
        OnGameStart -= SetNextMinigame;
        OnGameStart -= ResetScore;
        MinigameBase.OnMinigameWin -= IncrementScore;
        PostMinigameUI.OnPostGameTimerEnd -= SetNextMinigame;
        MinigameBase.OnMinigameStop -= EnableMinigameRandomization;
        PostMinigameUI.OnPostGameHalfWay -= IncrementTimerModifier;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (!hasStartedGame) return;
        UpdateTimer();
        UpdateFuelCountdown();
    }

    private void UpdateTimer()
    {
        if (!canCountDownTimer) return;

        currentTime -= Time.deltaTime;
        TimerUI.Instance.UpdateTimer(currentTime, baseTimer - timerModifier);

        if(currentTime <= 0)
        {
            OnTimerFinish?.Invoke();
            canCountDownTimer = false;
        }
    }

    private void UpdateFuelCountdown()
    {
        if(!canCountDownTimer) return;

        fuelTimer -= Time.deltaTime;

        if(fuelTimer <= 0 && fuelCount < 3)
        {
            int random = UnityEngine.Random.Range(0, Fuels.Length);

            for(int i = 0; i < Fuels.Length; i++)
            {
                Fuels[random].GetComponent<Fuel>().EnableFuel();
            }
            fuelTimer = 2;
        }
    }

    private void ResetTimer()
    {
        currentTime = baseTimer - timerModifier;
        canCountDownTimer = true;
    }

    private void IncrementScore(object sender, System.EventArgs e)
    {
        score += 100;
        PostMinigameUI.Instance.UpdateScoreText(score);
    }

    private void IncrementTimerModifier()
    {
        if (PlayedMinigamesCount > 24) return;

        if (PlayedMinigamesCount % 6 == 0 && PlayedMinigamesCount != 0)
        {
            if(!hasIncreasedTimerModOnetime)
            {
                timerModifier += 1f;
            }
            else
            {
                timerModifier += 0.5f;
            }
            timerModifier = Mathf.Min(2.5f, timerModifier);
            OnTimeModifierIncrease?.Invoke();
        }
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
        canRandomizeMinigame = false;
        PlayedMinigamesCount += 1;
    }

    private void SetNextMinigame()
    {
        if (!canRandomizeMinigame)
        {
            return;
        }

        ResetTimer();
        RandomizeMinigame();
    }

    private void ResetScore()
    {
        score = 0;
        PostMinigameUI.Instance.UpdateScoreText(score);
    }

    public void SetTimerToZero()
    {
        currentTime = 0;
    }

    public void StartGame()
    {
        for (int i = 0; i < Fuels.Length; i++)
        {
            Fuels[i].GetComponent<Fuel>().DisableFuel();
        }

        fuelCount = 0;
        fuelTimer = 2;
        PlayedMinigamesCount = 0;
        timerModifier = 0;
        hasStartedGame = true;
        canRandomizeMinigame = true;
        hasIncreasedTimerModOnetime = false;
        OnGameStart?.Invoke();
    }

    public void EnableMinigameRandomization(object sender, System.EventArgs e)
    {
        canRandomizeMinigame = true;
    }

    public void GainTime(float amount)
    {
        currentTime += amount;
        TimerUI.Instance.UpdateTimer(currentTime, baseTimer - timerModifier);
    }

    public void GainScore(float amount)
    {
        score += amount;
        PostMinigameUI.Instance.UpdateScoreText(score);
    }

    public void DecrementFuelCount()
    {
        fuelCount--;
        fuelCount = Mathf.Max(0, fuelCount);
    }

    public void IncrementFuelCount()
    {
        fuelCount++;
        fuelCount = MathF.Min(3, fuelCount);
    }
}
