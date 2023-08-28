using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//목적: 게임의 상태(Ready, Start, GameOver)를 구별하고 게임의 시작과 끝을 TextUI로 표현 하고싶다.
//필요속성: 게임 상태 열거형 변수, TextUI

//목적2: 2초 후 Ready 상태에서 Start상태로 변경되며 게임이 시작된다.

//목적3: 플레이어의 hp가 0보다 작으면 상태텍스트를 와 상태를 GameOver로 바꿔준다.
// 필요속성: hp가 들어있는 playerMove
//목적4: 플레이어의 hp가 0이하라면 플레이어의 애니메이션을 멈추다.
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    //필요속성: 게임 상태 열거형 변수, TextUI

    public enum GameState
    {
        Ready,
        Start,
        GameOver
    }

    public GameState state = GameState.Ready;
    public TMP_Text stateText;

    // 필요속성: hp가 들어있는 playerMove
    PlayerMove player;

    Animator animator;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stateText.text = "Ready";
        stateText.color = new Color32(255, 185, 0, 255);

        StartCoroutine(GameStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();

        animator = player.GetComponentInChildren<Animator>();


    }
    //목적2: 2초 후 Ready 상태에서 Start상태로 변경되며 게임이 시작된다.
    IEnumerator GameStart()
    {
        //2초를 기다린다.
        yield return new WaitForSeconds(2);

        stateText.text = "Game Start";
        stateText.color = new Color32(0, 255, 0, 255);

        //0초를 기다린다.
        yield return new WaitForSeconds(0.5f);

        stateText.gameObject.SetActive(false);

        state = GameState.Start;

    }

    void CheckGameOver()
    {
        //목적3: 플레이어의 hp가 0보다 작으면 상태텍스트를 와 상태를 GameOver로 바꿔준다.
        if(player.hp <= 0)
        {
            //상태 텍스트ON
            stateText.gameObject.SetActive(true);

            //상태 텍스트를 GameOver로 변경
            stateText.text = "Game Over";

            stateText.color = new Color32(255, 0, 0, 255);

            state = GameState.GameOver;

        }

    }

    // Update is called once per frame
    void Update()
    {
        CheckGameOver();
    }
}
