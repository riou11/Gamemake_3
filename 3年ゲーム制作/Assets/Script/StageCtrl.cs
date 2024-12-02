using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//インゲーム中の、進行度等に応じたUI遷移の管理クラス

public class StageCtrl : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;

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
    [SerializeField] private GameObject _retryButton;
    [Header("NextStageボタン")]
    [SerializeField] private GameObject _nextStageButton;
    //[Header("失敗時のBackToTitleボタン")]
    //[SerializeField] private GameObject _gmBackToTitleButton;
    //[Header("Clear時のBackToTitleボタン")]
    //[SerializeField] private GameObject _gcBackToTitleButton;

    
    public bool doGameOver = false;
    public bool doGameClear = false;
    private bool retryGame = false;
    private int nextStageNum;

    private float _evalution = 0f; //（獲得チーズ数 / そのステージの上限チーズ数）の計算結果
    private int _value = 0; //評価値計算過程の計算結果格納用（評価値の査定を、整数値で行いたいためint型）
    private int _result = 0; //そのステージの評価値（星の数）、保存はされない


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        SoundManager.Instance.PlayBGM(SoundManager.SoundType.Stage1);

        //gameManager = FindObjectOfType<GameManager>();

        if (playerObj != null && gameOverObj != null && stageClrObj != null && InGameUIObj != null)
        {
            UIImgSetUp();
            doGameOver = false;
            doGameClear = false;
        }
        else
        {
            Debug.Log("設定が足りてないよ！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doGameClear)
        {
            TransSelection();
        }
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

    //爆弾チーズを獲ってしまった時のゲームオーバー処理
    public void OnCheeseCollected()
    {
        Debug.Log("爆弾チーズが取得されました！");
        InGameUIObj.SetActive(false);
        gameOverObj.SetActive(true);
        SoundManager.Instance.PlayBGM(SoundManager.SoundType.GameOver);

        doGameOver = true;
        Time.timeScale = 0f;
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

        gameOverObj.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_retryButton);

        SoundManager.Instance.PlayBGM(SoundManager.SoundType.GameOver);

        doGameOver = true;
        Time.timeScale = 0f;
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void Retry()
    {
        SceneManager.LoadScene(gameManager.CurrentStage().ToString());
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
    IEnumerator ClearEvent()
    {
        //yield return new WaitForSeconds(0.5f);
        //_clearText.SetActive(true);
        _result = CalcEvalution(gameManager.cheeseScore);

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

        _evalution = score / gameManager.RecentCheeseLimit();

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
        EventSystem.current.SetSelectedGameObject(_nextStageButton);
    }

    public void ToNextStage()
    {
        SceneManager.LoadScene(nextStage);
    }

}
