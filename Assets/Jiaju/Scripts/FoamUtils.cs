using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuRegion
{
    UPPER,
    RIGHT,
    LOWER,
    LEFT,
    MIDDLE
}

public enum CreateMenuItem
{
    CUBE,
    CYLINDER,
    CONE,
    SPHERE,
    NULL
}

public enum ManiMenuItem
{
    MOVE,
    SCALE,
    ONE,
    TWO,
    NULL
}

public static class FoamUtils
{
    public static readonly Color ObjManiSelectedColor = new Color(247f/255f, 238f/255f, 144f/255f);
    public static readonly Color IconNormalColor = new Color(1f, 1f, 1f, 0.78f);

    public static readonly float ObjCreatedOffset = 0.13f;
    public static readonly int ObjCreatedAnimTime = 40;

    public static bool IsGlobalGrabbing = false;

    public static bool isInsideTri(Vector3 s, Vector3 a, Vector3 b, Vector3 c)
	{
		float as_x = s.x - a.x;
		float as_y = s.y - a.y;

		bool s_ab = (b.x - a.x) * as_y - (b.y - a.y) * as_x > 0;

		if ((c.x - a.x) * as_y - (c.y - a.y) * as_x > 0 == s_ab) return false;
		if ((c.x - b.x) * (s.y - b.y) - (c.y - b.y) * (s.x - b.x) > 0 != s_ab) return false;

		return true;
	}

    public static MenuRegion checkMenuRegion(Vector3 currPos, Vector3 initPos, Vector3 UppL, Vector3 UppR, Vector3 LowL, Vector3 LowR, float middleRadius)
	{
        if (Vector3.Distance(currPos, initPos) > middleRadius)
        {
            if (isInsideTri(currPos, UppL, UppR, initPos))
            {
                //Debug.Log("FOAMFILTER INSIDE UPPER TRI");
                return MenuRegion.UPPER;


                // right tri
            }
            else if (isInsideTri(currPos, UppR, LowR, initPos))
            {
                //Debug.Log("FOAMFILTER INSIDE RIGHT TRI");
                return MenuRegion.RIGHT;


                // lower tri
            }
            else if (isInsideTri(currPos, LowR, LowL, initPos))
            {
                //Debug.Log("FOAMFILTER INSIDE LOWER TRI");
                return MenuRegion.LOWER;


                // left tri
            }
            else
            {
                //Debug.Log("FOAMFILTER INSIDE LEFT TRI");
                return MenuRegion.LEFT;
            }
        }

        return MenuRegion.MIDDLE;
    }


    public static float LinearMap(float input, float ogMin, float ogMax, float tarMin, float tarMax)
    {
        float t = Mathf.Abs(input-ogMin) / Mathf.Abs(ogMax - ogMin);
        return Mathf.Lerp(tarMin, tarMax, t);
    }


    public static float LinearMapReverse(float input, float ogMin, float ogMax, float tarMin, float tarMax)
    {
        float t = Mathf.Abs(ogMax - input) / Mathf.Abs(ogMax - ogMin);
        return Mathf.Lerp(tarMin, tarMax, t);
    }


    public static float SinWave(int step)
    {
        float angle = (2f * Mathf.PI / 45f) * (float)step;
        return LinearMap(Mathf.Sin(angle), -1f, 1f, 0.5f, 1.0f);
    }


    public static int AnimateWaveTransparency(Renderer primRenderer, int transStep)
    {
        Color curC = primRenderer.material.color;
        float newA = SinWave(transStep);
        primRenderer.material.color = new Color(curC.r, curC.g, curC.b, newA);
        return transStep + 1;
    }

    public static int AnimateGrowSize(int animCount, Vector3 initalScale, Transform prim, Vector3 ObjCreatedPos)
    {
        float newX = FoamUtils.LinearMap(animCount, 0, FoamUtils.ObjCreatedAnimTime, 0.0f, initalScale.x);
        float newY = FoamUtils.LinearMap(animCount, 0, FoamUtils.ObjCreatedAnimTime, 0.0f, initalScale.y);
        float newZ = FoamUtils.LinearMap(animCount, 0, FoamUtils.ObjCreatedAnimTime, 0.0f, initalScale.z);

        prim.localScale = new Vector3(newX, newY, newZ);
        prim.position = ObjCreatedPos;
        return animCount + 1;
    }

 }
