using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandomPointInPolygon : MonoBehaviour
{
    private PolygonCollider2D polygonCollider;
    private List<float> cumulativeAreas;
    private List<Triangle> triangles;

    void Awake()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();

        if (polygonCollider == null)
        {
            Debug.LogError("No se encontró un PolygonCollider2D en el GameObject.");
            return;
        }

        if (polygonCollider.points == null || polygonCollider.points.Length < 3)
        {
            Debug.LogError("El PolygonCollider2D no tiene suficientes puntos para formar un polígono.");
            return;
        }

        TriangulatePolygon();
    }

    // Genera un punto aleatorio dentro del polígono
    public Vector2 GetRandomPoint()
    {
        if (triangles == null || triangles.Count == 0)
        {
            Debug.LogError("El polígono no ha sido triangulado correctamente.");
            return Vector2.zero;
        }

        // Seleccionar un triángulo al azar, ponderado por el área
        float randomSample = Random.value * cumulativeAreas[cumulativeAreas.Count - 1];
        int triangleIndex = cumulativeAreas.BinarySearch(randomSample);
        if (triangleIndex < 0)
        {
            triangleIndex = ~triangleIndex;
        }

        Triangle selectedTriangle = triangles[triangleIndex];

        // Generar un punto aleatorio dentro del triángulo seleccionado
        Vector2 randomPoint = RandomPointInTriangle(selectedTriangle.p1, selectedTriangle.p2, selectedTriangle.p3);
        return randomPoint;
    }


    // Triangula el polígono y calcula las áreas acumulativas de los triángulos
    private void TriangulatePolygon()
    {
        Vector2[] points = polygonCollider.points;
        int[] indices = Triangulate(points);

        triangles = new List<Triangle>();
        cumulativeAreas = new List<float>();
        float totalArea = 0f;

        for (int i = 0; i < indices.Length; i += 3)
        {
            Vector2 p1 = transform.TransformPoint(points[indices[i]]);
            Vector2 p2 = transform.TransformPoint(points[indices[i + 1]]);
            Vector2 p3 = transform.TransformPoint(points[indices[i + 2]]);

            Triangle triangle = new Triangle(p1, p2, p3);
            triangles.Add(triangle);

            float area = Mathf.Abs(Vector3.Cross(p2 - p1, p3 - p1).z) / 2f;
            totalArea += area;
            cumulativeAreas.Add(totalArea);
        }
    }

    // Método para generar un punto aleatorio dentro de un triángulo
    private Vector2 RandomPointInTriangle(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float r1 = Random.value;
        float r2 = Random.value;

        // Asegurar que el punto esté dentro del triángulo
        if (r1 + r2 >= 1)
        {
            r1 = 1 - r1;
            r2 = 1 - r2;
        }

        Vector2 randomPoint = p1 + r1 * (p2 - p1) + r2 * (p3 - p1);
        return randomPoint;
    }

    // Estructura para representar un triángulo
    private struct Triangle
    {
        public Vector2 p1;
        public Vector2 p2;
        public Vector2 p3;

        public Triangle(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            p1 = point1;
            p2 = point2;
            p3 = point3;
        }
    }

    // Método para triangular el polígono (algoritmo Ear Clipping)
    private int[] Triangulate(Vector2[] vertices)
    {
        List<int> indices = new List<int>();

        int n = vertices.Length;
        if (n < 3)
            return indices.ToArray();

        int[] V = new int[n];
        if (Area(vertices) > 0)
        {
            for (int v = 0; v < n; v++)
                V[v] = v;
        }
        else
        {
            for (int v = 0; v < n; v++)
                V[v] = (n - 1) - v;
        }

        int nv = n;
        int count = 2 * nv;
        for (int m = 0, v = nv - 1; nv > 2;)
        {
            if ((count--) <= 0)
                break;

            int u = v;
            if (nv <= u)
                u = 0;
            v = u + 1;
            if (nv <= v)
                v = 0;
            int w = v + 1;
            if (nv <= w)
                w = 0;

            if (Snip(vertices, u, v, w, nv, V))
            {
                int a = V[u];
                int b = V[v];
                int c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);

                for (int s = v, t = v + 1; t < nv; s++, t++)
                    V[s] = V[t];
                nv--;
                count = 2 * nv;
            }
        }

        return indices.ToArray();
    }

    private float Area(Vector2[] vertices)
    {
        int n = vertices.Length;
        float A = 0.0f;
        for (int p = n - 1, q = 0; q < n; p = q++)
        {
            Vector2 pval = vertices[p];
            Vector2 qval = vertices[q];
            A += pval.x * qval.y - qval.x * pval.y;
        }
        return A * 0.5f;
    }

    private bool Snip(Vector2[] vertices, int u, int v, int w, int n, int[] V)
    {
        Vector2 A = vertices[V[u]];
        Vector2 B = vertices[V[v]];
        Vector2 C = vertices[V[w]];

        if (Mathf.Epsilon > (((B.x - A.x) * (C.y - A.y)) - ((B.y - A.y) * (C.x - A.x))))
            return false;

        for (int p = 0; p < n; p++)
        {
            if ((p == u) || (p == v) || (p == w))
                continue;

            Vector2 P = vertices[V[p]];

            if (InsideTriangle(A, B, C, P))
                return false;
        }
        return true;
    }

    private bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        float ax = C.x - B.x;
        float ay = C.y - B.y;
        float bx = A.x - C.x;
        float by = A.y - C.y;
        float cx = B.x - A.x;
        float cy = B.y - A.y;
        float apx = P.x - A.x;
        float apy = P.y - A.y;
        float bpx = P.x - B.x;
        float bpy = P.y - B.y;
        float cpx = P.x - C.x;
        float cpy = P.y - C.y;

        float aCrossBP = ax * bpy - ay * bpx;
        float cCrossAP = cx * apy - cy * apx;
        float bCrossCP = bx * cpy - by * cpx;

        return ((aCrossBP >= 0.0f) && (bCrossCP >= 0.0f) && (cCrossAP >= 0.0f));
    }
}
