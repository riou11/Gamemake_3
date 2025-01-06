using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globe : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFallen = false;
    [Header("プレイヤーの判定")]
    public PlayerTriggerCheck playerCheck;

    [Header("落ちてくるまでのラグ")]
    public float fallDelay=0f;
    [Header("初動に加える力")]
    public Vector2 initialForce = new Vector2(-1f,-1f);
    public float forceMultiplier=10f;

    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn&&!hasFallen)
        {
            hasFallen = true;
            StartCoroutine(FallAfterDelay());
        }
    }
    private IEnumerator FallAfterDelay()
    {
        if(fallDelay>0f)
        {
            yield return new WaitForSeconds(fallDelay);
        }

        rb.constraints=RigidbodyConstraints2D.None;
        rb.AddForce(initialForce.normalized * forceMultiplier, ForceMode2D.Impulse);
        Debug.Log("Rigidbodyの制約解除");
    }
}