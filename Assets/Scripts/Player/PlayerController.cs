using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���a����}���A�t�d�B�z���a�����ʡB�����B���D���ާ@
public class PlayerController : MonoBehaviour
{
    [Header("���ⱱ��")]
    public CharacterController controller;  // �ϥ� Unity �� CharacterController �B�z�I���M����
    public Animator animator;               // �Ω󱱨��ʵe

    [Header("���ʳ]�w")]
    public float speed = 3f;                // ���ʳt��
    public float gravity = -9.81f;          // ���O��
    public float jumpHeight = 1f;           // ���D����

    [Header("�a���˴�")]
    public Transform ground_check;          // �a���˴��I�A�Ω�P�_����O�_�b�a���W
    public float ground_distance = 0.5f;    // �˴��d�򪺥b�|
    public LayerMask ground_mask;           // �Ω�P�_���Ǫ���Q�{�w���a��

    [Header("�Y�׳B�z")]
    public float slopeSpeedFactor = 0.5f;   // ����b�Y�D�W���ʮɡA�t�ת��վ�]�l
    public float maxSlopeAngle = 45f;       // �̤j�Y�ר��סA�W�L�����׮ɳt�׷|���C

    [Header("���h�]�w")]
    public float knockbackDuration = 0.5f;  // ���h���򪺮ɶ�
    public float stunDuration = 0.5f;       // �������򪺮ɶ�

    [Header("�����]�w")]
    public float attackRange = 2f;          // �����˴����g�{�]�Q�� Raycast�^
    public float attackDamage = 20f;        // �����y�����ˮ`
    public float attackKnockbackForce = 5f; // �����ɹ�ĤH�I�[�����h�O��
    public float attackCooldown = 0.5f;     // �������j�ɶ�
    private float attackTimer = 0f;         // �Ω�p������N�o�ɶ����p�ɾ�

    [Header("��q�]�w")]
    public float health = 100f;             // ���a��q

    // �p���ܼ�
    private Vector3 velocity;             // �Ω�p�⨤������O�v�T�����ʳt��
    private bool isGrounded;              // �P�_����O�_�b�a���W
    private Vector3 moveDirection;        // ���a���ʤ�V
    private Vector3 knockbackDirection;   // ���h��V
    private float knockbackTimer = 0f;    // ���h���򪺭p�ɾ�
    private float stunTimer = 0f;         // �������򪺭p�ɾ�
    private bool isStunned = false;       // �O�_�B��������A
    public bool isDead = false;           // ���a�O�_���`

    // Start() �b�C���}�l�ɰ���@��
    void Start()
    {
        // ���o Animator ����
        animator = GetComponent<Animator>();
    }

    // Update() �C�@�V���|����A�B�z���a��J�P�ʧ@
    void Update()
    {
        // �p�G���a�w���`�A�h���B�z�����J�P�欰
        if (isDead)
        {
            return; 
        }

        // �����N�o�p�ɡA�C�@�V��֧N�o�p�ɾ�
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        // �a���˴��A�Q�� CharacterController �� isGrounded �Ψϥ� Raycast �˴��a��
        isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, ground_distance + 0.1f, ground_mask);
        if (isGrounded && velocity.y < 0)
        {
            // �p�G�b�a���W�åB�U���t�׬��t�A�h�N�U���t�׳]�w�����L���t�ȥH�O�����a���A
            velocity.y = -2f;
        }

        // �B�z���h�M�������A
        if (knockbackTimer > 0)
        {
            // �b���h���A���A�Q�� CharacterController ����
            controller.Move(knockbackDirection * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                // ���h������i�J�������A
                isStunned = true;
                stunTimer = stunDuration;
            }
        }
        else if (isStunned)
        {
            // �B��������A�A�˼ƭp��
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                // ��������
                isStunned = false;
            }
        }
        else
        {
            // ���a���`�����
            MovePlayer();
            // �����P�w�G���a���U�ƹ�����åB�����N�o�ɶ�������
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackTimer <= 0)
            {
                // Ĳ�o�����ʵe
                animator.SetTrigger("attack");
                // ����������g�u�˴�
                AttackRaycast();
                // ���m�����N�o�p�ɾ�
                attackTimer = attackCooldown;
            }
        }
    }

    // �B�z���a���ʪ��޿�
    private void MovePlayer()
    {
        // ���o�����]x �b�^�M�����]z �b�^����J��
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // �إߤ@�ӦV�q��ܲ��ʤ�V�]�ȭ� x �P z �b�^
        moveDirection = new Vector3(x, 0, z);
        // �p�G��J�V�q�j�� 1�A�h�i�楿�W�ơA�H�O�����ʳt�פ@�P
        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        // �p��b�Y�D�W���ʮɪ��t�׽վ�Y��
        float slopeMultiplier = GetSlopeSpeedMultiplier();
        // �p��̲ײ��ʦV�q�G����J��V�B�t�שM�Y�D�վ�]�l
        Vector3 finalMove = moveDirection * speed * slopeMultiplier;

        if (moveDirection != Vector3.zero)
        {
            // �p�G�����ʿ�J�A�h�����⭱�V���ʤ�V
            transform.rotation = Quaternion.LookRotation(moveDirection);
            // �]�w���ʰʵe�ѼơA�o�̳]���t�׭�
            animator.SetFloat("MoveSpeed", speed);
            // �Q�� CharacterController ���ʨ���
            controller.Move(finalMove * Time.deltaTime);
        }
        else
        {
            // �S�����ʿ�J�ɡA�N���ʰʵe�ѼƳ]��0�A����R��
            animator.SetFloat("MoveSpeed", 0);
        }

        // �B�z���D�޿�G����U���D����B����b�a���W��
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // �p����D����l�t�סA�Q�θ��D���׻P���O����
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // �]�w�ʵe���O�_�b�Ť��]���D���A�^�A�ھڬO�_�b�a���W
        animator.SetBool("IsJump", !isGrounded);
        // �֥[���O�v�T�A�Ϩ���v���[�t�U�Y
        velocity.y += gravity * Time.deltaTime;
        // �Q�� CharacterController ���ʨ���A�]�A������V�����O�ĪG
        controller.Move(velocity * Time.deltaTime);
    }

    // �Q�ήg�u�˴��i������P�w
    private void AttackRaycast()
    {
        // �إߤ@���g�u�G�q����W��]�����ݳ����ס^�V�e�o�g
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        RaycastHit hit;
        // �p�G�g�u�b attackRange ���I���쪫��
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            // �ˬd�I�����O�_�аO�� "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                // ���o�Q�I�����W�� EnemyController �}��
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    // �ϼĤH����ˮ`
                    enemy.TakeDamage(attackDamage);
                    // �p��I�[�b�ĤH���W�����h��V�]�q���a���V�ĤH�^
                    Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
                    // �ϼĤH�������h�ĪG
                    enemy.ApplyKnockback(knockbackDir, attackKnockbackForce);
                }
            }
        }
    }

    // ���a����~�����h�ɩI�s����k
    public void ApplyKnockback(Vector3 direction, float force)
    {
        // �]�w���h��V�����W�ƫ᭼�W�O��
        knockbackDirection = direction.normalized * force;
        // �]�w���h����ɶ�
        knockbackTimer = knockbackDuration;
        // �b�������h�ɡA�����������A
        isStunned = false;
    }

    // ���a��������ɩI�s�A�B�z�ˮ`�M�ʵe
    public void TakeDamage(float damage)
    {
        // ����Q�����ʵe�A�нT�O Animator ���]�� "GetHit" �� Trigger
        animator.SetTrigger("GetHit");

        // ������q
        health -= damage;
        // �p�G��q�p�󵥩� 0�A�h���榺�`�{��
        if (health <= 0f)
        {
            Die();
        }
    }

    // �B�z���a���`�޿�
    private void Die()
    {
        // �b Console ��X���`�T��
        Debug.Log("Player died.");
        // Ĳ�o���`�ʵe�AAnimator �����]�� "Die" �� Trigger
        animator.SetTrigger("Die");

        // �]�w���`�X�СA�קK����ʧ@
        isDead = true;

        // ���Ψ��ⱱ��A�����b���`���~�򲾰�
        controller.enabled = false;

        // ���ܼ��ҡA���ĤH���A�N����@�ؼ�
        gameObject.tag = "Untagged";

        // ���B�i�H�K�[�C�������έ��ͪ���L�޿�
    }

    // �p��Y�D�ﲾ�ʳt�ת��v�T�A�^�ǳt�׭���
    float GetSlopeSpeedMultiplier()
    {
        RaycastHit hit;
        // �q�����m�V�U�o�g�g�u�˴��a��
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            // �p��a���k�u�P������V�����������A�o�N�O�Y�ר�
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > maxSlopeAngle)
            {
                // �p�G�Y�פj��̤j���\���סA��^���C���t�׭���
                return slopeSpeedFactor;
            }
            // �_�h�ھکY�ר��i��u�ʴ��ȡA��^���� 1 �M slopeSpeedFactor ����������
            return Mathf.Lerp(1f, slopeSpeedFactor, slopeAngle / maxSlopeAngle);
        }
        // �Y�S���˴���a���A��^�q�{�t�׭��� 1
        return 1f;
    }
}