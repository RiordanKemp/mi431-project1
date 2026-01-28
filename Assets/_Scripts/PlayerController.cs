using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

[Header("Inscribed")]
    public ParticleSystem dust;
    public bool isActive = true;
    public float speed = 8;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode holdChainKey = KeyCode.LeftShift;
    public KeyCode earlyFloatKey = KeyCode.F;
    public FanController activeFanController;
    public float clampedAcceleration = 200;
    public float accelerationRate = 0.05f;
    [Tooltip("The maximum speed the player can fall up or down.  Rigid Y velocity clamped to the +- of this value.")]
    public float maxVerticalSpeed = 50;

 [Tooltip("The amount of time the player has to input a leap after leaving a climbable wall.")]
    public float coyoteLeapTime = 0.5f;
    [Tooltip("Checkpoint to be used at the start of the game")]
    public Checkpoint currentCheckpoint;



    [Header("Dynamic")]
       public float interactClickRemaining = 0;
       public MovingPlatform[] movingPlatforms;
    public bool interactClickActive = false;
    [SerializeField] Rigidbody2D playerRigid;
    [SerializeField] float startingGravity;
    [Tooltip("Whether or not the player is currently in the collider of a chain.")]
    [SerializeField] bool nearChain = false;
    [Tooltip("Whether or not the player is holding down the 'hold chain' button")]
    [SerializeField] bool holdingChainKey = false;
    [SerializeField] bool holdingChain = false;
    [Tooltip("This is the dynamic version of starting gravity: it's what the player gravity should be if they release a chain.")]
    [SerializeField] float holdGravityScale = 0;
    [SerializeField] ChainAttributes currentChain;
    [SerializeField] float resetLeapTimer = 0;
    [SerializeField] PlayerJump pJumpScript;
    public bool nextLevel = false;
    public bool forJumpCanLeap = false;
    [SerializeField] SettingsManager settingsManager;
    // Start is called before the first frame update
    FanBlock[] fanBlockScripts;
    void Awake(){
        playerRigid = GetComponent<Rigidbody2D>();
        startingGravity = playerRigid.gravityScale;
        if (activeFanController == null){
            print("There is no inscribed fan controller in the player controller");
        }
        holdGravityScale = startingGravity;
        pJumpScript = this.gameObject.GetComponent<PlayerJump>();
        settingsManager = Camera.main.GetComponent<SettingsManager>();

        fanBlockScripts = FindObjectsOfType<FanBlock>();

        movingPlatforms = FindObjectsOfType<MovingPlatform>();
    }

    void Update(){

        if (nextLevel) {return;}

        if (Input.GetKeyDown(settingsManager.controls["triggerfan"])){
            EnableFansEarly();
        }

        if (resetLeapTimer > 0){
            resetLeapTimer -= Time.deltaTime;
        }
        else if (forJumpCanLeap){
            forJumpCanLeap = false;
            resetLeapTimer = 0;
        }

        if (Input.GetKeyDown(holdChainKey)){
            holdingChainKey = true;
        }
        else if (Input.GetKeyUp(holdChainKey)){
            holdingChainKey = false;
        }

          
        if (interactClickRemaining > 0){
            interactClickRemaining -= Time.deltaTime;
        }
        else if (interactClickActive && interactClickRemaining <= 0){
            ResetBuffer();
        }
    }

           public void ResetBuffer(){
            interactClickActive = false;
            interactClickRemaining = 0;
        }

        public void InteractCooldown()
    {
        interactClickActive = true;
        interactClickRemaining = 0.1f;
    }

        void EnableFansEarly(){
            print("enable fans early");
            if (!activeFanController.fansEnabled && activeFanController.remainingDuration > 1 && activeFanController.remainingDuration < 9000){
                activeFanController.FanChangeCue();
                print("successfully started fan early");
            }
        }

    void FixedUpdate()
    {
        if (nextLevel){return;}
        if (activeFanController != null){
            if (activeFanController.fansEnabled == true){
            //activeFanController.FloatPlayer(playerRigid);
            FloatPlayer();
            }
            if (!activeFanController.updatedGravity){
            activeFanController.updatedGravity = true;
            UpdateGravity();
            }
        }

        float hAxis = 0;

        if (Input.GetKey(settingsManager.controls["right"])){
            hAxis += 1;
        }
        if (Input.GetKey(settingsManager.controls["left"])){
            hAxis -= 1;
        }
        
        RotatePlayer(hAxis);

        // While holding a chain, the player can rotate but not move via inputs
        if (holdingChain) hAxis = 0;
        float rigidVelX = speed * hAxis;
        

        Vector2 holdVel = Vector2.zero;

        //print("rigid vel x:" + rigidVelX);

        //decelerate towards a steady speed, or 0 if there isnt any input
        if (playerRigid.velocity.x < 6 && playerRigid.velocity.x > -6){
        holdVel.x = playerRigid.velocity.x + (hAxis / 2);
        }
        else{
            holdVel.x = playerRigid.velocity.x;
        }

        if (hAxis == 0){
            holdVel.x = Mathf.MoveTowards(holdVel.x, 0, 0.5f);
        }

        //limit the maximum vertical velocity
        holdVel.y = Mathf.Clamp(playerRigid.velocity.y, -maxVerticalSpeed, maxVerticalSpeed);

        //decelerate the player at the top of their fall
        if (-5 < playerRigid.velocity.y && playerRigid.velocity.y < 0){
            holdVel.y = Mathf.Lerp(playerRigid.velocity.y, -5, 0.5f);
        }

        playerRigid.velocity = holdVel;

        //if the player is in the collider of a chain, check if the player is holding
        //the "chain hold" button
        if (Input.GetKey(settingsManager.controls["holdobject"]) && nearChain){
            HoldingChain(currentChain);
        }

        //if the player is currently holding a chain, but they release the key or move out of the hitbox, end the effect
        if (holdingChain && !pJumpScript.leapOnCooldown){

            //if the player is holding the chain, allow them to go up/down with W + S keys
            float vAxis = Input.GetAxis("Vertical");
            holdVel = new Vector2(playerRigid.velocity.x, vAxis * currentChain.climbSpeed);
            playerRigid.velocity = holdVel;

            if (!nearChain || !holdingChainKey){
                ReleaseChain();
            }
        }

     

    }

    void OnTriggerStay2D(Collider2D other){
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("Chain")){
            //print("chain layer");
            nearChain = true;
            currentChain = other.gameObject.GetComponent<ChainAttributes>();
        }
    }

    void OnTriggerExit2D(Collider2D other){
            if (other.transform.gameObject.layer == LayerMask.NameToLayer("Chain")){
            //print("exited chain layer");
            nearChain = false;
        }
    }

    void UpdateGravity(){
        playerRigid.gravityScale = startingGravity;
        holdGravityScale = startingGravity;
        foreach (FanBlock fbScr in fanBlockScripts){
            fbScr.UpdateBlockStatus(false);
        }
        print("updated gravity");
    }

    void HoldingChain(ChainAttributes currentChainScript){
        if (pJumpScript.leapOnCooldown){
            print("leap is on cooldown, returning");
            ReleaseChain();
            return;
        }
        playerRigid.gravityScale = 0;
        Vector3 holdTrans = new Vector3(currentChainScript.transform.position.x + currentChainScript.xOffset, transform.position.y, transform.position.z);
        transform.position = holdTrans;

        holdingChain = true;
        if (currentChainScript.isClimbableWall){
            print("reset leap timer");
            resetLeapTimer = coyoteLeapTime;
            forJumpCanLeap = true;}

        //if the player just grabbed onto the chain, reset their vertical velocity
        if (!holdingChain){
            Vector2 holdVel = new Vector2(0, 0);
            playerRigid.velocity = holdVel;
            print("just grabbed chain");
            }
 
    }

    void ReleaseChain(){
        //print("release chain");
        playerRigid.gravityScale = holdGravityScale;
        holdingChain = false;
        
    }

    public void FloatPlayer(){
        playerRigid.gravityScale = 0;
        holdGravityScale = 0;

        //attempt to accelerate the player towards 4 velocity if they aren't there yet. capped at 4
        float yVelocity = Mathf.Lerp(playerRigid.velocity.y, 4, 0.05f);
        Vector2 holdVec2 = new Vector2(playerRigid.velocity.x, yVelocity);
        playerRigid.velocity = holdVec2;

        foreach (FanBlock fbScr in fanBlockScripts){
            fbScr.UpdateBlockStatus(true);
        }
    }

    void RotatePlayer(float hAxis){
        if (hAxis > 0){
            this.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            
        }
        if (hAxis < 0){
            this.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            
        }
    }

    public void CreateDust(int particleCount = 15)
    {
        var emission = dust.emission;
        emission.SetBursts(new[]
                               {
                                   new ParticleSystem.Burst(0, particleCount), //float_time, short_count
                               });
        dust.Play();
    }

//accessed by player health
    public void Respawn(){
        Vector3 respawnVec = currentCheckpoint.transform.position - currentCheckpoint.respawnOffset;
        this.transform.position = respawnVec;
        playerRigid.velocity = Vector2.zero;
        UpdateGravity(); //reset the player's gravity
        activeFanController.FanReset();
        activeFanController.CancelInvoke("EnableFans");
        activeFanController.CancelInvoke("DisableFans");

        foreach(MovingPlatform platform in movingPlatforms)
        {
            platform.ResetPosition();
        }

        print("respawn");
    }

    //accessed by level triggers
    public void NextLevel(Canvas canvasObj){
        canvasObj.enabled = true;
        nextLevel = true;
        playerRigid.gravityScale = 0;
        playerRigid.velocity = Vector2.zero;
        activeFanController.FanReset();
        activeFanController.CancelInvoke("EnableFans");
        activeFanController.CancelInvoke("DisableFans");
        activeFanController.gameObject.SetActive(false);

    }
}
