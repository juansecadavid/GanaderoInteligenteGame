using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private UnityEvent OnDrop;
    [SerializeField] private UnityEvent onTap;
    private bool isDragging = false;
    public bool isEnabled = true;
    private Vector3 offset;
    public static Action OnMouseUpAction;
    
    private Vector3 previousPosition;
    
    private Direction currentDirection = Direction.None;
    

    // Detectar cuando el mouse hace clic sobre el objeto
    void OnMouseDown()
    {
        if (!isEnabled) return;
        isDragging = true;
        previousPosition = transform.position;
        print(" Me tocó");
        // Calcular la diferencia entre la posición del objeto y la del mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - mousePos;
    }

    // Actualizar la posición mientras se mantiene el clic
    void OnMouseDrag()
    {
        if (!isEnabled) return;
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 newPosition = mousePos + offset;

            // Detectar la dirección del arrastre
            if (newPosition.x > previousPosition.x)
            {
                // Se está arrastrando hacia la derecha
                if (currentDirection != Direction.Right)
                {
                    // Solo activa el trigger si la dirección cambia
                    AnimController.Instance.PlayMoveAnimation(Direction.Right);
                    currentDirection = Direction.Right;
                }
            }
            else if (newPosition.x < previousPosition.x)
            {
                // Se está arrastrando hacia la izquierda
                if (currentDirection != Direction.Left)
                {
                    // Solo activa el trigger si la dirección cambia
                    AnimController.Instance.PlayMoveAnimation(Direction.Left);
                    currentDirection = Direction.Left;
                }
            }

            previousPosition = newPosition;
            transform.position = newPosition;
        }
    }

    // Cuando el mouse suelta el objeto
    void OnMouseUp()
    {
        if (!isEnabled) return;
        isDragging = false;
        AnimController.Instance.PlayIdleAnimation();
        OnMouseUpAction?.Invoke();
    }
}
