using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    /// <Variables>
    //Public Variables:
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;
    public GameObject titleScreen;
    public Button restartButton;
    public List<GameObject> targetPrefabs;
    public bool isGameActive;

    //Private Variables:
    private int score;
    private float time;

    //Private Readonly Variables:
    private readonly float spawnRate = 1.5f;
    private readonly float spaceBetweenSquares = 2.5f;
    private readonly float minValueX = -3.75f; //  x value of the center of the left-most square
    private readonly float minValueY = -3.75f; //  y value of the center of the bottom-most square

    void Start()
    {
        time = 90.0f;
    }

    private void Timing() // Update score with value from target clicked
    {
        time -= Time.time;
        timerText.text = $"Timer: {time}";
    }

    public void StartGame() // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    {
        isGameActive = true;
        StartCoroutine(SpawnTarget());
        score = 0;
        UpdateScore(0);
        titleScreen.SetActive(false);
        Timing();
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

        Vector3 spawnPosition = new (spawnPosX, spawnPosY, 0);
        return spawnPosition;
    }

    int RandomSquareIndex() // Generates random square index from 0 to 3, which determines which square the target will appear in
    {
        return Random.Range(0, 4);
    }
   
    public void UpdateScore(int scoreToAdd) // Update score with value from target clicked
    {
        score += scoreToAdd;
        scoreText.text = $"Score: {score}";
    }

    public void GameOver() // Stop game, bring up game over text and restart button
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        isGameActive = false;
    }

    public void RestartGame() // Restart game by reloading the scene
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
