using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMotor : MonoBehaviour
{
    private HingeJoint2D hingeJoint;

    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
        hingeJoint.useMotor = false; // �����͖���
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hingeJoint.useMotor = true; // Player���͈͂ɓ�������motor��L����
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hingeJoint.useMotor = false; // Player���͈͂���o����motor�𖳌���
        }
    }
}
