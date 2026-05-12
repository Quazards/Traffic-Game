using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;

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
    [SerializeField] private RectTransform[] fasterScreens;

    [Header("Settings - result screen")]
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
        GameManager.OnTimeModifierIncrease += ShowFasterScreen;
    }

    private void OnDisable()
    {
        MinigameBase.OnMinigameStop -= ResetBreakTimer;
        GameManager.OnGameStart -= StartPostMinigameCountdown;
        GameManager.OnGameStart -= CloseGameEndScreen;
        HealthSystem.OnZeroHealth -= StopPostMinigameCountdown;
        HealthSystem.OnZeroHealth -= OpenGameEndScreen;
        GameManager.OnTimeModifierIncrease -= ShowFasterScreen;

    }

    private void Update()
    {
        if (!canCountdown) return;
        if (currentTimer <= 0) return;

        currentTimer -= Time.deltaTime;
        
        if (currentTimer <= 0)
        {
            OnPostGameTimerEnd?.Invoke();
            //CloseScreen(resultScreen);
            MoveOut(resultScreen);
        }
    }

    private void MoveIn(RectTransform screen)
    {
        screen.transform.localPosition = new Vector3(endXPos, screen.transform.localPosition.y, screen.transform.localPosition.z);
        screen.DOAnchorPos(new Vector2(initialXPos, screen.transform.localPosition.y), moveDuration, false).SetEase(Ease.InOutSine);
    }

    private void MoveIn(RectTransform screen, float initialPos)
    {
        screen.transform.localPosition = new Vector3(endXPos, screen.transform.localPosition.y, screen.transform.localPosition.z);
        screen.DOAnchorPos(new Vector2(initialPos, screen.transform.localPosition.y), moveDuration, false).SetEase(Ease.InOutSine);
    }

    private void MoveOut(RectTransform screen)
    {
        screen.transform.localPosition = new Vector3(initialXPos, screen.transform.localPosition.y, screen.transform.localPosition.z);
        screen.DOAnchorPos(new Vector2(endXPos, screen.transform.localPosition.y), moveDuration, false).SetEase(Ease.InOutSine);

        if (postMiniTween != null)
        {
            postMiniTween.Kill(false);
        }

        //postMiniTween.onComplete += onEnd;
    }

    private void MoveOut(RectTransform screen, float initialPos)
    {
        screen.transform.localPosition = new Vector3(initialPos, screen.transform.localPosition.y, screen.transform.localPosition.z);
        screen.DOAnchorPos(new Vector2(endXPos, screen.transform.localPosition.y), moveDuration, false).SetEase(Ease.InOutSine);
    }

    private void CloseScreen(RectTransform screen)
    {
        //MoveOut( screen, () =>
        //{
        //    scoreText.gameObject.SetActive(false);
        //    winScreen.SetActive(false);
        //    loseScreen.SetActive(false);
        //});
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

    private IEnumerator DisplayFasterScreen(RectTransform fasterScreen)
    {
        float startingPos = fasterScreen.transform.localPosition.x;
        fasterScreen.gameObject.SetActive(true);
        MoveIn(fasterScreen, fasterScreen.transform.localPosition.x);
        yield return new WaitForSeconds(1.5f);
        MoveOut(fasterScreen, fasterScreen.transform.localPosition.x);
        yield return new WaitForSeconds(1.5f);
        fasterScreen.gameObject.SetActive(false);
        fasterScreen.transform.localPosition = new Vector3(startingPos, fasterScreen.transform.localPosition.y, fasterScreen.transform.localPosition.z);
    }
    private IEnumerator DisplayFasterScreenWithDelay(RectTransform fasterScreeen, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(DisplayFasterScreen(fasterScreeen));
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

    private void ShowFasterScreen()
    {
        if (HealthSystem.Instance.onZeroHealth) return;

        for (int i = 0; i < fasterScreens.Length; i++)
        {
            StartCoroutine(DisplayFasterScreenWithDelay(fasterScreens[i], (0.25f * i)));

        }
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
        MoveIn(resultScreen);
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
        MoveIn(resultScreen);
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
