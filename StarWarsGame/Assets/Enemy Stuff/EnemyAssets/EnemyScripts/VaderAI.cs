using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;     //enables the project to use Unity AI library
using UnityEngine.SceneManagement;

/// <summary>
/// enum ...
/// </summary>
//FSM States for the enemy
public enum VaderState { CHASE, MOVING, DEFAULT, ATTACK};
//Default - init state (when it is not moving)
//other states will be based on the player's rotation/movement

/// <summary>
/// VaderAI ...
/// </summary>
/// 
[RequireComponent(typeof(NavMeshAgent))]
public class VaderAI : MonoBehaviour
{
    GameObject player;  //the player
    NavMeshAgent agent; //used to move along the navmesh
    GameObject vader; //vader
    Vector3 vLoc;
    
    public float chaseDistance = 20.0f; //distance the enemy must be before it chases the player
    public float attackDistance = 15.0f; //distance the enemy must be before it attacks the player

    protected VaderState state = VaderState.DEFAULT;    //init state in FSM is Default
    protected Vector3 destination = new Vector3(0, 0, 0);   //initial destination is the zero vector


    public int enemyHealth = 10;

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
        FindObjectOfType<AudioManager>().Play("vIntro");

        player = GameObject.FindWithTag("Player");
        //assigns anything with the tag "Player" in the scene to the object "player"

        vader = this.gameObject;

        vLoc = vader.transform.position;

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
            case VaderState.DEFAULT:
                destination = transform.position + RandomPosition();
                //add a random position to the enemy's position that the enemy will move to

                //if the player is less than the "chaseDistance" from the enemy, switch to the "CHASE" state
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = VaderState.CHASE;
                }
                else    //move the enemy randomly if the enemy is too far from the enemy
                {
                    
                    state = VaderState.MOVING;
                    agent.SetDestination(destination);  //destination will be a random location
                }

                if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                    state = VaderState.ATTACK;
                }
                break;
            //Moving state controls random movement
            case VaderState.MOVING:
                //Debug.Log("Dest = " + destination);
                //when enemy is < 5 from the random distance, change destination again (done in Default state)
                if (Vector3.Distance(transform.position, destination) < 5)
                {
                    state = VaderState.DEFAULT;
                    //firePoint.shoot = false;
                }
                //if the enemy gets close enough to the player, switch to the chase state and chase the player
                if (Vector3.Distance(transform.position, player.transform.position) < chaseDistance)
                {
                    state = VaderState.CHASE;
                    //firePoint.shoot = false;
                }
                
                if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                    state = VaderState.ATTACK;
                    //firePoint.shoot = true;
                }
                break;
            //state that chases the player
            case VaderState.CHASE:
                FindObjectOfType<AudioManager>().Play("V chase");
                //if the distance b/w the enemy and player exceeds the chase distance, switch to Default state
                if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance)
                {
                    FindObjectOfType<AudioManager>().Play("V lost");
                    state = VaderState.DEFAULT;
                    //firePoint.shoot = false;
                }

                if(Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
                {
                    state = VaderState.ATTACK;
                    //firePoint.shoot = true;
                }
                FaceTarget();
                agent.SetDestination(player.transform.position);    //moves the enemy to the player's position

                break;
            //state where the enemy attacks the player
            case VaderState.ATTACK:
                FindObjectOfType<AudioManager>().Play("V shoot");

                if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
                {
                    FindObjectOfType<AudioManager>().Play("V get back");
                    state = VaderState.CHASE;
                    //firePoint.shoot = false;
                }

                if (Vector3.Distance(transform.position, player.transform.position) > chaseDistance)
                {
                    state = VaderState.DEFAULT;
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

    void Teleport()
    {
        vader.transform.rotation = player.transform.rotation;
        vader.transform.position = player.transform.localPosition - (new Vector3(0, 0, -8));
        FindObjectOfType<AudioManager>().Play("vBehind");
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
            if (enemyHealth == 9)
            {
                FindObjectOfType<AudioManager>().Play("v9HP");
            }
            if (enemyHealth == 8)
            {
                Teleport();
                FindObjectOfType<AudioManager>().Play("v8HP");
            }
            if (enemyHealth == 7)
            {
                FindObjectOfType<AudioManager>().Play("v7HP");
            }
            if (enemyHealth == 6)
            {
                FindObjectOfType<AudioManager>().Play("v6HP");
            }
            if (enemyHealth == 5)
            {
                Teleport();
                FindObjectOfType<AudioManager>().Play("v5HP");
            }
            if (enemyHealth == 4)
            {
                FindObjectOfType<AudioManager>().Play("v4HP");
            }
            if (enemyHealth == 3)
            {
                FindObjectOfType<AudioManager>().Play("v3HP");
            }
            if (enemyHealth == 2)
            {
                Teleport();
                FindObjectOfType<AudioManager>().Play("v2HP");
            }
            if (enemyHealth == 1)
            {
                FindObjectOfType<AudioManager>().Play("v1HP");
            }
            enemyHealth--;

            if(enemyHealth <= 0)
            {
                killCount++;

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
        //FindObjectOfType<AudioManager>().Play("vNo");
        FindObjectOfType<AudioManager>().Play("vNo");
        yield return new WaitForSeconds(waitTime);
        StopExplosion();    //stops the explosion
        SceneManager.LoadScene("Win Scene Final2");
        Destroy(gameObject);
        //SceneManager.LoadScene("Win Scene");
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
}
