using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;

    public Text scoreText;

    public Text winText;

    public Text livesText;

    public GameObject Player;

    public AudioSource musicSource;

    public AudioClip backgroundMusic;

    public AudioClip winMusic;

    private int score;
    private int lives;

    private bool facingRight = true;
    private bool isJumping = true;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        score = 0;
        SetScoreText();

        lives = 3;
        SetLivesText();
        winText.text = "";

        musicSource.clip = backgroundMusic;
        musicSource.Play();
        musicSource.loop = true;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (isJumping == true && verMovement > 0)
        {
            anim.SetInteger("State", 2);
        }
        else if (isJumping == false && verMovement < 0)
        {
            anim.SetInteger("State", 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            score = score + 1;
            SetScoreText();
            Destroy(collision.collider.gameObject);
        }
        
        if(collision.collider.tag == "Enemy")
        {
            lives = lives - 1;
            SetLivesText();
            Destroy(collision.collider.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 8), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);
            }
        }
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
        if (score >= 8)
        {
            winText.text = "You Win! Game Created By: Mia Torres";
            musicSource.clip = winMusic;
            musicSource.Play();
            musicSource.loop = false;
            speed = 0;
            rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        if (score == 4)
        {
            transform.position = new Vector2(44.0f, 0.0f);
            lives = 3;
            SetLivesText();
        }
    }

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives <=0)
        {
            winText.text = "You Lose, Game Created By: Mia Torres";
            musicSource.clip = backgroundMusic;
            musicSource.Stop();
            gameObject.SetActive(false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
