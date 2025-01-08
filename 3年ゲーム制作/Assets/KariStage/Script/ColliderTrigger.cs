using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    public CatMove catMove; // CatMoveスクリプトへの参照

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // CatMoveに通知
            catMove.OnPlayerTriggered();
        }
    }
}
