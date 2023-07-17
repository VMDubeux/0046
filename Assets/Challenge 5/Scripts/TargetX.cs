using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetX : MonoBehaviour
{
    #region Variables
    //Public Variables:
    public int pointValue;
    public GameObject explosionFx;
    public float timeOnScreen = 1.0f;

    //Private Variables:
    private Rigidbody rb;
    private GameManagerX gameManagerX;

    //Private Readonly Variables:
    private readonly float minValueX = -3.75f; // the x value of the center of the left-most square
    private readonly float minValueY = -3.75f; // the y value of the center of the bottom-most square
    private readonly float spaceBetweenSquares = 2.5f; // the distance between the centers of squares on the game board 
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManagerX = GameObject.Find("Game Manager").GetComponent<GameManagerX>();

        transform.position = RandomSpawnPosition();
        StartCoroutine(RemoveObjectRoutine()); // begin timer before target leaves screen
    }

    /*private void OnMouseDown() // When target is clicked, destroy it, update score, and generate explosion
     {
         if (gameManagerX.isGameActive)
         {
             Destroy(gameObject);
             gameManagerX.UpdateScore(pointValue);
             Explode();
         }
     }*/

    public void DestroyTargetX()
    {
        if (gameManagerX.isGameActive)
        {
            Destroy(gameObject);
            Instantiate(explosionFx, transform.position, explosionFx.transform.rotation);
            gameManagerX.UpdateScore(pointValue);
            if (gameObject.CompareTag("Bad")) 
            {
                gameManagerX.LivesCount(1);
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

    void OnTriggerEnter(Collider other) // If target that is the bad object collides with sensor
    {
        Destroy(gameObject);

        if (gameManagerX.isGameActive && other.gameObject.CompareTag("Sensor") && !gameObject.CompareTag("Bad"))
        {
            gameManagerX.LivesCount(1);
        }
    }

    IEnumerator RemoveObjectRoutine() // After a delay, Moves the object behind background so it collides with the Sensor object
    {
        yield return new WaitForSeconds(timeOnScreen);
        if (gameManagerX.isGameActive)
        {
            transform.Translate(Vector3.forward * 5, Space.World);
        }
    }
}