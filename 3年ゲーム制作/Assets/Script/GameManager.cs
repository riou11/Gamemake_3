using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    //シーン名とロック状況
    //[System.Serializable]
    //public class StageData
    //{
    //    public GameScene gameScene;
    //    public bool isUnlocked;
    //}

    //ゲームのシーンを、番号ではなくenemを使ってシーン名で管理
    public enum GameScene
    {
        Title,
        SceneSelect,//Menu,StageSelect,Option,PlayGuideを、このシーンのなかで切り替え
        StageSelect,
        ReFirstStage,
        SecondStage,
    }

    //[SerializeField] private List<StageData> _stages; // インスペクターでシーンをロックするか設定（ステージ以外（ + firstStage）はtrue）

    public GameScene currentScene { get; private set; } //現在のシーン

    private TitleControl titleControl;
    private MenuSelectControl menuSelectControl;
    private StageSelectControl stageSelectControl;
    private PlayerMove _player;
    private StageCtrl _stageCtrl; //ステージUI切り替え周りの処理
    //private Dictionary<GameScene, bool> _stageDatas = new(); //シーン遷移の際に消えないようにプライベートで保管
    private GameScene _gameScene; //ステージ遷移に使う変数（現在のシーンを示すものではない）
    private bool _isStageCtrlGet = false; //各ステージのStageCtrl（UI管理）を取得したか
    private bool _getReady = false;


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
        //LoadStagesData();
        SearchCurrentScene();
    }

    // Start is called before the first frame update
    void Start()
    {

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
            case GameScene.StageSelect:
                ProcessStageSelect();
                break;
            case GameScene.ReFirstStage:
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
        titleControl = FindObjectOfType<TitleControl>();
    }

    void ProcessSceneSelect()
    {
        menuSelectControl = FindObjectOfType<MenuSelectControl>();
    }

    void ProcessStageSelect()
    {
        stageSelectControl = FindObjectOfType<StageSelectControl>();
    }

    void ProcessStage()
    {
        if (!_getReady)
        {
            if (!_isStageCtrlGet)
            {
                _stageCtrl = FindObjectOfType<StageCtrl>();
                _isStageCtrlGet = true;
            }

            if (_stageCtrl != null)
            {
                _getReady = true;
            }
        }  
    }

    //現在のシーンを返す関数
    void SearchCurrentScene()
    {
        var index = SceneManager.GetActiveScene().buildIndex;
        currentScene = (GameScene)index;
    }

    //インスペクターで設定された内容を、プライベートに取り込む（FirstStageのみ開放）
    //void LoadStagesData()
    //{
    //    foreach (var stageData in _stages)
    //    {
    //        _stageDatas[stageData.gameScene] = stageData.isUnlocked;
    //    }

    //    _stageDatas[GameScene.ReFirstStage] = true;
    //}

    //ステージ遷移（ボタンにこの関数を入れて、飛びたいシーンの番号(enum(GameScene)で定義)を設定する）
    public void TransitionScene(int scene)
    {

        _gameScene = (GameScene)scene;

        StopCoroutine(currentScene);
        SceneManager.LoadScene(_gameScene.ToString());
        currentScene = _gameScene;

        //クリア時はステージを開放する
        //if (_stageCtrl != null)
        //{
        //    if (_stageCtrl.doGameClear)
        //    {
        //        UnlockNextStage(_gameScene);
        //    }
        //}

        //開放されていれば次のステージに飛ぶ
        //if (_stageDatas[_gameScene])
        //{
        //    StopCoroutine(currentScene);
        //    SceneManager.LoadScene(_gameScene.ToString());
        //    currentScene = _gameScene;
        //}
    }

    // ステージクリア時に次のステージ（インスペクターで設定）をアンロック
    //void UnlockNextStage(GameScene gameScene)
    //{
    //    if (!_stageDatas[gameScene])
    //    {
    //        _stageDatas[gameScene] = true;
    //    }
    //}

    //それぞれのシーンのコルーチンを遷移前に止める（必要ないかも）
    void StopCoroutine(GameScene scene)
    {
        switch (scene)
        {
            case GameScene.Title:
                titleControl.StopCoroutine();
                break;
            case GameScene.SceneSelect:
                menuSelectControl.StopCoroutine();
                break;
            case GameScene.StageSelect:
                stageSelectControl.StopCoroutine();
                break;
            case GameScene.ReFirstStage:
                _getReady = false;
                _isStageCtrlGet = false;
                break;
            case GameScene.SecondStage:
                _getReady = false;
                _isStageCtrlGet = false;
                break;
            default:
                break;
        }
    }

    //現在のシーンを返す
    public GameScene CurrentScene()
    {
        return currentScene;
    }
}
