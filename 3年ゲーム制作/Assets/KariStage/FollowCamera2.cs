using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowCamera2 : MonoBehaviour
{

    GameObject playerObj;
    PlayerMove player;
    Transform playerTransform;
    [SerializeField] Transform playerTr;
    [SerializeField] Vector2 cameraMaxPos=new Vector2(5, 5);
    [SerializeField] Vector2 cameraMinPos=new Vector2(-5,-5);

    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.GetComponent<PlayerMove>();
        playerTransform = playerObj.transform;
    }

    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(playerTr.position.x, cameraMinPos.x, cameraMaxPos.x),
            Mathf.Clamp(playerTr.position.y, cameraMinPos.y, cameraMaxPos.y),
            -10f);
    }

}