using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfFullHeart;
    public Sprite emptyHeart;
    public FloatValue heartContainers;
    public Animator animator;
    public Collider hitCollider;  // �n����collider
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("isDead", false);
        animator.SetBool("getHit", false);
    }
public void InitHearts() 
    {
        for (int i = 0; i < heartContainers.initialValue; i++) 
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite= fullHeart;
        }
    }
}
