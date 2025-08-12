using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem Instance { get; private set; }
    public Button rock, paper, scissors;
    public TextMeshProUGUI timerLabel;
    public TextMeshProUGUI outcomeLabel;
    public TextMeshProUGUI scoreLabel;


    private GameObject petData;
    private GameObject enemyData;

    private enum Move { Rock, Paper, Scissors, None }
    private Move playerMove = Move.None;
    private Move enemyMove = Move.None;
    private int playerScore = 0;
    private int enemyScore = 0;
    private float turnTime = 5f;
    public bool start = false;
    void Start()
    {
        StartCoroutine(WaitForStart());
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitUntil(() => start);
        petData = BattleManager.Instance.selectedPet;
        enemyData = BattleManager.Instance.selectedEnemy;

        rock.onClick.AddListener(() => PlayerSelectMove(Move.Rock));
        paper.onClick.AddListener(() => PlayerSelectMove(Move.Paper));
        scissors.onClick.AddListener(() => PlayerSelectMove(Move.Scissors));

        StartCoroutine(StartTurn(1));

    }

    void PlayerSelectMove(Move move)
    {
        playerMove = move;
    }

    IEnumerator StartTurn(int turn)
    {
        playerMove = Move.None;
        float timer = turnTime;

        if (turn == 1)
        {
        yield return StartCoroutine(ShowInfo("Game starting in 3", 1));
        yield return StartCoroutine(ShowInfo("Game starting in 2", 1));
        yield return StartCoroutine(ShowInfo("Game starting in 1", 1));
        }

        while (timer > 0f && playerMove == Move.None)
        {
            timerLabel.text = timer.ToString("F2");
            timer -= Time.deltaTime;
            yield return null;
        }

        if (playerMove == Move.None)
        {
            playerMove = (Move)Random.Range(0, 2);
            yield return StartCoroutine(ShowInfo("You didn't make a move this round!", 3f));
        }

        enemyMove = (Move)Random.Range(0, 3);
        yield return StartCoroutine(ResolveRound());
        if (playerScore == 5)
        {
            yield return StartCoroutine(ShowInfo("You win", 3));
            FinishGame();
            yield break;
        }
        else if (enemyScore == 5)
        {
            yield return StartCoroutine(ShowInfo("You lose", 3));
            FinishGame();
            yield break;
        }
        turnTime -= 0.5f;
        yield return new WaitForSeconds(2f);
        StartCoroutine(StartTurn(++turn));
    }

    void FinishGame()
    {
        Destroy(BattleManager.Instance.selectedEnemy);
        Destroy(BattleManager.Instance.selectedPet);
        SceneManager.UnloadSceneAsync("BattleScene");
        SceneManager.LoadScene("novime", LoadSceneMode.Additive);
    }

    IEnumerator ResolveRound()
    {
        string outcome = "";

        if (playerMove == enemyMove)
        {
            outcome = $"Draw! Both chose {playerMove}";
        }
        else if (
            (playerMove == Move.Rock && enemyMove == Move.Scissors) ||
            (playerMove == Move.Paper && enemyMove == Move.Rock) ||
            (playerMove == Move.Scissors && enemyMove == Move.Paper)
        )
        {
            outcome = $"You Win! {playerMove} beats {enemyMove}";
            playerScore++;
        }
        else
        {
            outcome = $"You Lose! {enemyMove} beats {playerMove}";
            enemyScore++;
        }

        scoreLabel.text = playerScore + " : " + enemyScore;
        yield return StartCoroutine(ShowInfo(outcome , 2));
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    IEnumerator ShowInfo(string info, float delay)
    {
        outcomeLabel.text = info;
        outcomeLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        outcomeLabel.gameObject.SetActive(false);
    }

}
