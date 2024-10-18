using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationBaseController : MonoBehaviour
{
    public virtual void Initialize()
    {
        gameObject.SetActive(true);
    }

    public virtual void Conclude()
    {
        gameObject.SetActive(false);
    }
}
