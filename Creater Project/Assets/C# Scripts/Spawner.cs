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
[System.Serializable] // ����ȭ: ��ü�� ���� Ȥ�� �����ϱ� ���� ��ȯ. >>> �ν����Ϳ��� �ʱ�ȭ ����.
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}