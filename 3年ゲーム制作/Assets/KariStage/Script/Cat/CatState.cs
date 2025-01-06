using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatState : MonoBehaviour
{
    private bool isStopped = false;

    public void SetStopped(bool value)
    {
        isStopped = value;
    }

    public bool IsStopped()
    {
        return isStopped;
    }
}
