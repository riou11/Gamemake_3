using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TitleControl : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;

    [SerializeField] private GameObject _titlePanel;
    [SerializeField] private GameObject _creditPanel;
    [SerializeField] private UnityEngine.UI.Button _startButton;
    [SerializeField] private UnityEngine.UI.Button _creditButton;

    private GameInputs _gameInputs;
    private Vector2 _moveInputValue;

    ButtonScaler start;
    ButtonScaler credit;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    void SetUp()
    {
        _titlePanel.SetActive(true);
        _creditPanel.SetActive(false);
        start = _startButton.GetComponent<ButtonScaler>();
        credit = _creditButton.GetComponent<ButtonScaler>();

        Debug.Log((int)gameManager.currentScene);
        Debug.Log(((int)gameManager.currentScene) + 1);
        _startButton.onClick.AddListener(() => gameManager.TransitionScene(((int)gameManager.currentScene) + 1));
        _creditButton.onClick.AddListener(() => TransitionToCredit());

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_startButton.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (_creditPanel.activeSelf)
        {
            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                BackToTitle();
            }
        }
    }

    public void TransitionToCredit()
    {
        _titlePanel.SetActive(false);
        _creditPanel.SetActive(true);
    }

    public void BackToTitle()
    {
        _creditPanel.SetActive(false);
        _titlePanel.SetActive(true);
    }

    public void StopCoroutine()
    {
        start.OnDeleated();
        credit.OnDeleated();
    }
}
