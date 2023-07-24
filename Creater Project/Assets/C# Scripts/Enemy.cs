using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float speed;
    public Rigidbody2D target;  // 따라갈 타겟
    public RuntimeAnimatorController[] animCon;

    bool isAlive;
    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriteRenderer;
    Animator animator;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  
        animator = GetComponent<Animator>();   
        wait = new WaitForFixedUpdate();
    } 
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        if (!isAlive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) // 피격시 물리 움직임을 끊어주는 로직   
        {
            return;
        }
        // 이동 로직 구현
        Vector2 dirVex = target.position - rigid.position; // 방향 설정
        Vector2 nextVec = dirVex.normalized * speed * Time.fixedDeltaTime; // 다음 움직일 벡터
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // 물리적 속도는 0으로 설정
    }
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        if (!isAlive)
        {
            return;
        }
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isAlive = true;
        health = maxHealth;
        coll.enabled = true;
        rigid.simulated = true;
        spriteRenderer.sortingOrder = 2; 
        animator.SetBool("Dead", false);  // Dead 함수와 정반대로 만들어주기
    }
    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;   
    }

    void OnTriggerEnter2D(Collider2D collision)  // enemy의 collider와 부딪힐 때
    {
        if (!collision.CompareTag("Bullet") || !isAlive)
        {
            return;
        }

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(knockBack());

        if (health > 0)
        {  // 피격 이벤트 자리
            animator.SetTrigger("Hit");
        }
        else
        {  // 사망 이벤트 
            isAlive = false;
            coll.enabled = false;
            rigid.simulated = false;
            // 콜라이더, 리지드바디 비활성화.

            spriteRenderer.sortingOrder = 1; // 안보이게 처리.
            animator.SetBool("Dead", true);  // 사망 애니메이션
            // Dead함수는 애니메이션 이벤트에서 실행해줌.
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator knockBack()
    {
        yield return wait;    // 물리 프레임 하나 딜레이.
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec= transform.position - playerPos;     
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); 
    }
    // yield return null << 1프레임 쉬기 // yield: 코루틴 반환 키워드, 
    //yield return new WaitForSecond(float); :float 만큼 쉬기. new를 반복해서 쓰면 최적화 문제있을 수 있음.

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
