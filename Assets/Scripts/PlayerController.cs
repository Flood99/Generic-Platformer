using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpHeight = 5;
    public float spd = 5;
    public int maxJumps = 1;
    public float maxSpdX;
    public float maxSpdY;
    private bool isOnGround = false;
    private int jumps;
   
    Vector2 velocity; 
    private Rigidbody2D rb;


    private enum Playerstate
    {
        Grounded,
        Falling,
        OnWall,
        InAir
    }   

    Playerstate currentState;
    // Start is called before the first frame update
    void Start()
    {
        //Get Player Rigidbody
        rb = GetComponent<Rigidbody2D>();
        //set jump count at start
        jumps = maxJumps;
        //default state to grounded
        currentState = Playerstate.Grounded;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position,Vector3.left*100,Color.red);
        bool jumpPressed = Input.GetButtonDown("Jump");
        //State Machine
        switch(currentState)
        {
            case Playerstate.Grounded:
            if(jumpPressed)
            {
               
                // convert jumpheight variable into constant units
                float jumpForce = Mathf.Sqrt(2f * Physics2D.gravity.magnitude * GetComponent<Rigidbody2D>().gravityScale * jumpHeight);
                //set y velocity to 0 for double jumps, then add force
                rb.velocity = new Vector2(rb.velocity.x,0);
                rb.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);
                //remove jump
                jumps -=1;
                //set new state
                currentState = Playerstate.InAir;

            }
            break;

            case Playerstate.InAir:
            //same as grounded except checks jump count
            if(jumpPressed && jumps > 0)
            {
               
                float jumpForce = Mathf.Sqrt(2f * Physics2D.gravity.magnitude * GetComponent<Rigidbody2D>().gravityScale * jumpHeight);
                rb.velocity = new Vector2(rb.velocity.x,0);
                rb.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);
                jumps -=1;
                currentState = Playerstate.InAir;

            }
            if(rb.velocity.y < 0) currentState = Playerstate.Falling;
            break;

            case Playerstate.Falling:
            //same as grounded except checks jump count
            if(jumpPressed && jumps > 0)
            {
               
                
                float jumpForce = Mathf.Sqrt(2f * Physics2D.gravity.magnitude * GetComponent<Rigidbody2D>().gravityScale * jumpHeight);
                rb.velocity = new Vector2(rb.velocity.x,0);
                rb.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);
                jumps -=1;
                currentState = Playerstate.InAir;

            }
            break;
            

        }
        
        CheckWall();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Only allow player to be grounded when falling and touching floor
        if(collision.gameObject.CompareTag("Floor") && currentState == Playerstate.Falling)
        {
            currentState = Playerstate.Grounded;
            //reset jumps
            jumps = maxJumps;
            Debug.Log("On Ground");
        }
        
            
        


    }
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        
        rb.AddForce(Vector2.right*horizontal*spd);
        
        
        
        //limit x speed
        if(Mathf.Abs(rb.velocity.x) > maxSpdX)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpdX,rb.velocity.y);
        }
        //limit y speed
        if(Mathf.Abs(rb.velocity.y) > maxSpdY)
        {
            rb.velocity = new Vector2(rb.velocity.x,Mathf.Sign(rb.velocity.y) * maxSpdX);
        }
        Debug.Log(rb.velocity.magnitude);

    }
    void CheckWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,Vector2.left,10);
       
        
    }
    
}
