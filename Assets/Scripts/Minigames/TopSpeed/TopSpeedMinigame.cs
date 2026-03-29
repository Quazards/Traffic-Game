using UnityEngine;
using UnityEngine.InputSystem;

public class TopSpeedMinigame : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    private float currentSpeed;
    private float speedModifier;

    private void Start()
    {

        SetUpMinigame();
    }

    private void Update()
    {
        ChangeSpeedOverTime();
    }

    private void ChangeSpeedOverTime()
    {
        currentSpeed += (-1 + (speedModifier * 2)) * Time.deltaTime;

        Debug.Log($"current speed: {currentSpeed}, speed modifier: {speedModifier}");
    }

    private void ChangeSpeedMod(InputAction.CallbackContext context)
    {
        speedModifier = context.ReadValue<float>();
    }

    private void SetUpMinigame()
    {
        inputActions = InputManager.Instance.GetInputAction();

        inputActions.TopSpeed.IncreaseSpeed.performed += ChangeSpeedMod;
        inputActions.TopSpeed.IncreaseSpeed.canceled += ChangeSpeedMod;

        inputActions.TopSpeed.Enable();
    }
}
