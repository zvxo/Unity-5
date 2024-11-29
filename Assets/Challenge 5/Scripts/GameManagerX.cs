using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI timeText;
    public GameObject titleScreen;
    public Button restartButton; 

    public List<GameObject> targetPrefabs;

    private int score;
    public float timeValue;
    private float spawnRate = 1.5f;  // Default spawn rate
    public bool isGameActive;

    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; // X value of the center of the left-most square
    private float minValueY = -3.75f; // Y value of the center of the bottom-most square
    
    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame(int difficulty)
    {
        // Adjust spawn rate based on difficulty. 
        spawnRate /= difficulty;
        isGameActive = true;
        score = 0;
        timeValue = 60;
        UpdateScore(0);
        titleScreen.SetActive(false);

        // Start spawning targets
        StartCoroutine(SpawnTarget());
    }

    public void UpdateTime()
    {
        if (isGameActive)
        {
            timeValue -= Time.deltaTime;
            timeText.text = "Time: " + Mathf.Round(timeValue);
            if (timeValue <= 0)
            {
                GameOver();
            }
        }
    }
    
    // While game is active, spawn a random target at intervals
    IEnumerator SpawnTarget()
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

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        int spawnIndexX = RandomSquareIndex();
        int spawnIndexY = RandomSquareIndex();

        // Calculate spawn position
        float spawnPosX = minValueX + (spawnIndexX * spaceBetweenSquares);
        float spawnPosY = minValueY + (spawnIndexY * spaceBetweenSquares);

        return new Vector3(spawnPosX, spawnPosY, 0);
    }

    // Generates a random square index from 0 to 3, which determines where on the board the target will appear
    int RandomSquareIndex()
    {
        return Random.Range(0, 4);
    }

    // Update score with the value from target clicked
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    // Stop game, show game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart the game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
