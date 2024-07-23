using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystemButton : MonoBehaviour
{
    [SerializeField] Material defaultButtonMaterial;
    [SerializeField] Material pushedButtonMaterial;
    [SerializeField] UnityEvent onButtonPressed;

    [SerializeField] private float buttonPressDepth = 0.1f;//ボタンが押し下げられる深さ
    [SerializeField] private float buttonPressDuration = 0.2f;//ボタンが押し下げられる時間

    private Vector2 originalPosition;
    private bool isPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = defaultButtonMaterial;
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "cheese")
        {
            CheeseController cheese = collision.gameObject.GetComponent<CheeseController>();
            if (cheese != null && !cheese.IsHeld && !isPressed)
            {
                isPressed = true;
                GetComponent<Renderer>().material = pushedButtonMaterial;
                onButtonPressed.Invoke();
                StartCoroutine(PressButton());
            }
        }
    }

    private IEnumerator PressButton()
    {
        Vector2 targetposition = originalPosition - new Vector2(0, buttonPressDepth);
        float elapsedtime = 0;

        while (elapsedtime < buttonPressDuration)
        {
            transform.position = Vector2.Lerp(originalPosition, targetposition, elapsedtime / buttonPressDuration);
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetposition;
    }
}