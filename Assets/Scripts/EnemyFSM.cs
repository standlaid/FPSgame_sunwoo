using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ǥ: ���� FSM ���̾� �׷��� ���� ���� ��Ű�� �ʹ�.
//�ʿ�Ӽ�: �� ����

//��ǥ2: �÷��̾���� �Ÿ��� �����ؼ� Ư�� ���·� ������ش�.
//�ʿ�Ӽ�2: �÷��̾���� �Ÿ�, �÷��̾� Ʈ������

//��ǥ3: ���� ���°� Move�� ��, �÷��̾���� �Ÿ��� ���� ���� ���̸� ���� �÷��̾ ���󰣴�.
//�ʿ�Ӽ�3: �̵� �ӵ�, ���� �̵��� ���� ĳ���� ��Ʈ�ѷ�, ���� ����

//��ǥ4: �÷��̾ ���� ���� ���� ������ Ư�� �ð��� �ѹ��� ���� �Ŀ���ŭ �����Ѵ�.
//�ʿ�Ӽ�4: ����ð�, Ư�����ݵ�����

//��ǥ5: �÷��̾ ���󰡴ٰ� �ʱ� ��ġ���� ���� �Ÿ��� ����� Return ���·� ��ȯ�Ѵ�.
//�ʿ�Ӽ�5: �ʱ� ��ġ, �̵����� ����

// ��ǥ6: �ʱ� ��ġ�� ���ƿ´�. Ư�� �Ÿ� �̳���, Idle ���·� ��ȯ�Ѵ�.
//�ʿ�Ӽ�6: Ư�� �Ÿ�

//��ǥ7: �÷��̾��� ������ ������ hitDamage ��ŭ ���ʹ��� hp�� ���ҽ�Ų��.
//�ʿ�Ӽ�7: hp

//��ǥ8: 2�� �Ŀ� �� �ڽ��� �����ϰڴ�.



public class EnemyFSM : MonoBehaviour
{
    //1�� �Ӽ�
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    //�ʿ�Ӽ�2: �÷��̾���� �Ÿ�, �÷��̾� Ʈ������
    public float findDistance = 10f;
    Transform player;

    //�ʿ�Ӽ� 3: �̵� �ӵ�, ���� �̵��� ���� ĳ���� ��Ʈ�ѷ�
    public float moveSpeed;
    CharacterController characterController;
    public float attackDistance = 2f;

    //�ʿ�Ӽ�4: ����ð�, Ư�����ݵ�����, �����Ŀ�
    float currenTime = 0;
    public float attackDelay = 2f;
    public int attackPower = 3;

    //�ʿ�Ӽ�5: �ʱ� ��ġ, �̵����� ����
    Vector3 originPos;
    public float moveDistance =10;

    //�ʿ�Ӽ�6: Ư�� �Ÿ�
    public float returnDistance;

    //�ʿ�Ӽ�7: hp
    public int hp = 3;

    EnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        characterController = GetComponent<CharacterController>();

        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //��ǥ:���� FSM ���̾�׷��� ���� ���۽�Ű�� �ʹ�.
        switch (enemyState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }
    }



    private void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }
    //��ǥ9: 2�� �Ŀ� �� �ڽ��� �����ϰڴ�.

    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(2);

        print("���");
        Destroy(gameObject);
    }

    //��ǥ7: �÷��̾��� ������ ������ hitDamage ��ŭ ���ʹ��� hp�� ���ҽ�Ų��.
    //7-1: ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ
    //7-2: �׷��� ������ die ���·� ��ȯ
    public void DamageAction(int damage)
    {   
        //���� �̹� ���ʹ̰� �ǰݵưų�, ��� ���¶�� �������� ���� �ʴ´�.
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
        {
            return;
        }

        //�÷��̾��� ���ݷ� ��ŭ hp�� ����
        hp -= damage;

        //��ǥ8: ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ
        if (hp > 0)
        {
            enemyState = EnemyState.Damaged;
            print("���� ��ȯ: Any State -> Damaged");
            Damaged();
        }
        //��ǥ9: �׷��� ������ die ���·� ��ȯ
        else
        {
            enemyState = EnemyState.Die;
            print("���� ��ȯ: Any State -> Die");
            Die();
        }

    }

    private void Damaged()
    {   
        //�ǰݸ�� 0.5 
        //�ǰ� ���� ó���� ���� �ڷ�ƾ ����
        StartCoroutine(DamageProcess());
    }
    //������ ó����
    IEnumerator DamageProcess()
    {
        //�ǰ� ��� �ð���ŭ ��ٸ���.
        yield return new WaitForSeconds(0.5f);

        //���� ���¸� �̵� ���·� ��ȯ�Ѵ�.
        enemyState = EnemyState.Move;
        print("���� ��ȯ: Damaged -> Move");
    }


    // ��ǥ6: �ʱ� ��ġ�� ���ƿ´�. Ư�� �Ÿ� �̳���, Idle ���·� ��ȯ�Ѵ�.
    private void Return()
    {
        float distanceToOriginPos = (originPos - transform.position).magnitude;
        //�ʱ� ��ġ�� ���ƿ´�
        if (distanceToOriginPos > returnDistance)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            characterController.Move(dir * moveSpeed *Time.deltaTime);
        }
        //Ư�� �Ÿ� �̳���, Idle ���·� ��ȯ�Ѵ�.
        else
        {
            enemyState = EnemyState.Idle;
            print("���� ��ȯ: Return -> Idle");
        }
    }

    private void Attack()
    {
        //��ǥ4: �÷��̾ ���� ���� ���� ������ Ư�� �ð��� �ѹ��� �����Ѵ�.
        float distanceToPlayer = (player.position - transform.position).magnitude;
        if (distanceToPlayer < attackDistance)
        {
            //Ư�� �ð��� �ѹ��� �����Ѵ�.
            currenTime += Time.deltaTime;
            if(currenTime > attackDelay)
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("���� ����!");
                currenTime = 0;
            }
        }
        else
        {
            //�׷��� ������ Move�� ���¸� ��ȯ�Ѵ�.
            enemyState = EnemyState.Move;
            print("���� ��ȯ: Attack -> Move");
            currenTime = attackDelay;

        }

    }
    //��ǥ3: ���� ���°� Move�� ��, �÷��̾���� �Ÿ��� ���� ���� ���̸� ���� �÷��̾ ���󰣴�.
    private void Move()
    {
        //�÷��̾���� �Ÿ��� ���� ���� ���̸� ���� �÷��̾ ���󰣴�.
        float distanceToPlayer = (player.position - transform.position).magnitude;

        //��ǥ5: �÷��̾ ���󰡴ٰ� �ʱ� ��ġ���� ���� �Ÿ��� �����
        float distanceToOriginPos = (originPos - transform.position).magnitude;
        if (distanceToOriginPos > moveDistance)
        {
            enemyState = EnemyState.Return;
            print("���� ��ȯ: Move -> Return");
        }

        if (distanceToPlayer > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;

            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }

        else
        {
            //���� ���� ���� ������ ���¸� ��ȯ�Ѵ�.
            enemyState = EnemyState.Attack;
            print("������ȯ: Move -> Attack");
        }
    }
    //��ǥ2: �÷��̾���� �Ÿ��� �����ؼ� Ư�� ���·� ������ش�.
    private void Idle()
    {
        float distanceToPlayer = (player.position - transform.position).magnitude;
        //float tempDist = Vector3.Distance(transform.position, player.position);

        // ���� �÷��̾���� �Ÿ��� Ư�� �������� �� ������ ���¸� Move�� �ٲ��ش�.
        if(distanceToPlayer < findDistance)
        {
            enemyState = EnemyState.Move;
            print("������ȯ: Idle -> Move");
        }
        
    }

    
}
