using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ScoreCheese : MonoBehaviour
{
    [SerializeField] AudioClip getSE=null;
    [Header("加算するスコア")]public int myScore;
    [Header("プレイヤーの判定")]public PlayerTriggerCheck playerCheck;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCheck.isOn)
        {
            //ここでGameManager側にスコア加算したりする
            gameManager.getCheese(1);
            //SEあるならここで再生
            Destroy(this.gameObject);
        }
    }
}
