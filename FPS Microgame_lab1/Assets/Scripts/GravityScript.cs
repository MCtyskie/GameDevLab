using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScript : MonoBehaviour
{
    private float gravity = -9.81f;
    private float groundDistance = .1f;
    private float jumpHeight = 1f;
    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundMask;
    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //if grounded -> zero velocity.y
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -.01f;
        }
        //check if jump -> JUMP!
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * gravity * (-2));
        }
        //add gravity force and move Player
        velocity.y += gravity * Time.deltaTime;
        gameObject.GetComponent<CharacterController>().Move(velocity * Time.deltaTime);
    }
}
