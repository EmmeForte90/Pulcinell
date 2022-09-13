using UnityEngine;
using System.Collections;
public class RespawnEnemy : MonoBehaviour
{
    public int level;
    public GameObject enemy;
    public GameObject player;
    float spawnNewEnemyTimer = 10;
    void Start() { }
    void Update()
    {
        spawnNewEnemyTimer -= Time.deltaTime;
        if (spawnNewEnemyTimer <= 0)
        {
            spawnNewEnemyTimer = 5 - (level * 0.25f);
            Instantiate(enemy, new Vector3(player.transform.position.x + 50.0f,
                    player.transform.position.y, 0.0f), Quaternion.identity);
        }
    }
}
