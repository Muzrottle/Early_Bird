using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] CollisionHandler collisionHandler;
    [SerializeField] AmmoHandler ammoHandler;
    [SerializeField] GameObject masterTimeline;
    [SerializeField] GameObject enemyWaves;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject ammoAmount;
    [SerializeField] GameObject scoreBoard;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerController.enabled = true;
        collisionHandler.enabled = true;
        ammoHandler.enabled = true;
        masterTimeline.SetActive(true);
        enemyWaves.SetActive(true);
        startMenu.SetActive(false);
        ammoAmount.SetActive(true);
        scoreBoard.SetActive(true);
    }

    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
