using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoHandler : MonoBehaviour
{
    [Header("Ammonution")]
    [SerializeField] int ammoSize = 20;
    [SerializeField] float fireRate = 15f;
    [SerializeField] float reloadTime = 3f;

    bool isFiring = false;
    public bool isReloading = false;
    int currentAmmo;
    float nextTimeToFire = 0f;

    public TMP_Text ammoAmountTxt;
    AudioSource audioSource;
    public PlayerController playerController;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentAmmo = ammoSize;
        ammoAmountTxt.text = currentAmmo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessAmmo();
        ProcessReloading();
    }

    private void ProcessAmmo()
    {
        Debug.Log(currentAmmo);
        if (isReloading)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            isFiring = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            isFiring = false;
        }

        if (isFiring && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            currentAmmo--;
            ammoAmountTxt.text = currentAmmo.ToString();
        }

        if ((Input.GetKey(KeyCode.R) || currentAmmo == 0) && currentAmmo != ammoSize)
        {
            StartCoroutine(ProcessReloading());
        }
    }

    private IEnumerator ProcessReloading()
    {
        isFiring = false;
        playerController.FireGuns(false);
        isReloading = true;
        audioSource.Play();

        yield return new WaitForSeconds(reloadTime);
        currentAmmo = ammoSize;
        ammoAmountTxt.text = currentAmmo.ToString();
        isReloading = false;
    }
}
