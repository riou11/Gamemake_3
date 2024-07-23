using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toaster_Gimmick : MonoBehaviour
{
    [SerializeField] private float distance = 1.0f;//“®‚­‹——£
    [SerializeField] private float moveDuration = 1.0f;//ª‚Ì‹——£‚ği‚ŞŠÔ
    [SerializeField] private float waitTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTrap()
    {
        Debug.Log("Gimmick activated");
    }

    public void Toaster()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        // ‘Ò‹@ŠÔ‚ğ‘Ò‚Â
        yield return new WaitForSeconds(waitTime);

        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + new Vector2(0, distance);
        float elapsedtime = 0;

        while (elapsedtime < moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, elapsedtime / moveDuration);
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
    }
}
