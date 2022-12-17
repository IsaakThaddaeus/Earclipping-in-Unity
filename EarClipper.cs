using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarClipper
{
    static public List<int> triangulate(List<Vector2> inputPolygon)
    {
        List<Vector2> triangles = new List<Vector2>();
        List<Vector2> polygon = new List<Vector2>(inputPolygon);
        List<int> output = new List<int>();

        triangulatePolygon(polygon, triangles);

        foreach (Vector2 i in triangles)
        {
            output.Add(inputPolygon.IndexOf(i));
        }

        return output;
    }
    static List<Vector2> triangulatePolygon(List<Vector2> polygon, List<Vector2> triangles)
    {
        List<Vector2> enclosed = new List<Vector2>();

        for (int i = 0; i < polygon.Count; i++)
        {
            Vector2 x = Utills.getItem<Vector2>(polygon, i - 1);
            Vector2 a = Utills.getItem<Vector2>(polygon, i);
            Vector2 y = Utills.getItem<Vector2>(polygon, i + 1);

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

            if (Utills.cross(ax, ay) > 0 && isAnyPointOfPolygonInsideTriangle(enclosed, polygon))
            {
                triangles.Add(x);
                triangles.Add(a);
                triangles.Add(y);

                polygon.RemoveAt(i);
                return triangulatePolygon(polygon, triangles);
            }
        }

        return null;
    }

    static bool isAnyPointOfPolygonInsideTriangle(List<Vector2> trig, List<Vector2> allVerts)
    {
        foreach (Vector2 verts in allVerts)
        {
            if (Utills.insidePolygon(verts, trig) == true)
            {
                return false;
            }
        }

        return true;
    }

}
