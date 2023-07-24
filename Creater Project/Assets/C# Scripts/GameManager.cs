using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("#Game Control")]
    public float gameTime;
    public float maxTime = 3 * 10f;
    public bool isLive;
    [Header("#Player Info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

    [Header("#Game Object")]
    public PlayerAction player;
    public PoolManager poolManager;
    public LevelUp uiLevelUp;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        health = maxHealth;

        // �ӽ�
        uiLevelUp.Select(0); // �ʱ� ���� ���� ��
    }
    private void Update()
    {
        if (!isLive)
        {
            return;  
        }
        gameTime += Time.deltaTime;
        
        if (gameTime > maxTime)
        {
            gameTime = maxTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)])  // �ʱ� �����ص� �ְ� ����ġ�� �Ѿ�� �ְ����ġ�� ���� ���������� ����.
        {
            level++;
            exp = 0;
            uiLevelUp.Show(); // Hide�� Unity Item�� OnClick�� ����.
        }
    }
    public void Stop() // ������ �� ���� ���� �Լ�.
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume() // �簳
    { 
        isLive = true;  
        Time.timeScale = 1;
    }
}
