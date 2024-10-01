using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ScoreCheese : MonoBehaviour
{
    [SerializeField] AudioClip getSE=null;
    [Header("加算するスコア")]public int myScore;
    [Header("プレイヤーの判定")]public PlayerTriggerCheck playerCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCheck.isOn)
        {
            //ここでGameManager側にスコア加算したりする
            //SEあるならここで再生
            Destroy(this.gameObject);
        }
    }
}
