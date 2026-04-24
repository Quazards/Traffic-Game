using UnityEngine;

public class RectHelp : MonoBehaviour
{
    public static bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        if(rect1 == null || rect2 == null) return false;

        Vector3[] corners1 = new Vector3[4];
        Vector3[] corners2 = new Vector3[4];

        rect1.GetWorldCorners(corners1);
        rect2.GetWorldCorners(corners2);

        Rect rectA = new Rect(corners1[0], corners1[2] - corners1[0]);
        Rect rectB = new Rect(corners2[0], corners2[2] - corners2[0]);

        return rectA.Overlaps(rectB);
    }
}
