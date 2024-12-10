using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleControl : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;

    [SerializeField] private UnityEngine.UI.Button _startButton;

    // Start is called before the first frame update
    void Start()
    {
        ButtonSetUp();
    }

    void ButtonSetUp()
    {
        Debug.Log((int)gameManager.currentScene);
        Debug.Log(((int)gameManager.currentScene) + 1);
        _startButton.onClick.AddListener(() => gameManager.TransitionScene(((int)gameManager.currentScene) + 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
