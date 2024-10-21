using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class StageData
{
    public int stageNum;
    public bool isUnlocked;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public List<StageData> stages;

    //private bool retryGame = false;
    private int nextStageNum;

    PlayerMove player;

    //ステージUI切り替え周りの処理
    StageCtrl stageCtrl;

    //チーズ取得数
    private int cheeseScore = 0;

    //各ステージのチーズ上限数
    private int[] cheeseScores = { 8, 0, 0 };

    //firstStageの速度一覧
    private float[] firstStgPlySpeeds = { 6f, 7f, 8f, 8.5f, 9f, 9.5f, 10f, 10.5f, 11f, 11.5f };

    //現在のプレイヤー速度保管用
    private float currentSpeed = 0f;

    //現在のステージ番号
    private int stgNum = 0;

    private float normalRunning = 0.1f;
    private float speedRunning = 0.3f;

    //チーズ取得率
    public float percentCheese { get; private set; }

    private bool IsStageCtrlGet = false;

    //シングルトンの実装
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //セットアップ
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if ((stageCtrl == null) || (player == null)) 
        {
            SetUp();
        }

        if (stageCtrl != null)
        {
            if (!stageCtrl.doGameOver)
            {
                if (IsStageCtrlGet)
                {
                    //チーズゲージの更新
                    UpdateCheeseParameter();

                    //チーズ保有率によるプレイヤー速度の更新
                    UpdateCurrentSpeed(stgNum);

                    //プレイヤーの体力周りの更新
                    ManageHealth();
                }
                else
                {
                    StageCtrlSetUp();
                }
            }
            else
            {

            }
        } 
    }

    //セットアップ関数
    void SetUp()
    {
        Time.timeScale = 1.0f;

        player = FindObjectOfType<PlayerMove>();
        stageCtrl = FindObjectOfType<StageCtrl>();

        cheeseScore = 0;     

        //現在のステージ番号によって、初速度を変えている。
        switch (stgNum)
        {
            //firstStage
            case 0:
                currentSpeed = firstStgPlySpeeds[0];
                break;
        }

        if (stageCtrl != null)
        {
            StageCtrlSetUp();
        }
    }

    //外部のStageCtrlクラスの初期化(Find関数で取得するまで、行わないようにする)
    void StageCtrlSetUp()
    {
        stageCtrl.cheeseParameters.fillAmount = 0;
        stageCtrl.healthGaugeSlider.value = 1;
        IsStageCtrlGet = true;
    }

    void UpdateCheeseParameter()
    {
        //獲得チーズ数とステージに配置されたチーズ数を除算した結果を、チーズパラメーターに反映
        percentCheese = (float)cheeseScore / (float)cheeseScores[stgNum];
        stageCtrl.cheeseParameters.fillAmount = percentCheese;
        Debug.Log(cheeseScore);
        Debug.Log(percentCheese);
    }

    //速度の更新(変数は現在のステージ番号)
    void UpdateCurrentSpeed(int stageNum)
    {
        //プレイヤーの速度を、チーズの獲得数（cheeseScoreで管理）から変更
        switch (stageNum)
        {
            case 0:
                currentSpeed = firstStgPlySpeeds[cheeseScore];
                break;
        }
    }

    //PlayerMoveに、現在の速度を返す
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    //プレイヤーが走っていたら、体力ゲージを減らす。0になったら、チーズを減らす
    void ManageHealth()
    {
        if (player != null)
        {
            if (player.IsRunningPlayer())
            {
                if (cheeseScore != 0)
                {
                    if (player.IsPlayerDushing())
                    {
                        stageCtrl.healthGaugeSlider.value -= Time.deltaTime * speedRunning;
                    }
                    else
                    {
                        stageCtrl.healthGaugeSlider.value -= Time.deltaTime * normalRunning;
                    }
                }
            }
        }
        else
        {

        }

        if (stageCtrl.healthGaugeSlider.value <= 0)
        {
            //チーズの取得数をデクリメントし、一つ下のスピードに変える
            cheeseScore--;
            //体力ゲージをリセット
            stageCtrl.healthGaugeSlider.value = 1;
        }
    }

    // ステージデータをセーブする
    public void SaveStageData()
    {
        // データをシリアライズして保存
        PlayerPrefs.SetString("StageData", JsonUtility.ToJson(this));
    }

    // ステージデータをロードする
    public void LoadStageData()
    {
        if (PlayerPrefs.HasKey("StageData"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("StageData"), this);
        }
        else
        {
            // 初回起動時に最初のステージのみ解放
            stages[0].isUnlocked = true;
        }
    }

    // ステージクリア時に次のステージをアンロック
    public void UnlockNextStage(int clearedStageNumber)
    {
        if (clearedStageNumber < stages.Count - 1)
        {
            stages[clearedStageNumber + 1].isUnlocked = true;
            SaveStageData();
        }
    }

    public void StageCleared(int stageNumber)
    {
        UnlockNextStage(stageNumber);
        SceneManager.LoadScene("StageSelect");
    }

    public void getCheese(int cheese)
    {
        cheeseScore += cheese;
        //新しくチーズをゲットしたら、体力ゲージをリセット
        stageCtrl.healthGaugeSlider.value = 1;
    }
}
