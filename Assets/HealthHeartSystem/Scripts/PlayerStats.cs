/*
 *  Author: ariel oliveira [o.arielg@gmail.com]
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;
    public PlayerController controller;
    public WinOrDie WinOrDie;
    public DeathMageAI DeathMageAI;

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
    private void Start()
    {
        health= controller.health;
        maxHealth = health;
        maxTotalHealth = health;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) controller.health = maxHealth;
        if(health!= controller.health) health = controller.health; ClampHealth();
        if (DeathMageAI.isDead)
        {
            WinOrDie.Win();
        }
        else if (controller.isDead)
        {
            WinOrDie.Die();
        }
    }

    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxTotalHealth;

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }

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
        controller.health = health;
        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }
}
