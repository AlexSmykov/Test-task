using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Bezie 
{
    public static Vector3 GetCords(List<Vector3> dots, float t)
    {
        if (dots.Count > 2)
        {
            List<Vector3> nextDots = new List<Vector3>();

            for (int i = 1; i < dots.Count; i++)
            {
                nextDots.Add(Vector3.Lerp(dots[i - 1], dots[i], t));
            }

            return GetCords(nextDots, t);
        }
        else
        {
            return Vector3.Lerp(dots[0], dots[1], t);
        }
    }
}
