using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;

public class SnapInteractable : MonoBehaviour
{
    [Header("Inscribed")]
    public float cooldown;
    public bool maintainVelocity = true;
    public float snapForce = 200;
    public enum eSnapType {upClick, downClick, vent};
    public eSnapType _snapType = eSnapType.upClick;
    public float interactBufferTime = 0.05f;
    public GameObject instantiateParticlesGO;
    public CameraEffectType cameraEffects;
    public Transform teleportLocation;



    [Header("Dynamic")]
    [SerializeField] Light2D objectLight;
    [SerializeField] ParticleSystem objectParticles;
    [SerializeField] bool active = false;
    [SerializeField] float cooldownTimer = 0;
    [SerializeField] bool cooldownActive = false;
    [SerializeField] KeyCode playerInteract = KeyCode.None;
    [SerializeField] Rigidbody2D playerRigid;
    [SerializeField] float remainingBufferTime = 0;
    [SerializeField] bool bufferedInteract = false;
    [SerializeField] PlayerController player;
    [SerializeField] CameraTracking cameraController;
    SettingsManager settingsManager;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other){
        player = other.gameObject.GetComponent<PlayerController>();
        if (player != null){
            active = true;
            playerInteract = player.interactKey;
            playerRigid = player.GetComponent<Rigidbody2D>();
            Debug.LogWarning("Set active");

            
        }
    }

    void Awake(){
        objectParticles = GetComponent<ParticleSystem>();
        objectLight = GetComponent<Light2D>();
        cameraController = FindObjectOfType<CameraTracking>();
        settingsManager = Camera.main.GetComponent<SettingsManager>();
    }

    void OnTriggerExit2D(Collider2D other){
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null){
            Debug.LogWarning("Set inactive");
            active = false;}
    }

    void Update(){
        // if (Input.GetKeyDown(settingsManager.controls["interact"]) && player != null){
        //     player.InteractCooldown();
        // }
      
        if (!cooldownActive){
            if (active && Input.GetKeyDown("space") && player != null){
                
                

                cameraController.SetCameraType(cameraEffects);
                player.InteractCooldown();
                switch (_snapType){
                    case eSnapType.upClick:
                    ClickUp();
                    break;

                    case eSnapType.downClick:
                    ClickDown();
                    break;

                    case eSnapType.vent:
                    print("case: vent");
                    ClickVent();
                    break;
                                    }

                                                        }
                                }
        
        else{
              //print("cooldownactive is true");
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0){
                EndCooldown();
                                    }
            }
                }
    

        void ClickUp(){
            if (!maintainVelocity){
            playerRigid.velocity = new Vector2(0, snapForce);

            }
            else{
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, snapForce);
            }
         //playerRigid.AddForce(new Vector2(playerRigid.velocity.x, snapForce));
            StartCooldown();
        }

        void ClickDown(){
            //if (playerRigid.velocity.y > 0){
            if (!maintainVelocity){
                playerRigid.velocity = new Vector2(0, -snapForce);

            }
            else{
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, -snapForce);
            }
        //}
            //playerRigid.AddForce(new Vector2(playerRigid.velocity.x, -snapForce));
            StartCooldown();
        }

        void ClickVent(){
            print("clickvent");
            playerRigid.transform.position = teleportLocation.transform.position;

            StartCooldown();
        }

 

        

        void StartCooldown(){
            cooldownActive = true;
            cooldownTimer = cooldown;
            objectLight.intensity = 0.1f;
            objectParticles.Stop(true);

            if (instantiateParticlesGO == null) return;
            GameObject particlesGO = Instantiate<GameObject>(instantiateParticlesGO);
            particlesGO.transform.position = transform.position;
        }

        void EndCooldown(){
            cooldownActive = false;
            cooldownTimer = cooldown;
            objectLight.intensity = 1;
            objectParticles.Play();
        }
    }


