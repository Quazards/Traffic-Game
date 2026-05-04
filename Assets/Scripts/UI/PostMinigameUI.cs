using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PostMinigameUI : MonoBehaviour
{
    public static PostMinigameUI Instance;

    public static Action OnPostGameTimerEnd;
    public static Action OnPostGameHalfWay;
    public static Action OnPostGameThreeFourths;

    [Header("References")]
    [SerializeField] private RectTransform resultScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameEndScreen;

    [Header("Settings")]
    [SerializeField] private float initialXPos;
    [SerializeField] private float endXPos;
    [SerializeField] private float moveDuration;

    private Tween postMiniTween;
    private float breakTimer = 3;
    private float currentTimer = 0;
    private bool canCountdown = true;
    private bool hasOpenedPostScreen = false;

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
        MinigameBase.OnMinigameStop += ResetBreakTimer;
        GameManager.OnGameStart += StartPostMinigameCountdown;
        GameManager.OnGameStart += CloseGameEndScreen;
        HealthSystem.OnZeroHealth += StopPostMinigameCountdown;
        HealthSystem.OnZeroHealth += OpenGameEndScreen;
    }

    private void OnDisable()
    {
        MinigameBase.OnMinigameStop -= ResetBreakTimer;
        GameManager.OnGameStart -= StartPostMinigameCountdown;
        GameManager.OnGameStart -= CloseGameEndScreen;
        HealthSystem.OnZeroHealth -= StopPostMinigameCountdown;
        HealthSystem.OnZeroHealth -= OpenGameEndScreen;
    }

    private void Update()
    {
        if (!canCountdown) return;
        if (currentTimer <= 0) return;

        currentTimer -= Time.deltaTime;
        
        if (currentTimer <= 0)
        {
            OnPostGameTimerEnd?.Invoke();
            CloseScreen();
        }
    }

    private void MoveIn()
    {
        resultScreen.transform.localPosition = new Vector3(endXPos, resultScreen.transform.localPosition.y, resultScreen.transform.localPosition.z);
        resultScreen.DOAnchorPos(new Vector2(initialXPos, resultScreen.transform.localPosition.y), moveDuration, false).SetEase(Ease.InOutSine);
    }

    private void MoveOut(TweenCallback onEnd)
    {
        resultScreen.transform.localPosition = new Vector3(initialXPos, resultScreen.transform.localPosition.y, resultScreen.transform.localPosition.z);
        resultScreen.DOAnchorPos(new Vector2(endXPos, resultScreen.transform.localPosition.y), moveDuration, false).SetEase(Ease.InOutSine);

        if (postMiniTween != null)
        {
            postMiniTween.Kill(false);
        }

        //postMiniTween.onComplete += onEnd;
    }

    private void CloseScreen()
    {
        MoveOut( () =>
        {
            scoreText.gameObject.SetActive(false);
            winScreen.SetActive(false);
            loseScreen.SetActive(false);
        });
    }

    private IEnumerator HalfwayTimer()
    {
        yield return new WaitForSeconds(breakTimer / 2);
        OnPostGameHalfWay?.Invoke();
    }

    private IEnumerator ThreeFourthsTimer()
    {
        yield return new WaitForSeconds (breakTimer * 0.75f);
        OnPostGameThreeFourths?.Invoke();
    }

    private void OpenGameEndScreen()
    {
        gameEndScreen.SetActive(true);
    }

    private void CloseGameEndScreen()
    {
        gameEndScreen.SetActive(false);
    }

    private void CheckHalfwayPoint()
    {
        StartCoroutine(HalfwayTimer());
    }

    private void CheckThreeFourthsPoint()
    {
        StartCoroutine(ThreeFourthsTimer());
    }

    public void OpenWinScreen()
    {
        hasOpenedPostScreen = true;
        CheckHalfwayPoint();
        CheckThreeFourthsPoint();
        scoreText.gameObject.SetActive(false);
        loseScreen.SetActive(false);

        scoreText.gameObject.SetActive(true);
        winScreen.SetActive(true);
        MoveIn();
    }

    public void OpenLoseScreen()
    {
        hasOpenedPostScreen = true;
        CheckHalfwayPoint();
        CheckThreeFourthsPoint();
        scoreText.gameObject.SetActive(false);
        winScreen.SetActive(false);

        scoreText.gameObject.SetActive(true);
        loseScreen.SetActive(true);
        MoveIn();
    }

    public void UpdateScoreText(float amount)
    {
        scoreText.text = "score: " + amount.ToString();
    }

    public void ResetBreakTimer(object sender, System.EventArgs e)
    {
        currentTimer = breakTimer;
    }

    public void StopPostMinigameCountdown()
    {
        canCountdown = false;
    }

    public void StartPostMinigameCountdown()
    {
        canCountdown = true;

        if(hasOpenedPostScreen)
        {
            currentTimer = 0.01f;
        }
    }
}
