using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class FakeScoreCheese : MonoBehaviour
{
    [SerializeField] AudioClip getSE = null;
    [Header("���Z����X�R�A")] public int penaltyScore;
    [Header("�v���C���[�̔���")] public PlayerTriggerCheck playerCheck;

    [SerializeField] private StageCtrl stageCtrl;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn)
        {
            //������GameManager���ɃX�R�A���Z�����肷��
            if (stageCtrl != null)
            {
                Debug.Log("���e�`�[�Y�ɐG��܂���");
                Destroy(this.gameObject);
                stageCtrl.OnCheeseCollected();
            }
            //SE����Ȃ炱���ōĐ�     
        }
    }
}
