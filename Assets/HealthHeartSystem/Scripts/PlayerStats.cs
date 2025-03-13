/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public PlayerController player;
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region Sigleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerStats>();
            return instance;
        }
    }
    #endregion

    private float health;
    private float maxHealth;
    private float maxTotalHealth;

    private void Start()
    {
        //以player為主初始化
        health = player.health;
        maxHealth = 100;
        maxTotalHealth = 100;
    }
    private void Update()
    {
        //檢查player的血量有沒有因為其他程式降低並更新
        if (health != player.health) health = player.health; ClampHealth();
    }
    public float Health { 
        get {
            return health; }
        set { if (value <= maxHealth) health = value;
            DataApply();
        }
    }
    public float MaxHealth { 
        get {
            return maxHealth; }
        set { if (value <= health) health = value;
            if (value <= maxTotalHealth) maxHealth = value;
            DataApply();
        }
    }
    public float MaxTotalHealth { 
        get {
            return maxTotalHealth;}
        set { if(value<= maxHealth) maxHealth = value;
            maxTotalHealth = value;
            DataApply();
        }
    }

    public void Heal(float health)
    {
        this.health += health;
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        ClampHealth();
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }   
    }

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }
    void DataApply()//在使用這裡的函式改變數值之後用這條複寫player的資料
    {
        player.health = health;
    }
}
