using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSController : MonoBehaviour
{
    public float speed = 12f;
    public CharacterController controller;

    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float height = 1f;
    public float crouchSpeed = 0.5f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded = true;
    public bool isCrouched = false;
    public GameObject self;
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouched = true;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (isCrouched && isGrounded)
        {
            //Debug.Log("crouching");
            self.transform.localScale = new Vector3(1, height * 0.5f, 1);
        }

        Vector3 move = new Vector3();
        if (isCrouched)
        {
            move = transform.right * x * crouchSpeed + transform.forward * z * crouchSpeed;
        }
        else
        {
            move = transform.right * x + transform.forward * z;
        }
        controller.Move(move * speed * Time.deltaTime);


        

        if(Input.GetButtonDown("Jump") && isGrounded && !isCrouched)
        {
            FindObjectOfType<AudioManager>().Play("pJump");
            velocity.y = Mathf.Sqrt(jumpHeight * gravity * -2);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (!Input.GetKey((KeyCode.LeftControl)))
        {
            isCrouched = false;
            //Debug.Log("not crouch");
            self.transform.localScale = new Vector3(1, height, 1);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Interior Change"))
        {
            SceneManager.LoadScene("Interior Build 2");
        }
    }

}
