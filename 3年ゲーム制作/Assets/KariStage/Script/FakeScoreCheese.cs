using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class FakeScoreCheese : MonoBehaviour
{
    [SerializeField] AudioClip getSE = null;
    [Header("減算するスコア")] public int penaltyScore;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn)
        {
            //ここでGameManager側にスコア加算したりする
            //SEあるならここで再生

            Debug.Log("爆弾チーズに触れました");
            Destroy(this.gameObject);
        }
    }
}
