using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    public static AnimController Instance;
    [SerializeField] private Animator animator;
    [SerializeField] private bool hasRunAnim;
    [SerializeField] private bool hasIdleAnim;
    [SerializeField] private bool hasHitAnim;
    [SerializeField] private bool hasPickUpAnim;
    private bool isHitting;

    private Direction currentDirection = Direction.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Cow.cowHit += PlayHitAnimation;
            isHitting = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlayMoveAnimation(Direction direction)
    {
        if(isHitting)
            return;
        currentDirection = direction;
        switch (direction)
        {
            case Direction.Right:
                animator.SetTrigger("MoveRight");
                break;
            case Direction.Left:
                animator.SetTrigger("MoveLeft");
                break;
        }
    }

    public void PlayHitAnimation()
    {
        //animator.SetTrigger("Hit");
        //isHitting = true;
    }

    public void PlayPickUpAnimation()
    {
        animator.SetTrigger("PickUp");
    }

    public void PlayIdleAnimation()
    {
        animator.SetTrigger("Idle");
    }

    public void OnHitEnded()
    {
        isHitting = false;
        
        if (currentDirection == Direction.Right)
        {
            PlayMoveAnimation(Direction.Right);
        }
        else if (currentDirection == Direction.Left)
        {
            PlayMoveAnimation(Direction.Left);
        }
    }
}

public enum Direction
{
    Right,
    Left,
    None
}

