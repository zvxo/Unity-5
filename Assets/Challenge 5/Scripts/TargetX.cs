using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetX : MonoBehaviour
{
    private GameManagerX gameManagerX;
    public int pointValue;
    public GameObject explosionFx;

    public float timeOnScreen = 1.0f;

    private float minValueX = -3.75f; // the x value of the center of the left-most square
    private float minValueY = -3.75f; // the y value of the center of the bottom-most square
    private float spaceBetweenSquares = 2.5f; // the distance between the centers of squares on the game board

    void Start()
    {
        gameManagerX = GameObject.Find("Game Manager").GetComponent<GameManagerX>();
        transform.position = RandomSpawnPosition(); 
        StartCoroutine(RemoveObjectRoutine()); // begin timer before target leaves screen
    }

    // When target is clicked, destroy it, update score, and generate explosion
    private void OnMouseDown()
    {
        if (gameManagerX.isGameActive)
        {
            Destroy(gameObject);
            gameManagerX.UpdateScore(pointValue);
            Explode();
        }
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition()
    {
        // Randomize X and Y independently for a more varied grid pattern
        float spawnPosX = minValueX + (Random.Range(0, 4) * spaceBetweenSquares);  // Random X position
        float spawnPosY = minValueY + (Random.Range(0, 4) * spaceBetweenSquares);  // Random Y position

        return new Vector3(spawnPosX, spawnPosY, 0);
    }

    // If target that is NOT the bad object collides with sensor, trigger game over
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sensor") && !gameObject.CompareTag("Bad"))
        {
            gameManagerX.GameOver();
            Destroy(gameObject); // Destroy after logic
        }
    }

    // Display explosion particle at object's position
    void Explode()
    {
        Instantiate(explosionFx, transform.position, explosionFx.transform.rotation);
    }

    // After a delay, destroy object or move off-screen
    IEnumerator RemoveObjectRoutine()
    {
        yield return new WaitForSeconds(timeOnScreen);
        if (gameManagerX.isGameActive)
        {
            Destroy(gameObject); // Destroy after time is up
        }
    }
}
