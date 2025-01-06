using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public GameObject player;
    public int speed;
    private Rigidbody2D rb;
    private Animator anim;
    private CatState catState;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        catState = GetComponent<CatState>();
    }

    void FixedUpdate()
    {
        if (catState.IsStopped())
        {
            anim.SetBool("run", false);
            return;
        }

        Vector2 targeting = (player.transform.position - transform.position).normalized;

        if (targeting.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            anim.SetBool("run", true);
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
            anim.SetBool("run", true);
        }

        rb.velocity = new Vector2(targeting.x * speed, rb.velocity.y);
    }
}
