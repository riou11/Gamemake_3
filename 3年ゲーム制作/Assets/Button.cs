using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField] Material defaultButtonMaterial;
    [SerializeField] Material pushedButtonMaterial;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = defaultButtonMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("è’ìÀ");
        if (collision.gameObject.tag=="cheese")
        {
            CheeseController cheese = collision.gameObject.GetComponent<CheeseController>();
            if (cheese != null && !cheese.IsHeld)
            {
                GetComponent<Renderer>().material = pushedButtonMaterial;
            }
        }
    }
}
