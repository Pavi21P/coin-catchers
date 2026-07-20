using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] int totalCoinsToWin = 44;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject loseScreen;

    void Awake()
    {
        int numberGameSessions = FindObjectsByType<GameSession>(FindObjectsSortMode.None).Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        scoreText.text = score.ToString();

        if (score >= totalCoinsToWin)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
        StartCoroutine(ReturnToStageOneAfterDelay());
    }

    IEnumerator ReturnToStageOneAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        winScreen.SetActive(false);

        playerLives = 3;
        score = 0;
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();

        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(1); // your Stage 1 build index
        Destroy(gameObject);
    }

    void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    void ResetGameSession()
    {
        StartCoroutine(ShowLoseScreenThenReset());
    }

IEnumerator ShowLoseScreenThenReset()
    {
        loseScreen.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;
        loseScreen.SetActive(false);

        FindFirstObjectByType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}