using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����: HP bar�� �� ������ Ÿ���� �� �������� ���Ѵ�.
//�ʿ�Ӽ�: Ÿ��
public class HpBarTarget : MonoBehaviour
{

    //�ʿ�Ӽ�: Ÿ��
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
