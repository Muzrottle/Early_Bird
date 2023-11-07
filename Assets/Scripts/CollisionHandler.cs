using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelay = 1.0f;
    [SerializeField] GameObject planeCrashFX;
    [SerializeField] GameObject playerPlane;
    [SerializeField] GameManager gameManager;

    private void OnEnable()
    {
        Invoke(nameof(EnableCollision), 1);
    }

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} bumped into {other.gameObject.name}");

        if (other.gameObject.tag != "Finish")
        {
            StartCrashSequence();

        }
    }

    private void EnableCollision()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    private void StartCrashSequence()   
    {
        GetComponent<PlayerController>().audioSource.Stop();
        GetComponent<PlayerController>().SetBulletsActive(false);
        GetComponent<PlayerController>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        //Instantiating the planeCrashFX and caching it so that we can parent it to the enemy plane.
        //We parented it because we want to make explosion particles to stay in direction that plane was flying.
        GameObject deathFXInstance = Instantiate(planeCrashFX);
        deathFXInstance.transform.SetParent(playerPlane.transform, false);

        foreach (var meshRenderer in playerPlane.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }

        //planeCrashFX.SetActive(true);

        gameManager.Invoke(nameof(gameManager.ReloadLevel), loadDelay);
    }


}
