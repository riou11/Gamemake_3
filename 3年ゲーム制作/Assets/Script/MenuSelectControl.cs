using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

/// <summary>
/// SceneSelectシーンのスクリプト
/// </summary>

public class MenuSelectControl : MonoBehaviour
{
    //SubMenu(Menuから選択できるメニュー)をenumで識別(Inspectorは数字で区別)
    public enum SubPanel
    {
        None = 0, //MainMenu 0
        StageSelect = 1, //StageSelect 1
        Option = 2, //Option 2
        PlayGuide = 3, //PlayGuide 3
    }

    [System.Serializable]
    public enum Stage
    {
        FirstStage,
        SecondStage,
        ThirdStage
    }

    [System.Serializable]
    public class SubPanelData
    {
        public SubPanel panel;
        public bool isSelected;
    }

    //Inspectorで登録する、Stage名と、それに対応するボタン
    [System.Serializable]
    public class StageButtonData
    {
        public GameManager.GameScene stage;
        public UnityEngine.UI.Button button;
    }

    GameManager manager => GameManager.Instance;

    /// <summary>
    /// Panel取り込み
    /// </summary>
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject StageSelectPanel;
    //[SerializeField] private GameObject OptionPanel;
    [SerializeField] private GameObject PlayGuidePanel;

    [SerializeField] private GameObject[] PlayGuideImages; //ゲーム説明スライド
    /// <summary>
    /// 初期ボタン取り込み
    /// </summary>
    [SerializeField] private GameObject MainMenuPanelFirstButton;
    //[SerializeField] private GameObject StageSelectPanelFirstButton;
    //[SerializeField] private GameObject OptionPanelFirstButton;
    //[SerializeField] private GameObject PlayGuidePanelFirstButton;

    [SerializeField] private ButtonScaler stageSelect;
    [SerializeField] private ButtonScaler playGuide;
    //[SerializeField] private ButtonScaler option;

    [SerializeField] private StageButtonData[] StageButtons;

    [SerializeField] private List<SubPanelData> SubPanels;


    private Dictionary<SubPanel, bool> _subMenuData = new Dictionary<SubPanel, bool>(); //各サブメニュー（StageSelect,Option,PlayGuide）の表示状態の保持
    private Dictionary<GameManager.GameScene, UnityEngine.UI.Button> _stageButtonsData = new Dictionary<GameManager.GameScene, UnityEngine.UI.Button>(); //(追伸)これ、いらないかも。 このスクリプト上で保持する各ステージに飛ぶボタンの情報

    private int slideCount = 0;
    bool _subSelected;
    SubPanel _subPanel;
    //private bool _isActiveMainMenu;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
        //LoadOnClickData();
    }

    // Update is called once per frame
    void Update()
    {
        //PlayGuideかOptionが開かれたときに行う処理
        if (_subMenuData[SubPanel.PlayGuide])
        {
            UnitPlayGuide();          
        }
        //else if (_subMenuData[SubPanel.Option])
        //{
        //    if (Gamepad.current.buttonEast.wasPressedThisFrame)
        //    {
        //        SelectedMenuPanel(SubPanel.Option);
        //    }
        //}
        else
        {
            //一つ前の画面に戻る処理
            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                manager.TransitionScene((int)GameManager.GameScene.Title);
            }
        }  
    }


    //最初のUI表示
    void SetUp()
    {
        StageSelectPanel.SetActive(false);
        //OptionPanel.SetActive(false);
        PlayGuidePanel.SetActive(false);
        MainMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MainMenuPanelFirstButton);

        _subSelected = false;
        _subPanel = SubPanel.None;

        foreach (var subPanel in SubPanels)
        {
            _subMenuData[subPanel.panel] = subPanel.isSelected;
        }

        LoadStageButtonsData();
    }

    //PlayGuide画面の時の処理
    void UnitPlayGuide()
    {
        foreach (var img in PlayGuideImages)
        {
            img.SetActive(false);
        }

        if ((Gamepad.current.dpad.right.wasPressedThisFrame) || (Gamepad.current.leftStick.right.wasPressedThisFrame))
        {
            if (slideCount < 3)
            {
                slideCount++;
            }
        }
        else if ((Gamepad.current.dpad.left.wasPressedThisFrame) || (Gamepad.current.leftStick.left.wasPressedThisFrame))
        {
            if (slideCount > 0)
            {
                slideCount--;
            }
        }

        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            SelectedMenuPanel(SubPanel.PlayGuide);
        }

        if (slideCount > 3)
        {
            slideCount = 3;
        }
        else if (slideCount < 0)
        {
            slideCount = 0;
        }

        PlayGuideImages[slideCount].SetActive(true);
    }

    //ステージボタン情報のロード
    void LoadStageButtonsData()
    {
        foreach (var stageData in StageButtons)
        {
            if (stageData != null)
            {
                //Dictionary越しだとOnClick()を登録出来ないらしいので、シリアライズから直接AddListenerしたら出来た。
                stageData.button.onClick.AddListener(() => manager.TransitionScene((int)stageData.stage));
                _stageButtonsData[stageData.stage] = stageData.button;
            }
        }
    }

    //Stage選択ボタンにOnClick関数を追加（GameManagerの関数をアタッチするが、シングルトンの影響で消えてしまうため）
    //void LoadOnClickData()
    //{
    //    if (IsButtonActive())
    //    {
    //        foreach (var _stageData in _stageButtonsData)
    //        {
    //            if (_stageData.Value != null)
    //            {
    //                Debug.Log(_stageData.Key);
    //                Debug.Log((int)_stageData.Key);
    //                //_stageData.Value.onClick.AddListener(() => manager.TransitionScene((int)_stageData.Key));
    //            }
    //        }
    //    }
    //}

    //Stage選択ボタンがアクティブになっているか
    //bool IsButtonActive()
    //{
    //    foreach (var data in _stageButtonsData)
    //    {
    //        if (!data.Value.gameObject.activeSelf)
    //        {
    //            return false;
    //        }
    //    }

    //    return true;
    //}

    //ボタンが押されたら（全ボタン共通）(numは行先)
    public void SelectedPanel(int num)
    {
        _subSelected = false;
        _subPanel = SubPanel.None;

        //MainMenuへの遷移時のみ処理
        if (num == 0)
        {
            foreach (var data in _subMenuData) //SubMenuが開かれているか判定
            {
                if (_subMenuData[data.Key])
                {
                    _subSelected = true;
                    _subPanel = data.Key;
                    break;
                }
            }
        }

        //CheckSelectButton(num);
        if (!_subSelected)
        {
            _subPanel = (SubPanel)num;
        }

        //プレイヤーが現在見ている画面に応じて、メニュー遷移の処理を変える
        if (_subSelected)
        {
            SelectedMenuPanel(_subPanel);
        }
        else
        {
            SelectedSubPanel(_subPanel);
        }
    }

    //メインメニュー（Menu）に戻る時
    void SelectedMenuPanel(SubPanel subPanel)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MainMenuPanelFirstButton);

        MainMenuPanel.SetActive(true);

        switch (subPanel)
        {
            case SubPanel.StageSelect:
                StageSelectPanel.SetActive(false);
                _subMenuData[subPanel] = false;
                break;
            //case SubPanel.Option:
            //    OptionPanel.SetActive(false);
            //    _subMenuData[subPanel] = false;
            //    break;
            case SubPanel.PlayGuide:
                PlayGuidePanel.SetActive(false);
                _subMenuData[subPanel] = false;
                break;
            default:
                break;
        }

    }

    //いずれかのサブメニュー（StageSelect,Option,PlayGuide）が選択されたとき
    void SelectedSubPanel(SubPanel subPanel)
    {
        EventSystem.current.SetSelectedGameObject(null);

        switch (subPanel)
        {
            case SubPanel.StageSelect:
                manager.TransitionScene((int)GameManager.GameScene.StageSelect);
                break;
            //case SubPanel.Option:
            //    //EventSystem.current.SetSelectedGameObject(OptionPanelFirstButton);
            //    MainMenuPanel.SetActive(false);
            //    OptionPanel.SetActive(true);
            //    _subMenuData[subPanel] = true;
            //    break;
            case SubPanel.PlayGuide:
                //EventSystem.current.SetSelectedGameObject(PlayGuidePanelFirstButton);
                MainMenuPanel.SetActive(false);
                PlayGuidePanel.SetActive(true);
                _subMenuData[subPanel] = true;
                slideCount = 0;
                break;
            default:
                break;
        }
    }

    public void StopCoroutine()
    {
        stageSelect.OnDeleated();
        playGuide.OnDeleated();
        //option.OnDeleated();
    }
}
