using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    //シーン名とロック状況
    [System.Serializable]
    public class StageData
    {
        public GameScene gameScene;
        public bool isUnlocked;
    }

    //ゲームのシーンを、番号ではなくenemを使ってシーン名で管理
    public enum GameScene
    {
        Title,
        SceneSelect,//Menu,StageSelect,Option,PlayGuideを、このシーンのなかで切り替え
        FirstStage,
        SecondStage,
    }

    [SerializeField] private List<StageData> _stages; // インスペクターでシーンをロックするか設定（ステージ以外（ + firstStage）はtrue）

    public float percentCheese { get; private set; } //チーズ取得率
    public int stgNum { get; private set; } //現在のステージ番号
    public int cheeseScore { get; private set; }//チーズ取得数
    public GameScene currentScene { get; private set; } //現在のシーン


    private PlayerMove _player;
    private StageCtrl _stageCtrl; //ステージUI切り替え周りの処理
    private Dictionary<GameScene, bool> _stageDatas = new(); //シーン遷移の際に消えないようにプライベートで保管
    private GameScene _gameScene; //ステージ遷移に使う変数（現在のシーンを示すものではない）
    private int[] _cheeseScores = { 18, 0, 0 }; //各ステージのチーズ上限数
    private float[] _firstStgPlySpeeds = { 6f, 7f, 8f, 8.5f, 9f, 9.5f, 10f, 10.5f, 11f, 11.5f, 12f, 13f, 13.5f, 14f, 15.5f, 16f, 17f }; //firstStageの速度一覧  
    private float _currentSpeed = 0f; //現在のプレイヤー速度保管用   
    private float _gaugeDecreRate = 0.3f; //体力ゲージ減少率
    private float _gaugeIncreRate = 0.1f; //体力ゲージ回復率
    private bool _isStageCtrlGet = false; //各ステージのStageCtrl（UI管理）を取得したか


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

        //ステージデータの取り込み
        LoadStagesData();
        SearchCurrentScene();
    }

    // Start is called before the first frame update
    void Start()
    {
        //セットアップ
        SetUp();
    }

    //enumで処理の区別が出来るようにする
    // Update is called once per frame
    void Update()
    {
        switch (currentScene)
        {
            case GameScene.Title:
                ProcessTitle();
                break;
            case GameScene.SceneSelect:
                ProcessSceneSelect();
                break;
            case GameScene.FirstStage:
                ProcessStage();
                break;
            case GameScene.SecondStage:
                ProcessStage();
                break;
        }
    }

    //-----------------------------------ステージ遷移に関する処理-----------------------------------//

    void ProcessTitle()
    {

    }

    void ProcessSceneSelect()
    {

    }

    void ProcessStage()
    {
        //シーン遷移があったときの処理（データの初期化）
        if ((_stageCtrl == null) || (_player == null))
        {
            SetUp();
        }
        else
        {
            //stageCtrlを取得するまで行わないようにする
            if (_isStageCtrlGet)
            {
                if (_stageCtrl.doGameClear) //ゲームクリア時
                {
                    
                }
                else　
                {
                    if (!_stageCtrl.doGameOver)
                    {
                        UpdateInGame(stgNum);
                    }
                    else //ゲームオーバーになったら
                    {

                    }
                }
            }
            else
            {
                StageCtrlSetUp();
            }
        }
    }

    void SearchCurrentScene()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        currentScene = (GameScene)index;
    }

    //インスペクターで設定された内容を、プライベートに取り込む（FirstStageのみ開放）
    void LoadStagesData()
    {
        foreach (var stageData in _stages)
        {
            _stageDatas[stageData.gameScene] = stageData.isUnlocked;
        }

        _stageDatas[GameScene.FirstStage] = true;
    }

    //ステージ遷移（ボタンにこの関数を入れて、飛びたいシーンの番号(enum(GameScene)で定義)を設定する）
    public void TransitionScene(int scene)
    {

        _gameScene = (GameScene)scene;

        //クリア時はステージを開放する
        if (_stageCtrl != null)
        {
            if (_stageCtrl.doGameClear)
            {
                UnlockNextStage(_gameScene);
            }
        }
        
        //開放されていれば次のステージに飛ぶ
        if (_stageDatas[_gameScene])
        {
            SceneManager.LoadScene(_gameScene.ToString());
            currentScene = _gameScene;
        }
    }

    // ステージクリア時に次のステージ（インスペクターで設定）をアンロック
    void UnlockNextStage(GameScene gameScene)
    {
        if (!_stageDatas[gameScene])
        {
            _stageDatas[gameScene] = true;
        }
    }

    //bool IsInGame()
    //{
    //    return SceneManager.GetActiveScene().name == "SceneSelect" || SceneManager.GetActiveScene().name == "Title";
    //}

    //現在のシーンを返す
    public GameScene CurrentScene()
    {
        return currentScene;
    }
    //-----------------------------------ステージプレイ処理-----------------------------------//

    //セットアップ関数
    void SetUp()
    {
        Time.timeScale = 1.0f;

        _player = FindObjectOfType<PlayerMove>();
        _stageCtrl = FindObjectOfType<StageCtrl>();

        cheeseScore = 0;

        //現在のステージ番号によって、初速度を変えている。
        switch (stgNum)
        {
            //firstStage
            case 0:
                _currentSpeed = _firstStgPlySpeeds[0];
                break;
        }

        if (_stageCtrl != null)
        {
            StageCtrlSetUp();
        }
    }

    //外部のStageCtrlクラスの初期化(Find関数で取得するまで、行わないようにする)
    void StageCtrlSetUp()
    {
        _stageCtrl.cheeseParameters.fillAmount = 0;
        _stageCtrl.healthGaugeSlider.value = 1;
        _isStageCtrlGet = true;
    }

    //インゲーム内の更新処理
    void UpdateInGame(int num)
    {
        //チーズゲージの更新
        UpdateCheeseParameter();

        //チーズ保有率によるプレイヤー速度の更新
        UpdateCurrentSpeed(num);

        //プレイヤーの体力周りの更新
        ManageHealth();
    }

    void UpdateCheeseParameter()
    {
        //獲得チーズ数とステージに配置されたチーズ数を除算した結果を、チーズパラメーターに反映
        percentCheese = (float)cheeseScore / (float)_cheeseScores[stgNum];
        _stageCtrl.cheeseParameters.fillAmount = percentCheese;
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
                _currentSpeed = _firstStgPlySpeeds[cheeseScore];
                break;
        }
    }

    //PlayerMoveに、現在の速度を返す
    public float GetCurrentSpeed()
    {
        return _currentSpeed;
    }

    //プレイヤーが走っていたら、体力ゲージを減らす。0になったら、チーズを減らす
    void ManageHealth()
    {
        if (_player != null)
        {
            if (_player.IsRunningPlayer()) //プレイヤーが走っていたら
            {
                if (cheeseScore != 0)
                {
                    _stageCtrl.healthGaugeSlider.value -= Time.deltaTime * _gaugeDecreRate;
                }
            }
            else
            {
                //走っていなければスタミナ回復
                if (_stageCtrl.healthGaugeSlider.value < 1)
                {
                    _stageCtrl.healthGaugeSlider.value += Time.deltaTime * _gaugeIncreRate;
                }
            }
        }
        else
        {

        }

        if (_stageCtrl.healthGaugeSlider.value <= 0)
        {
            //チーズの取得数をデクリメントし、一つ下のスピードに変える
            cheeseScore--;
            //体力ゲージをリセット
            _stageCtrl.healthGaugeSlider.value = 1;
        }
    }

    //チーズを獲得したときのスコア更新
    public void GetCheese(int cheese)
    {
        cheeseScore += cheese;
        //新しくチーズをゲットしたら、体力ゲージをリセット
        _stageCtrl.healthGaugeSlider.value = 1;
    }

    public int RecentCheeseLimit()
    {
        return _cheeseScores[stgNum];
    }
}
