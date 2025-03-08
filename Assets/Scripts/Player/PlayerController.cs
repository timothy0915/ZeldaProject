using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // **���ⱱ���**
    public CharacterController controller; // Unity ���ت� CharacterController �Ω�B�z�I��
    public Animator animator; // ����ʵe���

    [Header("Movement")] // **���ʰѼ�**
    public float speed = 3f; // ���ʳt��
    public float gravity = -9.81f; // ���O�v�T
    public float jumpHeight = 1f; // ���D����

    [Header("Ground Check")] // **�a���˴�**
    public Transform ground_check; // �Ω��˴��a���O�_�s�b
    public float ground_distance = 0.5f; // �a���˴��d��
    public LayerMask ground_mask; // �]�m�a���h�A�Ω��˴��a��

    [Header("Slope Handling")] // **�Y�׳B�z**
    public float slopeSpeedFactor = 0.5f; // �b�Y���W�����ʳt���Y��Y��
    public float maxSlopeAngle = 45f; // �̤j�Y�ר���

    [Header("Knockback Settings")] // **���h�]�w**
    public float knockbackDuration = 0.5f; // ���h����ɶ�
    public float stunDuration = 0.5f; // �Q���h�᪺�����ɶ�

    // **�p���ܼ�**
    private Vector3 velocity; // �����e�t�ס]�]�A���O�v�T�^
    private bool isGrounded; // �O�_��Ĳ�a��
    private Vector3 moveDirection; // ���a���ʤ�V
    private Vector3 knockbackDirection; // ���h��V
    private float knockbackTimer = 0; // ���h�p�ɾ�
    private float stunTimer = 0; // �����p�ɾ�
    private bool isStunned = false; // �O�_�B��������A

    void Start()
    {
        animator = GetComponent<Animator>(); // �������ʵe���
    }

    void Update()
    {
        // **�˴�����O�_�b�a���W**
        isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, out _, ground_distance + 0.1f, ground_mask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f; // �קK����b�a���W�ɦ��L�p���U���t��
        }

        // **�B�z���h�ĪG**
        if (knockbackTimer > 0)
        {
            controller.Move(knockbackDirection * Time.deltaTime); // �ھ����h��V����
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                isStunned = true;
                stunTimer = stunDuration; // �������h�����᪺�����ĪG
            }
        }
        else if (isStunned)
        {
            // **�B�z�����ĪG**
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                isStunned = false; // ��������
            }
        }
        else
        {
            MovePlayer(); // ���`����
        }
    }

    private void MovePlayer()
    {
        // **������a��J**
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDirection = new Vector3(x, 0, z);

        // **����ʤ�V�j��1�]�קK�W�L�̤j�t�ס^**
        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        // **�B�z�Y�׼v�T�t��**
        float slopeMultiplier = GetSlopeSpeedMultiplier();
        Vector3 finalMove = moveDirection * speed * slopeMultiplier;

        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection); // ���⭱�V���ʤ�V
            animator.SetFloat("MoveSpeed", speed); // ���񲾰ʰʵe
            controller.Move(finalMove * Time.deltaTime); // ���β���
        }
        else
        {
            animator.SetFloat("MoveSpeed", 0); // ����ʰʵe
        }

        // **���� & ���m**
        if (Input.GetKeyDown(KeyCode.Mouse0)) // �ƹ��������
        {
            animator.SetTrigger("attack");
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) // �ƹ��k�䨾�m
        {
            animator.SetBool("defend", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetBool("defend", false);
        }

        // **���D�޿�**
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // �p����D�t��
        }

        animator.SetBool("IsJump", !isGrounded); // �]�w���D�ʵe���A

        // **���έ��O**
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // **�������h�ĪG**
    public void ApplyKnockback(Vector3 direction, float force)
    {
        knockbackDirection = direction.normalized * force; // �]�w���h��V�P�O�D
        knockbackTimer = knockbackDuration; // �}�l���h
        isStunned = false; // �����������A
    }

    // **�p��Y�׼v�T�����ʳt��**
    float GetSlopeSpeedMultiplier()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up); // �p��Y�ר���
            if (slopeAngle > maxSlopeAngle)
            {
                return slopeSpeedFactor; // �p�G�W�L�̤j�Y�ר��סA�ϥΩY�״�t�Y��
            }
            return Mathf.Lerp(1f, slopeSpeedFactor, slopeAngle / maxSlopeAngle); // �w�C���C�Y�פW���t��
        }
        return 1f; // �q�{��^���`�t��
    }
}
