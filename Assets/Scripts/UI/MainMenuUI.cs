using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] ScrollingBackground scrollingBackground;

    private void FixedUpdate()
    {
        scrollingBackground.ScrollXAxis();
    }
}
