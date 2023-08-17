using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//목적: 폭탄을 특정 방향으로 발사하고 싶다.
//필요속성: 폭탄 게임오브젝트, 발사 위치, 방향
//순서1. 마우스 오른쪽 버튼을 누른다.
//순서2. 폭탄 게임오브젝트를 생성하고 firePosition에 위치시킨다.
//순서3. 폭탄 오브젝트의 rigidBody 를 가져와서 힘을 가한다.
public class PlayerFire : MonoBehaviour
{
    //필요속성: 폭탄 게임오브젝트, 발사 위치, 방향
    public GameObject bomb;
    public GameObject firePosition;
    public float power;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //순서1. 마우스 오른쪽 버튼을 누른다.
        if (Input.GetMouseButtonDown(1)) //왼쪽 (0) 오른쪽(1) 휠(2)
        {
            //순서2. 폭탄 게임오브젝트를 생성하고 firePosition에 위치시킨다.
            GameObject bombGO = Instantiate(bomb);
            bombGO.transform.position = firePosition.transform.position;

            //순서3. 폭탄 오브젝트의 rigidBody 를 가져와서 카메라 정면 방향으로 힘을 가한다.
            Rigidbody rigidbody = bombGO.GetComponent<Rigidbody>();
            rigidbody.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);
        }
    }
}
