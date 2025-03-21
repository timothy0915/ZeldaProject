using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingAndFloat : MonoBehaviour
{
    public PlayerController controller;
    public Animator animator;
    public GameObject player;
    public GameObject spirit;
    public float deathTime;

    // Start is called before the first frame update
    void Start()
    {
        spirit.SetActive(false);
        player.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (controller != null)
        {
            if (controller.isDead )
            {
                if (!animator.GetCurrentAnimatorStateInfo(1).IsName("Die01_SwordAndShield"))
                {

                }
                Dying();
            }
        }
     }
    public void Dying()
    {
        deathTime += Time.deltaTime;
        if (deathTime >= 1)
        {
            player.SetActive(false);
            spirit.SetActive(true);
            spirit.transform.position += spirit.transform.up * Time.deltaTime;
        }
    }
}
