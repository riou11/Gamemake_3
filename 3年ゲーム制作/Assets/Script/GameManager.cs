using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class StageData
{
    public int stageNum;
    public bool isUnlocked;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public List<StageData> stages;

    [SerializeField]
    private Image cheeseParameters;

    private int cheeseScore = 0;

    private int[] cheeseScores = { 8, 0, 0 };
    private int stgNum = 0;

    //�V���O���g���̎���
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //LoadStageData();
        cheeseScore = 0;
        cheeseParameters.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //�`�[�Y�Q�[�W�̍X�V
        UpdateCheeseParameter();

        //�G�ɒǂ����ꂽ�Ƃ��̃Q�[���I�[�o�[����

        //���e�`�[�Y���擾�����Ƃ��̃Q�[���I�[�o�[����

        //Goal�ɓ��B�����Ƃ��̃Q�[���N���A����

        //
    }

    void UpdateCheeseParameter()
    {
        //�l���`�[�Y���ƃX�e�[�W�ɔz�u���ꂽ�`�[�Y�������Z�������ʂ��A�`�[�Y�p�����[�^�[�ɔ��f
        float fillAmount_ = (float)cheeseScore / (float)cheeseScores[stgNum];
        cheeseParameters.fillAmount = fillAmount_;
        Debug.Log(cheeseScore);
        Debug.Log(fillAmount_);
    }

    // �X�e�[�W�f�[�^���Z�[�u����
    public void SaveStageData()
    {
        // �f�[�^���V���A���C�Y���ĕۑ�
        PlayerPrefs.SetString("StageData", JsonUtility.ToJson(this));
    }

    // �X�e�[�W�f�[�^�����[�h����
    public void LoadStageData()
    {
        if (PlayerPrefs.HasKey("StageData"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("StageData"), this);
        }
        else
        {
            // ����N�����ɍŏ��̃X�e�[�W�̂݉��
            stages[0].isUnlocked = true;
        }
    }

    // �X�e�[�W�N���A���Ɏ��̃X�e�[�W���A�����b�N
    public void UnlockNextStage(int clearedStageNumber)
    {
        if (clearedStageNumber < stages.Count - 1)
        {
            stages[clearedStageNumber + 1].isUnlocked = true;
            SaveStageData();
        }
    }

    public void StageCleared(int stageNumber)
    {
        UnlockNextStage(stageNumber);
        SceneManager.LoadScene("StageSelect");
    }

    public void getCheese(int cheese)
    {
        cheeseScore += cheese;
    }
}
