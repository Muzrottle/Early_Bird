using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    [SerializeField] ParticleSystem hitVFX;
    [SerializeField] GameObject topParent;
    [SerializeField] int hitScore;
    [SerializeField] int killScore;
    [SerializeField] int hitPoint;

    ScoreBoard scoreBoard;
    Rigidbody rb;

    private void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();

        if (GetComponent<Rigidbody>() == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        hitVFX.Play();

        if (hitPoint != 0)
        {
            ProcessHit(hitScore);
            hitPoint--;
        }
        else
        {
            ProcessHit(killScore);
            KillEnemy(other);
        }
        
    }

    private void ProcessHit(int scoreToAdd)
    {
        scoreBoard.IncreaseScore(scoreToAdd);
    }

    private void KillEnemy(GameObject other)
    {
        Debug.Log($"{name}: I got hit by {other.gameObject.GetComponentInParent<Transform>().name}");

        GetComponent<MeshCollider>().enabled = false;

        //Instantiating the deathFX and caching it so that we can parent it to the enemy plane.
        //We parented it because we want to make explosion particles to stay in direction that plane was flying.
        GameObject deathFXInstance = Instantiate(deathFX);
        deathFXInstance.transform.SetParent(transform, false);

        //Disabling all MeshRenderers of the plane when it exploded.
        foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }

        //To calculate the duration of the deathVFX so that it destroys itself after effect is almost ended.
        ParticleSystem deathFXParticleSystem = deathFXInstance.GetComponent<ParticleSystem>();
        float deathFXDuration = deathFXParticleSystem.main.duration;
        float deathFXSimulationSpeed = deathFXParticleSystem.main.simulationSpeed;
        float deathFXMinLifetime = deathFXParticleSystem.main.startLifetime.constantMin;
        float destroyDuration = (deathFXDuration / deathFXSimulationSpeed) + deathFXMinLifetime;

        Destroy(topParent, destroyDuration);
    }
}
