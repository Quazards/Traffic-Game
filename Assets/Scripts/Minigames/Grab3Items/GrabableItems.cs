using UnityEngine;

public class GrabableItems : MonoBehaviour, ICustomDrag
{
    [SerializeField] private RectTransform destination;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnCurrentDrag()
    {
        rectTransform.position = Input.mousePosition;

        if(RectHelp.IsOverlapping(rectTransform, destination))
        {
            //Debug.Log("overlapping");
            Grab3ItemsMinigame.OnItemOverlap?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
