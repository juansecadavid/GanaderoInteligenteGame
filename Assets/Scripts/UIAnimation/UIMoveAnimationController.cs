using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoveAnimationController : UIAnimationBaseController
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 targetPos;

    public override void Initialize()
    {
        transform.localPosition = startPos;
        base.Initialize();

        tween = LeanTween.moveLocal(gameObject, targetPos, speed);
    }

    public override void Conclude()
    {
        if (tween != null)
        {
            LeanTween.cancel(tween.uniqueId);
        }

        tween = LeanTween.moveLocal(gameObject, startPos, speed).setOnComplete(()=> base.Conclude());
    }
}
