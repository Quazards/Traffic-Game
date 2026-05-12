using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] ScrollingBackground scrollingBackground;

    private void Update()
    {
        scrollingBackground.ScrollXAxis();
    }
}
