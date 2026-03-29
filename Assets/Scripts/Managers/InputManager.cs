using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private InputSystem_Actions action;

    private void Awake()
    {
        Instance = this;

        action = new InputSystem_Actions();
    }

    public InputSystem_Actions GetInputAction()
    {
        return action;
    }
}
