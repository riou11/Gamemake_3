using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ScoreCheese : MonoBehaviour
{
    [SerializeField] AudioClip getSE=null;
    [Header("���Z����X�R�A")]public int myScore;
    [Header("�v���C���[�̔���")]public PlayerTriggerCheck playerCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCheck.isOn)
        {
            //������GameManager���ɃX�R�A���Z�����肷��
            //SE����Ȃ炱���ōĐ�
            Destroy(this.gameObject);
        }
    }
}
