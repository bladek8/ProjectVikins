using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour {
    
    public GameObject player;

    public float offset;
    public float speed;
    public Vector2 minMaxXPosition;
    public Vector2 minMaxYPosition;
    private float screenWidth;
    private float screenHeight;
    private Vector3 cameraMove;

    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        cameraMove.x = transform.position.x;
        cameraMove.y = transform.position.y;
        cameraMove.z = transform.position.z;
    }

    private void FixedUpdate()
    {
        if ((Camera.main.WorldToScreenPoint(player.transform.position).x > screenWidth - offset) && transform.position.x < minMaxXPosition.y)
        {
            cameraMove.x += MoveSpeed();
        }
        if ((Camera.main.WorldToScreenPoint(player.transform.position).x < offset) && transform.position.x > minMaxXPosition.x)
        {
            cameraMove.x -= MoveSpeed();
        }
        if ((Camera.main.WorldToScreenPoint(player.transform.position).y > screenHeight - offset) && transform.position.y < minMaxYPosition.y)
        {
            cameraMove.y += MoveSpeed();
        }
        if ((Camera.main.WorldToScreenPoint(player.transform.position).y < offset) && transform.position.y > minMaxYPosition.x)
        {
            cameraMove.y -= MoveSpeed();
        }

        transform.position = cameraMove;
    }

    float MoveSpeed()
    {
        return speed * Time.deltaTime;
    }
}
