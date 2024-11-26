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
        alphaStage,
        FirstStage,
        SecondStage,
        ThirdStage,
    }

    public float percentCheese { get; private set; } //チーズ取得率

    [SerializeField] private List<StageData> _stages; // インスペクターでシーンをロックするか設定（ステージ以外（ + firstStage）はtrue）

    private PlayerMove _player;
    private StageCtrl _stageCtrl; //ステージUI切り替え周りの処理
    private Dictionary<GameScene, bool> _stageDatas = new(); //シーン遷移の際に消えないようにプライベートで保管
    private GameScene _gameScene;
    private int[] _cheeseScores = { 8, 0, 0 }; //各ステージのチーズ上限数
    private int _cheeseScore = 0; //チーズ取得数
    private int _stgNum = 0; //現在のステージ番号
    private float[] _firstStgPlySpeeds = { 6f, 7f, 8f, 8.5f, 9f, 9.5f, 10f, 10.5f, 11f, 11.5f }; //firstStageの速度一覧
    //private float[] secondStgPlySpeeds = { };    
    private float _currentSpeed = 0f; //現在のプレイヤー速度保管用   
    private float _normalRunning = 0.1f; //通常速度の体力ゲージ変化率
    private float _speedRunning = 0.3f; //ダッシュ時の体力ゲージ変化率
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
    }

    // Start is called before the first frame update
    void Start()
    {
        //LoadStageData();

        //セットアップ
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        //シーン遷移があったときの処理（データの初期化）
        if ((_stageCtrl == null) || (_player == null))
        {
            //ステージセレクト画面にいる時
            if (IsInGame())
            {
                //インゲーム外の時は、ここに来る
            }
            else //プレイステージにいる時
            {
                SetUp();
            }
        }

        //stageCtrlがある（インゲーム中）ときの処理
        if (_stageCtrl != null)
        {
            if (!_stageCtrl.doGameOver) //ゲームオーバーでなければ
            {
                //stageCtrlを取得するまで行わないようにする
                if (_isStageCtrlGet)
                {
                    UpdateInGame(_stgNum);
                }
                else
                {
                    StageCtrlSetUp();
                }
            }
            else //ゲームオーバーになったら
            {

            }
        }
    }

    //-----------------------------------ステージ遷移に関する処理-----------------------------------//

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
        if (_stageCtrl.doGameClear)
        {
            UnlockNextStage(_gameScene);
        }

        if (_stageDatas[_gameScene])
        {
            SceneManager.LoadScene(_gameScene.ToString());
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

    bool IsInGame()
    {
        return SceneManager.GetActiveScene().name == "SceneSelect" || SceneManager.GetActiveScene().name == "Title";
    }

    //-----------------------------------ステージプレイ処理-----------------------------------//

    //セットアップ関数
    void SetUp()
    {
        Time.timeScale = 1.0f;

        _player = FindObjectOfType<PlayerMove>();
        _stageCtrl = FindObjectOfType<StageCtrl>();

        _cheeseScore = 0;

        //現在のステージ番号によって、初速度を変えている。
        switch (_stgNum)
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
        percentCheese = (float)_cheeseScore / (float)_cheeseScores[_stgNum];
        _stageCtrl.cheeseParameters.fillAmount = percentCheese;
        Debug.Log(_cheeseScore);
        Debug.Log(percentCheese);
    }

    //速度の更新(変数は現在のステージ番号)
    void UpdateCurrentSpeed(int stageNum)
    {
        //プレイヤーの速度を、チーズの獲得数（cheeseScoreで管理）から変更
        switch (stageNum)
        {
            case 0:
                _currentSpeed = _firstStgPlySpeeds[_cheeseScore];
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
                if (_cheeseScore != 0)
                {
                    if (_player.IsPlayerDushing()) //ダッシュしているとき
                    {
                        _stageCtrl.healthGaugeSlider.value -= Time.deltaTime * _speedRunning;
                    }
                    else //通常の走りのとき
                    {
                        _stageCtrl.healthGaugeSlider.value -= Time.deltaTime * _normalRunning;
                    }
                }
            }
            else
            {
                //走っていなければスタミナ回復
                if (_stageCtrl.healthGaugeSlider.value < 1)
                {
                    _stageCtrl.healthGaugeSlider.value += Time.deltaTime * _normalRunning;
                }
            }
        }
        else
        {

        }

        if (_stageCtrl.healthGaugeSlider.value <= 0)
        {
            //チーズの取得数をデクリメントし、一つ下のスピードに変える
            _cheeseScore--;
            //体力ゲージをリセット
            _stageCtrl.healthGaugeSlider.value = 1;
        }
    }

    //チーズを獲得したときのスコア更新
    public void GetCheese(int cheese)
    {
        _cheeseScore += cheese;
        //新しくチーズをゲットしたら、体力ゲージをリセット
        _stageCtrl.healthGaugeSlider.value = 1;
    }
}
