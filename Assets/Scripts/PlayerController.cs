using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController: MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    private int lives;
    public Text lifeText;
    public Text winText;
    public int lvl = 1;
    public static bool win = false;
    Animator anim;

    private bool facingRight = true;
    public AudioSource musicSource;
    public AudioClip jumpClip;
    public AudioClip enemyHitClip;

    public float targetTime;
    public Text timeText;

    public float powerupTime;
    public Text powerupText;
    private bool invincibility = false;
    private SpriteRenderer spriteRen;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        lives = 3;
        SetCountText();
        SetLifeText();
        SetTimeText();
        winText.text = "";
        powerupText.text = "";
        spriteRen = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        if (facingRight == false && hozMovement > 0) { Flip(); }
        else if (facingRight == true && hozMovement < 0) { Flip(); }
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        if (scoreValue == 4 && lvl == 1)
        {
            lvl = 2;
            transform.position = new Vector2(50.0f, 50.0f);
            lives = 3;
            SetLifeText();
        }

        if (Input.GetKeyDown(KeyCode.A)) //running left
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.A)) //cease running left
        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyDown(KeyCode.D)) //running left
        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D)) //cease running left
        {
            anim.SetInteger("State", 0);
        }
        //Timer Function
        if(win == false)
        {
            targetTime -= Time.deltaTime;
            SetTimeText();
            if (targetTime <= 0.0f)
            {
                lives = 0;
                SetLifeText();
            }
        }
        //Powerup Timer
        if(win == false)
        {
            powerupTime -= Time.deltaTime;
            setPowerupText();
        }
    }

    void setPowerupText() {
        if (powerupTime > 0.0f) {
            powerupText.text = "Invincibility: " + Mathf.Ceil(powerupTime) + " seconds";
        }
        else if (powerupTime < 0.0f){
            powerupText.text = "";
            invincibility = false;
            spriteRen.color = new Color(255, 255, 255);
        }
    }

    void SetTimeText()
    {
        timeText.text = "Time Remaining: " + Mathf.Ceil(targetTime) + " seconds";
    }

    void SetCountText()
    {
        score.text = "Score: " + scoreValue.ToString();
        if (scoreValue >= 8)
        {
            win = true;
            winText.text = "You win! Game created by Travis Kosier.";
        }
    }

    void SetLifeText()
    {
        lifeText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            winText.text = "You lose! Game created by Travis Kosier.";
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            SetCountText();
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Enemy")
        {
            if (win == false && invincibility == false) {
                lives -= 1;
                SetLifeText();
            }
            //ENEMY COLLISION SOUND
            musicSource.clip = enemyHitClip;
            musicSource.Play();

            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.tag == "Powerup")
        {
            powerupTime = 15.0f;
            invincibility = true;
            spriteRen.color = new Color(0,255,30);
            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W)) //jumping
            { 
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); 
                if (anim.GetInteger("State") != 2)
                {
                    anim.SetInteger("State", 2);
                }
                // JUMP SOUND
                musicSource.clip = jumpClip;
                musicSource.Play();

            }
            else if (anim.GetInteger("State") == 2) {
                anim.SetInteger("State", 0);
            }
        }
    }
    private void Flip() {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}