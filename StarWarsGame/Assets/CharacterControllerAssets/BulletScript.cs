using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    public GameObject Bullet;
    public float BulletForce = 50f;
    public float DestroyTime = 2.0f;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        GameObject currentBullet = Instantiate(Bullet, this.transform.position, this.transform.rotation) as GameObject;
        currentBullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        currentBullet.GetComponent<Rigidbody>().AddForce(transform.up * BulletForce);
        Destroy(currentBullet, DestroyTime);
    }
}
