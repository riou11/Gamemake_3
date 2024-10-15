using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class StageSelect : MonoBehaviour
{
    public List<Button> stageButtons;

    public GameManager stageManager;

    private void Start()
    {
        stageManager = FindObjectOfType<GameManager>();
        Button[] buttons = FindObjectsOfType<Button>();
        stageButtons = new List<Button>(buttons);
        UpdateStageButtons();
    }

    // ステージのロック/アンロックを反映する
    public void UpdateStageButtons()
    {
        for (int i = 0; i < stageButtons.Count; i++)
        {
            //stageButtons[i].interactable = stageManager.stages[i].isUnlocked;
        }
    }

    // ボタン押下でステージに遷移
    public void OnStageSelected(int stageNumber)
    {
        if (stageManager.stages[stageNumber].isUnlocked)
        {
            // シーン遷移を行う
            SceneManager.LoadScene("stage" + (stageNumber + 1));
        }
    }
}
