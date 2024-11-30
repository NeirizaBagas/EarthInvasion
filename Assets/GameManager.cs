using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject boss;
    public bool startGame;
    public GameObject startPanel;
    public GameObject losePanel;
    public bool gameOver;
    public GameObject winPanel;
    public bool win;
    public int enemyDestroy;
    public int score;
    public int finalScore;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalText;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Hancurkan instance kedua
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.SetActive(false);
        enemy1.SetActive(false);
        enemy2.SetActive(false);
        boss.SetActive(false);
        startGame = true;
        startPanel.SetActive(true);
        gameOver = false;
        losePanel.SetActive(false);
        win = false;
        winPanel.SetActive(false);
        enemyDestroy = 0;
        score = 0;
        UpdateScore();
        scoreText.gameObject.SetActive(false);
        finalText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && startGame && !gameOver)
        {
            startGame = false;
            startPanel.SetActive(false);
            scoreText.gameObject.SetActive(true);
            Player.SetActive(true);
            enemy1.SetActive(true);
        }

        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }

        if (win)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }

        if (enemyDestroy == 5)
        {
            Phase2();
        }
        else if (enemyDestroy == 9)
        {
            Phase3();
        }
        else if (enemyDestroy == 15)
        {
            WinCondition();
        }
    }


    public void Phase2()
    {
        if (enemy2 != null) enemy2.SetActive(true);
    }

    public void Phase3()
    {
        if (boss != null) boss.SetActive(true);
    }

    public void GameOver()
    {
        gameOver = true;
        losePanel.SetActive(true);
        FinalScore();
    }

    public void WinCondition()
    {
        win = true;
        winPanel.SetActive(true);
        FinalScore();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();

    }

    public void FinalScore()
    {
        scoreText.gameObject.SetActive(false);
        finalText.gameObject.SetActive(true);
        finalScore = score;
        finalText.text = finalScore.ToString();
    }

    public void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

}
