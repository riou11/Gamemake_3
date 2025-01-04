using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobeButton : MonoBehaviour
{
    [Header("判定用の設定")]
    [SerializeField] private string requiredTag = "Globe"; // 地球儀オブジェクトに設定するTag
    [SerializeField] private Material defaultButtonMaterial;
    [SerializeField] private Material pushedButtonMaterial;
    [SerializeField] private UnityEvent onButtonPressed;

    [SerializeField] private float buttonPressDepth = 0.1f; // ボタンが押し下げられる深さ
    [SerializeField] private float buttonPressDuration = 0.2f; // ボタンが押し下げられる時間
    [SerializeField] private AudioClip buttonPressSE; // ボタンを押したときのSE

    private AudioSource audioSource;
    private Vector2 originalPosition;
    private bool isPressed = false;

    void Start()
    {
        GetComponent<Renderer>().material = defaultButtonMaterial;
        originalPosition = transform.position;

        // AudioSourceがなければ追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPressed) return;

        // 指定したTagを持つオブジェクトのみボタンを押せる
        if (other.CompareTag(requiredTag))
        {
            isPressed = true;
            GetComponent<Renderer>().material = pushedButtonMaterial;

            // SE再生
            PlayButtonPressSE();

            onButtonPressed.Invoke();
            StartCoroutine(PressButton());
        }
    }

    private IEnumerator PressButton()
    {
        Vector2 targetPosition = originalPosition - new Vector2(0, buttonPressDepth);
        float elapsedTime = 0;

        while (elapsedTime < buttonPressDuration)
        {
            transform.position = Vector2.Lerp(originalPosition, targetPosition, elapsedTime / buttonPressDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    private void PlayButtonPressSE()
    {
        if (buttonPressSE != null)
        {
            audioSource.PlayOneShot(buttonPressSE);
        }
        else
        {
            Debug.LogWarning("ボタンのSEが設定されていません！");
        }
    }
}
