using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody playerBody; // ���⪺���� (���z��������)
    public Animator animator; // �����ʵe�� Animator

    public FloatValue maxHealth;  // �o�O Scriptable Object�A�x�s�̤j��q
    private float currentHealth;  // **���a�ܼơA��ڹB��ɪ���q**

    public float moveSpeed; // ���Ⲿ�ʳt��
    public float jumpForce; // ���D�O�D
    

    private Vector3 moveDirection; // ���⪺���ʤ�V

    void Start()
    {
        playerBody = GetComponent<Rigidbody>(); // ��� Rigidbody �ե�
        animator = GetComponent<Animator>(); // ������⪺ Animator �ե�
        currentHealth = maxHealth.initialValue; // �]�w��e��q���̤j��q      
    }

    void Update()
    {
        playerMove(); // �C�V���沾�ʤ�k
    }

    private void playerMove()
    {
        float x = Input.GetAxis("Horizontal"); // ���o������V��J 
        float z = Input.GetAxis("Vertical");   // ���o������V��J 

        moveDirection = new Vector3(x, 0, z); // �p�Ⲿ�ʤ�V�A�å��W�� (����﨤�u���ʹL��)

        // **�B�z����**
        if (moveDirection != Vector3.zero) // �p�G�����ʿ�J
        {
            transform.rotation = Quaternion.LookRotation(moveDirection); // ���⭱�V���ʤ�V
            playerBody.velocity = new Vector3(moveDirection.x * moveSpeed, playerBody.velocity.y, moveDirection.z * moveSpeed); // �]�w�����t��
            animator.SetFloat("MoveSpeed", moveSpeed); // �]�w�ʵe�ѼơA���񲾰ʰʵe
        }
        else
        {
            playerBody.velocity = new Vector3(0, playerBody.velocity.y, 0); // ����ʮɫO�� Y �b�t�� (�קK����)
            animator.SetFloat("MoveSpeed", 0); // ����񲾰ʰʵe
        }

        // **�����ʧ@ (����)**
        if (Input.GetKeyDown(KeyCode.Mouse0)) // �p�G���U�ƹ�����
        {
            animator.SetTrigger("attack"); // Ĳ�o�����ʵe
        }

        // **���m�ʧ@ (�k��)**
        if (Input.GetKeyDown(KeyCode.Mouse1)) // ����ƹ��k��ɶi�J���m���A
        {
            animator.SetBool("defend", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1)) // ��}�ƹ��k��ɨ������m
        {
            animator.SetBool("defend", false);
        }

        // **���D�ʧ@ (�ť���)**
        if (Input.GetKeyDown(KeyCode.Space)) // ���U�ť�����D
        {
            
             playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // �����V�W���O�D
                animator.SetBool("IsJump", true); // �]�w�ʵe�ѼơA�i�J���D�ʵe
            
        }
        else
        {
            animator.SetBool("IsJump", false); // �������D��ɡA�������D�ʵe
        }
    }
}