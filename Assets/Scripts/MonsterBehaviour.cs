using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterBehaviour : MonoBehaviour
{

    float health;
    float attackDamage;
    bool hasDealtDamage=false;
    bool startChasing = false;
    float startChasingAtDistance;
    NavMeshAgent agent;
    GameObject player;
    Animator animator;
    ParticleSystem hitParticle;
    Rigidbody rb;
    AudioSource audioSource;
    public UImanager uiManager;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        health=100f;
        attackDamage=50f;
        startChasingAtDistance=20;
        rb = GetComponent<Rigidbody>();
        hitParticle = GetComponentInChildren<ParticleSystem>();
        uiManager=GameObject.FindGameObjectWithTag("GameManager").GetComponent<UImanager>();
        audioSource = GetComponent<AudioSource>();
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Sword" && PlayerAttributes.isPlayerAttacking)
        {
            RotateMonsterTowardsPlayer();
           
            hitParticle.Play();
            health-=PlayerAttributes.playerDamage;
        }
       
    }


    void Update()
    {
        if (GameManager.gameState==GameManager.GameState.running)
        {
            if (startChasingAtDistance>=Vector3.Distance(player.transform.position, transform.position))
                startChasing=true;
            if (health <= 0)
            {
                if (!animator.GetBool("isDead"))
                {
                    rb.isKinematic=true;    //da ne registruje dodatne kolizije kada je mrtav
                    animator.speed = 1;
                    animator.SetBool("isDead", true);
                    MonsterSpawner.totalMonsters--;
                    audioSource.Play();
                    uiManager.UpdateMonsterText();
                    if(MonsterSpawner.totalMonsters==0)
                        GameObject.FindGameObjectWithTag("Wall").SetActive(false);  //unistavam zid do zmaja
                    Destroy(this.gameObject, 2f);
                    return;
                }

            }
            else if (startChasing)
            {

                agent.destination=player.transform.position;
                if (HasMonsterArrived())//da li je monster stigao do igraca
                {
                    animator.speed = 3;
                    animator.SetBool("isChasing", false);
                    if (!hasDealtDamage)
                    {
                        RotateMonsterTowardsPlayer();
                        animator.SetBool("isAttacking", true);
                        StartCoroutine(DamagePlayer());
                        hasDealtDamage=true;
                    }

                }
                else
                {
                    //RotateMonsterTowardsPlayer();
                    animator.speed = 3;
                    animator.SetBool("isChasing", true);
                    animator.SetBool("isAttacking", false);

                }
            }
        }
        //else agent.destination=transform.position;


    }

    IEnumerator DamagePlayer()
    {
        yield return new WaitForSeconds(0.5f);
        if (HasMonsterArrived() && animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"))
        {
            RotateMonsterTowardsPlayer();
            PlayerAttributes.playerHealth-=attackDamage;
           
           
        }
        hasDealtDamage = false;
    }

    bool HasMonsterArrived()
    {
        return agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending;
    }

    void RotateMonsterTowardsPlayer()
    {
        Vector3 lookPos = new Vector3(agent.destination.x, transform.position.y, agent.destination.z);
        Quaternion targetRotation = Quaternion.LookRotation(lookPos - transform.position);
        transform.rotation = targetRotation;
    }
}
