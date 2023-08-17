using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//목적: 나(이펙트)가 특정 시간이 지나면 제거된다.
public class EffectDestroy : MonoBehaviour
{
    public float destroyTime = 0;
    public float currentTime = 0;

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > destroyTime)
        {
            Destroy(gameObject);
            currentTime = 0;
        }
    }

}
