using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private RectTransform background;

    [Header("Settings")]
    [SerializeField] private float maxXPos;
    [SerializeField] private float maxYPos;
    [SerializeField] private float speed = 0.01f;
    
    private float currentXPos;
    private float currentYPos;

    public void ResetPositions()
    {
        currentXPos = 0;
        currentYPos = 0;
    }

    public void ScrollXAxis()
    {
        currentXPos -= speed;
        background.transform.localPosition = new Vector3(currentXPos, 0, 0);

        if(currentXPos < -maxXPos)
        {
            background.transform.localPosition = new Vector3(0, 0, 0);
            currentXPos = 0;
        }
    }

    public void ScrollYAxis()
    {
        currentYPos += speed;
        background.transform.localPosition = new Vector3(0, currentYPos, 0);

        if (currentYPos > maxYPos)
        {
            background.transform.localPosition = new Vector3(0, 0, 0);
            currentYPos = 0;
        }
    }
}
