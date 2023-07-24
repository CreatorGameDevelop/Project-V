using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.4f, -0.15f, 0);
    Vector3 rightPosRevers = new Vector3(-0.1f, -0.15f, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -30);
    Quaternion leftRotRevers = Quaternion.Euler(0, 0, -130  );

    private void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }
    private void LateUpdate()
    {
        bool isRevers = player.flipX;

        if (isLeft) // 근접무기
        {
            transform.localRotation = isRevers ? leftRotRevers : leftRot;
            spriter.flipY = isRevers;
            spriter.sortingOrder = isRevers ? 1 : 3;
        }
        else  // 원거리무기
        {
            transform.localPosition = isRevers ? rightPosRevers : rightPos;
            spriter.flipX = isRevers;
            spriter.sortingOrder = isRevers ? 3 : 1;
        }
    }
}
