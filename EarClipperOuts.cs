using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarClipperOuts
{
    static public void triangulate(List<Vector2> inputPolygon, out List<Vector2> allVerts, out List<int> trianglesOut)
    {
        List<Vector2> triangles = new List<Vector2>();
        List<Vector2> polygon = new List<Vector2>(inputPolygon);
        List<int> output = new List<int>();

        triangulatePolygon(polygon, triangles);

        foreach (Vector2 i in triangles)
        {
            output.Add(inputPolygon.IndexOf(i));
        }

        allVerts = new List<Vector2>(inputPolygon);
        trianglesOut = new List<int>(output);
    }
    static public void triangulateWithHole(List<Vector2> inputPolygon, List<List<Vector2>> inputInnerPolygons, out List<Vector2> allVerts, out List<int> trianglesOut)
    {
        List<Vector2> triangles = new List<Vector2>();
        List<Vector2> polygon = new List<Vector2>(inputPolygon);
        List<List<Vector2>> innerPolygons = new List<List<Vector2>>(inputInnerPolygons);
        List<int> output = new List<int>();

        List<Vector2> wholePolygon = getWholePolygon(polygon, inputInnerPolygons);

        makeBridges(polygon, innerPolygons);
        triangulatePolygon(polygon, triangles);


        foreach (Vector2 i in triangles)
        {
            output.Add(wholePolygon.IndexOf(i));
        }

        allVerts = new List<Vector2>(wholePolygon);
        trianglesOut = new List<int>(output);
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



    //Specific to polygons with n-holes:  one to infinite holes
    static bool makeBridges(List<Vector2> outerPolygon, List<List<Vector2>> innerPolygons)
    {
        for(int i = 0; i < innerPolygons.Count; i ++)
        {
            for(int j = 0; j < innerPolygons[i].Count; j++)
            {
                for(int k = 0; k < outerPolygon.Count; k ++)
                {

                    Vector2 outerBefore = Utills.getItem<Vector2>(outerPolygon, k-1) - outerPolygon[k];
                    Vector2 outerNext = innerPolygons[i][j] - outerPolygon[k];

                    Vector2 afterBefore = innerPolygons[i][j] - outerPolygon[k];
                    Vector2 afterNext   = Utills.getItem<Vector2>(outerPolygon, k + 1) - outerPolygon[k];


                    if (Utills.cross(outerBefore, outerNext) > 0 && Utills.cross(afterBefore, afterNext) > 0 && intersectsWithAnyVectors(outerPolygon[k], innerPolygons[i][j], outerPolygon, innerPolygons))
                    {
                        Debug.Log("Bridge from outer " + outerPolygon[k] + " To inner " + innerPolygons[i][j]);

                        insert(j, k, innerPolygons[i], outerPolygon);
                        innerPolygons.RemoveAt(i);

                        return makeBridges(outerPolygon, innerPolygons);
                    }
                }
            }
        }

        return false;
    }
    static void insert(int innerIndex, int outerIndex, List<Vector2> innerPolygon, List<Vector2> outerPolygon)
    {

        if (outerIndex + 1 >= outerPolygon.Count)
        {
            outerPolygon.Add(outerPolygon[outerIndex]);
        }

        else
        {
            outerPolygon.Insert(outerIndex + 1, outerPolygon[outerIndex]);
        }


        for (int i = 0; i < innerPolygon.Count + 1; i++)
        {
            outerPolygon.Insert(outerIndex + 1, Utills.getItemPlus<Vector2>(innerPolygon, innerIndex + i));
        }
    }
    static bool intersectsWithAnyVectors(Vector2 aStart, Vector2 aEnd, List<Vector2> outerPolygon, List<List<Vector2>> innerPolygons)
    {
        for (int i = 0; i < outerPolygon.Count; i++)
        {
            Vector2 bStart = Utills.getItem<Vector2>(outerPolygon, i);
            Vector2 bEnd = Utills.getItem<Vector2>(outerPolygon, i - 1);

            if (Utills.getIntersectionV2Cramer(aStart, aEnd, bStart, bEnd, out Vector2 inter) == true)
            {
                return false;
            }
        }

        foreach(List<Vector2> innerPolygon in innerPolygons)
        {
            for (int i = 0; i < innerPolygon.Count; i++)
            {
                Vector2 bStart = Utills.getItem<Vector2>(innerPolygon, i);
                Vector2 bEnd = Utills.getItem<Vector2>(innerPolygon, i - 1);

                if (Utills.getIntersectionV2Cramer(aStart, aEnd, bStart, bEnd, out Vector2 inter) == true)
                {
                    return false;
                }
            }
        }


        return true;
    }


    static List<Vector2> getWholePolygon(List<Vector2> polygon, List<List<Vector2>> inputInnerPolygons)
    {
        List<Vector2> wholePolygon = new List<Vector2>();
        wholePolygon.AddRange(polygon);
        foreach (List<Vector2> list in inputInnerPolygons)
        {
            wholePolygon.AddRange(list);
        }

        return wholePolygon;
    }
}
