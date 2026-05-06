using UnityEngine;
using UnityEngine.InputSystem;

public class FollowDirectionMinigame : MinigameBase
{
    private InputSystem_Actions inputActions;

    [Header("References")]
    private GameObject[] directions;
    private GameObject minigameUI;

    private bool isRight = false;

    private void OnEnable()
    {
        PostMinigameUI.OnPostGameHalfWay += CloseMinigame;
    }

    private void OnDisable()
    {
        inputActions.FollowDirection.TurnRight.performed -= TurnRight;
        inputActions.FollowDirection.TurnLeft.performed -= TurnLeft;
        inputActions.FollowDirection.Disable();

        PostMinigameUI.OnPostGameHalfWay -= CloseMinigame;
    }

    private void RandomizeDirection()
    {
        int random = Random.Range(0, directions.Length);

        for(int i = 0; i < directions.Length; i++)
        {
            directions[random].SetActive(true);
            CheckCurrentDirection(random);
        }
    }

    private void CheckCurrentDirection(int direction)
    {
        switch(direction)
        {
            case 0:
                isRight = true; 
                break;
            case 1:
                isRight = true;
                break;
            case 2:
                isRight = false;
                break;
            case 3:
                isRight = false;
                break;
        }

        Debug.Log($"current direction is right: {isRight}");
    }

    private void TurnRight(InputAction.CallbackContext context)
    {
        if(isRight)
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
        for (int i = 0; i < directions.Length; i++)
        {
            directions[i].SetActive(false);
        }

        minigameUI.SetActive(false);
        this.enabled = false;
    }

    public override void SetupMinigame()
    {
        this.enabled = true;
        inputActions = InputManager.Instance.GetInputAction();

        inputActions.FollowDirection.TurnRight.performed += TurnRight;
        inputActions.FollowDirection.TurnLeft.performed += TurnLeft;
        inputActions.FollowDirection.Enable();

        RandomizeDirection();


    }

    public override void SetupUI()
    {
        minigameUI.SetActive(true);
    }
}
