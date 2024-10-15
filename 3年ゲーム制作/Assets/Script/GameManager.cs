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

    [SerializeField]
    private Image cheeseParameters;

    private int cheeseScore = 0;

    private int[] cheeseScores = { 8, 0, 0 };
    private int stgNum = 0;

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
        //LoadStageData();
        cheeseScore = 0;
        cheeseParameters.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //チーズゲージの更新
        UpdateCheeseParameter();

        //敵に追いつかれたときのゲームオーバー処理

        //爆弾チーズを取得したときのゲームオーバー処理

        //Goalに到達したときのゲームクリア処理

        //
    }

    void UpdateCheeseParameter()
    {
        //獲得チーズ数とステージに配置されたチーズ数を除算した結果を、チーズパラメーターに反映
        float fillAmount_ = (float)cheeseScore / (float)cheeseScores[stgNum];
        cheeseParameters.fillAmount = fillAmount_;
        Debug.Log(cheeseScore);
        Debug.Log(fillAmount_);
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
    }
}
