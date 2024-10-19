using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleAnimationController : UIAnimationBaseController
{
    [SerializeField] private Vector2 startScale;
    [SerializeField] private Vector2 targetScale;

    public override void Initialize()
    {
        transform.localScale = startScale;
        base.Initialize();

        if (tween != null)
        {
            LeanTween.cancel(tween.uniqueId);
        }

        tween = LeanTween.scale(gameObject, targetScale, speed).setEase(type);
    }

    public override void Conclude()
    {
        if (tween != null)
        {
            LeanTween.cancel(tween.uniqueId);
        }

        tween = LeanTween.scale(gameObject, targetScale, speed / 2).setOnComplete(()=> base.Conclude());
    }
}
