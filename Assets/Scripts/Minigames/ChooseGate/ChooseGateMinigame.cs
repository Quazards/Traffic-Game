using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChooseGateMinigame : MinigameBase
{
    private InputSystem_Actions inputActions;

    [Header("References")]
    [SerializeField] private GameObject minigameUI;
    [SerializeField] private TextMeshProUGUI leftSign;
    [SerializeField] private TextMeshProUGUI rightSign;

    private bool isRight = false;

    private void OnEnable()
    {
        PostMinigameUI.OnPostGameHalfWay += CloseMinigame;
    }

    private void OnDisable()
    {
        inputActions.ChooseGate.TurnRight.performed -= TurnRight;
        inputActions.ChooseGate.TurnLeft.performed -= TurnLeft;
        inputActions.ChooseGate.Disable();

        PostMinigameUI.OnPostGameHalfWay -= CloseMinigame;
    }

    private void RandomizeGate()
    {
        int random = Random.Range(0, 2);

        switch(random)
        {
            case 0:
                isRight = true;
                rightSign.text = "2.5 M";
                leftSign.text = "1.5 M";
                break;
            case 1:
                isRight = false;
                rightSign.text = "1.5 M";
                leftSign.text = "2.5 M";
                break;
        }
    }

    private void TurnRight(InputAction.CallbackContext context)
    {
        if (isRight)
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

    private void TurnLeft(InputAction.CallbackContext context)
    {
        if (!isRight)
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

    public override void CloseMinigame()
    {
        this.enabled = false;
        minigameUI.SetActive(false);
    }

    public override void SetupMinigame()
    {
        this.enabled = true;
        inputActions = InputManager.Instance.GetInputAction();

        inputActions.ChooseGate.TurnRight.performed += TurnRight;
        inputActions.ChooseGate.TurnLeft.performed += TurnLeft;
        inputActions.ChooseGate.Enable();

        RandomizeGate();
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
