using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FocusUtils
{
    public static Vector3 WorldToScreenSpace(Vector3 worldPos)
    {
        return Camera.main.WorldToScreenPoint(worldPos);
    }


    public static Vector3 WorldToUISpace(Canvas parentCanvas, Vector3 worldPos)
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

    public static Vector3 CalcFocusCenter(List<Vector3> markers)
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

    public static GameObject RankFocusedObjects(List<GameObject> focusedObjects, Vector3 cylinderPosition)
    {

        if (focusedObjects.Count == 0)
        {
            return null;
        }

        GameObject num_one = null;
        float min_dis = 9999999f;

        for (int i = 0; i < focusedObjects.Count; i++)
        {
            float dis = Vector3.Distance(WorldToScreenSpace(focusedObjects[i].transform.position), WorldToScreenSpace(cylinderPosition));

            if (dis < min_dis)
            {
                min_dis = dis;
                num_one = focusedObjects[i];
            }
        }

        return num_one;
    }

    public static GameObject RankFocusedObjects(List<GameObject> focusedObjects, Vector3 cylinderPosition, Canvas canvas)
    {

        if (focusedObjects.Count == 0)
        {
            return null;
        }

        GameObject num_one = null;
        float min_dis = 9999999f;

        for (int i = 0; i < focusedObjects.Count; i++)
        {
            float dis = Vector3.Distance(WorldToUISpace(canvas, focusedObjects[i].transform.position), WorldToUISpace(canvas, cylinderPosition));

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

    public static void ToggleTimeStamp(bool isStart)
    {
        if (Jetfire.IsConnected2())
        {
            string message = "";

            if (isStart)
            {
                message += "Time start,";
            }
            else
            {
                message += "Time end,";
            }

            message += AddTimeStamp();


            Jetfire.SendMsg2(message);
            Debug.Log("JETFIRE start/end");
        }

    }

    public static string AddTimeStamp()
    {
        return System.DateTime.Now.ToString("MM / dd / yyyy hh: mm: ss") + " , " + System.DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
    }

    public static float LinearMapReverse(float input, float ogMin, float ogMax, float tarMin, float tarMax)
    {
        float t = Mathf.Abs(ogMax - input) / Mathf.Abs(ogMax - ogMin);
        return Mathf.Lerp(tarMin, tarMax, t);
    }

    public static void UpdateMaterialAlpha(Renderer renderer, float alpha)
    {
        Color currColor = renderer.material.color;
        renderer.material.color = new Vector4(currColor.r, currColor.g, currColor.b, alpha);
    }
}
