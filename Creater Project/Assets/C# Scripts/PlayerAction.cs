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
        hands = GetComponentsInChildren<Hand>(true);    // true�� ���ڰ����� �־��ָ� ��Ȱ��ȭ ������Ʈ�� �Ҵ��.
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
        rigid.MovePosition(rigid.position + nextVec);  // ��ġ �̵��ϴ� ������� Player�� �̵���ų ��. 
        // ���� position�� nextVec�� �����ִ� ���, nomalized:���Ͱ��� ũ�Ⱑ 1�� �ǵ��� ��ǥ�� ������ ��.
        // InputManager�� ����ؼ� nomalized ����.
    }
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        animator.SetFloat("Speed", inputVec.magnitude);   // magitude: ������ ���̸� ��ȯ.

        if (inputVec.x!=0) // x�� �������� ������ ��
        {
            spriteRenderer.flipX = inputVec.x < 0; // x�� �̵� ������ ������� flip
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive) // ��� �̺�Ʈ
        {
            return;
        }

        GameManager.instance.health -= 10 ; // �ӽ÷� ü�±�� 

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
