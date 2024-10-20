using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMotor : MonoBehaviour
{
    private HingeJoint2D hingeJoint;

    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
        hingeJoint.useMotor = false; // 初期は無効
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hingeJoint.useMotor = true; // Playerが範囲に入ったらmotorを有効化
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hingeJoint.useMotor = false; // Playerが範囲から出たらmotorを無効化
        }
    }
}
