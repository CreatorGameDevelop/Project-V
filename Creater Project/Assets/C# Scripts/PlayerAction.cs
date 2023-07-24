using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteRenderer;

    public Scanner scanner;
    public Vector2 inputVec;
    public float speed;
    public Hand[] hands;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);    // true를 인자값으로 넣어주면 비활성화 오브젝트도 할당됨.
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);  // 위치 이동하는 방식으로 Player를 이동시킬 것. 
        // 현재 position에 nextVec을 더해주는 방법, nomalized:벡터값의 크기가 1이 되도록 좌표가 수정된 값.
        // InputManager를 사용해서 nomalized 빼줌.
    }
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        animator.SetFloat("Speed", inputVec.magnitude);   // magitude: 벡터의 길이만 반환.

        if (inputVec.x!=0) // x축 방향으로 움직일 때
        {
            spriteRenderer.flipX = inputVec.x < 0; // x축 이동 방향이 음수라면 flip
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive) // 사망 이벤트
        {
            return;
        }

        GameManager.instance.health -= 10 ; // 임시로 체력깎기 

        if (GameManager.instance.health < 0)
        {
            for (int index = 2; index < transform.childCount; index++)
            {
                transform.GetChild(index).gameObject.SetActive(false);
            }
            GameManager.instance.isLive = false;
            animator.SetTrigger("Dead");
        }
    }
}
