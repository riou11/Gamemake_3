using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseController : MonoBehaviour
{
    public GameObject player; // �v���C���[�I�u�W�F�N�g
    public GameObject cheeseHolder; // ���GameObject�i�v���C���[�̎q�I�u�W�F�N�g�j
    public float launchForce = 10f;
    public float upwardForce = 2f;
    private Rigidbody2D rb;
    private bool isHeld = true;
    private bool isTouchingPlayer = false;

    public bool IsHeld { get { return isHeld; } }
    public int lives = 3;//�`�[�Y�̎c�@

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HoldCheese();
    }

    void Update()
    {
        if (isHeld)
        {
            transform.position = cheeseHolder.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Z) && isHeld)
        {
            isHeld = false;
            rb.isKinematic = false;
            Vector2 launchDirection = new Vector2(player.transform.right.x, player.transform.right.y + upwardForce).normalized;
            rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
        }
        if (isTouchingPlayer && !isHeld && Input.GetKeyDown(KeyCode.Z))
        {
            HoldCheese();
        }
        // �`�[�Y����ʊO�ɏo�����������
        if (transform.position.x < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x)
        {
            LoseCheese();
        }
    }

    void HoldCheese()
    {
        transform.position = cheeseHolder.transform.position;
        rb.isKinematic = true;
        isHeld = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player && !isHeld)
        {
            isTouchingPlayer = true; // �v���C���[�ɐG��Ă����Ԃ��L�^
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            isTouchingPlayer = false; // �v���C���[���痣�ꂽ��Ԃ��L�^
        }
    }

    private void LoseCheese()
    {

        lives--;
        if(lives>0)
        {
            Invoke("HoldNewCheese", 0.5f);
        }
        else
        {
            Debug.Log("Game Over");
            //�Q�[���I�[�o�[���������Ȃ炱����
        }
        Destroy(gameObject);
    }

    private void HoldNewCheese()
    {
        Debug.Log("New Cheese");
        //�V�����`�[�Y��������
        GameObject newCheese = Instantiate(gameObject, cheeseHolder.transform.position, Quaternion.identity);
        newCheese.GetComponent<CheeseController>().player = player;
        newCheese.GetComponent<CheeseController>().cheeseHolder = cheeseHolder;
        newCheese.GetComponent<CheeseController>().lives = lives;
    }
}

