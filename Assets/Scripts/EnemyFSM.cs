using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//목표: 적을 FSM 다이어 그램에 따라 동작 시키고 싶다.
//필요속성: 적 상태

//목표2: 플레이어와의 거리를 측정해서 특정 상태로 만들어준다.
//필요속성2: 플레이어와의 거리, 플레이어 트랜스폼

//목표3: 적의 상태가 Move일 때, 플레이어와의 거리가 공격 범위 밖이면 적이 플레이어를 따라간다.
//필요속성3: 이동 속도, 적의 이동을 위한 캐릭터 컨트롤러, 공격 범위

//목표4: 플레이어가 공격 범위 내에 들어오면 특정 시간에 한번씩 어택 파워만큼 공격한다.
//필요속성4: 현재시간, 특정공격딜레이

//목표5: 플레이어를 따라가다가 초기 위치에서 일정 거리를 벗어나면 Return 상태로 전환한다.
//필요속성5: 초기 위치, 이동가능 범위

// 목표6: 초기 위치로 돌아온다. 특정 거리 이내면, Idle 상태로 전환한다.
//필요속성6: 특정 거리

//목표7: 플레이어의 공격을 받으면 hitDamage 만큼 에너미의 hp를 감소시킨다.
//필요속성7: hp

//목표8: 2초 후에 내 자신을 제거하겠다.



public class EnemyFSM : MonoBehaviour
{
    //1적 속성
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    //필요속성2: 플레이어와의 거리, 플레이어 트랜스폼
    public float findDistance = 10f;
    Transform player;

    //필요속성 3: 이동 속도, 적의 이동을 위한 캐릭터 컨트롤러
    public float moveSpeed;
    CharacterController characterController;
    public float attackDistance = 2f;

    //필요속성4: 현재시간, 특정공격딜레이, 어택파워
    float currenTime = 0;
    public float attackDelay = 2f;
    public int attackPower = 3;

    //필요속성5: 초기 위치, 이동가능 범위
    Vector3 originPos;
    public float moveDistance =10;

    //필요속성6: 특정 거리
    public float returnDistance;

    //필요속성7: hp
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
        //목표:적을 FSM 다이어그램에 따라 동작시키고 싶다.
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
    //목표9: 2초 후에 내 자신을 제거하겠다.

    IEnumerator DieProcess()
    {
        yield return new WaitForSeconds(2);

        print("사망");
        Destroy(gameObject);
    }

    //목표7: 플레이어의 공격을 받으면 hitDamage 만큼 에너미의 hp를 감소시킨다.
    //7-1: 에너미의 체력이 0보다 크면 피격 상태로 전환
    //7-2: 그렇지 않으면 die 상태로 전환
    public void DamageAction(int damage)
    {   
        //만약 이미 에너미가 피격됐거나, 사망 상태라면 데미지를 주지 않는다.
        if (enemyState == EnemyState.Damaged || enemyState == EnemyState.Die)
        {
            return;
        }

        //플레이어의 공격력 만큼 hp를 감소
        hp -= damage;

        //목표8: 에너미의 체력이 0보다 크면 피격 상태로 전환
        if (hp > 0)
        {
            enemyState = EnemyState.Damaged;
            print("상태 전환: Any State -> Damaged");
            Damaged();
        }
        //목표9: 그렇지 않으면 die 상태로 전환
        else
        {
            enemyState = EnemyState.Die;
            print("상태 전환: Any State -> Die");
            Die();
        }

    }

    private void Damaged()
    {   
        //피격모션 0.5 
        //피격 상태 처리를 위한 코루틴 실행
        StartCoroutine(DamageProcess());
    }
    //데미지 처리용
    IEnumerator DamageProcess()
    {
        //피격 모션 시간만큼 기다린다.
        yield return new WaitForSeconds(0.5f);

        //현재 상태를 이동 상태로 전환한다.
        enemyState = EnemyState.Move;
        print("상태 전환: Damaged -> Move");
    }


    // 목표6: 초기 위치로 돌아온다. 특정 거리 이내면, Idle 상태로 전환한다.
    private void Return()
    {
        float distanceToOriginPos = (originPos - transform.position).magnitude;
        //초기 위치로 돌아온다
        if (distanceToOriginPos > returnDistance)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            characterController.Move(dir * moveSpeed *Time.deltaTime);
        }
        //특정 거리 이내면, Idle 상태로 전환한다.
        else
        {
            enemyState = EnemyState.Idle;
            print("상태 전환: Return -> Idle");
        }
    }

    private void Attack()
    {
        //목표4: 플레이어가 공격 범위 내에 들어오면 특정 시간에 한번씩 공격한다.
        float distanceToPlayer = (player.position - transform.position).magnitude;
        if (distanceToPlayer < attackDistance)
        {
            //특정 시간에 한번씩 공격한다.
            currenTime += Time.deltaTime;
            if(currenTime > attackDelay)
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("적의 공격!");
                currenTime = 0;
            }
        }
        else
        {
            //그렇지 않으면 Move로 상태를 전환한다.
            enemyState = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currenTime = attackDelay;

        }

    }
    //목표3: 적의 상태가 Move일 때, 플레이어와의 거리가 공격 범위 밖이면 적이 플레이어를 따라간다.
    private void Move()
    {
        //플레이어와의 거리가 공격 범위 밖이면 적이 플레이어를 따라간다.
        float distanceToPlayer = (player.position - transform.position).magnitude;

        //목표5: 플레이어를 따라가다가 초기 위치에서 일정 거리를 벗어나면
        float distanceToOriginPos = (originPos - transform.position).magnitude;
        if (distanceToOriginPos > moveDistance)
        {
            enemyState = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }

        if (distanceToPlayer > attackDistance)
        {
            Vector3 dir = (player.position - transform.position).normalized;

            characterController.Move(dir * moveSpeed * Time.deltaTime);
        }

        else
        {
            //공격 범위 내로 들어오면 상태를 변환한다.
            enemyState = EnemyState.Attack;
            print("상태전환: Move -> Attack");
        }
    }
    //목표2: 플레이어와의 거리를 측정해서 특정 상태로 만들어준다.
    private void Idle()
    {
        float distanceToPlayer = (player.position - transform.position).magnitude;
        //float tempDist = Vector3.Distance(transform.position, player.position);

        // 현재 플레이어와의 거리가 특정 범위보다 더 가까우면 상태를 Move로 바꿔준다.
        if(distanceToPlayer < findDistance)
        {
            enemyState = EnemyState.Move;
            print("상태전환: Idle -> Move");
        }
        
    }

    
}
