using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    private readonly Assets.Script.Controller.CameraController cameraController = new Assets.Script.Controller.CameraController();

    public Transform playerTranform;

    CinemachineVirtualCamera cam;

    CinemachineFramingTransposer camBody;


    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        camBody = cam.GetCinemachineComponent<CinemachineFramingTransposer>();
        cam.Follow = playerTranform;
    }

    //public float offset;
    //public float speed;
    //public Vector2 minMaxXPosition;
    //public Vector2 minMaxYPosition;
    //private float screenWidth;
    //private float screenHeight;
    //private Vector3 cameraMove;

    //void Start()
    //{
    //    screenWidth = Screen.width;
    //    screenHeight = Screen.height;
    //    cameraMove.x = transform.position.x;
    //    cameraMove.y = transform.position.y;
    //    cameraMove.z = transform.position.z;
    //}

    private void FixedUpdate()
    {
        //if ((Camera.main.WorldToScreenPoint(playerTranform.transform.position).x > screenWidth - offset) && transform.position.x < minMaxXPosition.y)
        //{
        //    cameraMove.x += MoveSpeed();
        //}
        //if ((Camera.main.WorldToScreenPoint(playerTranform.transform.position).x < offset) && transform.position.x > minMaxXPosition.x)
        //{
        //    cameraMove.x -= MoveSpeed();
        //}
        //if ((Camera.main.WorldToScreenPoint(playerTranform.transform.position).y > screenHeight - offset) && transform.position.y < minMaxYPosition.y)
        //{
        //    cameraMove.y += MoveSpeed();
        //}
        //if ((Camera.main.WorldToScreenPoint(playerTranform.transform.position).y < offset) && transform.position.y > minMaxYPosition.x)
        //{
        //    cameraMove.y -= MoveSpeed();
        //}

        //transform.position = cameraMove;
    }

    //float MoveSpeed()
    //{
    //    return speed * Time.deltaTime;
    //}

    public void UpdatePlayerTranform()
    {
        var player = cameraController.UpdatePlayerTranform();
        playerTranform = player.transform;
        cam.Follow = player.transform;
        //speed = player.SpeedRun + 1;
        //transform.position = new Vector3(playerTranform.position.x, playerTranform.position.y, transform.position.z);
        //cameraMove = transform.position;
    }

    public void InAttack()
    {
        //if(cam.m_Lens.OrthographicSize < 2.5f) 
        //    cam.m_Lens.OrthographicSize += 0.1f;
        cam.m_Lens.OrthographicSize = 2.5f;
        camBody.m_DeadZoneHeight = 0.15f;
        camBody.m_DeadZoneWidth = 0.15f;
    }

    public void OutAttack()
    {
        cam.m_Lens.OrthographicSize = 1.40f;
        camBody.m_DeadZoneHeight = 0.45f;
        camBody.m_DeadZoneWidth = 0.45f;
    }
}
