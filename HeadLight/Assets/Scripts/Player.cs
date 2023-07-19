using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    
    private float speed;

    [Header("Coins")]
    public int coins;
    [SerializeField] private TMP_Text coinsText;
    
    [Header("Player")]
    [SerializeField] private float playerRunSpeed;
    [SerializeField] private float playerCrouchSpeed;
    [SerializeField] private float jumpForce;
    private float movementX;

    [Header("Health")]
    public float health;
    private float healthMax = 100f;
    [SerializeField] private Image healthImage;
    
    [Header("Components")]
    private Rigidbody2D rb2d;
    private Animator animator;

    private bool isGrounded;
    
    private bool doubleJump;
    private int state;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        Movement();
        FlipSprite();
        Crouch();
        PlayerDeath();
        animator.SetInteger("State", state);
        
    }
    
    private void Update()
    {
        PlayerJump();
        
        if (health > 100)
        {
            health = 100;
        }
        
        healthImage.fillAmount = health / healthMax;
        coinsText.text = $"${coins}";
    }

    private void Movement()
    {
        movementX = Input.GetAxisRaw("Horizontal");

        state = 0;

        rb2d.velocity = new Vector2(movementX * speed, rb2d.velocity.y);

        animator.SetFloat("xVelocity", rb2d.velocity.magnitude);
        animator.SetFloat("yVelocity", rb2d.velocity.y);
    }

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.S))
        {
            speed = playerCrouchSpeed;
            state = 2;
        }
        else
        {
            speed = playerRunSpeed;
        }
    }

    private void PlayerDeath()
    {
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            rb2d.bodyType = RigidbodyType2D.Static;
            StartCoroutine(WaitBeforeDeath());
        }
    }

    private void FlipSprite()
    {
        if (movementX < 0 && health > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            Arrow.view = -1;
            state = 1;
        }
        else if (movementX > 0 && health > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            Arrow.view = 1;
            state = 1;
        }
    }


    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                doubleJump = true;
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
            else if (doubleJump)
            {
                doubleJump = false;
                animator.SetBool("isJumping", false);
                animator.SetBool("DoubleJump", true);
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            doubleJump = true;
            animator.SetBool("isJumping", !isGrounded);
            animator.SetBool("DoubleJump", false);
            animator.SetBool("onGround", isGrounded);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Trap"))
        {
            health = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
            animator.SetBool("onGround", isGrounded);
        } 
    }

    IEnumerator WaitBeforeDeath()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
