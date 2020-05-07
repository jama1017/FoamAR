using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FocusUtils
{

    public static readonly Vector3 NullVector3 = new Vector3(-99999f, -99999f, -99999f);

    /// <summary>
    /// Constants
    /// </summary>
    // Depth cue
    public static readonly float NearHandDis = 0.11f; // to be adjusted based on user preference.
    public static readonly float FarHandDis = 0.21f;  // to be adjusted based on user preference.
    public static readonly float FarAlpha = 0.2f;
    public static readonly float NearAlpha = 1.0f;

    // Object colors for user task and selection aid
    public static readonly Color ObjNormalColor = new Color(150f / 255f, 100f / 255f, 0f, FarAlpha);
    public static readonly Color ObjFocusedColor = new Color(1f, 200f / 255f, 0f, FarAlpha);
    public static readonly Color ObjTargetColor = new Color(18f / 255f, 20f / 255f, 125f / 255f, FarAlpha);
    public static readonly Color ObjTargetFocusedColor = new Color(85f / 255f, 180f / 255f, 1f, FarAlpha);

    //public static readonly Color ObjRankedColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
    public static readonly Color ObjRankedColor = new Color(0.9f, 0.0f, 0.0f, 1.0f);

    public static readonly Vector3 SmallObjSnapExpandedScale = new Vector3(0.06f / 0.01f, 0.06f / 0.007f, 0.06f / 0.01f);


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


    public static Vector3 CalcFocusCenter(Queue<KeyValuePair<Vector2, long>> markers)
    {
        if (markers.Count == 0)
        {
            return Vector3.zero;
        }

        float x_num_sum = 0f;
        float y_num_sum = 0f;
        float weight_sum = 0f;

        IEnumerator<KeyValuePair<Vector2, long>> marker_enum = markers.GetEnumerator();

        //Debug.Log("QUEUEE start ------ ");

        while (marker_enum.MoveNext())
        {
            float weight = (marker_enum.Current.Value - markers.Peek().Value) + 1.0f;

            weight_sum += weight;

            x_num_sum += marker_enum.Current.Key.x * weight;
            y_num_sum += marker_enum.Current.Key.y * weight;

            //Debug.Log("QUEUEE offset : " + smallest_offset.ToString("F10"));
            //Debug.Log("QUEUEE weight : " + weight.ToString());
            //Debug.Log("QUEUEE key : " + marker_enum.Current.Key.ToString());
        }

        //Debug.Log("QUEUEE res: " + x_num_sum / weight_sum + " , " + y_num_sum / weight_sum);
        //Debug.Log("QUEUEE end ------ ");

        return new Vector3(x_num_sum / weight_sum, y_num_sum / weight_sum, 0f);
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


    public static GameObject RankFocusedObjects(List<GameObject> focusedObjects, Vector3 cylinderPosition, SelectionDataManager sDM, Canvas canvas)
    {

        if (focusedObjects.Count == 0)
        {
            return null;
        }

        GameObject num_one = null;
        float max_score = -9999999f;

        for (int i = 0; i < focusedObjects.Count; i++)
        {
            Vector3 objPos = focusedObjects[i].transform.position;

            float objCylinderDis = Vector3.Distance(WorldToUISpace(canvas, objPos), WorldToUISpace(canvas, cylinderPosition));
            float objHandDis = 0.0f;

            Vector3 handPos = GetIndexThumbPos(sDM);
            if (handPos != NullVector3) objHandDis = Vector3.Distance(objPos, handPos);

            float objCylPortion = (1.0f / objCylinderDis) * 20000f;
            float objHandPortion = 1.0f / Mathf.Pow(objHandDis, 3.0f);

            float score = objCylPortion + objHandPortion;
            //Debug.Log("DATAA cy obj score: " + objCylPortion + " , " + objHandPortion + " , " + score);

            if (score > max_score)
            {
                max_score = score;
                num_one = focusedObjects[i];
            }
        }

        //Debug.Log("DATAA -----------");

        return num_one;
    }


    //public static void UpdateLinePos(LineRenderer line, Collider other, GameObject ActivePalm)
    //{
    //    line.SetPosition(0, ActivePalm.transform.position);
    //    line.SetPosition(1, other.gameObject.transform.position);
    //}


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
            //Debug.Log("JETFIRE start/end");
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


    public static Vector3 GetIndexThumbPos(SelectionDataManager sDM)
    {
        if (!sDM.ActiveHand) return NullVector3;

        Vector3 indexPos = sDM.ActiveIndex.transform.position;
        Vector3 thumbPos = sDM.ActiveThumb.transform.position;

        float factor = 0.5f;

        float x = (indexPos.x + thumbPos.x) * factor;
        float y = (indexPos.y + thumbPos.y) * factor;
        float z = (indexPos.z + thumbPos.z) * factor;

        return new Vector3(x, y, z);
    }
}
