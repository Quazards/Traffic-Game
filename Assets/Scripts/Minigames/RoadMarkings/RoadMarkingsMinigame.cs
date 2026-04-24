using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoadMarkingsMinigame : MinigameBase
{
    private InputSystem_Actions inputActions;

    public static Action OnChangingLanes;

    [Header("References")]
    [SerializeField] private ScrollingBackground scrollingBackground;
    [SerializeField] private RectTransform car;
    [SerializeField] private RectTransform leftSideAnchor;
    [SerializeField] private RectTransform rightSideAnchor;

    [Header("Settings")]
    [SerializeField] private float changeSpeed;

    private bool isLeft = true;
    private bool isChangingLanes = false;
    private Vector2 targetPos;

    private void Start()
    {
        SetupMinigame();
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

            OnChangingLanes?.Invoke();

            if (Vector2.Distance(car.anchoredPosition, targetPos) < 0.01f)
            {
                car.anchoredPosition = targetPos;
                isChangingLanes = false;
            }
        }
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

    public override void CloseMinigame()
    {
        throw new System.NotImplementedException();
    }

    public override void SetupMinigame()
    {
        inputActions = InputManager.Instance.GetInputAction();

        inputActions.RoadMarkings.SwitchLanes.performed += SwitchLanes;

        inputActions.RoadMarkings.Enable();
    }

    public override void SetupUI()
    {
        throw new System.NotImplementedException();
    }
}
