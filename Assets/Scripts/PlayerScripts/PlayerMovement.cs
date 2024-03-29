﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //Variables
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Text countText;
    public Text win;
    Vector2 movement;
    Vector2 mousePos;
    public Enemy enemy;
    private Camera gameCamera;
    public Camera camera;
    private int count;
    private int enemyCount;
    public GameObject gameOverText, quitButton, youWinText, restartButton, nextWaveButton, startButton;

    // when player reaches specific count perfom action
    [SerializeField] private int winCount;
    [SerializeField] private int outOfCount;

    void Start()
    {
        // hide all UI Texts on game start
        gameOverText.SetActive(false);
        restartButton.SetActive(false);
        youWinText.SetActive(false);
        nextWaveButton.SetActive(false);
        quitButton.SetActive(false);
        startButton.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        // setting current counter to ZERO on start
        count=0;
        win.text="";
        setCountText();
    }

    void Update()
    {
        // Move using WSAD around the designated playing area
        // References : Brackeys  - https://www.youtube.com/watch?v=LNLVOjbrQj4
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        float yValue = Mathf.Clamp(rb.position.y, -4.5f, 4.5f);
        float xValue = Mathf.Clamp(rb.position.x, -8.95f, 8.95f);

        rb.position = new Vector2(xValue, yValue);

        // mouse position determines the players direction
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate(){
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if player collides with enemy display GAME OVER text UI's and Buttons
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            gameOverText.SetActive(true);
            restartButton.SetActive(true);
            quitButton.SetActive(true);
            gameObject.SetActive(false);
            startButton.SetActive(true);
        }
    }

    // Collectable items
    void OnTriggerEnter2D(Collider2D other){
        // if player collides with PickUp add to the counter calling setCountText() method
        if(other.gameObject.CompareTag("PickUp")){
            // Destroy(other.gameObject);
            other.gameObject.SetActive(false);
            count= count+1;
            setCountText();     
        }

        enemy = other.GetComponent<Enemy>();

        // if player collides with an enemy destroy the player
        if (enemy)
        {
            Destroy(gameObject);
        }
    }

    //Score counter and winner message
    void setCountText(){
        // TEXT UI DISPLAY
        countText.text="SCORE : "+count.ToString() + "/"+outOfCount;

        // if count reaches =round win count activate UI components to move to the next wave
        if (count== winCount)
        {
            nextWaveButton.SetActive(true);
            youWinText.SetActive(true);
            Destroy(gameObject);
            win.text="YOU WIN!";
        }
    }

    // References :
    // Brackeys - https://www.youtube.com/watch?v=LNLVOjbrQj4
}