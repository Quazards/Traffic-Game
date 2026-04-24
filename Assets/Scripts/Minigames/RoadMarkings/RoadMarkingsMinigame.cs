using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoadMarkingsMinigame : MinigameBase
{
    private InputSystem_Actions inputActions;

    public static Action OnMoving;
    public static Action OnCollision;

    [Header("References")]
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private ScrollingBackground scrollingBackground;
    [SerializeField] private RectTransform car;
    [SerializeField] private RectTransform leftSideAnchor;
    [SerializeField] private RectTransform rightSideAnchor;

    [Header("Settings")]
    [SerializeField] private float changeSpeed;

    private bool isLeft = true;
    private bool isChangingLanes = false;
    private bool hasWonMinigame = false;
    private bool hasLostMinigame = false;
    private Vector2 targetPos;

    private void Start()
    {
        //SetupMinigame();
    }

    private void OnEnable()
    {
        OnCollision += LoseMinigame;
        GameManager.OnTimerFinish += WinMinigame;
        PostMinigameUI.OnPostGameHalfWay += CloseMinigame;
    }

    private void OnDisable()
    {
        OnCollision -= LoseMinigame;
        GameManager.OnTimerFinish -= WinMinigame;
        inputActions.RoadMarkings.SwitchLanes.performed -= SwitchLanes;
        PostMinigameUI.OnPostGameHalfWay -= CloseMinigame;

        inputActions.RoadMarkings.Disable();
    }

    private void Update()
    {
        scrollingBackground.ScrollYAxis();

        if (isChangingLanes)
        {
            car.anchoredPosition = Vector2.Lerp(
                car.anchoredPosition,
                targetPos,
                changeSpeed * Time.deltaTime
            );  

            if (Vector2.Distance(car.anchoredPosition, targetPos) < 0.01f)
            {
                car.anchoredPosition = targetPos;
                isChangingLanes = false;
            }
        }
    }

    private void FixedUpdate()
    {
        OnMoving?.Invoke();
    }

    private void SwitchLanes(InputAction.CallbackContext context)
    {
        if (isChangingLanes) return;

        isLeft = !isLeft;
        if (isLeft)
        {
            targetPos = leftSideAnchor.anchoredPosition;
        }
        else
        {
            targetPos = rightSideAnchor.anchoredPosition;
        }

        isChangingLanes = true;
    }
    private void LoseMinigame()
    {
        if (!hasLostMinigame)
        {
            if (hasWonMinigame) return;
            hasLostMinigame = true;
            MinigameLose();
            PostMinigameUI.Instance.OpenLoseScreen();
        }
    }

    private void WinMinigame()
    {
        hasWonMinigame = true;
        MinigameWin();
        PostMinigameUI.Instance.OpenWinScreen();
    }
    public override void CloseMinigame()
    {
        minigameUI.SetActive(false);
        this.enabled = false;
    }
    
    public override void SetupMinigame()
    {
        this.enabled = true;
        inputActions = InputManager.Instance.GetInputAction();
        inputActions.RoadMarkings.SwitchLanes.performed += SwitchLanes;
        inputActions.RoadMarkings.Enable();
        hasLostMinigame = false;
        hasWonMinigame = false;

        car.position = leftSideAnchor.position;
        isLeft = true;
        scrollingBackground.ResetYAxis();
    }

    public override void SetupUI()
    {
        minigameUI.SetActive(true);
    }
}
