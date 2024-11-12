using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour
{
    GameObject playerObj;
    Transform playerTransform;

    public float lowerStageYLimit = 0.0f; // 下のステージのカメラY位置
    public float upperStageYLimit = 15.0f; // 上のステージのカメラY位置
    public float yThreshold = 5.0f; // 上のステージに移行するためのY軸高さ

    private bool isUpperStage = false; // 現在のステージが上かどうか

    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerTransform = playerObj.transform;
    }

    void LateUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        float newYPosition = transform.position.y;

        // 上のステージに移動する途中ではカメラが追従する
        if (playerTransform.position.y > lowerStageYLimit && playerTransform.position.y <= yThreshold && !isUpperStage)
        {
            newYPosition = playerTransform.position.y;
        }
        // 上のステージに移行したら固定
        else if (playerTransform.position.y > yThreshold && !isUpperStage)
        {
            isUpperStage = true;
            newYPosition = upperStageYLimit; // 上のステージでの固定Y位置
        }
        // 下のステージに戻ったときは追従し、再度固定
        else if (playerTransform.position.y <= yThreshold && isUpperStage)
        {
            isUpperStage = false;
            newYPosition = lowerStageYLimit; // 下のステージでの固定Y位置
        }

        // カメラの位置を更新
        transform.position = new Vector3(playerTransform.position.x, newYPosition, transform.position.z);
    }
}
