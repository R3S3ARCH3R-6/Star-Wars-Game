using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public float attackSpeed = 0.5f;
    public bool isAttacking = false;
    public bool isAiming = false;
    public bool isBlocking = false;
    public bool isDeflecting = false;
    public GameObject hitbox;
    public GameObject bulletEmitter;
    public Transform weapon;
    
    
    // Start is called before the first frame update
    void Start()
    {
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for the normal attack!
        if (Input.GetMouseButtonDown(0) && isAttacking == false && isAiming == false && isDeflecting == false && isBlocking == false)
        {
            FindObjectOfType<AudioManager>().Play("pAtk");
            StartAttack();
        }
        
        //Check to go into our "shoot" stance!
        if(Input.GetMouseButton(1) && isAttacking == false && isDeflecting == false && isBlocking == false)

        {
            //FindObjectOfType<AudioManager>().Play("pAim");
            isAiming = true;
            weapon.localRotation = Quaternion.Euler(90, 0, 0);
            weapon.localPosition.Set(0, 0, 0);

            //check if we wanna shoot!
            if (Input.GetMouseButtonDown(0) && isAttacking == false)
            {
                FindObjectOfType<AudioManager>().Play("pShoot");
                Shoot();
                
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
            weapon.localRotation = Quaternion.Euler(-17, 0, -5);
        }

        if (Input.GetKey(KeyCode.LeftShift) && isAttacking == false && isAiming == false)
        {
            weapon.localRotation = Quaternion.Euler(0, 0, 70);
            StartDeflect();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isBlocking = false;
            isDeflecting = false;
            weapon.localRotation = Quaternion.Euler(-17, 0, -5);
        }

    }

    void StartAttack()
    {
        isAttacking = true;
        StartCoroutine(waitAttack(attackSpeed));
    }
    
    void Shoot()
    {
        bulletEmitter.GetComponent<BulletScript>().Shoot();
        StartCoroutine(waitShoot((attackSpeed)));
    }

    void StartDeflect()
    {
        //FindObjectOfType<AudioManager>().Play("pBlock");
        StartCoroutine(waitDeflect(attackSpeed));
    }

    IEnumerator waitAttack(float attackSpeed)
    {
        Debug.Log("attacked");
        checkAttack();
        yield return new WaitForSeconds(attackSpeed);
        isAttacking = false;
        Debug.Log("stop attack");
        checkAttack();
    }

    IEnumerator waitShoot(float attackSpeed)
    {
        Debug.Log("shot!");
        isAttacking = true;
        yield return new WaitForSeconds(attackSpeed);
        Debug.Log("can shoot again!");
        isAttacking = false;
    }

    IEnumerator waitDeflect(float waitTime)
    {
        Debug.Log("deflecting!");
        isDeflecting = true;
        isBlocking = true;
        yield return new WaitForSeconds(waitTime);
        Debug.Log("end deflect");
        isDeflecting = false;

    }

    void checkAttack()
    {
        hitbox.SetActive(isAttacking);
    }
}
