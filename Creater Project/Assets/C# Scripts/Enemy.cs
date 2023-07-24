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
    public Rigidbody2D target;  // ���� Ÿ��
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
        if (!isAlive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) // �ǰݽ� ���� �������� �����ִ� ����   
        {
            return;
        }
        // �̵� ���� ����
        Vector2 dirVex = target.position - rigid.position; // ���� ����
        Vector2 nextVec = dirVex.normalized * speed * Time.fixedDeltaTime; // ���� ������ ����
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // ������ �ӵ��� 0���� ����
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
        animator.SetBool("Dead", false);  // Dead �Լ��� ���ݴ�� ������ֱ�
    }
    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;   
    }

    void OnTriggerEnter2D(Collider2D collision)  // enemy�� collider�� �ε��� ��
    {
        if (!collision.CompareTag("Bullet") || !isAlive)
        {
            return;
        }

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(knockBack());

        if (health > 0)
        {  // �ǰ� �̺�Ʈ �ڸ�
            animator.SetTrigger("Hit");
        }
        else
        {  // ��� �̺�Ʈ 
            isAlive = false;
            coll.enabled = false;
            rigid.simulated = false;
            // �ݶ��̴�, ������ٵ� ��Ȱ��ȭ.

            spriteRenderer.sortingOrder = 1; // �Ⱥ��̰� ó��.
            animator.SetBool("Dead", true);  // ��� �ִϸ��̼�
            // Dead�Լ��� �ִϸ��̼� �̺�Ʈ���� ��������.
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator knockBack()
    {
        yield return wait;    // ���� ������ �ϳ� ������.
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec= transform.position - playerPos;     
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); 
    }
    // yield return null << 1������ ���� // yield: �ڷ�ƾ ��ȯ Ű����, 
    //yield return new WaitForSecond(float); :float ��ŭ ����. new�� �ݺ��ؼ� ���� ����ȭ �������� �� ����.

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
