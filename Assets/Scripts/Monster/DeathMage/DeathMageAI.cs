using System.Collections;
using UnityEngine;

public class DeathMageAI : MonoBehaviour, IDamageable
{
    [Header("移動參數")]
    public float speed = 3f;
    public float detectionRange = 10f;
    public float knockbackDuration = 0.5f;
    public float stunDuration = 0.5f;

    [Header("血量設定")]
    public float health = 8f;
    public Transform player;
    public Animator animator;

    private CharacterController characterController;
    private Vector3 knockbackDirection;
    private float knockbackTimer = 0f;
    private float stunTimer = 0f;

    private bool isStunned = false;
    public bool isDead = false;

    public Transform MyTransform => transform;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (isDead) return;

        //動畫狀態判斷，避免攻擊 / 死亡時被移動動畫蓋掉
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
        bool isInAction = currentState.IsTag("Attack") || currentState.IsTag("Spell") || currentState.IsTag("Death");

        if (knockbackTimer > 0)
        {
            characterController.Move(knockbackDirection * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                isStunned = true;
                stunTimer = stunDuration;
            }

            if (!isInAction) animator.SetBool("IsMoving", false);
        }
        else if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
            }

            if (!isInAction) animator.SetBool("IsMoving", false);
        }
        else
        {
            //唯一負責移動的區塊
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player == null) return;

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && playerController.isDead)
        {
            animator.SetBool("IsMoving", false);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector3 direction = (player.position - transform.position);
            direction.y = 0;

            if (direction.magnitude > 0.1f)
            {
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                animator.SetBool("IsMoving", true);
                characterController.SimpleMove(transform.forward * speed);

            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    

}


public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        animator.CrossFade("Take Damage", 0f ,0);

        if (health <= 0f)
        {
            Die();
        }
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        if (isDead) return;

        knockbackDirection = direction.normalized * force;
        knockbackTimer = knockbackDuration;
        isStunned = false;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.CrossFade("Die", 0f, 0);
        characterController.enabled = false;

        DeathMageAttack oldAI = GetComponent<DeathMageAttack>();
        if (oldAI != null)
        {
            oldAI.enabled = false;
        }
        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
