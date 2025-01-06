using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveState : MonoBehaviour
{
    [Header("対象オブジェクト")]
    [Tooltip("アクティブ化/非アクティブ化を切り替えたいオブジェクト")]
    [SerializeField] private GameObject targetObject;

    [Header("初期状態")]
    [Tooltip("スタート時にオブジェクトをアクティブにするかどうか")]
    [SerializeField] private bool startActive = true;

    void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(startActive);
        }
        else
        {
            Debug.LogWarning("ターゲットオブジェクトが設定されていません。");
        }
    }

    public void Toggle()
    {
        if (targetObject != null)
        {
            bool currentState = targetObject.activeSelf;
            targetObject.SetActive(!currentState);
        }
        else
        {
            Debug.LogWarning("ターゲットオブジェクトが設定されていません。");
        }
    }
}

