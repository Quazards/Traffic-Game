using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopSpeedMinigame : MinigameBase
{
    private InputSystem_Actions inputActions;

    [Header("References")]
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private ScrollingBackground scrollingBackground;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI[] topSpeedSigns;
    [SerializeField] private TextMeshProUGUI[] minSpeedSigns;

    [Header("Settings")]
    [SerializeField] private float startingSpeed = 50;
    [SerializeField] private float minSpeedThreshold = 60;
    [SerializeField] private float maxSpeedThreshold = 100;
    [SerializeField] private float speedIncrease;

    private float currentSpeed;
    private float speedModifier;


    private void OnEnable()
    {
        PostMinigameUI.OnPostGameTimerEnd += ResetMinigame;
        PostMinigameUI.OnPostGameHalfWay += CloseMinigame;
    }

    private void OnDisable()
    {
        PostMinigameUI.OnPostGameTimerEnd -= ResetMinigame;
        inputActions.TopSpeed.IncreaseSpeed.performed -= ChangeSpeedMod;
        inputActions.TopSpeed.IncreaseSpeed.canceled -= ChangeSpeedMod;
        GameManager.OnTimerFinish -= CheckResults;
        PostMinigameUI.OnPostGameHalfWay -= CloseMinigame;
        inputActions.TopSpeed.Disable();
    }

    private void Update()
    {
        UpdateSpeedText();
    }

    private void FixedUpdate()
    {
        ChangeSpeedOverTime();
        scrollingBackground.ScrollXAxis();
    }

    private void ChangeSpeedOverTime()
    {
        currentSpeed += (-1 + (speedModifier * speedIncrease)) * Time.deltaTime;
    }

    private void ChangeSpeedMod(InputAction.CallbackContext context)
    {
        speedModifier = context.ReadValue<float>();
    }

    private void UpdateSpeedText()
    {
        speedText.text = currentSpeed.ToString("F0") + "km/J";
    }

    private void CheckResults()
    {
        if(currentSpeed <= maxSpeedThreshold && currentSpeed >= minSpeedThreshold)
        {
            MinigameWin();
            PostMinigameUI.Instance.OpenWinScreen();
        }
        else
        {
            MinigameLose();
            PostMinigameUI.Instance.OpenLoseScreen();
        }
    }

    private void ResetMinigame()
    {
        currentSpeed = Random.Range(45, 55);
    }

    private void RandomizeThreshold()
    {
        minSpeedThreshold = Random.Range(50, 55);
        maxSpeedThreshold = Random.Range(60, 65);

        for(int i = 0; i < topSpeedSigns.Length; i++)
        {
            topSpeedSigns[i].text = maxSpeedThreshold.ToString();
        }

        for(int i = 0; i < minSpeedSigns.Length; i++)
        {
            minSpeedSigns[i].text = minSpeedThreshold.ToString();
        }
    }

    public override void SetupMinigame()
    {
        this.enabled = true;
        inputActions = InputManager.Instance.GetInputAction();

        inputActions.TopSpeed.IncreaseSpeed.performed += ChangeSpeedMod;
        inputActions.TopSpeed.IncreaseSpeed.canceled += ChangeSpeedMod;
        GameManager.OnTimerFinish += CheckResults;

        inputActions.TopSpeed.Enable();

        currentSpeed = startingSpeed;
        ShowIndicator();
        RandomizeThreshold();
    }

    public override void SetupUI()
    {
        minigameUI.SetActive(true);
    }

    public override void CloseMinigame()
    {
        this.enabled = false;
        minigameUI.SetActive(false);
    }

    public override void ShowIndicator()
    {
        base.ShowIndicator();
    }
}
