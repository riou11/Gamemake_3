using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour
{
    [SerializeField] private Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f); // 選択時のスケール
    [SerializeField] private float transitionDuration = 0.2f; // スケール変更のアニメーション時間
    private Vector3 originalScale; // 元のスケール
    private Coroutine scaleCoroutine; // 実行中のコルーチン

    private void Awake()
    {
        // 初期スケールを保存
        originalScale = transform.localScale;

        // EventTrigger をセットアップ
        SetupEventTrigger();
    }

    private void SetupEventTrigger()
    {
        // EventTrigger コンポーネントを取得または追加
        EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        // 古いイベントをクリア
        eventTrigger.triggers.Clear();

        // Select イベントの設定
        EventTrigger.Entry selectEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Select
        };
        selectEntry.callback.AddListener((eventData) => OnSelect());
        eventTrigger.triggers.Add(selectEntry);

        // Deselect イベントの設定
        EventTrigger.Entry deselectEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Deselect
        };
        deselectEntry.callback.AddListener((eventData) => OnDeselect());
        eventTrigger.triggers.Add(deselectEntry);
    }

    private void OnSelect()
    {
        // 実行中のコルーチンを停止してから新しいコルーチンを開始
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleTo(selectedScale));
    }

    private void OnDeselect()
    {
        // 実行中のコルーチンを停止してから新しいコルーチンを開始
        if (scaleCoroutine != null)
        {
            StopCoroutine(scaleCoroutine);
        }
        scaleCoroutine = StartCoroutine(ScaleTo(originalScale));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        scaleCoroutine = null; // 完了後、参照をクリア
    }

    public void OnDeleated()
    {
        StopAllCoroutines();
    }
}