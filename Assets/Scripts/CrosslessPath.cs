using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosslessPath : MonoBehaviour
{
    public static List<Vector3> CreatePath(int count)
    {
        List<Vector3> path = new List<Vector3>();

        for (int i = 0; i < count; i++)
        {
            int k = 0;
            while (k < 100)
            {
                k++;
                Vector3 dot = new Vector3(Random.Range(-100, 100) * Random.value, 0, Random.Range(-100, 100) * Random.value);

                bool cross = false;

                for(int j = 1; j < path.Count - 1; j++)
                {
                    if (linesIntersect(dot, path[path.Count - 1], path[j - 1], path[j]))
                    {
                        cross = true;
                        break;
                    }
                }

                if (!cross)
                {
                    path.Add(dot);
                    break;
                }
            }
        }

        return path;
    }

    public static bool linesIntersect(Vector3 first, Vector3 second, Vector3 third, Vector3 fourth)
    {
        if (fromDifferentSides(first, second, first, third, first, fourth))
        {
            return fromDifferentSides(third, fourth, third, first, third, second);
        }
        return false;
    }

    public static bool fromDifferentSides(
        Vector3 mainStart, Vector3 mainEnd, 
        Vector3 firstStart, Vector3 firstEnd, 
        Vector3 secondStart, Vector3 secondEnd)
    {
        float areaFirst = VectorArea(mainStart, mainEnd, firstStart, firstEnd);
        float areaSecond = VectorArea(mainStart, mainEnd, secondStart, secondEnd);

        return (areaFirst >= 0 && areaSecond <= 0 || areaFirst <= 0 && areaSecond >= 0);
    }

    private static float VectorArea(
        Vector3 firstStart, Vector3 firstEnd, 
        Vector3 secondStart, Vector3 secondEnd)
    {
        return (firstEnd.x - firstStart.x) * (secondEnd.z - secondStart.z) - (firstEnd.z - firstStart.z) * (secondEnd.x - secondStart.x);
    }
}
