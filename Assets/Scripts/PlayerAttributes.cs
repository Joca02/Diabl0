using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    public static bool isPlayerAttacking = false;
    public static float playerHealth;
    public static float MAX_PLAYER_HEALTH = 200f;
    public  Slider healthSlider;
    public static float playerDamage = 35f;
    Animator animator;
    public GameObject playerSpawner;

    void Start()
    {
        //healthSlider=GameObject.FindGameObjectWithTag("Healthbar").GetComponentInChildren<Slider>();
        healthSlider.maxValue=MAX_PLAYER_HEALTH;
        animator=GetComponent<Animator>();
    }

    
    public void SetSliderHealth()
    {
        healthSlider.value = playerHealth;
    }

    public void ResetPlayer() 
    {
        NavMeshAgent agent=GetComponent<NavMeshAgent>();    //moram da ga disablujem jer mi plejer izbaguje kad trebam da ga vratim na start
        agent.enabled=false;
        transform.position=playerSpawner.transform.position;
        if(animator!=null)
            animator.SetBool("isDead", false);
        //animator.SetBool("isDead", false); //pravi problem u .exe verziji pa moram ovu proveru
        playerHealth=MAX_PLAYER_HEALTH;
        agent.enabled = true;
    }


}
