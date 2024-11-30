using System.Collections;
using UnityEngine;

public class gasstove : MonoBehaviour
{
    public StageCtrl stageCtrl;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [Header("スタート時の状態")] public bool isStart;
    [Header("SEを鳴らすまでの遅延時間 (秒)")] public float seDelay = 0.5f; // SEを鳴らすまでの遅延時間
    public AudioClip stoveSE; // ガスコンロのSE
    private AudioSource audioSource;
    private Animator animator;
    private BoxCollider2D boxCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isOn", isStart);

        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = !isStart;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (playerCheck.isOn && animator.GetBool("isOn"))
        {
            // stageCtrl.OnEnemyCollected(); // 必要に応じて処理を追加
        }
    }

    public void SwitchStove()
    {
        SoundManager.Instance.PlaySFX(SoundManager.SoundType.gass);
        bool currentStatus = animator.GetBool("isOn");
        animator.SetBool("isOn", !currentStatus);
        boxCollider.isTrigger = !boxCollider.isTrigger;

        // SEを再生する処理を遅延実行
        StartCoroutine(PlayStoveSEWithDelay(seDelay));
    }

    private IEnumerator PlayStoveSEWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (stoveSE != null)
        {
            audioSource.PlayOneShot(stoveSE);
        }
        else
        {
            Debug.LogWarning("Stove SE が設定されていません！");
        }
    }
}
