using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler
{
    [SerializeField] private GameObject dragObject;
    private ICustomDrag onDrag;

    private void Start()
    {
        onDrag = dragObject.GetComponent<ICustomDrag>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrag.OnCurrentDrag();
    }
}
