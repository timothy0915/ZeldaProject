using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilmageAI : MonoBehaviour, IDamageable   //IDamageable�ϦU��AI�Τ@��X

{
    [Header("���ʰѼ�")]
    public float speed = 0f;              // ���ʳt��
    public float detectionRange = 10f;      // �������a���Z��
    public float knockbackDuration = 0.5f;  // ���h���򪺮ɶ�
    public float stunDuration = 0.5f;       // �������򪺮ɶ�

    [Header("��q�]�w")]
    public float health = 3f;             // �ĤH��l��q
    public Transform player;              // ���a���� Transform
    public Animator animator;             // ����ʵe�� Animator

    // �p���ܼ�
    private CharacterController characterController; // �Ω󲾰ʨ��⪺ CharacterController ����
    private Vector3 knockbackDirection;  // �O�����h����V
    private float knockbackTimer = 0f;   // �O�����h�ĪG����ɶ����p�ɾ�
    private float stunTimer = 0f;        // �O�������ĪG����ɶ����p�ɾ�
    private bool isStunned = false;      // �O�_�B��������A
    public bool isDead = false;          // �O�_�w�g���`
    public Transform MyTransform => transform;

    // Start() �b�C���}�l�ɰ���@��
    private void Start()
    {
        // ���o���ⱱ��A�t�d���ʻP�I��
        characterController = GetComponent<CharacterController>();

        // �Y animator �|���Q���w�A���ձq��e������o Animator ����
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        // �Q�� Tag �M�䪱�a����A�è��o�� Transform
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    // Update() �C�@�V����@���A�t�d�B�z�ĤH���欰
    private void Update()
    {
        // �p�G�ĤH�w���`�A�h���B�z����欰
        if (isDead)
        {
            return;
        }

        // �ˬd�O�_�B�����h�λ������A
        if (knockbackTimer > 0)
        {
            // �b���h���A�U�A�Q�Ψ��ⱱ�����
            characterController.Move(knockbackDirection * Time.deltaTime);
            // ������h�p�ɾ�
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                // �����h�ĪG������A�i�J�������A
                isStunned = true;
                stunTimer = stunDuration;
            }
            // ���񯸥߰ʵe�A���i�沾�ʰʵe
            animator.SetBool("IsMoving", false);
        }
        else if (isStunned)
        {
            // �B��������A�ɡA�˼ƭp��
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                // �����ɶ������A�����������A
                isStunned = false;
            }
            // ���񯸥߰ʵe
            animator.SetBool("IsMoving", false);
        }
        else
        {
            // �Y���b���h�λ������A�A�h�i�楿�`���ʦV���a�a��
            MoveTowardsPlayer();
        }
    }

    // ��k�G���ĤH���ʦV���a�a��
    private void MoveTowardsPlayer()
    {
        // �p�G�䤣�쪱�a�h������^
        if (player == null) return;

        // �ˬd���a�O�_���`�]���]���a���W�� PlayerController�^
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null && playerController.isDead)
        {
            // �Y���a�w���`�A����ʻP����
            animator.SetBool("IsMoving", false);
            return;
        }

        // �p��ĤH�P���a�������Z��
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // �p�G���a�b�����d�򤺫h�l�����a
        if (distanceToPlayer <= detectionRange)
        {
            // �p��q�ĤH���V���a����V
            Vector3 direction = (player.position - transform.position);
            // �ȦҼ{������V
            direction.y = 0;
            if (direction.magnitude > 0)
            {
                // �ϼĤH���V���a
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                // ���񲾰ʰʵe
                animator.SetBool("IsMoving", true);
            }
            else
            {
                // �p�G��V�V�q���s�A�h����ʰʵe
                animator.SetBool("IsMoving", false);
            }
            // �ϥ� SimpleMove ��k���ʼĤH�ASimpleMove �۰ʦҼ{���O�ĪG
            characterController.SimpleMove(direction.normalized * speed);
        }
        else
        {
            // �p�G���a�W�X�����d��A����ʰʵe
            animator.SetBool("IsMoving", false);
        }
    }

    // ��k�G�B�z��������ɴ�֦�q���欰
    public void TakeDamage(float damage)
    {
        // �p�G�w�g���`�h�����ˮ`
        if (isDead)
        {
            return;
        }
        // ������q
        health -= damage;
        // Ĳ�o���˰ʵe
        animator.CrossFade("GetHit", 0f, 0);
        // �p�G��q�p�󵥩� 0�A�h���榺�`�{��
        if (health <= 0f)
        {
            Die();
        }

    }

    // ��k�G��ĤH��������ɬI�[���h�ĪG
    public void ApplyKnockback(Vector3 direction, float force)
    {
        // �p�G�w���`�h���B�z
        if (isDead) return;

        // �]�w���h��V�����W�ƫ᪺��V���W�O��
        knockbackDirection = direction.normalized * force;
        // �]�w���h�p�ɾ����w�]����ɶ�
        knockbackTimer = knockbackDuration;
        // �b���h���������������A
        isStunned = false;
    }

    // ��k�G�B�z�ĤH���`���޿�
    private void Die()
    {
        // ����ư��榺�`�޿�
        if (isDead) return;
        isDead = true;

        // Ĳ�o���`�ʵe�A�b Animator �����Ӧ� "Die" �� Trigger
        animator.CrossFade("Die", 0f, 0);

        // ���Ψ��ⱱ��A�קK���`�ᤴ�i��I���β���
        characterController.enabled = false;

        // �p�G�s�b�����}���A�h���Χ����欰
        EvilmageAI EvilmageAI = GetComponent<EvilmageAI>();
        if (EvilmageAI != null)
        {
            EvilmageAI.enabled = false;
        }

        // �Ұʨ�{�A���ݦ��`�ʵe���񧹲���P������
        StartCoroutine(DeathRoutine());
    }

    // ��{�G���ݤ@�q�ɶ���P���ĤH����
    private IEnumerator DeathRoutine()
    {
        // ���� 2 ��A�i�H�ھڦ��`�ʵe���׶i��վ�
        yield return new WaitForSeconds(2f);

        // ���ݵ�����P���Ӫ���
        Destroy(gameObject);
    }
}