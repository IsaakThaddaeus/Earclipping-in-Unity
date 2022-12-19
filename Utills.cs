using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utills 
{

    static float sum;

    static public bool getIntersectionV2Cramer(Vector2 aStart, Vector2 aEnd, Vector2 bStart, Vector2 bEnd, out Vector2 intersection)
    {
        intersection = new Vector2(0, 0);

        Vector2 ab = aEnd - aStart;
        Vector2 cd = bEnd - bStart;

        //no intersection because of colinearity
        if (ab.normalized == cd.normalized || ab.normalized == -cd.normalized || -ab.normalized == cd.normalized)
        {
            return false;
        }


        //Solving equation by Cramers Rule
        float a1 = ab.x;
        float b1 = -cd.x;
        float c1 = bStart.x - aStart.x;

        float a2 = ab.y;
        float b2 = -cd.y;
        float c2 = bStart.y - aStart.y;

        float D = (a1 * b2) - (b1 * a2);
        float Dx = (c1 * b2) - (b1 * c2);
        float Dy = (a1 * c2) - (c1 * a2);

        float x = Dx / D;
        float y = Dy / D;

        intersection = aStart + x * (ab);


        //is intersectionpoint on Vector?
        float siA = Vector2.Distance(aStart, intersection);
        float ieA = Vector2.Distance(intersection, aEnd);
        float seA = Vector2.Distance(aStart, aEnd);

        float siB = Vector2.Distance(bStart, intersection);
        float ieB = Vector2.Distance(intersection, bEnd);
        float seB = Vector2.Distance(bStart, bEnd);


        if (siA + ieA > seA + 0.01 || siB + ieB > seB + 0.01)
        {
            return false;
        }

        else if (Vector2.Distance(intersection, aStart) < 0.01 || Vector2.Distance(intersection, aEnd) < 0.01 || Vector2.Distance(intersection, bStart) < 0.01 || Vector2.Distance(intersection, bEnd) < 0.01)
        {
            return false;
        }

        return true;
    }
    static public bool insidePolygon(Vector2 aPoint, List<Vector2> bVertList)
    {
        sum = 0;

        for (int i = 0; i < bVertList.Count; i++)
        {
            Vector2 xA;
            Vector2 xB;

            if (i + 1 != bVertList.Count)
            {
                xA = bVertList[i] - aPoint;
                xB = bVertList[i + 1] - aPoint;
            }
            else
            {
                xA = bVertList[i] - aPoint;
                xB = bVertList[0] - aPoint;
            }

            sum += Vector2.SignedAngle(xA, xB);
        }


        if (sum < -359 && sum > -361)
        {
            return true;
        }

        else
        {
            return false;
        }

    }
    static public bool polygonsIntersecting(List<Vector2> polygonA, List<Vector2> polygonB)
    {
        for (int i = 0; i < polygonA.Count; i++)
        {
            for (int j = 0; j < polygonB.Count; j++)
            {

                if (getIntersectionV2Cramer(polygonA[i], getItem<Vector2>(polygonA,i+1), polygonB[j], getItem<Vector2>(polygonB, j + 1), out Vector2 inter) == true)
                {
                    return true;
                }
            }
        }

        return false;
    }

    static public float cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    public static T getItem<T>(List<T> list, int index)
    {
        if (index < 0)
        {
            return list[list.Count - 1];
        }

        else if (index >= list.Count)
        {
            return list[0];
        }

        else
        {
            return list[index];
        }
    }
    public static T getItemPlus<T>(List<T> list, int index)
    {
        if (index >= list.Count)
        {
            return list[index - list.Count];
        }

        else
        {
            return list[index];
        }
    }


}

