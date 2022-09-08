using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UserGraphicController : MonoBehaviour
{
    public bool userGraphicsUp;

    private void Awake()
    {
        userGraphicsUp = false;
    }

    public virtual void Update()
    {
        ClearUserGraphics();
    }

    public virtual void ClearUserGraphics()
    {
        if(userGraphicsUp && ResetCondition() && Input.GetMouseButtonDown(1))
        {
            ResetSelf();
        }
    }

    public abstract bool ResetCondition();
    public virtual void ResetSelf()
    {
        userGraphicsUp = false;
    }
}
