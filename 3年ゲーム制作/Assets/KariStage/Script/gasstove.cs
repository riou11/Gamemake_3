using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gasstove : MonoBehaviour
{
   
    [SerializeField] Material OnMaterial;
    [SerializeField] Material Offmaterial;
    public StageCtrl stageCtrl;
    [Header("�v���C���[�̔���")] public PlayerTriggerCheck playerCheck;
    [Header("�X�^�[�g���̏��")] public bool isOn;
    // Start is called before the first frame update
    void Start()
    {
        if (isOn)
            GetComponent<Renderer>().material = OnMaterial;
        else
            GetComponent<Renderer>().material = Offmaterial;

    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            GetComponent<Renderer>().material = OnMaterial;
            if(playerCheck.isOn)
            {
                stageCtrl.OnEnemyCollected();
            }
        }
        else
            GetComponent<Renderer>().material = Offmaterial;
    }

    public void SwitchStove()
    {
        isOn = !isOn;
    }

}
