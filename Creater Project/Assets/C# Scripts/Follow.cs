using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();   
    }

    private void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position); 
        // world와 screen의 좌표를 맞춰주는 로직.
    }
}
