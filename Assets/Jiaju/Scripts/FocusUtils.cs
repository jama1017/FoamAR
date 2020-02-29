using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FocusUtils
{
    public static Vector3 worldToScreenSpace(Vector3 worldPos)
    {
        return Camera.main.WorldToScreenPoint(worldPos);
    }


    public static Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, null, out movePos);

        //Convert the local point to world point
        //return parentCanvas.transform.TransformPoint(movePos);
        return movePos;
    }

    public static Vector3 calcFocusCenter(List<Vector3> markers)
    {
        if (markers.Count == 0)
        {
            return Vector3.zero;
        }

        float x_sum = 0f;
        float y_sum = 0f;

        for (int i = 0; i < markers.Count; i++)
        {
            x_sum += markers[i].x;
            y_sum += markers[i].y;
        }

        return new Vector3(x_sum / markers.Count, y_sum / markers.Count, 0f);
    }
}
