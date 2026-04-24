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

    [Header("References")]
    [SerializeField] private RectTransform resultScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Settings")]
    [SerializeField] private float initialXPos;
    [SerializeField] private float endXPos;
    [SerializeField] private float moveDuration;

    private Tween postMiniTween;
    private float breakTimer = 3;
    private float currentTimer = 0;

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
    }

    private void Update()
    {
        if (currentTimer <= 0) return;

        currentTimer -= Time.deltaTime;
        
        if (currentTimer <= 0)
        {
            OnPostGameTimerEnd?.Invoke();
            CloseScreen();
            //Debug.Log("post game timer ended");
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

    private void CheckHalfwayPoint()
    {
        StartCoroutine(HalfwayTimer());
    }

    public void OpenWinScreen()
    {
        CheckHalfwayPoint();
        scoreText.gameObject.SetActive(false);
        loseScreen.SetActive(false);

        scoreText.gameObject.SetActive(true);
        winScreen.SetActive(true);
        MoveIn();
    }

    public void OpenLoseScreen()
    {
        CheckHalfwayPoint();
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

        //Debug.Log("break timer reset");
    }
}
