using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : ScriptableObject
{
    public bool DestroyAfterFinish;

    public BaseAction ChildAction;

    public abstract void ExecuteAction(System.Action onFinish = null);
}
