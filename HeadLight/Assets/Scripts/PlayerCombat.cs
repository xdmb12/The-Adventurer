using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Animator")]
    private Animator animator;
    
    [Header("Attack")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform bowAttackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private GameObject arrow;

    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private int damage;
    
    [SerializeField] private float attackRate = 2f;
    private float nextAttackTime = 0f;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!PauseMenu.isPaused)
        {
            if (Time.time >= nextAttackTime)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
                
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    animator.SetTrigger("BowAttack");
                    nextAttackTime = Time.time + 5f / attackRate;
                }
            }
        }
    }

    void Attack()
    {
        animator.SetTrigger("SwordAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (var enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out FlyingEnemy script))
            {
                script.TakeDamage(damage);
            }
            else
            {
                enemy.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    public void BowAttack()
    {
        Instantiate(arrow, bowAttackPoint.position, Quaternion.identity, null);
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
