using System.Collections;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float attackDamage;
    [SerializeField] private int cooldownTime;
    [SerializeField] private int maxHealth = 100;
    
    [Header("Enemy Patrol")]
    private int currentPoint;
    private int currentHealth;
    private float speed;
    private bool isOnCooldown;
    
    [Header("Enemy Components")]
    [SerializeField] private Transform[] points;
    private Transform player;
    private Vector3 target;

    private Rigidbody2D rb2d;
    private Animator animator;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        Patrol();
        FlipSprite();
        
        rb2d.velocity = Vector3.zero;
    }

    private void FlipSprite()
    {
        if (transform.position.x < target.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (transform.position.x > target.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private void Patrol()
    {
        if (player != null)
        {
            target = player.position;

            if (isOnCooldown)
            {
                target.x += 1.5f;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

            speed = attackSpeed;
        }
        else
        {
            target = points[currentPoint].position;
            speed = patrolSpeed;
            animator.SetBool("isFlying", true);

            if (transform.position == points[currentPoint].position)
            {
                currentPoint++;

                if (currentPoint == points.Length)
                {
                    currentPoint = 0;
                }
            }
        }
        
        transform.position = Vector3.MoveTowards(transform.position, target, speed);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            animator.SetBool("IsDead", true);
            rb2d.bodyType = RigidbodyType2D.Static;
            attackSpeed = 0;
            Destroy(gameObject, 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.transform;
            animator.SetTrigger("Attack");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().health -= attackDamage;
            isOnCooldown = true;
            animator.SetBool("isFlying", false);
            StartCoroutine(CooldownTimer());
        }
    }

    IEnumerator CooldownTimer()
    {
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}
