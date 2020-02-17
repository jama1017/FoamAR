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
    SPHERE,
    CONE,
    CYLINDER,
    NULL
}

public enum ManiMenuItem
{
    MOVE,
    SCALE,
    ROTATE,
    COLOR,
    NULL
}

public static class FoamUtils
{
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
}
