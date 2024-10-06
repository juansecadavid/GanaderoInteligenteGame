using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject attacher;
    [SerializeField] private DragAndDrop _dragAndDrop;
    
    
    public void Initialize()
    {
        Seed.enableDragAndDrop += EnableDragAndDrop;
    }
    public void Conclude()
    {
        Seed.enableDragAndDrop -= EnableDragAndDrop;
    }
    private void EnableDragAndDrop(bool enable)
    {
        _dragAndDrop.isEnabled = enable;
    }
}
