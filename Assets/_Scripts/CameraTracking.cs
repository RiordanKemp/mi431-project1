using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [Header("Inscribed")]
    public bool followPlayer = true;
    public float clampX = 3;
    public float clampY = 1;
    public CameraEffectType defaultCameraScript;
    [Header("Dynamic")]
    [SerializeField] GameObject player;
    public CameraEffectType cameraEffectScript;
    float remainingDuration = 0;
    float cameraCurrentShake = 0;
    float cameraTimeElapsed = 0;
    float cameraDuration;
    void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    void FixedUpdate()
    {


        switch (cameraEffectScript._camEffectType){
            case CameraEffectType.eCamType.none:
            DefaultCameraBehavior();
            break;
        
            case CameraEffectType.eCamType.snapInteract:
            SnapCameraBehavior(cameraEffectScript.shakeStrength);
            break;

            case CameraEffectType.eCamType.lockPos:
            LockCameraBehavior(cameraEffectScript.lockedPos);
            break;
        }

    }

    public void SetCameraType(CameraEffectType camType){
        //Called by other scripts to change the camera's behavior temporarily


        //delete any leftover UI elements from the last object
        if (cameraEffectScript.uiGO != null){
            cameraEffectScript.uiGO.SetActive(false);
            }
        cameraEffectScript = camType;
        remainingDuration = cameraEffectScript.effectDuration;
        cameraDuration = cameraEffectScript.effectDuration;
        cameraTimeElapsed = 0;
        if (camType.uiGO != null){
            cameraEffectScript.uiGO.SetActive(true);
        }
    } 


    void DefaultCameraBehavior(){
        Vector3 holdVec3 = new Vector3(Mathf.Clamp(player.transform.position.x, -clampX, clampX), Mathf.Clamp(player.transform.position.y, clampY, float.MaxValue), -10);
        transform.position = holdVec3;  
    }

    void LockCameraBehavior(Vector3 lockPos){
        transform.position = lockPos;
        remainingDuration -= 0.02f;
        if (remainingDuration <= 0){
            ResetCameraBehavior();
        }
    }

    void SnapCameraBehavior(float cameraShake){
                    //shake the camera
            Vector3 holdPos = transform.position;
            holdPos.x = Random.Range(-cameraCurrentShake + player.transform.position.x, cameraCurrentShake + player.transform.position.x);
            holdPos.y = Random.Range(-cameraCurrentShake / 2 + player.transform.position.y, cameraCurrentShake / 2  + player.transform.position.y);
            transform.position = Vector2.MoveTowards(player.transform.position, holdPos, cameraCurrentShake);
           // print("hold pos:" + holdPos + "transform pos:" + player.transform.position);
            
            //fix the Z position
            Vector3 holdVec = new Vector3(Mathf.Clamp(transform.position.x, -clampX, clampX), Mathf.Clamp(transform.position.y, clampY, float.MaxValue), -10);
            transform.position = holdVec;

            //relax the shake amount over time
            cameraCurrentShake = Mathf.Lerp(cameraShake, 0, (cameraTimeElapsed / cameraDuration));

        remainingDuration -= 0.02f;
        cameraTimeElapsed += .02f;
        if (remainingDuration <= 0){
            ResetCameraBehavior();
        }
    }

    void ResetCameraBehavior(){
            if (cameraEffectScript.uiGO != null){
            cameraEffectScript.uiGO.SetActive(false);
            }
        SetCameraType(defaultCameraScript);
    
    }
}
