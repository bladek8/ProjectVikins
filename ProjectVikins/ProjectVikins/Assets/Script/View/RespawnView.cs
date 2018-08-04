using UnityEngine;

public class RespawnView : MonoBehaviour
{
    public GameObject seagull;

    private void Awake()
    {
        InvokeRepeating("Spawn", 0, 1);
    }
    void Spawn()
    {
        Instantiate(seagull, new Vector3(16, Random.Range(-19, 19), -99), Quaternion.identity);
    }
}
