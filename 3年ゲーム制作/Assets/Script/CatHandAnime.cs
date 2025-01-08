using System.Collections;
using UnityEngine;

public class CatHandAnime : MonoBehaviour
{
    [SerializeField] private Animator anim;           // アニメーターの参照
    [SerializeField] private float upToDossunDelay = 1.0f; // "Up" から "Dossun" への切り替え時間
    [SerializeField] private float dossunToFallDelay = 0.5f; // "Dossun" から "Fall" への切り替え時間
    [SerializeField] private float fallToUpDelay = 0.8f; // "Fall" から "Up" への切り替え時間

    private int currentState = 0; // 現在のアニメーション状態

    void Start()
    {
        if (anim == null)
        {
            Debug.LogError("Animator is not assigned!");
            enabled = false;
            return;
        }

        StartCoroutine(ChangeAnimation());
    }

    private IEnumerator ChangeAnimation()
    {
        while (true)
        {
            float delay = 0f; // 次の状態に行くまでの待機時間

            switch (currentState)
            {
                case 0: // "Up" 状態
                    anim.SetBool("Up", true);
                    anim.SetBool("Dossun", false);
                    anim.SetBool("Fall", false);
                    delay = upToDossunDelay;
                    break;

                case 1: // "Dossun" 状態
                    anim.SetBool("Up", false);
                    anim.SetBool("Dossun", true);
                    anim.SetBool("Fall", false);
                    delay = dossunToFallDelay;
                    break;

                case 2: // "Fall" 状態
                    anim.SetBool("Up", false);
                    anim.SetBool("Dossun", false);
                    anim.SetBool("Fall", true);
                    delay = fallToUpDelay;
                    break;
            }

            // 切り替え前の待機
            yield return new WaitForSeconds(delay);

            // 次の状態に進む（ループ）
            currentState = (currentState + 1) % 3;
        }
    }
}
