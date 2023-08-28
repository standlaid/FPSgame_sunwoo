using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//����: ���� A���� B���� 3�ʸ��� ���ڴ�.
//�ʿ�Ӽ�. pointA, pointB, Ư���ð�, ����ð�
public class LerpMove : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float duration;
    float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        transform.position = Vector3.Lerp(pointA.position, pointB.position, currentTime / duration);
    }
}
