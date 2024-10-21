using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//インゲーム中の、進行度等に応じたUI遷移の管理クラス

public class StageCtrl : MonoBehaviour
{
    [Header("チーズパラメーターUI")]
    [SerializeField] public Image cheeseParameters;
    [Header("体力ゲージUI")]
    [SerializeField] public Slider healthGaugeSlider;

    [Header("プレイヤーゲームオブジェクト")]
    public GameObject playerObj;
    [Header("ゲームオーバー")]
    public GameObject gameOverObj;
    [Header("ステージクリア")]
    public GameObject stageClrObj;
    [Header("インゲームUI")]
    public GameObject InGameUIObj;
    [Header("次のステージ")]
    public int nextStage;

    public bool doGameOver = false;
    private bool retryGame = false;
    private int nextStageNum;

    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

        gameManager = FindObjectOfType<GameManager>();

        if (playerObj != null && gameOverObj != null && stageClrObj != null && InGameUIObj != null)
        {
            gameOverObj.SetActive(false);
            stageClrObj.SetActive(false);
            InGameUIObj.SetActive(true);
            doGameOver = false;
        }
        else
        {
            Debug.Log("設定が足りてないよ！");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //爆弾チーズを獲ってしまった時のゲームオーバー処理
    public void OnCheeseCollected()
    {
        Debug.Log("爆弾チーズが取得されました！");
        InGameUIObj.SetActive(false);
        gameOverObj.SetActive(true);
        doGameOver = true;
        Time.timeScale = 0f;
    }

    //敵に捕まった時のゲームオーバー処理
    public void OnEnemyCollected()
    {
        Debug.Log("敵とプレイヤーが接触しました！");
        InGameUIObj.SetActive(false);
        gameOverObj.SetActive(true);
        doGameOver = true;
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        SceneManager.LoadScene("CatMoveTest");
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

    public void Retry2()
    {
        retryGame = true;
        ChangeScene(2);
    }

    public void ChangeScene(int num)
    {
        nextStageNum = num;
        SceneManager.LoadScene(nextStageNum); // シーンを変更する
    }

    public void arrivedGoal()
    {
        stageClrObj.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ToNextStage()
    {
        SceneManager.LoadScene(nextStage);
    }

}
