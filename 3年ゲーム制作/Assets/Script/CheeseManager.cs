using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cheesePrefab;
    public GameObject cheeseHolder;
    public int lives = 3;

    private GameObject currentCheese;
    // Start is called before the first frame update
    void Start()
    {
        HoldNewCheese();
    }

    public void LoseCheese()
    {
        if(currentCheese!=null)
        {
            Destroy(currentCheese);
        }

        lives--;
        if(lives>0)
        {
            Invoke("HoldNewCheese", 0.5f);
        }
        else
        {
            Debug.Log("Game Over");
        }
    }

    public void HoldNewCheese()
    {
        if (cheesePrefab != null && cheeseHolder != null)
        {
            currentCheese = Instantiate(cheesePrefab, cheeseHolder.transform.position, Quaternion.identity);
            CheeseController cheeseController = currentCheese.GetComponent<CheeseController>();
            cheeseController.Initialize(player, cheeseHolder, this);
        }
        else
        {
            Debug.LogError("CheesePrefab or CheeseHolder is not set.");
        }
    }

}
