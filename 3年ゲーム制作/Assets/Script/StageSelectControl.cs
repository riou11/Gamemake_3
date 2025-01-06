using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectControl : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;

    //[SerializeField] private GameObject _stageSelectPanel;
    //[SerializeField] private GameObject _thickFrame;
    //[SerializeField] private Vector3 _firstStageSelectPosition;
    //[SerializeField] private Vector3 _secondStageSelectPosition;

    [SerializeField] private RectTransform panel; // スライドさせるパネル
    [SerializeField] private Vector2 targetPosition; // 目標位置
    [SerializeField] private float slideDuration = 0.5f; // スライドにかかる時間
    [SerializeField] private KeyCode slideKey = KeyCode.RightArrow; // スライドさせるキー
    [SerializeField] private KeyCode resetKey = KeyCode.LeftArrow; // 元に戻すキー
    private Vector2 originalPosition; // 元の位置


    public enum SelectState
    {
        FirstStage,
        SecondStage
    }

    private SelectState _state;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(_stageSelectPanel.transform.position);
        _state = SelectState.FirstStage;
        //_stageSelectPanel.transform.position = _firstStageSelectPosition;

        if (panel == null)
        {
            panel = GetComponent<RectTransform>();
        }

        // 初期位置を保存
        originalPosition = panel.anchoredPosition;
        Debug.Log(originalPosition);
    }

    // Update is called once per frame
    void Update()
    {
        //// スライドキーが押された場合
        //if (Input.GetKeyDown(slideKey))
        //{
        //    StopAllCoroutines(); // 途中のアニメーションを中断
        //    StartCoroutine(SlideTo(targetPosition));
        //}
        //// リセットキーが押された場合
        //else if (Input.GetKeyDown(resetKey))
        //{
        //    StopAllCoroutines(); // 途中のアニメーションを中断
        //    StartCoroutine(SlideTo(originalPosition));
        //}

        //方向キーを押されたら、パネルを移動
        switch (_state)
        {
            case SelectState.FirstStage:
                //if (Input.GetKey(KeyCode.RightArrow))
                //{
                //    _state = SelectState.SecondStage;
                //    _stageSelectPanel.transform.position = _secondStageSelectPosition;
                //}
                if (Input.GetKeyDown(slideKey))
                {
                    _state = SelectState.SecondStage;
                    StopAllCoroutines(); // 途中のアニメーションを中断
                    StartCoroutine(SlideTo(targetPosition));
                }
                else if (Input.GetKey(KeyCode.Return))
                {
                    gameManager.TransitionScene((int)GameManager.GameScene.ReFirstStage);
                }
                break;
            case SelectState.SecondStage:
                //if (Input.GetKey(KeyCode.LeftArrow))
                //{
                //    _state = SelectState.FirstStage;
                //    _stageSelectPanel.transform.position = _firstStageSelectPosition;
                //}
                if (Input.GetKeyDown(resetKey))
                {
                    _state = SelectState.FirstStage;
                    StopAllCoroutines(); // 途中のアニメーションを中断
                    StartCoroutine(SlideTo(originalPosition));
                }
                else if (Input.GetKey(KeyCode.Return))
                {
                    gameManager.TransitionScene((int)GameManager.GameScene.SecondStage);
                }
                break;
            default:
                break;
        }
    }

    private System.Collections.IEnumerator SlideTo(Vector2 targetPos)
    {
        Vector2 startPos = panel.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < slideDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = targetPos;
    }
}
