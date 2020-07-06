using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;     //enables the project to use Unity AI library

/// <summary>
/// enum ...
/// </summary>
//FSM States for the enemy
public enum EnemyState { CHASE, MOVING, DEFAULT, ATTACK};
//Default - init state (when it is not moving)
//other states will be based on the player's rotation/movement

/// <summary>
/// EnemyAI ...
/// </summary>
/// 
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    GameObject player;  //the player
    NavMeshAgent agent; //used to move along the navmesh
    
    public float chaseDistance = 20.0f; //distance the enemy must be before it chases the player
    public float attackDistance = 15.0f; //distance the enemy must be before it attacks the player

    protected EnemyState state = EnemyState.DEFAULT;    //init state in FSM is Default
    protected Vector3 destination = new Vector3(0, 0, 0);   //initial destination is the zero vector


    public int enemyHealth = 3;

    //Particle Sys. Explosion
    ParticleSystem explosion;
    bool explosionStarted = false;     //says whether the explosion has started or not (prevents repeats)
    
    EnemyShoot firePoint;

    public float shootTime = 1.0f;
    public float reloadTime = 1.0f;

    public int killCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //assigns anything with the tag "Player" in the scene to the object "player"

        firePoint = gameObject.GetComponentInChildren<EnemyShoot>() ;
        
        agent = this.GetComponent<NavMeshAgent>();  //gets the NavMeshAgent component

        explosion = GetComponentInChildren<ParticleSystem>();   //gets particle sys. component attached to the enemy
    }

    //creates a random position for the enemy to be in or go to
    private Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-50.0f, 50.0f), 0, Random.Range(-50.0f, 50.0f));
        //y-coord will be 0 (not instantiating in the air)
    }

    void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // Update is called once per frame
    void Update()
    {
        //FSM control code below
        switch (state)
        {
            //starts in the default state
            case EnemyState.DEFAULT:
                destination = transform.position + RandomPosition();
                //add a random position to the enemy's position that the enemy will move to

                //if the player is less than the "chaseDistance" from the enemy, switch to the "CHASE" state
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = EnemyState.CHASE;
                }
                else    //move the enemy randomly if the enemy is too far from the enemy
                {
                    
                    state = EnemyState.MOVING;
                    agent.SetDestination(destination);  //destination will be a random location
                }

                if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                    state = EnemyState.ATTACK;
                }
                break;
            //Moving state controls random movement
            case EnemyState.MOVING:
                //Debug.Log("Dest = " + destination);
                //when enemy is < 5 from the random distance, change destination again (done in Default state)
                if (Vector3.Distance(transform.position, destination) < 5)
                {
                    state = EnemyState.DEFAULT;
                    //firePoint.shoot = false;
                }
                //if the enemy gets close enough to the player, switch to the chase state and chase the player
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = EnemyState.CHASE;
                    //firePoint.shoot = false;
                }
                
                if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                    state = EnemyState.ATTACK;
                    //firePoint.shoot = true;
                }
                break;
            //state that chases the player
            case EnemyState.CHASE:
                FindObjectOfType<AudioManager>().Play("chase");
                //if the distance b/w the enemy and player exceeds the chase distance, switch to Default state
                if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance)
                {
                    FindObjectOfType<AudioManager>().Play("lost");
                    state = EnemyState.DEFAULT;
                    //firePoint.shoot = false;
                }

                if(Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                    state = EnemyState.ATTACK;
                    //firePoint.shoot = true;
                }
                FaceTarget();
                agent.SetDestination(player.transform.position);    //moves the enemy to the player's position

                break;
            //state where the enemy attacks the player
            case EnemyState.ATTACK:
                FindObjectOfType<AudioManager>().Play("shoot");

                if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
                {
                    FindObjectOfType<AudioManager>().Play("get back");
                    state = EnemyState.CHASE;
                    //firePoint.shoot = false;
                }

                if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance)
                {
                    state = EnemyState.DEFAULT;
                    //firePoint.shoot = false;
                }

                if (Time.time > shootTime)
                {
                    FindObjectOfType<AudioManager>().Play("blaster SFX");
                    firePoint.FireBullet();
                    shootTime = Time.time + reloadTime;
                }
                FaceTarget();
                agent.SetDestination(player.transform.position);
                
                    break;
            default:
                break;
        }
    }


    /// <summary>
    /// this enables the player to take damage from bullets, 
    /// ...
    /// </summary>
    /// <param name="col"></param>
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("PlayerAtk"))
        {
            enemyHealth--;
            FindObjectOfType<AudioManager>().Play("SThit");
            if (enemyHealth == 2)
            {
                FindObjectOfType<AudioManager>().Play("ST2hitleft");
            }
            if(enemyHealth == 1)
            {
                FindObjectOfType<AudioManager>().Play("ST1hitleft");
            }
            

            if(enemyHealth <= 0)
            {
                killCount++;
                energy.addScore(10);

                EnemyShoot[] allShots = gameObject.GetComponentsInChildren<EnemyShoot>();
                foreach (EnemyShoot c in allShots) c.enabled = false;
                
                // Disable all Renderers and Colliders
                Renderer[] allRenderers = gameObject.GetComponentsInChildren<Renderer>();
                foreach (Renderer c in allRenderers) c.enabled = false;
            
                Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();
                foreach (Collider c in allColliders) c.enabled = false;

                //StartCoroutine(PlayAndDestroy(myaudio.clip.length));

                //gameObject.GetComponent<ParticleSystemRenderer>().enabled = true;   //needed or the particle sys. won't show up
                gameObject.GetComponentInChildren<ParticleSystemRenderer>().enabled = true;   //needed or the particle sys. won't show up
                StartExplosion();   //makes explosion occur when the enemy is hit
                StartCoroutine(PlayAndDestroy(reloadTime));
            }
        }
    }

    //this enables the audio to play even after the object gets destroyed
    private IEnumerator PlayAndDestroy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StopExplosion();    //stops the explosion
        Destroy(gameObject);
    }

    /// <summary>
    /// ...
    /// </summary>
    private void StartExplosion()
    {
        if (explosionStarted == false)
        {
            explosion.Play();
            explosionStarted = true;
        }
    }

    /// <summary>
    /// ...
    /// </summary>
    private void StopExplosion()
    {
        explosionStarted = false;
        explosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //explosion.Stop();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    private void OnDestroy()
    {
        //energy.addScore(10);
    }
}
