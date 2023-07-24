using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩을 저장할 변수
    public GameObject[] prefabs;

    // 풀 당담을 하는 리스트가 
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++ )
        {
            pools[index] = new List<GameObject>();  
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 비활성화 된 게임 오브젝트에 접근 
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // 비활성화 오브젝트가 없다면
        if (!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select; 
    }
}
