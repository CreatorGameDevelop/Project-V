using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    void Awake()
    {
        coll = GetComponent<Collider2D>();  
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))  // Area태그가 아니면 실행하지 않음.
        {
            return;
        }

        Vector3 playerPos = GameManager.instance.player.transform.position; // 거리를 구하기 위해 플레이어, 타일맵 위치를 저장할 것.
        Vector3 myPos= transform.position;
        Vector3 playerDir = GameManager.instance.player.inputVec;

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffX= Mathf.Abs(dirX);
        float diffY= Mathf.Abs(dirY);

        dirX = dirX > 0 ? 1 : -1;
        dirY = dirY > 0 ? 1 : -1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40) ;
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                if (coll.enabled) // collider가 체크(Active) 되어 있을 때만 실행.
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));
                }
                break;
        } 
    }
}