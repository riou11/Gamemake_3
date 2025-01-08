using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;
using System.Threading;

//インゲーム中の、進行度等に応じたUI遷移の管理クラス

public class StageCtrl : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;

    public enum PlayState
    {
        Playing,
        Pause,
        GameOver,
        GameClear
    }

    //[Header("チーズパラメーターUI")]
    //[SerializeField] public Image cheeseParameters;
    [Header("体力ゲージUI")]
    [SerializeField] public Slider healthGaugeSlider;

    //[Header("プレイヤーゲームオブジェクト")]
    //public GameObject playerObj;
    [Header("ゲームオーバー")]
    public GameObject gameOverObj;
    [Header("ステージクリア")]
    public GameObject stageClrObj;
    [Header("インゲームUI")]
    public GameObject InGameUIObj;
    [Header("次のステージ")]
    public int nextStage;
    [Header("猫のダメージ絵")]//rio
    public GameObject CatDamageObj;

    /// <summary>
    /// リザルト画面UI
    /// </summary>
    //[Header("GameClearのロゴ")]
    //[SerializeField] private GameObject _clearText;
    [Header("「Press Any Key」の画像")]
    [SerializeField] private GameObject _messageText;
    //[Header("リザルト画面のネズミの画像")]
    //[SerializeField] private GameObject[] _ratImgObjs;
    [Header("リザルト画面のネズミと吹き出しと星の画像")]
    [SerializeField] private GameObject[] _resultImgObjs;

    /// <summary>
    /// リザルト画面の次の選択画面UI
    /// </summary>
    [Header("遷移選択画面")]
    [SerializeField] private GameObject _transSelectionUI;
    //[Header("Retryボタン")]
    //[SerializeField] private Button _retryButton;

    /// <summary>
    /// ボタン関係
    /// </summary>
    [Header("Retryボタン")]
    [SerializeField] private UnityEngine.UI.Button _retryButton;
    [Header("NextStageボタン")]
    [SerializeField] private UnityEngine.UI.Button _nextStageButton;
    [Header("失敗時のBackToTitleボタン")]
    [SerializeField] private UnityEngine.UI.Button _gmBackToTitleButton;
    [Header("Clear時のBackToTitleボタン")]
    [SerializeField] private UnityEngine.UI.Button _gcBackToTitleButton;

    [SerializeField] private Image _cheeseBase; //チーズの土台
    [SerializeField] private Image[] _cheeseImgs = new Image[6]; //cheeseの画像
    [SerializeField] private PlayerMove _player;
    [SerializeField] private int stgNum; //現在のステージ番号(0か1)
    public float percentCheese { get; private set; } //チーズ取得率

    public int cheeseScore { get; private set; }//チーズ取得数

    public bool getReady = false;
    public bool doGameOver = false;
    public bool doGameClear = false;

    private PlayState _playState;
    private bool retryGame = false;
    private int nextStageNum;
    private float _evalution = 0f; //（獲得チーズ数 / そのステージの上限チーズ数）の計算結果
    private int _value = 0; //評価値計算過程の計算結果格納用（評価値の査定を、整数値で行いたいためint型）
    private int _result = 0; //そのステージの評価値（星の数）、保存はされない
    private int[] _cheeseScores = { 18, 0, 0 }; //各ステージのチーズ上限数
    private float[] _firstStgPlySpeeds = { 6f, 7f, 8f, 8.5f, 9f, 9.5f, 10f, 10.5f, 11f, 11.5f, 12f, 13f, 13.5f, 14f, 15.5f, 16f, 17f }; //firstStageの速度一覧  
    private float _currentSpeed = 0f; //現在のプレイヤー速度保管用   
    private float _gaugeDecreRate = 0.3f; //体力ゲージ減少率
    private float _gaugeIncreRate = 0.1f; //体力ゲージ回復率




    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (doGameClear)
        {
            TransSelection();
        }
        else if (doGameOver)
        {

        }
        else
        {
            UpdateInGame(stgNum);
        }
    }

    void UpdateInGame(int num)
    {
        //チーズゲージの更新
        UpdateCheeseParameter();

        //チーズ保有率によるプレイヤー速度の更新
        UpdateCurrentSpeed(num);

        //プレイヤーの体力周りの更新
        ManageHealth();
    }

    void SetUp()
    {
        Time.timeScale = 1.0f;

        cheeseScore = 0;
        _playState = PlayState.Playing;
        SoundManager.Instance.PlayBGM(SoundManager.SoundType.Stage1);

        ButtonSetUp();

        if (_player != null && gameOverObj != null && stageClrObj != null && InGameUIObj != null)
        {
            UIImgSetUp();
            doGameOver = false;
            doGameClear = false;
        }
        else
        {
            Debug.Log("設定が足りてないよ！");
        }

        //現在のステージ番号によって、初速度を変えている。
        switch (stgNum)
        {
            //firstStage
            case 0:
                _currentSpeed = _firstStgPlySpeeds[0];
                break;
        }

        CheeseGaugeSetUp();
    }

    void CheeseGaugeSetUp()
    {
        //cheeseParameters.fillAmount = 0;
        healthGaugeSlider.value = 1;
    }

    //ステージ開始時のUIセットアップ
    void UIImgSetUp()
    {
        _transSelectionUI.SetActive(false);
        gameOverObj.SetActive(false);
        stageClrObj.SetActive(false);
        CatDamageObj.SetActive(false);
        InGameUIObj.SetActive(true);
    }

    //GameManagerがシングルトンの影響でボタンにアタッチしたデータがMisiingしてしまうため、スクリプトでOnClick()を追加
    void ButtonSetUp()
    {
        if (gameManager != null)
        {
            _gmBackToTitleButton.onClick.AddListener(() => gameManager.TransitionScene((int)GameManager.GameScene.Title));
            _gcBackToTitleButton.onClick.AddListener(() => gameManager.TransitionScene((int)GameManager.GameScene.Title));
            _retryButton.onClick.AddListener(() => Retry());

            switch (gameManager.currentScene)
            {
                case GameManager.GameScene.ReFirstStage:
                    _nextStageButton.onClick.AddListener(() => gameManager.TransitionScene((int)GameManager.GameScene.SecondStageGimmick));
                    break;
                case GameManager.GameScene.SecondStageGimmick:                    
                    //SecondStageの次を調べようとすると配列が範囲外になるため、ここでは前のステージに戻るようにしている
                    _nextStageButton.onClick.AddListener(() => gameManager.TransitionScene((int)GameManager.GameScene.ReFirstStage));
                    break;
            }
        }
    }

    void UpdateCheeseParameter()
    {
        var score = CalcCheeseScore(cheeseScore);
        CheeseImgUpdate(score);

        //獲得チーズ数とステージに配置されたチーズ数を除算した結果を、チーズパラメーターに反映
        //percentCheese = (float)cheeseScore / (float)_cheeseScores[stgNum];
        //cheeseParameters.fillAmount = percentCheese;
        //Debug.Log(cheeseScore);
        //Debug.Log(percentCheese);
    }

    //左上のチーズメーターのImageの更新（描画）
    void CheeseImgUpdate(int score)
    {
        ImageAlphaChange(score);
    }

    //ImageのAlpha値の変更
    void ImageAlphaChange(int score)
    {
        foreach (var img in _cheeseImgs)
        {
            var imgColor = img.color;
            imgColor.a = 0f;
            img.color = imgColor;
        }

        if (score == 0)
        {
            return;
        }

        for (int i = 0; i < score; ++i)
        {
            var imgColor = _cheeseImgs[i].color;
            imgColor.a = 1f;
            _cheeseImgs[i].color = imgColor;
        }
    }

    //獲得チーズ数が6個を越えたら6を渡す。それ以外は獲得分を返す
    int CalcCheeseScore(int count)
    {
        if (count > 6)
        {
            return 6;
        }
        else
        {
            return count;
        }
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
                    healthGaugeSlider.value -= Time.deltaTime * _gaugeDecreRate;
                }
            }
            else
            {
                //走っていなければスタミナ回復
                if (healthGaugeSlider.value < 1)
                {
                    healthGaugeSlider.value += Time.deltaTime * _gaugeIncreRate;
                }
            }
        }
        else
        {

        }

        if (healthGaugeSlider.value <= 0)
        {
            //チーズの取得数をデクリメントし、一つ下のスピードに変える
            cheeseScore--;
            //体力ゲージをリセット
            healthGaugeSlider.value = 1;
        }
    }

    //チーズを獲得したときのスコア更新
    public void GetCheese(int cheese)
    {
        cheeseScore += cheese;
        //新しくチーズをゲットしたら、体力ゲージをリセット
        healthGaugeSlider.value = 1;
    }

    public int RecentCheeseLimit()
    {
        return _cheeseScores[stgNum];
    }

    //爆弾チーズを獲ってしまった時のゲームオーバー処理
    public void OnCheeseCollected()
    {
        Debug.Log("爆弾チーズが取得されました！");
        InGameUIObj.SetActive(false);

        StartCoroutine(ShowGameOverWithDelay());

        //gameOverObj.SetActive(true);
        //SoundManager.Instance.PlayBGM(SoundManager.SoundType.GameOver);

        //doGameOver = true;
        //Time.timeScale = 0f;
    }

    //敵に捕まった時のゲームオーバー処理
    public void OnEnemyCollected()
    {
        Debug.Log("敵とプレイヤーが接触しました！");
        InGameUIObj.SetActive(false);

        StartCoroutine(ShowGameOverWithDelay());
    }


    public void OnCatDamage() //rio
    {

        CatDamageObj.SetActive(true);
        StartCoroutine(CatDamageWithDelay());
    }

    private IEnumerator CatDamageWithDelay()//rio
    {
        yield return new WaitForSeconds(1.5f); // x秒待つ (必要に応じて変更)
 
        CatDamageObj.SetActive(false);
    }


    private IEnumerator ShowGameOverWithDelay()//プレイヤーの死んだモーションを見せるための時間
    {
        yield return new WaitForSeconds(2.0f); // 2秒待つ（必要に応じて変更）

        HandlingOfGameOverUI();

        SoundManager.Instance.PlayBGM(SoundManager.SoundType.GameOver);

        doGameOver = true;
        Time.timeScale = 0f;
    }

    //GameOver時のUI周りの処理
    void HandlingOfGameOverUI()
    {
        gameOverObj.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_retryButton.gameObject);
    }

    public void BackToTitle()
    {
        gameManager.TransitionScene((int)GameManager.GameScene.Title);
    }

    //Retryの時に呼び出す関数
    public void Retry()
    {
        gameManager.TransitionScene((int)gameManager.currentScene);
    }

    public void Retry0()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SoundType.ButtonClick);

        retryGame = true;
        ChangeScene(0); //最初のステージに戻るので1
    }
    public void Retry1()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SoundType.ButtonClick);

        retryGame = true;
        ChangeScene(1); //最初のステージに戻るので1

    }

    public void Retry2()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SoundType.ButtonClick);

        retryGame = true;
        ChangeScene(2);
    }

    public void ChangeScene(int num)
    {
        nextStageNum = num;
        SceneManager.LoadScene(nextStageNum); // シーンを変更する
    }

    //ゴールに着いた時に呼び出す処理
    public void arrivedGoal()
    {
        doGameClear = true;
        stageClrObj.SetActive(true);
        InGameUIObj.SetActive(false);
        StartCoroutine(ClearEvent());
    }

    //リザルト画面周りの処理
    private IEnumerator ClearEvent()
    {
        //yield return new WaitForSeconds(0.5f);
        //_clearText.SetActive(true);
        _result = CalcEvalution(cheeseScore);

        //yield return new WaitForSeconds(0.5f);

        //_ratImgObjs[_result].SetActive(true);
        _resultImgObjs[_result].SetActive(true);

        yield return new WaitForSeconds(1.0f);

        _messageText.SetActive(true);

        Time.timeScale = 0f;
    }

    int CalcEvalution(int score)
    {
        _evalution = 0f;

        _evalution = score / RecentCheeseLimit();

        _value = (int)(_evalution * 10);

        if (_value <= 4)
        {
            return 0;
        }
        else if ((_value > 4) && (_value <= 8))
        {
            return 1;
        }
        else if (_value > 8)
        {
            return 2;
        }
        else
        {
            Debug.Log("calculation didn't go well");
            return 0;
        }
    }

    //「Press Any Key」が表示された後の処理
    void TransSelection()
    {
        //「Press Any Key」と表示されたら
        if (_messageText.activeSelf)
        {
            if (Input.anyKey)
            {
                ResultImgPassive();
                TransSelecImgActive();
            }
        }
        else if (_transSelectionUI.activeSelf) //遷移選択画面に飛んだら
        {

        }
    }
    
    //リザルト画面の非表示
    void ResultImgPassive()
    {
        //_clearText.SetActive(false);
        //_ratImgObjs[_result].SetActive(false);
        _resultImgObjs[_result].SetActive(false);
        _messageText.SetActive(false);
    }

    //遷移選択画面の表示
    void TransSelecImgActive()
    {
        _transSelectionUI.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_nextStageButton.gameObject);
    }

    public void ToNextStage()
    {
        SceneManager.LoadScene(nextStage);
    }

}
