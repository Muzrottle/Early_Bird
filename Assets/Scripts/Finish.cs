using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Finish : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;
    CollisionHandler collisionHandler;
    BoxCollider boxCollider;
    [SerializeField] GameObject scoreBoard;
    [SerializeField] GameObject ammoAmount;
    [SerializeField] GameObject finishUI;
    [SerializeField] TMP_Text scorePoints;
    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float verticalPos = -2f;
    bool isFinished = false;

    private void Update()
    {
        FinishSquence();
    }

    private void FinishSquence()
    {
        if (!isFinished)
        { 
            return; 
        }

        if (player != null)
        {
            Vector3 resetPosition = Vector3.zero;
            player.transform.localPosition = Vector3.Lerp(player.transform.localPosition, resetPosition, Time.deltaTime * rotationSpeed);

            Quaternion rotation = Quaternion.Euler(0, verticalPos, 0);
            player.transform.localRotation = Quaternion.Lerp(player.transform.localRotation, rotation, Time.deltaTime * rotationSpeed);
        }

        finishUI.SetActive(true);
        scorePoints.text = scoreBoard.GetComponent<ScoreBoard>().score.ToString();
        scoreBoard.SetActive(false);
        ammoAmount.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;

        if (player.GetComponent<PlayerController>() == null && player.tag != "Player")
        {
            return;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playerController = player.GetComponent<PlayerController>();
        collisionHandler = player.GetComponent<CollisionHandler>();
        boxCollider = player.GetComponent<BoxCollider>();
        playerController.enabled = false;
        collisionHandler.enabled = false;
        boxCollider.enabled = false;

        isFinished = true;
    }
}
