using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//목적: HP bar의 앞 방향이 타겟의 앞 방향으로 향한다.
//필요속성: 타겟
public class HpBarTarget : MonoBehaviour
{

    //필요속성: 타겟
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = target.forward;
    }
}
