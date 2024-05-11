using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasketScript : MonoBehaviour
{

    public AudioSource audio;
    int currentSceneIndex;
    public GameObject ball;
    public Logic logic;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Ball"))
        {
            if (isBallDroppingFromTop(collision))
            {
                audio.Play();
                ball.SetActive(false);
                StartCoroutine(delayCode());
            }
        }
    }

    bool isBallDroppingFromTop(Collider2D ballCollider)
    {
        Rigidbody2D ballRigidbody = ballCollider.GetComponent<Rigidbody2D>();
        return ballRigidbody != null && ballRigidbody.velocity.y < 0f;
    }

    IEnumerator delayCode()
    {
        yield return new WaitForSeconds(2f);
        if (currentSceneIndex <= 6)
            SceneManager.LoadScene(currentSceneIndex + 1);
        else
            logic.showWinGame();
    }
}
