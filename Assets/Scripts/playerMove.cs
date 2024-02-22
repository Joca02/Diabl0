using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class playerMove : MonoBehaviour
{
    private NavMeshAgent agent;
    float speed = 5f;
    Animator animator;
    public ParticleSystem healParticle;
    public ParticleSystem fastParticle;
    public Image[] abilityImage;
    public GameManager gameManager;

    bool[] isOnCooldown = { false, false, false };
    float[] abilityColdown = { 2f, 3f, 3f };
    private float[] abilityExecutionTime = { 2.85f, 3f, 3f };
    bool[] isExecuting = { false, false, false };
    bool isDead = false;
    PlayerAttributes playerAttributes;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        animator = GetComponent<Animator>();
        playerAttributes = GetComponent<PlayerAttributes>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Fire"))
        {
           PlayerAttributes.playerHealth-=DragonBehaviour.dragonAttack;
        }
    }

    void Update()
    {
        if (GameManager.gameState==GameManager.GameState.running)
        {

            playerAttributes.SetSliderHealth();
            if (PlayerAttributes.playerHealth>0)
            {
                isDead = false;

                PlayerAttributes.isPlayerAttacking=isExecuting[0];//sluzi mi za proveru u MonsterBehaviour ako dodje do kolizije sa macem, da li igrac napada

                // animator.ResetTrigger("tRoll");

                /////////////
                Ability1();
                Ability2(); //proveravam da li pozivam neku magiju
                Ability3();
                ////////////

                UpdateAbilityIcons();


                //kretanje playera na desni klik
                if (Input.GetMouseButtonDown(1))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Heal"))
                        {
                            if (!isExecuting[0])
                            {
                                Vector3 lookPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                                Quaternion targetRotation = Quaternion.LookRotation(lookPos - transform.position);
                                transform.rotation = targetRotation;
                            }

                            agent.SetDestination(hit.point);
                        }
                        //else Debug.Log("REGISTERED");

                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    animator.SetTrigger("tRoll");
                }

                //proveravam kojom brzinom se trenutno krece player
                float speed2 = agent.velocity.magnitude/agent.speed;
                animator.SetFloat("speed", speed2);
            }

            else if (PlayerAttributes.playerHealth<=0 && !isDead)
            {
                isDead = true;
                animator.SetTrigger("death");
                animator.SetBool("isDead", true);
                agent.destination=transform.position;
                StartCoroutine(DeathDelay());
            }

        }
        else agent.SetDestination(transform.position);


    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2);
        gameManager.GameOver();
    }

    private void UpdateAbilityIcons()
    {
        for (int i = 0; i<abilityColdown.Length; i++)   //za svaki coldown imam i 1 ability
        {
            if (isOnCooldown[i])
            {
                abilityImage[i].fillAmount+=1/abilityColdown[i]*Time.deltaTime;
                if (abilityImage[i].fillAmount>=1)
                    abilityImage[i].fillAmount=1;
            }
        }

    }

    public void ResetAbilityIcons()
    {
        for(int i = 0;i<abilityColdown.Length;i++)
        {
            abilityImage[i].fillAmount=1;
        }
    }

    void Ability1()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isOnCooldown[0] &&!isExecuting[0])
        {

            isExecuting[0]=true;
            animator.SetTrigger("ability1");
            animator.SetBool("isOverAbility1", false);
            abilityImage[0].fillAmount=0;
            StartCoroutine(AbilityExecution1());
        }
    }

    void Ability2()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isOnCooldown[1] &&!isExecuting[1])
        {
            //agent.speed=0;
            //agent.destination=transform.position;

            abilityImage[1].fillAmount=0;
            if (!isExecuting[0])
                animator.SetTrigger("ability2");

            healParticle.Play();
            StartCoroutine(AbilityExecution2());
            isExecuting[1]=true;


        }

    }

    void Ability3()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isOnCooldown[2] &&!isExecuting[2])
        {
            fastParticle.Play();
            abilityImage[2].fillAmount=0;

            if (!isExecuting[2])
            {
                StartCoroutine(AbilityExecution3());
                isExecuting[2]=true;
            }
        }
    }


    IEnumerator AbilityExecution1()
    {
        yield return new WaitForSeconds(abilityExecutionTime[0]);
        isExecuting[0] = false;
        isOnCooldown[0]=true;
        animator.ResetTrigger("ability"+1);

        animator.SetBool("isOverAbility"+1, true);
        StartCoroutine(ColdownAbility(0));
    }

    IEnumerator AbilityExecution2()
    {
        float totalHealingTime = abilityExecutionTime[1];
        float healingAmount = 50f;
        float healPerExecTime = healingAmount/totalHealingTime;

        for (int i = 0; i < totalHealingTime; i++)
        {
            if (PlayerAttributes.playerHealth + healPerExecTime<=PlayerAttributes.MAX_PLAYER_HEALTH)
                PlayerAttributes.playerHealth += healPerExecTime;
            else PlayerAttributes.playerHealth=PlayerAttributes.MAX_PLAYER_HEALTH;
            yield return new WaitForSeconds(1);
        }
        healParticle.Stop();
        isExecuting[1] = false;
        isOnCooldown[1]=true;
        StartCoroutine(ColdownAbility(1));

    }

    IEnumerator AbilityExecution3()
    {
        agent.speed*=1.5f;
        yield return new WaitForSeconds(abilityExecutionTime[2]);
        isExecuting[2]=false;
        isOnCooldown[2]=true;
        agent.speed=speed;
        fastParticle.Stop();
        StartCoroutine(ColdownAbility(2));
    }

    IEnumerator ColdownAbility(int abilityIndex)
    {
        isOnCooldown[abilityIndex] = true;
        yield return new WaitForSeconds(abilityColdown[abilityIndex]);
        isOnCooldown[abilityIndex]=false;
    }
}
