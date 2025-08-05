using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public BattleSystem Instance;
    public Button rock, paper, scissors;
    public TextMeshProUGUI timerLabel;
    public TextMeshProUGUI outcomeLabel;
    public TextMeshProUGUI scoreLabel;


    private PetData petData;
    private EnemyData enemyData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private enum Move {Rock, Paper, Scissors, None}
    private Move playerMove = Move.None;
    private Move enemyMove = Move.None;
    private int playerScore = 0;
    private int enemyScore = 0;
    private float turnTime = 5f;
    void Start()
    {
        petData = BattleManager.Instance.selectedPet;
        enemyData = BattleManager.Instance.selectedEnemy;

        rock.onClick.AddListener(() => PlayerSelectMove(Move.Rock));
        paper.onClick.AddListener(() => PlayerSelectMove(Move.Paper));
        scissors.onClick.AddListener(() => PlayerSelectMove(Move.Scissors));

        StartCoroutine(StartTurn());
    }

    void PlayerSelectMove(Move move) {
        playerMove = move;
    }

    IEnumerator StartTurn()
    {
        playerMove = Move.None;
        float timer = turnTime;

        while (turnTime > 0 && playerMove == Move.None)
        {
            timerLabel.text = "Time: " + timer;
            timer -= Time.deltaTime;
            yield return null;
        }

        if (playerMove == Move.None)
        {
            playerMove = (Move)Random.Range(0, 2);
            yield return new WaitForSeconds(5f);
        }

        enemyMove = (Move)Random.Range(0, 3);
        ResolveRound();
        if (playerScore == 5)
        {
            yield break;
        }
        else if (enemyScore == 5)
        {
            yield break;
        }
        turnTime -= 0.5f;
        yield return new WaitForSeconds(2f);
        StartCoroutine(StartTurn());
    }

    void ResolveRound()
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
        outcomeLabel.text = outcome;
        outcomeLabel.gameObject.SetActive(true);
        outcomeLabel.gameObject.SetActive(false);
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

}
