using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaler : MonoBehaviour,ISelectHandler, IDeselectHandler
{
    [SerializeField] private Vector3 selectedScale = new Vector3(1.2f, 1.2f, 1.2f); // 選択時のスケール
    [SerializeField] private float transitionDuration = 0.2f; // スケール変更のアニメーション時間
    private Vector3 originalScale; // 元のスケール

    private void Awake()
    {
        // 初期スケールを保存
        originalScale = transform.localScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        // 選択時にスケールアップ
        StopAllCoroutines(); // 途中のアニメーションを中断
        StartCoroutine(ScaleTo(selectedScale));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // 非選択時にスケールを戻す
        StopAllCoroutines(); // 途中のアニメーションを中断
        StartCoroutine(ScaleTo(originalScale));
    }

    private System.Collections.IEnumerator ScaleTo(Vector3 targetScale)
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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
