using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gasstove : MonoBehaviour
{
    public StageCtrl stageCtrl;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [Header("スタート時の状態")] public bool isStart;
    private Animator isOn;
    // Start is called before the first frame update
    void Start()
    {
        isOn = GetComponent<Animator>();
        isOn.SetBool("isOn",isStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn&&isOn.GetBool("isOn"))
        {
            stageCtrl.OnEnemyCollected();
        }
    }

    public void SwitchStove()
    {
        isOn.SetBool("isOn", !isOn.GetBool("isOn"));
        //isOn = !isOn;
    }

}
