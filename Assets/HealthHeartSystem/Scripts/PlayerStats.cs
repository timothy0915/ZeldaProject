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
        health = player.health;
        maxHealth = player.maxHealth;
        maxTotalHealth = player.maxTotalHealth;
    }
    private void Update()
    {
        if (health != player.health) health = player.health; ClampHealth();
        if (maxHealth != player.maxHealth) maxHealth = player.maxHealth; ClampHealth();
        if (maxTotalHealth != player.maxTotalHealth) maxTotalHealth = player.maxTotalHealth; ClampHealth();
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
    void DataApply()//把player的data複寫
    {
        player.health = health;
        player.maxHealth = maxHealth;
        player.maxTotalHealth =maxTotalHealth;
    }
}
