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

    /*private void Update()
    {
        if (!enabled)
            return;
    }*/

    // Detectar cuando el mouse hace clic sobre el objeto
    void OnMouseDown()
    {
        if (!isEnabled) return;
        isDragging = true;
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
            transform.position = mousePos + offset;
        }
    }

    // Cuando el mouse suelta el objeto
    void OnMouseUp()
    {
        if (!isEnabled) return;
        isDragging = false;
        OnMouseUpAction?.Invoke();
    }
}
