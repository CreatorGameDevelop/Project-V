using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    float timer;
    int level;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length-1);

        if (timer > spawnData[level].spawnTime) 
        {
            Spawn();
            timer = 0f;
        }
    }
    void Spawn()
    {
        GameObject enemy = GameManager.instance.poolManager.Get(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);;
    }
}
[System.Serializable] // 직렬화: 개체를 저장 혹은 전송하기 위해 변환. >>> 인스펙터에서 초기화 가능.
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}
