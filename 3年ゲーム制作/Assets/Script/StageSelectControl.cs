using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class StageSelectControl : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;

    [SerializeField] private RectTransform panel; // スライドさせるパネル
    [SerializeField] private Vector2 targetPosition; // 目標位置
    [SerializeField] private float slideDuration = 0.5f; // スライドにかかる時間
    [SerializeField] private float blinkInterval = 0.5f;
    //[SerializeField] private KeyCode slideKey = KeyCode.RightArrow; // スライドさせるキー
    //[SerializeField] private KeyCode resetKey = KeyCode.LeftArrow; // 元に戻すキー
    [SerializeField] private Image _rightArrow;
    [SerializeField] private Image _leftArrow;

    private Vector2 originalPosition; // 元の位置
    private Coroutine blinkCoroutine;


    public enum SelectState
    {
        FirstStage,
        SecondStage
    }

    private SelectState _state;

    // Start is called before the first frame update
    void Start()
    {
        _state = SelectState.FirstStage;

        if (panel == null)
        {
            panel = GetComponent<RectTransform>();
        }

        // 初期位置を保存
        originalPosition = panel.anchoredPosition;
        Debug.Log(originalPosition);

        StartBlinking(_state);
    }

    // Update is called once per frame
    void Update()
    {
        //方向キーを押されたら、パネルを移動
        switch (_state)
        {
            case SelectState.FirstStage:
                if ((Gamepad.current.dpad.right.wasPressedThisFrame) || (Gamepad.current.leftStick.right.wasPressedThisFrame))
                {
                    StopBlinking(_state);
                    _state = SelectState.SecondStage;
                    StopAllCoroutines(); // 途中のアニメーションを中断
                    StartCoroutine(SlideTo(targetPosition, _state));
                }
                else if (Gamepad.current.buttonSouth.wasPressedThisFrame) 
                {
                    StopAllCoroutines();
                    gameManager.TransitionScene((int)GameManager.GameScene.ReFirstStage);
                }
                break;
            case SelectState.SecondStage:
                if ((Gamepad.current.dpad.left.wasPressedThisFrame) || (Gamepad.current.leftStick.left.wasPressedThisFrame))
                {
                    StopBlinking(_state);
                    _state = SelectState.FirstStage;
                    StopAllCoroutines(); // 途中のアニメーションを中断
                    StartCoroutine(SlideTo(originalPosition, _state));
                }
                else if (Gamepad.current.buttonSouth.wasPressedThisFrame) 
                {
                    StopAllCoroutines();
                    gameManager.TransitionScene((int)GameManager.GameScene.SecondStage);
                }
                break;
            default:
                break;
        }

        if (Gamepad.current.buttonEast.wasPressedThisFrame) 
        {
            gameManager.TransitionScene((int)GameManager.GameScene.SceneSelect);
        }
    }

    //矢印の点滅を開始させる関数
    public void StartBlinking(SelectState state)
    {
        if (blinkCoroutine == null) // 二重に開始しないようチェック
        {
            blinkCoroutine = StartCoroutine(BlinkArrowCoroutine(state));
        }
    }

    //矢印の点滅を停止させる関数
    public void StopBlinking(SelectState state)
    {
        if (blinkCoroutine != null) // コルーチンが実行中の場合のみ停止
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
            HiddenArrow(state); // 停止時の状態をリセット
        }
    }

    //矢印を非表示にする関数
    private void HiddenArrow(SelectState state)
    {
        switch (state)
        {
            case SelectState.FirstStage:
                // アルファ値を0にし、非表示にする
                var rightArrowColor = _rightArrow.color;
                rightArrowColor.a = 0f;
                _rightArrow.color = rightArrowColor;
                break;
            case SelectState.SecondStage:
                // アルファ値を0にし、非表示にする
                var leftArrowColor = _leftArrow.color;
                leftArrowColor.a = 0f;
                _leftArrow.color = leftArrowColor;
                break;
        }  
    }

    //矢印を点滅させるコルーチン関数
    private IEnumerator BlinkArrowCoroutine(SelectState state)
    {
        while (true)
        {
            switch (state)
            {
                case SelectState.FirstStage:
                    // アルファ値を変える（透明⇔不透明）
                    var rightArrowColor = _rightArrow.color;
                    rightArrowColor.a = (rightArrowColor.a == 1f) ? 0f : 1f;
                    _rightArrow.color = rightArrowColor;
                    break;
                case SelectState.SecondStage:
                    // アルファ値を変える（透明⇔不透明）
                    var leftArrowColor = _leftArrow.color;
                    leftArrowColor.a = (leftArrowColor.a == 1f) ? 0f : 1f;
                    _leftArrow.color = leftArrowColor;
                    break;
            }
            

            yield return new WaitForSeconds(blinkInterval);
        }
    }

    //画面をスライドさせるコルーチン関数
    private IEnumerator SlideTo(Vector2 targetPos, SelectState state)
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

        StartBlinking(state);
    }

    public void StopCoroutine()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
    }
}
