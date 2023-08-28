using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//����: ������ ����(Ready, Start, GameOver)�� �����ϰ� ������ ���۰� ���� TextUI�� ǥ�� �ϰ�ʹ�.
//�ʿ�Ӽ�: ���� ���� ������ ����, TextUI

//����2: 2�� �� Ready ���¿��� Start���·� ����Ǹ� ������ ���۵ȴ�.

//����3: �÷��̾��� hp�� 0���� ������ �����ؽ�Ʈ�� �� ���¸� GameOver�� �ٲ��ش�.
// �ʿ�Ӽ�: hp�� ����ִ� playerMove
//����4: �÷��̾��� hp�� 0���϶�� �÷��̾��� �ִϸ��̼��� ���ߴ�.
public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    //�ʿ�Ӽ�: ���� ���� ������ ����, TextUI

    public enum GameState
    {
        Ready,
        Start,
        GameOver
    }

    public GameState state = GameState.Ready;
    public TMP_Text stateText;

    // �ʿ�Ӽ�: hp�� ����ִ� playerMove
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
    //����2: 2�� �� Ready ���¿��� Start���·� ����Ǹ� ������ ���۵ȴ�.
    IEnumerator GameStart()
    {
        //2�ʸ� ��ٸ���.
        yield return new WaitForSeconds(2);

        stateText.text = "Game Start";
        stateText.color = new Color32(0, 255, 0, 255);

        //0�ʸ� ��ٸ���.
        yield return new WaitForSeconds(0.5f);

        stateText.gameObject.SetActive(false);

        state = GameState.Start;

    }

    void CheckGameOver()
    {
        //����3: �÷��̾��� hp�� 0���� ������ �����ؽ�Ʈ�� �� ���¸� GameOver�� �ٲ��ش�.
        if(player.hp <= 0)
        {
            //���� �ؽ�ƮON
            stateText.gameObject.SetActive(true);

            //���� �ؽ�Ʈ�� GameOver�� ����
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
