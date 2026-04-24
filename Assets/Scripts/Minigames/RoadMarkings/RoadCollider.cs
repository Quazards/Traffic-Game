using UnityEngine;

public class RoadCollider : MonoBehaviour
{
    [SerializeField] private RectTransform targetCar;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        RoadMarkingsMinigame.OnMoving += Collide;
    }

    private void OnDisable()
    {
        RoadMarkingsMinigame.OnMoving -= Collide;
    }

    private void Collide()
    {
        if(RectHelp.IsOverlapping(rectTransform, targetCar))
        {
            RoadMarkingsMinigame.OnCollision?.Invoke();
            Debug.Log("has triggered collision");
        }
    }
}
