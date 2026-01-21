using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class FanController : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject lightHolder;
    public Slider fanSlider;
    public Image sliderFill;
    public Color fansOnColor;
    public Color fansOffColor;
    public bool active = true;
    public bool fansEnabled = false;
    public float minDelay = 8;
    public float maxDelay = 8;
    public float duration = 4;
    public float floatIntensity = 2;
    public float maxAcceleration = 4;
    public ParticleSystem particleSystem;
    public GameObject soundPrefab;
    
    [Header("Dynamic")]
    [SerializeField] Light2D[] lights;
    [SerializeField] public float remainingDuration;
    float sliderTime;
    [SerializeField] public bool updatedGravity = true;
    // Start is called before the first frame update
    void Awake()
    {
        lights = lightHolder.GetComponentsInChildren<Light2D>();
        if (fansEnabled == false)
        {
            remainingDuration = UnityEngine.Random.Range(minDelay, maxDelay);
            sliderTime = remainingDuration;
            sliderFill.color = fansOffColor;
        }

        else
        {
            remainingDuration = duration;
            sliderTime = remainingDuration;
            sliderFill.color = fansOnColor;
        }


    }



    // Update is called once per frame
    void Update()
    {
        remainingDuration -= Time.deltaTime;
        sliderTime -= Time.deltaTime;
        if (remainingDuration <= 1)
        {
            FanChangeCue();
        }

        if (!fansEnabled)
        {
            float timePassed = (minDelay - sliderTime);
            fanSlider.value = (timePassed / minDelay) * 100;
            // Debug.Log($"time passed: {timePassed}");
        }
        
        if (fansEnabled)
        {
            float timePassed = (duration - sliderTime);
            fanSlider.value = Mathf.Abs(100 - (timePassed / duration) * 100);
            // Debug.Log($"time passed: {timePassed}");
        }
    
    }

    public void FanReset(){
        print("fan reset");
        DisableFans();

    }

public void FanChangeCue(){
    //modify the intensity of each light in the holder. set duration to 9999 to prevent reruns
    foreach(Light2D light in lights){
        light.intensity = 0.4f;
        Invoke("FanLightsFlicker", 0.2f);
        GameObject soundCue = Instantiate<GameObject>(soundPrefab);
            remainingDuration = 9999;
            sliderTime = 1;
        }

    if (fansEnabled == false){
            Invoke("EnableFans", 1);}
            else{
                Invoke("DisableFans", 1);
            }
}

void FanLightsFlicker(){
    foreach(Light2D light in lights){
        light.intensity = 1f;
    }
}
    void EnableFans(){
        remainingDuration = duration;
        sliderTime = remainingDuration;
        fansEnabled = true;
        sliderFill.color = fansOnColor;
        


        ParticleSystem.MainModule psm = particleSystem.main;
        psm.gravityModifier = -0.3f;
     
        int particleCount = particleSystem.particleCount;
ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleCount];
particleSystem.GetParticles(particles);
for (int i = 0; i < particles.Length; i++)
{
    particles[i].velocity = new UnityEngine.Vector3(0, 0, 0);
    particleSystem.SetParticles(particles, particleCount);
}
    }

   

    void DisableFans(){
        remainingDuration = UnityEngine.Random.Range(minDelay, maxDelay);
        sliderTime = remainingDuration;
        sliderFill.color = fansOffColor;
        fansEnabled = false;
        updatedGravity = false;

                ParticleSystem.MainModule psm = particleSystem.main;
        psm.gravityModifier = 0.05f;
     
        int particleCount = particleSystem.particleCount;
ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleCount];
particleSystem.GetParticles(particles);
for (int i = 0; i < particles.Length; i++)
{
    particles[i].velocity = new UnityEngine.Vector3(0, 0, 0);
    particleSystem.SetParticles(particles, particleCount);
}
    }

    //public void FloatPlayer(Rigidbody2D pRigid){
      //  pRigid.gravityScale = 0f;

        //attempt to accelerate the player towards 4 velocity if they aren't there yet. capped at 4
        //float yVelocity = Mathf.Lerp(pRigid.velocity.y, 4, 0.05f);
        //UnityEngine.Vector2 holdVec2 = new UnityEngine.Vector2(pRigid.velocity.x, yVelocity);
        //pRigid.velocity = holdVec2;
    
    //}
}
