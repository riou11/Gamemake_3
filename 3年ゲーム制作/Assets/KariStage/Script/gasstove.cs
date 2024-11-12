using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gasstove : MonoBehaviour
{
    public StageCtrl stageCtrl;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [Header("スタート時の状態")] public bool isStart;
    private Animator animator;
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isOn",isStart);
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger=!isStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn&&animator.GetBool("isOn"))
        {
            //stageCtrl.OnEnemyCollected();
        }
    }

    public void SwitchStove()
    {
        bool currentStatus = animator.GetBool("isOn");
        animator.SetBool("isOn", !currentStatus);
        //animator.SetBool("isOn", !animator.GetBool("isOn"));
        boxCollider.isTrigger=!boxCollider.isTrigger;
        //isOn = !isOn;
    }

}
