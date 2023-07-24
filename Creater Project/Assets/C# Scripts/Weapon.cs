using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    PlayerAction player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:   // case 1 
                timer += Time.deltaTime;

                if (timer > speed) // case 1에서 speed는 연사속도로 작동.
                {
                    timer = 0;
                    Fire();
                }
                break;
        }
         
        if (Input.GetButtonDown("Jump"))
        {
            LvUp(20, 3);
        }
    }
    public void LvUp(float damage, int count )
    {
        this.damage = damage;  
        this.count += count;

        if (id==0)
        {
            Place();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // 나중에 생성된 무기에게도 함수를 적용시키기.
    }

    public void Init(ItemData data)
    {   // Basic Set
        name = "weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int index = 0; index < GameManager.instance.poolManager.prefabs.Length; index++) 
        {
            if (data.projectile == GameManager.instance.poolManager.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 200;
                Place();

                break;

            default:
                speed = 0.3f;
                break;
        }
        // Hand Set
        Hand hand = player.hands[(int)data.itemType]; // enum 타입은 int로 형변환해 사용가능.
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // 나중에 생성된 무기에게도 함수를 적용시키기.
    }

    void Place() // 근접무기 배치하는 함수.
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet=GameManager.instance.poolManager.Get(prefabId).transform;
            }
            bullet.parent = transform;

            bullet.localPosition= Vector3.zero; 
            bullet.localRotation= Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.2f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is infinity Per.
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget) // null
        {
            return;
        }
        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.poolManager.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);  // 지정된 축을 중심으로 목표를 향해 회전함.
        bullet.GetComponent<Bullet>().Init(damage, count, dir);   // 원거리에선 count를 관통력으로 사용.
    }
}
