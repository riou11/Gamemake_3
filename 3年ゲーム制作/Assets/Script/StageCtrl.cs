using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrl : MonoBehaviour
{
    [Header("プレイヤーゲームオブジェクト")]
    public GameObject playerObj;
    [Header("ゲームオーバー")]
    public GameObject gameOverObj;

    private bool doGameOver = false;
    private bool retryGame = false;
    private int nextStageNum;

    // Start is called before the first frame update
    void Start()
    {
        if (playerObj != null && gameOverObj != null)
        {
            gameOverObj.SetActive(false);
        }
        else
        {
            Debug.Log("設定が足りてないよ！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doGameOver)
        {
            // ゲームオーバー時の処理をここに追加
        }
    }

    public void OnCheeseCollected()
    {
        Debug.Log("爆弾チーズが取得されました！");
        gameOverObj.SetActive(true);
        doGameOver = true;
    }

    public void OnEnemyCollected()
    {
        Debug.Log("敵とプレイヤーが接触しました！");
        gameOverObj.SetActive(true);
        doGameOver = true;
    }
    public void Retry0()
    {
        retryGame = true;
        ChangeScene(0); //最初のステージに戻るので1
    }
    public void Retry1()
    {
        retryGame = true;
        ChangeScene(1); //最初のステージに戻るので1
    }

    public void ChangeScene(int num)
    {
        nextStageNum = num;
        SceneManager.LoadScene(nextStageNum); // シーンを変更する
    }
}
