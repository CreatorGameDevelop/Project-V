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

        // 임시
        uiLevelUp.Select(0); // 초기 무기 선택 삽
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

        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)])  // 초기 설정해둔 최고 경험치를 넘어가면 최고경험치가 다음 레벨업으로 설정.
        {
            level++;
            exp = 0;
            uiLevelUp.Show(); // Hide는 Unity Item에 OnClick에 있음.
        }
    }
    public void Stop() // 레벨업 시 게임 정지 함수.
    {
        isLive = false;
        Time.timeScale = 0;
    }
    public void Resume() // 재개
    { 
        isLive = true;  
        Time.timeScale = 1;
    }
}
