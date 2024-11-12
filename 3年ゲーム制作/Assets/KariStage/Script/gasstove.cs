using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gasstove : MonoBehaviour
{
    public StageCtrl stageCtrl;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [Header("スタート時の状態")] public bool isStart;
    private Animator isOn;
    private BoxCollider2D BoxCollider;
    // Start is called before the first frame update
    void Start()
    {
        isOn = GetComponent<Animator>();
        isOn.SetBool("isOn",isStart);
        BoxCollider = GetComponent<BoxCollider2D>();
        BoxCollider.isTrigger=!isStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn&&isOn.GetBool("isOn"))
        {
            //stageCtrl.OnEnemyCollected();
        }
    }

    public void SwitchStove()
    {
        isOn.SetBool("isOn", !isOn.GetBool("isOn"));
        BoxCollider.isTrigger=!BoxCollider.isTrigger;
        //isOn = !isOn;
    }

}
