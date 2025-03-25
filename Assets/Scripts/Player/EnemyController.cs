using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("移動參數")]
    public float speed = 3f;
    public float detectionRange = 10f;
    public float knockbackDuration = 0.5f;
    public float stunDuration = 0.5f;

    [Header("血量設定")]
    public float health = 100f;
    public Transform player;
    public Animator animator;

    [Header("死亡特效")]
    // 將你的粒子特效Prefab拖入此欄位
    public GameObject deathEffect;

    private CharacterController characterController;
    private Vector3 knockbackDirection;
    private float knockbackTimer = 0f;
    private float stunTimer = 0f;
    private bool isStunned = false;
    public bool isDead = false;

    public Transform MyTransform => transform;

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        animator.SetTrigger("GetHit");

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

    private void Start()
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

    private void Update()
    {
        if (isDead) return;

        if (knockbackTimer > 0)
        {
            characterController.Move(knockbackDirection * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                isStunned = true;
                stunTimer = stunDuration;
            }

            animator.SetBool("IsMoving", false);
        }
        else if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false;
            }

            animator.SetBool("IsMoving", false);
        }
        else
        {
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

            if (direction.magnitude > 0)
            {
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

            characterController.SimpleMove(direction.normalized * speed);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Die");
        characterController.enabled = false;

        EnemyAttack enemyAttack = GetComponent<EnemyAttack>();
        if (enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }

        StartCoroutine(DeathRoutine());
    }

    // 修改後的死亡流程：等待後產生死亡粒子特效再銷毀物件
    private IEnumerator DeathRoutine()
    {
        // 立即在敵人位置生成粒子特效（不設定父物件，以免敵人銷毀影響特效）
        GameObject particleInstance = null;
        if (deathEffect != null)
        {
            particleInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // 敵人死亡後先停留2秒，讓死亡動畫與粒子特效同步播放
        yield return new WaitForSeconds(2f);

        // 開始敵人掉落的動作
        float dropDuration = 1f;      // 掉落的持續時間
        float dropDistance = 5f;      // 掉落的距離（根據場景調整）
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos - new Vector3(0, dropDistance, 0);

        while (elapsed < dropDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / dropDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;

        // 掉落完成後等待粒子特效完整播放
        if (particleInstance != null)
        {
            ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                float totalDuration = ps.main.duration + ps.main.startLifetime.constantMax;
                yield return new WaitForSeconds(totalDuration);
            }
            else
            {
                yield return new WaitForSeconds(2f);
            }
            Destroy(particleInstance);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        // 最後銷毀敵人物件
        Destroy(gameObject);
    }
    }
