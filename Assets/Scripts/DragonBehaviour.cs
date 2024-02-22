using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DragonBehaviour : MonoBehaviour
{
    public static float dragonHealth;
    float startChasingAtDistance;
    NavMeshAgent agent;
    Rigidbody rb;
    Animator animator;
    GameObject player;
    bool startChasing=false;
    UImanager uiManager;
    ParticleSystem fireParticle;
    GameManager gameManager;
    public static float dragonAttack = 10f;
    AudioSource audioSource;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        dragonHealth=1000f;
        startChasingAtDistance=15;
        uiManager=GameObject.FindGameObjectWithTag("GameManager").GetComponent<UImanager>();
        fireParticle = GetComponentInChildren<ParticleSystem>();
        gameManager=GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioSource=GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Sword" && PlayerAttributes.isPlayerAttacking)
        { 
            dragonHealth-=PlayerAttributes.playerDamage;
            uiManager.UpdateMonsterText();
        }

    }
    void Update()
    {
        if (GameManager.gameState==GameManager.GameState.running)
        {
            if (startChasingAtDistance>=Vector3.Distance(player.transform.position, transform.position))
                startChasing=true;
            if (dragonHealth <= 0)
            {
                if (!animator.GetBool("isDead"))
                {
                    rb.isKinematic=true;    //da ne registruje dodatne kolizije kada je mrtav
                    animator.SetBool("isDead", true);

                    audioSource.Play();
                    StartCoroutine(DelayDeath());
                    
                    return;
                }

            }
            else if (startChasing)
            {

                agent.destination=player.transform.position;
                if (HasMonsterArrived())//da li je zmaj stigao do igraca
                {
                    animator.SetBool("isChasing", false);
                    animator.SetBool("isAttacking", true);
                    StartCoroutine(StartFire()); //mali delay kod animacije pa da ne krene iste sekunde sa vatrom

                }
                else
                {
                    if(fireParticle.isPlaying && !animator.GetCurrentAnimatorStateInfo(0).IsName("Flame Attack"))
                        fireParticle.Stop();
                    animator.SetBool("isChasing", true);
                    animator.SetBool("isAttacking", false);

                }
            }
        }
    }

    IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
        gameManager.GameOver();
    }
    IEnumerator StartFire()
    {
        yield return new WaitForSeconds(1f);
        
        if (!fireParticle.isPlaying)
            fireParticle.Play();
    }
    bool HasMonsterArrived()
    {
        return agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending;
    }
  
}
