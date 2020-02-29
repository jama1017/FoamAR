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

    public static GameObject rankFocusedObjects(List<GameObject> focusedObjects, Vector3 camPosition)
    {

        if (focusedObjects.Count == 0)
        {
            return null;
        }

        GameObject num_one = null;
        float min_dis = 9999999f;

        for (int i = 0; i < focusedObjects.Count; i++)
        {
            float dis = Vector3.Distance(focusedObjects[i].transform.position, camPosition);
            if (dis < min_dis)
            {
                min_dis = dis;
                num_one = focusedObjects[i];
            }
        }

        return num_one;
    }

    public static void UpdateLinePos(LineRenderer line, Collider other, GameObject ActivePalm)
    {
        line.SetPosition(0, ActivePalm.transform.position);
        line.SetPosition(1, other.gameObject.transform.position);
    }
}
