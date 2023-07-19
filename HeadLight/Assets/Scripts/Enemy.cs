using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Components")]
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    private Animator animator;
    private Rigidbody2D rb2d;
    private Transform currentPoint;

    [Header("Enemy Stats")]
    [SerializeField] private float speed;
    [SerializeField] private int waitingTime;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    [Header("Enemy Attack")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask playerLayers;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRate = 5f;
    private float nextAttackTime = 0f;
    
    [Header("Enemy State")]
    public bool isWaiting;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        
        currentPoint = pointB.transform;
        currentHealth = maxHealth;
        
        isWaiting = false;
        
        animator.SetBool("isRunning", true);
    }

    private void Update()
    {
        EnemyPatrol();
        NextPatrolPoint();
    }

    private void NextPatrolPoint()
    {
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            isWaiting = true;
            StartCoroutine(WaitForNewPositionCheck());
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            isWaiting = true;
            StartCoroutine(WaitForNewPositionCheck());
            currentPoint = pointB.transform;
        }
    }

    private void EnemyPatrol()
    {
        if (currentPoint == pointB.transform && isWaiting == false)
        {
            rb2d.velocity = new Vector2(speed, 0);
            animator.SetBool("isRunning", true);
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (currentPoint == pointA.transform && isWaiting == false)
        {
            rb2d.velocity = new Vector2(-speed, 0);
            animator.SetBool("isRunning", true);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            rb2d.velocity = new Vector2(0, 0);
            animator.SetBool("isRunning", false);
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            animator.SetBool("IsDead", true);
            rb2d.bodyType = RigidbodyType2D.Static;
            Destroy(gameObject, 2);
        }
    }

    public void Attack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);

        foreach (var player in hitPlayer)
        {
            if (Time.time >= nextAttackTime)
            {
                player.GetComponent<Player>().health -= damage;
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    IEnumerator WaitForNewPositionCheck()
    {
        yield return new WaitForSeconds(waitingTime);
        isWaiting = false;
        animator.SetBool("isRunning", true);
    }
}
