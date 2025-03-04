using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public CharacterController controller; // �t�d����Ⲿ��
    public Animator animator; // �����ʵe

    [Header("Movement")]
    public float speed = 3f; // ���⪺�򥻲��ʳt��
    public float gravity = -9.81f; // ���O�ȡA�t�ȥN��V�U
    public float jumpHeight = 1f; // ���D����

    [Header("Ground Check")]
    public Transform ground_check; // ����}�U���˴��I�A�ΨӧP�_�O�_�ۦa
    public float ground_distance = 0.4f; // �a���˴��d��
    public LayerMask ground_mask; // �]�w���ؼh�ŬO�a���]�Ψ��˴�����O�_�b�a���W�^

    [Header("Slope Handling")]
    public float slopeSpeedFactor = 0.5f; // ��Y�׳̤j�ɡA�t���Y����
    public float maxSlopeAngle = 45f; // �̤j�Y�ר��סA�W�L���ȫh���ʳt�פj�T�U��

    Vector3 velocity; // ���⪺�����t�ס]�Ω���D�M���O�^
    bool isGrounded; // �O������O�_�b�a���W
    private Vector3 moveDirection; // ���ʤ�V

    void Start()
    {
        animator = GetComponent<Animator>();// ���o���⪺ Animator �ե�
    }

    void Update()
    {
        // **�˴�����O�_�b�a���W**
        isGrounded = Physics.CheckSphere(ground_check.position, ground_distance, ground_mask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f; // �������K�a���A�קK�����n���B��
        }

        // **���o���a��J**
        float x = Input.GetAxis("Horizontal"); // ��������
        float z = Input.GetAxis("Vertical");   // ��������

        // �]�w���ʤ�V�]���]�t Y �b�A�קK�v�T���O�^
        moveDirection = new Vector3(x, 0, z);

        // �T�O���ʤ�V�����פ��W�L 1�A����﨤�u���ʮɳt�ץ[��
        if (moveDirection.magnitude > 1)
            moveDirection.Normalize();

        // **�Y�׳t�׽վ�**
        float slopeMultiplier = GetSlopeSpeedMultiplier(); // �ھکY�׭p��t���Y����
        Vector3 finalMove = moveDirection * speed * slopeMultiplier; // �p��̲ײ��ʳt��

        // **���ʨ���**
        if (moveDirection != Vector3.zero) // �p�G����J������
        {
            transform.rotation = Quaternion.LookRotation(moveDirection); // ����¦V���ʤ�V
            animator.SetBool("IsRun", true); // ����]�B�ʵe
            controller.Move(finalMove * Time.deltaTime); // ���Ⲿ��
        }
        else
        {
            animator.SetBool("IsRun", false); // ����]�B�ʵe
        }

        // **���D�޿�**
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // �p����D�t��
        }

        // **�ʵe����**
        if (isGrounded)
        {
            animator.SetBool("IsJump", false); // �b�a���ɡA�������D�ʵe
        }
        else
        {
            animator.SetBool("IsJump", true); // �b�Ť��ɡA�}�Ҹ��D�ʵe
        }

        // **���έ��O**
        velocity.y += gravity * Time.deltaTime; // �����⪺ Y �b�t�ר��쭫�O�v�T
        controller.Move(velocity * Time.deltaTime); // ��������쭫�O�v�T�ӤU��
    }

    // **�p��Y�׼v�T�t�ת��禡**
    float GetSlopeSpeedMultiplier()
    {
        RaycastHit hit;
        // �V�}�U�o�g�@���g�u�A�˴��Y��
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up); // �p��Y�ר���

            // �p�G�Y�׶W�L�̤j�]�w���סA�t�׭��̧ܳC
            if (slopeAngle > maxSlopeAngle)
            {
                return slopeSpeedFactor;
            }

            // �ھکY�פ���Y��t�ס]�Y�׶V�j�A�t�׶V�C�^
            return Mathf.Lerp(1f, slopeSpeedFactor, slopeAngle / maxSlopeAngle);
        }
        return 1f; // �w�]�^�� 1�]�N���v�T�t�ס^
    }
}
