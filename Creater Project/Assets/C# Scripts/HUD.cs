using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    // Head Up Display
    public enum InfoType { Exp, Level, Kill, Time, Health} // 열거형 선언
    public InfoType type; // 열거형 변수 추가

    Text myText;
    Slider slider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        slider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                slider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv{0:F0}", GameManager.instance.level);
                // GameManager.instance.level을 string으로 형변환 해줌.    {0:f0}에서 :F0은 소수점 뒤로 없애줌.
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.maxTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}",min, sec);   // D는 자리수를 정해줌 ex) D3일 때 1 >> 001 
                break;
            case InfoType.Health:
                float curHp = GameManager.instance.health;
                float maxHp = GameManager.instance.maxHealth;
                slider.value = curHp / maxHp;
                break;
        }
    }
}
