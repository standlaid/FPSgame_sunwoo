using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//목적: WASD키를 누르면 캐릭터를 그 방향으로 이동 시키고 싶다.
//필요속성 이동속도
//순서1. 사용자의 입력을 받는다.
//순서2. 이동 방향을 설정한다.
//순서3. 이동 속도에 따라 나를 이동 시킨다.

//목적2. 스페이스를 누르면 수직으로 점프하고 싶다.
//필요속성: 캐릭터 컨트롤러, 중력 변수, 수직 속력 변수
//2-1. 캐릭터 수직 속도에 중력을 적용하고 싶다.
//2-2. 캐릭터 컨트롤러로 나를 이동시키고 싶다.
//2-3. 스페이스 키를 누르면 수직 속도에 점프 파워를 적용하고 싶다.

//목적3: 플레이어가 피격을 당하면 hp를 Damage만큼 깎는다.
//필요속성3: hp

public class PlayerMove : MonoBehaviour
{
    //필요속성 이동속도
    public float speed = 10;

    //필요속성: 캐릭터 컨트롤러, 중력 변수, 수직 속력 변수, 점프 파워, 점프 상태 변수
    CharacterController characterController;
    public float gravity = -20f;
    public float yVelocity = 0;
    public float jumpPower = 10;
    public bool isJumping = false;

    //필요속성3: hp
    public int hp = 10;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //순서1. 사용자의 입력을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //점프 중이었다면 점프 전 상태로 초기화 하고 싶다.

        if (isJumping && characterController.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;

            yVelocity = 0;
        }

        else if(characterController.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
        }
        //2-3. 스페이스 키를 누르면 수직 속도에 점프 파워를 적용하고 싶다.
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
            
        }
        
        //Debug.Log

        //순서2. 이동 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);

        //2-1. 캐릭터 수직 속도에 중력을 적용하고 싶다.

        yVelocity = yVelocity + gravity * Time.deltaTime;
        dir.y = yVelocity;


        //순서3. 이동 속도에 따라 나를 이동 시킨다.
        //transform.position += dir * speed * Time.deltaTime;

        //2-2. 캐릭터 컨트롤러로 나를 이동시키고 싶다.
        characterController.Move(dir * speed * Time.deltaTime);

    }

    //목적3: 플레이어가 피격을 당하면 hp를 Damage만큼 깎는다.
    public void DamageAction(int damage)
    {
        hp -= damage;
    }
}
