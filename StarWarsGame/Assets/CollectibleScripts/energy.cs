using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class energy : MonoBehaviour
{
    static public double playerEnergy;
    public double energyIncrement = 10;
    public float invincibilityTime = 5;
    static public bool isInvincible = false;
    static public float playerHealth;
    static public float playerScore = 0;
    public GameObject self;
    static private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
        playerEnergy = 100;
        playerHealth = 100f;
        isInvincible = false;
        //playerScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        print(playerEnergy);
        if (playerEnergy > 100)
        {
            playerEnergy = 100;
        }

        if (playerHealth <= 0)
        {
            isDead = true;
        }
        if (isDead)
        {
            GameOver();
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyAtk"))
        {
            if (self.GetComponent<Attack>().isDeflecting == true && playerEnergy > 0)
            {
                AddEnergy(energyIncrement);
                if (playerHealth >= 100)
                {
                    FindObjectOfType<AudioManager>().Play("fullHP");
                }
            }
            else if (self.GetComponent<Attack>().isBlocking == true & playerEnergy > 0)
            {
                AddEnergy(-energyIncrement);
            }
            else playerHealth -= 10f;
            if(playerHealth <= 20)
            {
                FindObjectOfType<AudioManager>().Play("pLowHP");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            collectibles collectible = (other.gameObject.GetComponent<collectibles>());
            if (collectible.collectibleType == 1)
            {
                FindObjectOfType<AudioManager>().Play("bat1");
                playerEnergy += energyIncrement;
            }
            if (collectible.collectibleType == 2)
            {
                FindObjectOfType<AudioManager>().Play("bat2");
                playerEnergy += (energyIncrement * 2);
            }
            if (collectible.collectibleType == 3)
            {
                FindObjectOfType<AudioManager>().Play("bat3");
                if (isInvincible == false)
                //{
                    playerEnergy += 0 - energyIncrement;
                //}
                if(playerEnergy <= 20)
                {
                    FindObjectOfType<AudioManager>().Play("pLowBP");
                }
            }
            if (collectible.collectibleType == 4)
            {
                FindObjectOfType<AudioManager>().Play("bat4");
                if (isInvincible == false)
                //{
                    playerEnergy += 0 - (energyIncrement * 2);
                //}
                if (playerEnergy <= 20)
                {
                    FindObjectOfType<AudioManager>().Play("pLowBP");
                }
            }
            if (collectible.collectibleType == 5)
            {
                FindObjectOfType<AudioManager>().Play("batINV");
                FindObjectOfType<AudioManager>().Play("bat5");
                StartCoroutine(Invincible(invincibilityTime));
            }

        }
        
    }
    static public void AddEnergy(double amount)
    {
            playerEnergy += amount;
    }

    static public void DecreaseHealth(float amount)
    {
        playerHealth -= amount;
    }

    private IEnumerator Invincible(float waitTime)
    {
        isInvincible = true;
        yield return new WaitForSeconds(waitTime);
        isInvincible = false;
    }

    static public void addScore(float amount)
    {
        playerScore += amount;
    }

    private void GameOver()
    {
        isDead = false;
        playerHealth = 100f;
        SceneManager.LoadScene("Dead Scene Final2");
    }
}
