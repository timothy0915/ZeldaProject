using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���a����}���G�t�d�B�z���a�����ʡB�����B���D�B��������H�Φ��`�����A�C
/// �Q�� Unity �� CharacterController �ӹ�{�I���˴��P���z���ʡA�èϥ� Animator ����ʵe����C
/// </summary>
public class PlayerController : MonoBehaviour
{
    
    [Header("���ⱱ��")]
    public CharacterController controller;  // �Q�� Unity ���ت� CharacterController �B�z���⪺�I���M����
    public Animator animator;               // �Ω󱱨��ʵe�� Animator ����
    public MusicPlayer musicPlayer;


    [Header("���ʳ]�w")]
    public float speed = 3f;                // ���⪺�򥻲��ʳt��
    public float gravity = -9.81f;          // �������O���ƭȡ]�t�Ȫ�ܦV�U�^
    public float jumpHeight = 1f;           // ���D��F�쪺����
   

   
    [Header("�a���˴�")]
    public Transform ground_check;          // �a���˴��I�A�q�`��b����}������
    public float ground_distance = 0.5f;      // �H���b�|�i��a���I���ˬd
    public LayerMask ground_mask;           // ���w���� Layer �Q�{�w���a���]�Ҧp Terrain�BPlatform ���^
    

    
    [Header("�Y�׳B�z")]
    public float slopeSpeedFactor = 0.5f;   // ����b�Y�D�W�ɡA���ʳt�ת����v�]�V�p�N��Y�פj�ɳt�׭��C�o�V����^
    public float maxSlopeAngle = 45f;       // ���\���Ⲿ�ʪ��̤j�Y�ר��סA�W�L�����׫Ჾ�ʳt�׷|�Q�i�@�B�վ�
  

    
    [Header("���h�]�w")]
    public float knockbackDuration = 0.5f;  // ����������h�ɡA���򲾰ʪ��ɶ�
    public float stunDuration = 0.5f;       // ���h������A����B��������A������ɶ��]�L�k�ާ@�^
   

    
    [Header("�����]�w")]
    public float attackRange = 2f;          // �����ɨϥ� Raycast �˴����g�{�Z��
    public float attackDamage = 20f;        // �����ɳy���ĤH���ˮ`��
    public float attackKnockbackForce = 5f; // �����ɹ�ĤH�I�[�����h�O�q
    public float attackCooldown = 0.5f;     // ������ݭn���ݪ��N�o�ɶ�
    private float attackTimer = 0f;         // �Ψӭp�ɧ����N�o���p�ɾ�
   

   
    // �z�L�s���ܼƱ�������ۦ��s���J�����A
    private int attackCombo = 0;            // �ثe���s�����A�]�Ҧp 1 ��ܲĤ@�ۡA2 ��ܲĤG�ۡ^
    public float comboResetTime = 1.0f;       // �s����J���j�A�Y�W�L�o�Ӯɶ��h���m�s�����A
    private float comboTimer = 0f;            // �p�ɳs�����j���˭p�ɾ�
   

   
    [Header("��q�]�w")]
    public float health = 100f;             // ���a��l����q��
   
   
    private Vector3 velocity;             // �Ψӭp�⭫�O�B���D�P��L�~�O�v�T�U���t��
    private bool isGrounded;              // �O�_��Ĳ�a�����X��
    private Vector3 moveDirection;        // ���a���ʤ�V���V�q
    private Vector3 knockbackDirection;   // ���h�ɪ���V�V�q
    private float knockbackTimer = 0f;    // ���h���A������˭p��
    private float stunTimer = 0f;         // �������A���˭p��
    private bool isStunned = false;       // �O�_���B��������A�]�L�k�ާ@�^
    public bool isDead = false;           // ���a�O�_�w�g���`
   

    // Start() �b�C���}�l�ɰ���@���A�q�`�ΨӪ�l�ƥ��n����
    void Start()
    {
        // ���o�� GameObject �W�� Animator ����A�ΥH����ʵe
        animator = GetComponent<Animator>();
    }

    // Update() �C�@�V�I�s�@���A�Ω�B�z���a��J�Ϊ��A��s
    void Update()
    {
        // �Y���a�w���`�A���A�B�z�����J�Ϊ��A��s
        if (isDead)
        {
            return;
        }

        // �����N�o�G�Y�����p�ɾ��|���k�s�A�h�����֭˼�
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        // �a���˴��G�Q�� CharacterController �� isGrounded �� Raycast �ӽT�{����O�_�b�a���W
        // ���B�P���ˬd��ؤ覡�H����í�w��
        isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, ground_distance + 0.1f, ground_mask);
        if (isGrounded && velocity.y < 0)
        {
            // ���⸨�a�ɡA�N�����t�׳]�����L�t�ȥH�O�ҫ��򱵦a�A�קK�]�t�׹L�j�y����z
            velocity.y = -2f;
        }

        // �B�z���h�P�������A
        if (knockbackTimer > 0)
        {
            // ���b�������h�G�ھ� knockbackDirection ���ʨ���
            controller.Move(knockbackDirection * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                // ���h������A�i�J�u�Ȫ��������A
                isStunned = true;
                stunTimer = stunDuration;
            }
        }
        else if (isStunned)
        {
            // �B��������A�G�˼ƭp�ɪ��ܵ���
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                // �������A�����A��_���a����
                isStunned = false;
            }
        }
        else
        {
            // ���`�ާ@���A�U�A�B�z���a���ʩM������J
            MovePlayer();

            // ���a���U�ƹ�����B�����N�o�p�ɾ��k�s�ɶi�����
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackTimer <= 0)
            {
                // �P�_�O�_�b���ĳs����J�ɶ���
                if (comboTimer > 0)
                {
                    // �Y�s�������٥��F�W���A�h�W�[�s����
                    if (attackCombo < 2)
                        attackCombo++;
                    else
                        attackCombo = 1;  // �Y�w�F�ĤG�ۡA�h���m���Ĥ@�ۡ]�̻ݨD�i�վ�s���W���^
                }
                else
                {
                    // �Y�s�����j�W�ɡA�q�Ĥ@�۶}�l
                    attackCombo = 1;
                }

                // �ھڷ�e�s�����AĲ�o�����������ʵe
                if (attackCombo == 1)
                    animator.SetTrigger("attack1");
                else if (attackCombo == 2)
                    animator.SetTrigger("attack2");


                // ��������˴��]�Q�� Raycast �P�w�e��O�_���ĤH�R���^
                AttackRaycast();
                // ���m�����N�o�p�ɾ�
                attackTimer = attackCooldown;
                // ���m�s���p�ɾ��A���ݤU�@���s����J
                comboTimer = comboResetTime;
            }

            // ��s�s���˭p�ɾ��A�Y�˭p�ɵ����h���m�s�����A
            if (comboTimer > 0)
            {
                comboTimer -= Time.deltaTime;
            }
            else
            {
                attackCombo = 0;
            }
        }

        // �B�z���D�G�ˬd�O�_���U���D���s�B������a���W
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // �ھڸ��D���פέ��O�p��ݭn����l�����t��
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // ��s�ʵe���A�G���⤣�b�a���W�ɡA�N "IsJump" �ѼƳ]�� true
        animator.SetBool("IsJump", !isGrounded);
        // �̷ӭ��O����v�T�����t��
        velocity.y += gravity * Time.deltaTime;
        // �ϥ� CharacterController ���ʨ���A�P�ɦҼ{���O�ĪG
        controller.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// MovePlayer() �B�z���a���򥻲��ʾާ@�A�]�A���o��J�B�p���V�B�Ҽ{�Y�׼v�T�B�H�Χ�s�ʵe�C
    /// </summary>
    private void MovePlayer()
    {
        // �q���a��J���o�����]x �b�^�P�����]z �b�^���ƭ�
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // �զX�����ʤ�V�V�q�A���� y �b�]������V�^
        moveDirection = new Vector3(x, 0, z);
        // �p�G�V�q���פj�� 1�]�Ҧp�P�ɫ��U��Ӥ�V��^�A�h�i�楿�W�ƥH�O���@�P�t��
        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        // �ھڥثe�a�����Y�׽վ㲾�ʳt�׭��v
        float slopeMultiplier = GetSlopeSpeedMultiplier();
        // �p��̲ײ��ʦV�q�G��󲾰ʤ�V�B�]�w�t�׻P�Y�׭��v
        Vector3 finalMove = moveDirection * speed * slopeMultiplier;

        if (moveDirection != Vector3.zero)
        {
            // �����ʿ�J�ɡA���ਤ�⭱�V���ʤ�V
            transform.rotation = Quaternion.LookRotation(moveDirection);
            // �]�w�ʵe�ѼơA�ھڳt�׼�����������ʰʵe
            animator.SetFloat("MoveSpeed", speed);
            // ���沾�ʾާ@
            controller.Move(finalMove * Time.deltaTime);
        }
        else
        {
            // �L��J�ɡA�]�w�ʵe�ѼƬ� 0�A����ݾ����A�ʵe
            animator.SetFloat("MoveSpeed", 0);
        }
    }

    /// <summary>
    /// AttackRaycast() �Q�ήg�u�˴��P�_�����O�_�R���ĤH�A
    /// �Y�R���h��ĤH�y���ˮ`�ìI�[���h�ĪG�C
    /// </summary>
    private void AttackRaycast()
    {
        // �g�u�q�����m�V�W������o�X�A��V�P���⥿�e��@�P
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        RaycastHit hit;
        // �b attackRange �Z�����˴��g�u�I��
        if (Physics.Raycast(ray, out hit, attackRange))
        {
            // �p�G�I�������Ҭ� "Enemy"�A�i������B�z
            if (hit.collider.CompareTag("Enemy"))
            {
                // ��������ĤH������}��
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    // �ϼĤH��������ˮ`
                    enemy.TakeDamage(attackDamage);
                    // �p��q���a��ĤH����V�]���W�ƫ�@�����h��V�^
                    Vector3 knockbackDir = (enemy.transform.position - transform.position).normalized;
                    // ��ĤH�I�[���h�ĪG�A�������a
                    enemy.ApplyKnockback(knockbackDir, attackKnockbackForce);
                    musicPlayer.s_hit();
                    Debug.Log("��");
                }
            }
            else
            {
                musicPlayer.s_swing();
                Debug.Log("��");
            }
        }
        else
        {
            musicPlayer.s_swing();
            Debug.Log("��");
        }
    }

    /// <summary>
    /// ApplyKnockback() ���a����~�����h�]�Ҧp�Q�ĤH�����θI�쳴���^�ɩI�s�A
    /// �ھګ��w��V�P�O�װ������h�ĪG�A�í��m�������A�C
    /// </summary>
    /// <param name="direction">���h����V�V�q</param>
    /// <param name="force">���h�O��</param>
    public void ApplyKnockback(Vector3 direction, float force)
    {
        // �]�w���h��V�A�í��W�O�ץH��o�̲ײ��ʳt��
        knockbackDirection = direction.normalized * force;
        // �Ұ����h�p�ɾ�
        knockbackTimer = knockbackDuration;
        // �M���������A�A�T�O�����~�O�ĪG�ߧY�o��
        isStunned = false;
    }

    /// <summary>
    /// TakeDamage() ���a��������ɩI�s�A
    /// ��������ʵe�æ���������q�A�Y��q�C��s�h�I�s���`�޿�C
    /// </summary>
    /// <param name="damage">�����y�����ˮ`�ƭ�</param>
    public void TakeDamage(float damage)
    {
        // ��������ʵe�A��ܪ��a�Q�������ĪG
        animator.SetTrigger("GetHit");
        // �������a��q
        health -= damage;
        // ���q�C�󵥩� 0 �ɡAĲ�o���`�{��
        if (health <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// Die() �B�z���a���`�ɪ��欰�A�]�A���񦺤`�ʵe�B�T�Ψ��ⱱ��A
    /// �ñN GameObject ���Ҳ����H�קK���򤣥��n���椬�C
    /// </summary>
    private void Die()
    {
        Debug.Log("Player died.");
        // ���񦺤`�ʵe
        animator.SetTrigger("Die");
        // �аO���a�����`���A�A�������ާ@
        isDead = true;
        // �T�� CharacterController �H����Ⲿ�ʻP�I���˴�
        controller.enabled = false;
        // �������a���ҡA�קK��L�����~���ѧO�����a
        gameObject.tag = "Untagged";
    }

    /// <summary>
    /// GetSlopeSpeedMultiplier() �ھڨ���U��a�����Y�׭p��@�ӳt�׭��v�A
    /// �Y�׶V�~�A���v�V�C�A�Ϩ���b�W�Y�ΤU�Y�ɲ��ʳt�צ��ҽվ�C
    /// </summary>
    /// <returns>���ʳt�ת����v</returns>
    float GetSlopeSpeedMultiplier()
    {
        RaycastHit hit;
        // �q�����m�V�U�o�g�g�u�A�˴��a�����A
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            // �p��I�����k�u�P������V�������A�Y�Y�ר�
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            // �Y�Y�ר��W�L�̤j���\���סA��^���C���t�׭��v
            if (slopeAngle > maxSlopeAngle)
            {
                return slopeSpeedFactor;
            }
            // �ھکY�ר��P�̤j���ת���ҡA�u�ʤ�����o�@�ӥ��ƪ��t�׭��v
            return Mathf.Lerp(1f, slopeSpeedFactor, slopeAngle / maxSlopeAngle);
        }
        // �Y�L�k�˴���a���A�h��^���`���ʳt�׭��v
        return 1f;
    }
}