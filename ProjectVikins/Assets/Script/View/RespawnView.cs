using UnityEngine;

public class RespawnView : MonoBehaviour
{
    public GameObject objeto1;
    float y;
    float random;
    public static bool maxX;

    void Start()
    {
        InvokeRepeating("Spawn", 0, 8);//25
    }

    private void Update()
    {
        random = Random.Range(0, 100);
        y = Random.Range(-19, 19);
    }

    void Spawn()
    {
        GameObject obj1 = Instantiate(objeto1) as GameObject;
        if (random > 50)
        {
            maxX = true;
            obj1.transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        else
        {
            maxX = false;
            obj1.transform.position = new Vector3(-transform.position.x, y, transform.position.z);
        }
    }
}
