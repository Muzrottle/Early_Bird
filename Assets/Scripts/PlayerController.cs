using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("General Setup Settings")]
    [Tooltip("How fast ship moves around based on player input.")]
    [SerializeField] float controlSpeed;
    [Tooltip("How Far Player Moves Horizontally")][SerializeField] float rangeX;
    [Tooltip("How Far Player Moves Vertically")][SerializeField] float rangeY;
    [SerializeField] float rotationSpeed = 1f;

    [Header("Bullet Guns Array")]
    [Tooltip("Add All Player Bullet Guns Here")]
    public GameObject[] bullets;

    [Header("Screen Position Based Tuning")]
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = 2f;

    [Header("Player Input Based Tuning")]
    [SerializeField] float controlPitchFactor = -10f;
    [SerializeField] float controlRollFactor = -10f;

    [Header("Bullets SFX")]
    [SerializeField] AudioClip bulletFire;
    [SerializeField] AudioClip bulletFireEnd;

    public AmmoHandler ammoHandler;
    public AudioSource audioSource;
    float xThrow, yThrow;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float newXPos = Mathf.Clamp(rawXPos, -rangeX, rangeX);

        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float newYPos = Mathf.Clamp(rawYPos, -rangeY, rangeY);

        transform.localPosition = new Vector3(newXPos, newYPos, transform.localPosition.z);
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yawDueToPosition = transform.localPosition.x * positionYawFactor;
        float yaw = yawDueToPosition;

        float rollDueToControlThrow = xThrow * controlRollFactor;
        float roll = rollDueToControlThrow;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, roll);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, Time.deltaTime * rotationSpeed);

    }

    private void ProcessFiring()
    {
        if (ammoHandler.isReloading)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            FireGuns(true);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            FireGuns(false);
        }
    }

    public void FireGuns(bool isFiring)
    {
        if (isFiring)
        {
            SetBulletsActive(true);

            audioSource.loop = true;
            audioSource.PlayOneShot(bulletFire, 0.7f);
        }
        else
        {
            SetBulletsActive(false);
            Debug.Log("Ateþ etmeyi býraktým.");
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.PlayOneShot(bulletFireEnd, 0.7f);
        }
    }

    public void SetBulletsActive(bool isActive)
    {
        foreach (GameObject bullet in bullets)
        {
            //Have to cache it. Because these structs are not behaving like normal C# structs.
            //Read https://docs.unity3d.com/ScriptReference/ParticleSystem.html for more info.
            var bulletEmission = bullet.GetComponent<ParticleSystem>().emission;
            bulletEmission.enabled = isActive;
        }
    }

    private void DeactivateBullets()
    {
        foreach (GameObject bullet in bullets)
        {
            //Have to cache it. Because these structs are not behaving like normal C# structs.
            //Read https://docs.unity3d.com/ScriptReference/ParticleSystem.html for more info.
            var bulletEmission = bullet.GetComponent<ParticleSystem>().emission;
            bulletEmission.enabled = false;
        }
    }

    
}
