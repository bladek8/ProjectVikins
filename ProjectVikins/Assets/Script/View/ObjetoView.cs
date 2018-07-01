using UnityEngine;
using System.Collections;

public class ObjetoView : MonoBehaviour
{
    public GameObject objeto;
    float speed;

    void Update()
    {
        speed = transform.position.x;
        if (RespawnView.maxX)
            speed -= 2 * Time.deltaTime;
        else
            speed += 2 * Time.deltaTime;
        objeto.transform.position = new Vector3(speed, transform.position.y, transform.position.z);

        if (transform.position.x <= -24)
            Destroy(transform.gameObject);
        else if (transform.position.x >= 24)
            Destroy(transform.gameObject);
    }
}
