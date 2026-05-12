using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PersonCrossingMinigame : MinigameBase
{
    private InputSystem_Actions inputActions;

    [Header("References")]
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private ScrollingBackground scrollingBackground;
    [SerializeField] private Slider progressSlider;

    private float currentProgress = 0;
    private float maxProgress = 100;

    private bool hasWon = false;
    private bool hasLost = false;

    private void OnEnable()
    {
        PostMinigameUI.OnPostGameHalfWay += CloseMinigame;
        GameManager.OnTimerFinish += LoseMinigame;
    }

    private void OnDisable()
    {
        inputActions.PersonCrossing.DecreaseSpeed.performed -= IncreaseProgress;
        inputActions.PersonCrossing.DecreaseSpeed.canceled -= IncreaseProgress;
        GameManager.OnTimerFinish -= LoseMinigame;
        PostMinigameUI.OnPostGameHalfWay -= CloseMinigame;

        inputActions.PersonCrossing.Disable();


    }

    private void Update()
    {
        scrollingBackground.ScrollXAxis();
    }

    private void IncreaseProgress(InputAction.CallbackContext context)
    {
        if (hasWon) return;
        if (hasLost) return;
        currentProgress += 2.5f;
        UpdateSlider();

        if(currentProgress >= maxProgress)
        {
            MinigameWin();
            PostMinigameUI.Instance.OpenWinScreen();
            hasWon = true;
        }
    }

    private void UpdateSlider()
    {
        progressSlider.value = currentProgress / maxProgress;
        
    }

    private void LoseMinigame()
    {
        if (hasWon) return;
        hasLost = true;
        MinigameLose();
        PostMinigameUI.Instance.OpenLoseScreen();
    }

    private void ResetProgress()
    {
        currentProgress = 0;
        scrollingBackground.ResetXAxis();
        hasWon = false;
        hasLost = false;
        UpdateSlider();
    }

    public override void CloseMinigame()
    {
        this.enabled = false;
        minigameUI.SetActive(false);
    }

    public override void SetupMinigame()
    {
        this.enabled = true;
        inputActions = InputManager.Instance.GetInputAction();

        inputActions.PersonCrossing.DecreaseSpeed.performed += IncreaseProgress;
        inputActions.PersonCrossing.DecreaseSpeed.canceled += IncreaseProgress;

        inputActions.PersonCrossing.Enable();
        ResetProgress();
        ShowIndicator();
    }

    public override void SetupUI()
    {
        minigameUI.SetActive(true);
    }

    public override void ShowIndicator()
    {
        base.ShowIndicator();
    }
}
