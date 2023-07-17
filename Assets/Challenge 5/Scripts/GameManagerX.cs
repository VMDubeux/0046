using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManagerX : MonoBehaviour
{
    #region Variables
    //Public Variables:
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameWinText;
    public TextMeshProUGUI livesText;
    public GameObject titleScreen;
    public Button restartButton;
    public List<GameObject> targetPrefabs;
    public bool isGameActive;
    public bool menuTitleActive = true;
    public GameObject PauseMenu;

    //Private Variables:
    private int score;
    private float time = 60.0f;
    private float spawnRate = 1.5f;
    private int lives = 3;

    //Private Readonly Variables:
    private readonly float spaceBetweenSquares = 2.5f;
    private readonly float minValueX = -3.75f; //  x value of the center of the left-most square
    private readonly float minValueY = -3.75f; //  y value of the center of the bottom-most square 
    #endregion

    void Start()
    {
        
    }

    private void Update()
    {
        if (isGameActive == true)
        {
            Timing();
        }
        if (menuTitleActive == false)
        {
            EnablePauseMenu();
        }
    }

    private void Timing() // Update score with value from target clicked
    {
        time -= Time.deltaTime;
        timerText.text = $"Timer: {time:00}";
        if (time <= 0)
        {
            gameWinText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            isGameActive = false;
        }
    }

    public void LivesCount(int damage)
    {
        lives -= damage;
        livesText.text = $"Lives: {lives:00}";
        if (lives <= 0) GameOver();
    }

    public void UpdateScore(int scoreToAdd) // Update score with value from target clicked
    {
        score += scoreToAdd;
        scoreText.text = $"Score: {score}";
    }

    public void StartGame(int difficulty) // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    {
        isGameActive = true;
        score = 0;
        spawnRate /= difficulty;

        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        titleScreen.SetActive(false);
        menuTitleActive = false;
        scoreText.text = $"Score: {score}";
        timerText.text = $"Timer: {time:00}";
        livesText.text = $"Lives: {lives:00}";
    }

    IEnumerator SpawnTarget() // While game is active spawn a random target
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            if (isGameActive)
            {
                Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
            }
        }
    }

    Vector3 RandomSpawnPosition() // Generate a random spawn position based on a random index from 0 to 3
    {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new(spawnPosX, spawnPosY, 0);
        return spawnPosition;
    }

    int RandomSquareIndex() // Generates random square index from 0 to 3, which determines which square the target will appear in
    {
        return Random.Range(0, 4);
    }

    void EnablePauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive == true)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
            isGameActive = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isGameActive == false)
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
            isGameActive = true;
        }
    }

    public void GameOver() // Stop game, bring up game over text and restart button
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    public void RestartGame() // Restart game by reloading the scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
