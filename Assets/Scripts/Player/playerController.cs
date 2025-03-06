using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    private Rigidbody playerBody; // 角色的剛體 (物理引擎控制)
    public Animator animator; // 控制角色動畫的 Animator

    public FloatValue maxHealth;  // 這是 Scriptable Object，儲存最大血量
    private float currentHealth;  // **本地變數，實際運行時的血量**

    public float moveSpeed; // 角色移動速度
    public float jumpForce; // 跳躍力道
    

    private Vector3 moveDirection; // 角色的移動方向

    void Start()
    {
        playerBody = GetComponent<Rigidbody>(); // 獲取 Rigidbody 組件
        animator = GetComponent<Animator>(); // 獲取角色的 Animator 組件
        currentHealth = maxHealth.initialValue; // 設定當前血量為最大血量      
    }

    void Update()
    {
        playerMove(); // 每幀執行移動方法
    }

    private void playerMove()
    {
        float x = Input.GetAxis("Horizontal"); // 取得水平方向輸入 
        float z = Input.GetAxis("Vertical");   // 取得垂直方向輸入 

        moveDirection = new Vector3(x, 0, z); // 計算移動方向，並正規化 (防止對角線移動過快)

        // **處理移動**
        if (moveDirection != Vector3.zero) // 如果有移動輸入
        {
            transform.rotation = Quaternion.LookRotation(moveDirection); // 角色面向移動方向
            playerBody.velocity = new Vector3(moveDirection.x * moveSpeed, playerBody.velocity.y, moveDirection.z * moveSpeed); // 設定水平速度
            animator.SetFloat("MoveSpeed", moveSpeed); // 設定動畫參數，播放移動動畫
        }
        else
        {
            playerBody.velocity = new Vector3(0, playerBody.velocity.y, 0); // 停止移動時保持 Y 軸速度 (避免掉落)
            animator.SetFloat("MoveSpeed", 0); // 停止播放移動動畫
        }

        // **攻擊動作 (左鍵)**
        if (Input.GetKeyDown(KeyCode.Mouse0)) // 如果按下滑鼠左鍵
        {
            animator.SetTrigger("attack"); // 觸發攻擊動畫
        }

        // **防禦動作 (右鍵)**
        if (Input.GetKeyDown(KeyCode.Mouse1)) // 按住滑鼠右鍵時進入防禦狀態
        {
            animator.SetBool("defend", true);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1)) // 放開滑鼠右鍵時取消防禦
        {
            animator.SetBool("defend", false);
        }

        // **跳躍動作 (空白鍵)**
        if (Input.GetKeyDown(KeyCode.Space)) // 按下空白鍵跳躍
        {
            
             playerBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // 給予向上的力道
                animator.SetBool("IsJump", true); // 設定動畫參數，進入跳躍動畫
            
        }
        else
        {
            animator.SetBool("IsJump", false); // 當未按跳躍鍵時，關閉跳躍動畫
        }
    }
}