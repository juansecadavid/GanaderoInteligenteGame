using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointInSquare : MonoBehaviour
{

    // Offset desde los bordes de la cámara
    public float offsetFromEdge = 0f;

    // Tamaño de la zona a evitar cerca de la esquina superior izquierda
    public float avoidAreaSize = 2f;

    // Variable para ajustar qué tan cerca o lejos de la esquina superior izquierda evitar
    public float cornerAvoidanceOffset = 0f;

    // Para visualizar los gizmos
    public bool showGizmos2 = true;

    // Lista interna para almacenar los bordes válidos
    private List<EdgeInfo> validEdges = new List<EdgeInfo>();
    
    private SpriteRenderer spriteRenderer;

    // Variable de ajuste de tamaño para corregir el tamaño del cuadrilátero
    public Vector2 sizeAdjustment = Vector2.one;

    // Para visualizar los gizmos
    public bool showGizmos = true;
    private Vector2[] quadVertices = new Vector2[4];
    
    private class EdgeInfo
    {
        public bool isVertical; // true para bordes verticales (izquierda y derecha)
        public float fixedCoordinate; // Coordenada fija (x para verticales, y para horizontales)
        public float min; // Valor mínimo de la coordenada variable
        public float max; // Valor máximo de la coordenada variable
    }

    void Awake()
    {
        // Obtener el componente SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Genera un punto aleatorio dentro del cuadrilátero
    public Vector2 GetRandomPoint()
    {
        // Obtener el tamaño del sprite en espacio local
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector2 localScale = transform.lossyScale;

        // Aplicar la escala y el ajuste de tamaño
        float width = spriteSize.x * localScale.x * sizeAdjustment.x;
        float height = spriteSize.y * localScale.y * sizeAdjustment.y;

        // Ajustar por el pivote del sprite
        Vector2 pivotOffset = spriteRenderer.sprite.bounds.center;

        // Definir los cuatro vértices en espacio local
        Vector2 localP0 = new Vector2(-width / 2, -height / 2) + pivotOffset;
        Vector2 localP1 = new Vector2(width / 2, -height / 2) + pivotOffset;
        Vector2 localP2 = new Vector2(width / 2, height / 2) + pivotOffset;
        Vector2 localP3 = new Vector2(-width / 2, height / 2) + pivotOffset;

        // Transformar los vértices al espacio mundial y almacenarlos para los gizmos
        quadVertices[0] = transform.TransformPoint(localP0);
        quadVertices[1] = transform.TransformPoint(localP1);
        quadVertices[2] = transform.TransformPoint(localP2);
        quadVertices[3] = transform.TransformPoint(localP3);

        // Generar el punto aleatorio dentro del cuadrilátero
        return RandomPointInQuad(quadVertices[0], quadVertices[1], quadVertices[2], quadVertices[3]);
    }

    // Método para generar un punto aleatorio dentro de un cuadrilátero definido por cuatro puntos
    private Vector2 RandomPointInQuad(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float s = Random.value;
        float t = Random.value;

        Vector2 randomPoint;

        if (s + t <= 1)
        {
            // Primer triángulo
            randomPoint = p0 + s * (p1 - p0) + t * (p3 - p0);
        }
        else
        {
            // Segundo triángulo
            s = 1 - s;
            t = 1 - t;
            randomPoint = p2 + s * (p3 - p2) + t * (p1 - p2);
        }

        return randomPoint;
    }
    
    public void PositionObjectOnEdge(GameObject objectToPosition)
    {
        // Obtener la cámara principal
        Camera cam = Camera.main;

        // Obtener las esquinas de la cámara en coordenadas del mundo
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        float leftX = bottomLeft.x;
        float rightX = topRight.x;
        float bottomY = bottomLeft.y;
        float topY = topRight.y;

        // Ajustar los bordes con el offset
        float adjustedLeftX = leftX + offsetFromEdge;
        float adjustedRightX = rightX - offsetFromEdge;
        float adjustedBottomY = bottomY + offsetFromEdge;
        float adjustedTopY = topY - offsetFromEdge;

        // Calcular la zona a evitar en la esquina superior izquierda
        float avoidLeftX = leftX + offsetFromEdge;
        float avoidTopY = topY - offsetFromEdge;
        float avoidRightX = avoidLeftX + avoidAreaSize + cornerAvoidanceOffset;
        float avoidBottomY = avoidTopY - avoidAreaSize - cornerAvoidanceOffset;

        // Crear la lista de bordes válidos
        validEdges.Clear();

        // Borde izquierdo (excluyendo la zona a evitar)
        if (adjustedTopY > adjustedBottomY)
        {
            float leftEdgeMinY = adjustedBottomY;
            float leftEdgeMaxY = adjustedTopY - avoidAreaSize - cornerAvoidanceOffset;

            if (leftEdgeMaxY > leftEdgeMinY)
            {
                validEdges.Add(new EdgeInfo
                {
                    isVertical = true,
                    fixedCoordinate = adjustedLeftX,
                    min = leftEdgeMinY,
                    max = leftEdgeMaxY
                });
            }
        }

        // Borde derecho
        if (adjustedTopY > adjustedBottomY)
        {
            validEdges.Add(new EdgeInfo
            {
                isVertical = true,
                fixedCoordinate = adjustedRightX,
                min = adjustedBottomY,
                max = adjustedTopY
            });
        }

        // Borde inferior
        if (adjustedRightX > adjustedLeftX)
        {
            validEdges.Add(new EdgeInfo
            {
                isVertical = false,
                fixedCoordinate = adjustedBottomY,
                min = adjustedLeftX,
                max = adjustedRightX
            });
        }

        // Borde superior (excluyendo la zona a evitar)
        if (adjustedRightX > adjustedLeftX + avoidAreaSize + cornerAvoidanceOffset)
        {
            float topEdgeMinX = adjustedLeftX + avoidAreaSize + cornerAvoidanceOffset;
            float topEdgeMaxX = adjustedRightX;

            if (topEdgeMaxX > topEdgeMinX)
            {
                validEdges.Add(new EdgeInfo
                {
                    isVertical = false,
                    fixedCoordinate = adjustedTopY,
                    min = topEdgeMinX,
                    max = topEdgeMaxX
                });
            }
        }

        // Verificar si hay bordes válidos
        if (validEdges.Count == 0)
        {
            Debug.LogError("No hay bordes válidos para posicionar el objeto. Verifica los valores de offset y avoidAreaSize.");
            return;
        }

        // Seleccionar un borde aleatorio
        int randomIndex = Random.Range(0, validEdges.Count);
        EdgeInfo chosenEdge = validEdges[randomIndex];

        // Generar una posición aleatoria en el borde seleccionado
        if (chosenEdge.isVertical)
        {
            float yPos = Random.Range(chosenEdge.min, chosenEdge.max);
            objectToPosition.transform.position = new Vector3(chosenEdge.fixedCoordinate, yPos, objectToPosition.transform.position.z);
        }
        else
        {
            float xPos = Random.Range(chosenEdge.min, chosenEdge.max);
            objectToPosition.transform.position = new Vector3(xPos, chosenEdge.fixedCoordinate, objectToPosition.transform.position.z);
        }
    }

    // Método para dibujar los gizmos y visualizar el cuadrilátero y los puntos generados
    void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (quadVertices == null || quadVertices.Length < 4)
            return;

        // Dibujar el cuadrilátero
        Gizmos.color = Color.green;
        for (int i = 0; i < 4; i++)
        {
            Vector2 currentVertex = quadVertices[i];
            Vector2 nextVertex = quadVertices[(i + 1) % 4];
            Gizmos.DrawLine(currentVertex, nextVertex);
        }

        // Dibujar puntos aleatorios dentro del cuadrilátero
        Gizmos.color = Color.red;
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomPoint = GetRandomPoint();
            Gizmos.DrawSphere(randomPoint, 0.05f);
        }
        
        if (!showGizmos2)
            return;
        
        // Obtener la cámara principal
        Camera cam = Camera.main;

        if (cam == null)
            return;

        // Obtener las esquinas de la cámara en coordenadas del mundo
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        float leftX = bottomLeft.x;
        float rightX = topRight.x;
        float bottomY = bottomLeft.y;
        float topY = topRight.y;

        // Ajustar los bordes con el offset
        float adjustedLeftX = leftX + offsetFromEdge;
        float adjustedRightX = rightX - offsetFromEdge;
        float adjustedBottomY = bottomY + offsetFromEdge;
        float adjustedTopY = topY - offsetFromEdge;

        // Calcular la zona a evitar en la esquina superior izquierda
        float avoidLeftX = leftX + offsetFromEdge;
        float avoidTopY = topY - offsetFromEdge;
        float avoidRightX = avoidLeftX + avoidAreaSize + cornerAvoidanceOffset;
        float avoidBottomY = avoidTopY - avoidAreaSize - cornerAvoidanceOffset;

        // Dibujar el rectángulo de la zona a evitar
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(avoidLeftX, avoidTopY, 0), new Vector3(avoidRightX, avoidTopY, 0));
        Gizmos.DrawLine(new Vector3(avoidRightX, avoidTopY, 0), new Vector3(avoidRightX, avoidBottomY, 0));
        Gizmos.DrawLine(new Vector3(avoidRightX, avoidBottomY, 0), new Vector3(avoidLeftX, avoidBottomY, 0));
        Gizmos.DrawLine(new Vector3(avoidLeftX, avoidBottomY, 0), new Vector3(avoidLeftX, avoidTopY, 0));

        // Dibujar los bordes válidos
        Gizmos.color = Color.green;

        // Borde izquierdo
        if (adjustedTopY - avoidAreaSize - cornerAvoidanceOffset > adjustedBottomY)
        {
            Gizmos.DrawLine(
                new Vector3(adjustedLeftX, adjustedBottomY, 0),
                new Vector3(adjustedLeftX, adjustedTopY - avoidAreaSize - cornerAvoidanceOffset, 0));
        }

        // Borde derecho
        Gizmos.DrawLine(
            new Vector3(adjustedRightX, adjustedBottomY, 0),
            new Vector3(adjustedRightX, adjustedTopY, 0));

        // Borde inferior
        Gizmos.DrawLine(
            new Vector3(adjustedLeftX, adjustedBottomY, 0),
            new Vector3(adjustedRightX, adjustedBottomY, 0));

        // Borde superior
        if (adjustedRightX > adjustedLeftX + avoidAreaSize + cornerAvoidanceOffset)
        {
            Gizmos.DrawLine(
                new Vector3(adjustedLeftX + avoidAreaSize + cornerAvoidanceOffset, adjustedTopY, 0),
                new Vector3(adjustedRightX, adjustedTopY, 0));
        }
    }
}
