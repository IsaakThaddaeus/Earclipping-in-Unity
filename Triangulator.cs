using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangulator
{

    float sum;

    public List<Vector2> polygon;
    private List<Vector2> enclosed = new List<Vector2>();
    public List<Vector2> triangles = new List<Vector2>();



    public List<int> triangulate(List<Vector2> inputPolygon)
    {
        triangles.Clear();
        polygon = new List<Vector2>(inputPolygon);

        triangulatePolygon();

        List<int> output = new List<int>();

        foreach (Vector2 i in triangles)
        {
            output.Add(inputPolygon.IndexOf(i));
        }

        return output;
    }

    public List<Vector2> triangulatePolygon()
    {

        for (int i = 0; i < polygon.Count; i++)
        {
            Vector2 x = getVectorInList(polygon, i - 1);
            Vector2 a = getVectorInList(polygon, i);
            Vector2 y = getVectorInList(polygon, i + 1);

            Vector2 ax = x - a;
            Vector2 ay = y - a;


            if (polygon.Count == 3)
            {
                triangles.Add(x);
                triangles.Add(a);
                triangles.Add(y);

                return triangles;
            }

            enclosed.Clear();
            enclosed.Add(x);
            enclosed.Add(a);
            enclosed.Add(y);

            if (cross(ax, ay) > 0 && isAnyPointOfPolygonInsideTriangle(enclosed, polygon))
            {
                triangles.Add(x);
                triangles.Add(a);
                triangles.Add(y);

                polygon.RemoveAt(i);
                return triangulatePolygon();
            }
        }

        return null;
    }
    float cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }
    bool isAnyPointOfPolygonInsideTriangle(List<Vector2> trig, List<Vector2> allVerts)
    {
        foreach (Vector2 verts in allVerts)
        {
            if (insidePolygon(verts, trig) == true)
            {
                return false;
            }
        }

        return true;
    }
    bool insidePolygon(Vector2 aPoint, List<Vector2> bVertList)
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
    Vector2 getVectorInList(List<Vector2> list, int index)
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

}
