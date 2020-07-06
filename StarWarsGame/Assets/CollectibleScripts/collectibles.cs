using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class collectibles : MonoBehaviour
{
    AudioSource collectionSound;
    public double collectibleType;
    public double collectibleBaseAmount = 10;

    // Start is called before the first frame update
    void Start()
    {
        collectionSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles += new Vector3(0, 5.0f, 0);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Renderer[] allRenders = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer c in allRenders) c.enabled = false;
            Collider[] allcolliders = gameObject.GetComponentsInChildren<Collider>();
            foreach (Collider c in allcolliders) c.enabled = false;
            Destroy(gameObject);
        }
    }
}
