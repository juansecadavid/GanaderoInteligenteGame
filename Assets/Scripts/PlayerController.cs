using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject attacher;
    [SerializeField] private DragAndDrop _dragAndDrop;
    [SerializeField] private AnimController _animController;
    public bool IsAttached { get;  set; }


    public void Initialize()
    {
        IsAttached = false;
        Seed.enableDragAndDrop += EnableDragAndDrop;
    }
    public void Conclude()
    {
        Seed.enableDragAndDrop -= EnableDragAndDrop;
        _dragAndDrop.isEnabled = false;
        _animController.PlayIdleAnimation();
    }
    private void EnableDragAndDrop(bool enable)
    {
        _dragAndDrop.isEnabled = enable;
    }

    public GameObject Attach()
    {
        IsAttached = true;
        return attacher;
    }
}
