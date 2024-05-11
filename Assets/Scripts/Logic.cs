using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Logic : MonoBehaviour
{
    Camera cam;

    public Ball ball;
    public PathWay trajectory;
    [SerializeField] float PushForce = 4f;

    bool isDragging;


    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector2 force;
    float distance;

    public Scrollbar scrollbar;
    public Image scrollbarHandle;
    public Text powerText;

    public AudioSource audio;

    public int heartNumber = 3;

    public GameObject gameOver;
    public GameObject winGame;

    public Image heart1, heart2, heart3;

    private Vector2 initBallPosition;

    public float outRange = -12f;

    private void Start()
    {
        cam = Camera.main;
        audio = GetComponent<AudioSource>();

        initBallPosition = ball.transform.position;

        gameOver.SetActive(false);
        heart1.enabled = true;
        heart2.enabled = true;
        heart3.enabled = true;
        heartNumber = 3;

    }

    private void Update()
    {
        Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {

            if (ball.col == Physics2D.OverlapPoint(pos))
            {
                Time.timeScale = 0.1f;
                isDragging = true;
                OnDragStart();
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1f;
            isDragging = false;
            OnDragEnd();
        }
        if (isDragging)
        {
            OnDrag();
        }

        ChangePowerScrollColor();

        if (ball.gameObject.transform.position.y < outRange)
        {
            if (!audio.isPlaying)
            {
                heartNumber--;
                updateHeart();
                resetBallPosition();
                audio.Play();

            }
        }

    }


    #region Drag

    private void OnDragStart()
    {
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);

        trajectory.Show();
    }

    private void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = distance * direction * PushForce;

        setScrollbarValue(force.magnitude / 40f);
        powerText.text = "" + ((int)force.magnitude);


        trajectory.UpdateDots(ball.Pos, force);
    }

    private void OnDragEnd()
    {
        ball.Push(force);
        trajectory.Hide();
    }

    #endregion

    public void setScrollbarValue(float value)
    {
        scrollbar.value = value;
    }

    void ChangePowerScrollColor()
    {
        Color color = Color.Lerp(Color.green, Color.red, scrollbar.value);
        scrollbarHandle.color = color;
    }

    public void updateHeart()
    {
        switch (heartNumber)
        {
            case 0:
                gameOver.SetActive(true);
                ball.hide();
                heart1.enabled = false;
                heart2.enabled = false;
                heart3.enabled = false;
                break;
            case 1:
                heart1.enabled = true;
                heart2.enabled = false;
                heart3.enabled = false;
                break;
            case 2:
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = false;
                break;
            case 3:
                heart1.enabled = true;
                heart2.enabled = true;
                heart3.enabled = true;
                break;
        }
    }

    public void resetBallPosition()
    {
        ball.transform.position = initBallPosition;
        ball.resetPosition();
        audio.Stop();
    }

    public void resetGameOnClick()
    {
        SceneManager.LoadScene(0);
    }

    public void showWinGame()
    {
        winGame.SetActive(true);
    }
}
