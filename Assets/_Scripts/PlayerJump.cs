using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    [Header("Inscribed")]
    public KeyCode jump = KeyCode.Space;
    public float jumpPower = 550;
    public int maxJumps = 1;
    public float bufferTime = 0.3f;
    public Vector2 boxSize;
    public float raycastDist;
    public LayerMask groundLayer;
    public float leapCooldown = 0.5f;
    public float leapStrength = 1000;
    public GameObject leapParticles;
    public float maxHoldJumpDuration = 0.3f;
    public float holdingJumpPower = 50;
    [Header("Dynamic")]
    [SerializeField] int jumpsLeft = 0;
    public int JumpsLeft => jumpsLeft;
    [SerializeField] Rigidbody2D playerRigid;
    [SerializeField] float remainingBuffer = 0;
    [SerializeField] PlayerController pcScript;
    [SerializeField] float remainingLeapCooldown;
    public bool leapOnCooldown = false;
    [SerializeField] bool bufferedJump = false;
    float heldJumpDuration = 0;
    SettingsManager settingsManager;

  
    void Awake(){
        playerRigid = GetComponent<Rigidbody2D>();
        pcScript = GetComponent<PlayerController>();
        settingsManager = Camera.main.GetComponent<SettingsManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (leapOnCooldown && remainingLeapCooldown > 0){
            remainingLeapCooldown -= Time.deltaTime;
        }
        if (remainingLeapCooldown <= 0 && leapOnCooldown){
            remainingLeapCooldown = 0;
            leapOnCooldown = false;
        }
        
        if (Input.GetKeyDown(settingsManager.controls["jump"]) && !pcScript.interactClickActive)
        {
            print("player just tapped jump");
            //if the player is leaping from a wall
            if (pcScript.forJumpCanLeap && !leapOnCooldown){
                Leap();
            }
            //if the player can jump normally
            else if (jumpsLeft > 0){Jump();}
            //if the player cannot jump and is trying to
            else{
                BufferJump();
                }
        }

        //if the player releases the jump key, they can't hold it down any more.
        if (Input.GetKeyUp(jump)){
            heldJumpDuration = 0;
        }
        

        //buffer a jump or leap
        if (remainingBuffer > 0 && bufferedJump){
            if (jumpsLeft > 0){Jump();}
            else if (pcScript.forJumpCanLeap && !leapOnCooldown){Leap();}
            remainingBuffer -= Time.deltaTime;
        }
        else if (remainingBuffer <= 0 && bufferedJump){
            bufferedJump = false;
            remainingBuffer = 0;
        }
    }

    void FixedUpdate(){
        isGrounded();

        if (heldJumpDuration > 0 && Input.GetKey(settingsManager.controls["jump"])){
            playerRigid.AddForce(new Vector2(0, holdingJumpPower));
            print("holding jump");
        }
        heldJumpDuration -= 0.02f;
    }

    public bool isGrounded(){
         ResetPlatParent();
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, raycastDist, groundLayer)){
         jumpsLeft = maxJumps;
         RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, raycastDist, groundLayer);
         if (hit.transform.gameObject.layer == LayerMask.NameToLayer("MovingPlatform")) {
  MovingPlatParent(hit);}

            return true;
        }
        else{
            jumpsLeft = 0;
            return false;
        }
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireCube(transform.position-transform.up * raycastDist, boxSize);
    }

    void Leap(){
        float hAxis = 0;
       if (Input.GetKey(settingsManager.controls["right"])){
        hAxis += 1;
       }
        if (Input.GetKey(settingsManager.controls["left"])){
            hAxis -= 1;
        }
        float vAxis = Input.GetAxisRaw("Vertical");

        leapOnCooldown = true;
        remainingLeapCooldown = leapCooldown;

        bufferedJump = false;

        Vector2 normalizeVec = Vector2.zero;
        if (vAxis > 0){
            normalizeVec = new Vector2(hAxis * leapStrength / 3, vAxis * leapStrength);
        }
        else if (vAxis < 0){
            normalizeVec = new Vector2(hAxis * leapStrength, vAxis * leapStrength / 2);
        }
        else{
            //print("v axis is 0");
            normalizeVec = new Vector2(hAxis * leapStrength * 2, leapStrength / 4);
        }
        playerRigid.AddForce(normalizeVec);
        //playerRigid.velocity = new Vector2(-10000, 10000);

        GameObject particles = Instantiate<GameObject>(leapParticles);
        particles.transform.position = transform.position;
    }

    void Jump(){
        
        playerRigid.AddForce(new Vector2(0, jumpPower));
        jumpsLeft -= 1;
        remainingBuffer = 0;
        bufferedJump = false;

        heldJumpDuration = maxHoldJumpDuration;
        pcScript.CreateDust(particleCount: 30);
    }

    void BufferJump(){
        print("buffer jump test");
        remainingBuffer = bufferTime;
        bufferedJump = true;
    }

    void MovingPlatParent(RaycastHit2D hit){
        this.transform.parent = hit.transform;
        MovingPlatform mpScript = hit.transform.gameObject.GetComponent<MovingPlatform>();
        mpScript.contactFrozen = false;
    }

    void ResetPlatParent(){
        this.transform.parent = null;
    }

   
}
