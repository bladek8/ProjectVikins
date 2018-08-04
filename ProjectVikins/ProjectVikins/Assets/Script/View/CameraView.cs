using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    private readonly Assets.Script.Controller.CameraController cameraController = new Assets.Script.Controller.CameraController();

    public GameObject playerGameObj;

    CinemachineStateDrivenCamera cam;
    CinemachineConfiner camConfiner;
    CinemachineVirtualCamera[] secVirtualCams;
    CinemachineFramingTransposer[] fTCams;

    [SerializeField] BoxCollider2D colliderTrigger;

    private void Awake()
    {
        cam = GetComponent<CinemachineStateDrivenCamera>();
        var secCams = cam.ChildCameras;
        secVirtualCams = new CinemachineVirtualCamera[secCams.Length];
        fTCams = new CinemachineFramingTransposer[secCams.Length];
        for (int i = 0; i < secCams.Length; i++)
        {
            secVirtualCams[i] = secCams[i].GetComponent<CinemachineVirtualCamera>();
            fTCams[i] = secVirtualCams[i].GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        camConfiner = GetComponent<CinemachineConfiner>();
        cam.Follow = playerGameObj.transform;
        cam.m_AnimatedTarget = playerGameObj.GetComponent<Animator>(); 
    }

    public void UpdatePlayerTranform()
    {
        var player = cameraController.UpdatePlayerTranform();
        playerGameObj = player.GameObject;
        cam.Follow = player.GameObject.transform;
        cam.m_AnimatedTarget = player.GameObject.GetComponent<Animator>();
    }

    public void CamUnconfined()
    {
        camConfiner.m_BoundingShape2D = null;
    }

    public void ChangeDamping(float value)
    {
        foreach (var fTCam in fTCams)
        {
            fTCam.m_XDamping = value;
            fTCam.m_YDamping = value;
        }
    }
}
