using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationBaseController : MonoBehaviour
{
    [SerializeField] protected bool enableInStart;
    [SerializeField] protected float delay;
    [SerializeField] protected float speed;
    [SerializeField] protected LeanTweenType type;

    protected LTDescr tween;

    private void Start()
    {
        if (enableInStart)
        {
            Initialize();
        }
    }

    public virtual void Initialize()
    {
        if (tween != null)
        {
            LeanTween.cancel(tween.uniqueId);
        }

        gameObject.SetActive(true);
    }

    public virtual void Conclude()
    {
        if (tween != null)
        {
            LeanTween.cancel(tween.uniqueId);
        }

        gameObject.SetActive(false);
    }
}
