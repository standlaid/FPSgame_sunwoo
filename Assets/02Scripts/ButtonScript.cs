using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

//��ǥ: �÷��̾ ��ư�� ������ �ٸ��� ������, �׺���̼� �޽ø� �ٽ� �����.
//�ʿ�Ӽ�: �ٸ� ���� ������Ʈ, navMeshSurface
public class ButtonScript : MonoBehaviour
{
    //�ʿ�Ӽ�: �ٸ� ���� ������Ʈ, navMeshSurface
    public GameObject bridge;
    public NavMeshSurface navMeshSurface;

    // Start is called before the first frame update
    void Start()
    {
        bridge.gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {

        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //��ǥ: �÷��̾ ��ư�� ������
        if (other.CompareTag("Player"))
        {
            Debug.Log("����");
            //�ٸ��� ������,
            bridge.gameObject.SetActive(true);

            // �׺���̼� �޽ø� �ٽ� �����.
            navMeshSurface.BuildNavMesh();
        }
    }
}
