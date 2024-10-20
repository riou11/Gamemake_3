using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMotor : MonoBehaviour
{
    private HingeJoint2D hingeJoint;

    void Start()
    {
        hingeJoint = GetComponent<HingeJoint2D>();
        hingeJoint.useMotor = false; // ‰Šú‚Í–³Œø
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hingeJoint.useMotor = true; // Player‚ª”ÍˆÍ‚É“ü‚Á‚½‚çmotor‚ğ—LŒø‰»
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hingeJoint.useMotor = false; // Player‚ª”ÍˆÍ‚©‚ço‚½‚çmotor‚ğ–³Œø‰»
        }
    }
}
