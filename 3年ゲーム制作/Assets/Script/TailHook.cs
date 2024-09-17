using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailHook : MonoBehaviour
{
    public Transform tailEnd; // �K���̒[��Transform
    public LayerMask hookLayer; // �Ђ������|�C���g�̃��C���[
    public float hookDistance = 3f; // �Ђ��������鋗��
    private Rigidbody2D rb;
    private HingeJoint2D tailHingeJoint;
    private bool isHooked = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tailHingeJoint = transform.GetComponent<HingeJoint2D>();
        tailHingeJoint.enabled = false; // ������ԂŖ�����
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isHooked)
            {
                ReleaseHook();
            }
            else
            {
                TryHook();
            }
        }
    }

    void TryHook()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, hookDistance, hookLayer);

        if (hit.collider != null)
        {
            isHooked = true;
            tailHingeJoint.connectedBody = hit.collider.attachedRigidbody;
            tailHingeJoint.connectedAnchor = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
            
            tailHingeJoint.enabled = true; // Hinge Joint��L����

        }
    }

    void ReleaseHook()
    {
        isHooked = false;
        tailHingeJoint.enabled = false; // Hinge Joint�𖳌���
    }
}
