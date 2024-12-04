using UnityEngine;
using UnityEngine.SceneManagement; 

public class NewMouseTrap : MonoBehaviour
{
    public Transform movingPart; // 稼働部分のTransform
    public Renderer targetRenderer; // マテリアルを変更するオブジェクトのRenderer
    public Material defaultMaterial; // 作動前のマテリアル
    public Material triggeredMaterial; // 作動後のマテリアル
    public float rotateAngle = 90f; // 回転する角度
    public float rotateSpeed = 5f; // 回転速度
    private bool isTriggered = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        // 稼働パーツの初期状態を記録
        initialRotation = movingPart.localRotation;
        targetRotation = Quaternion.Euler(0, 0, rotateAngle) * initialRotation;

        // 初期状態でのマテリアルを設定
        if (targetRenderer != null && defaultMaterial != null)
        {
            targetRenderer.material = defaultMaterial;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;

            // マテリアルを変更
            if (targetRenderer != null && triggeredMaterial != null)
            {
                targetRenderer.material = triggeredMaterial;
            }

            // ゲームオーバー処理を呼び出す
            GameOver();
        }
    }

    void Update()
    {
        if (isTriggered)
        {
            // 回転をスムーズに進行
            movingPart.localRotation = Quaternion.Lerp(movingPart.localRotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    // ゲームオーバー処理
    void GameOver()
    {
        // ここでゲームオーバーの処理を記述
        Debug.Log("Game Over!");
    }
}
