using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        // public Text coinText;

        public Text timerText;

        public GameObject spawnPoint;

        public Checkpoint lastCheckpoint;

        private GameObject deathPlayer;
        
        private float startTime;

        private float currentLevelTime;


        void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            // startTime = Time.time;
        }

        void OnEnable()
        {
            // If it is level 2, load the time from the previous level
            if (Application.loadedLevel == 2)
            {
                Debug.Log("Loading time from previous level");
                currentLevelTime = PlayerPrefs.GetFloat("time", 0);
                startTime = Time.time;
                // Debug.Log(startTime);
            }
            else
            {
                Debug.Log("Starting new timer");
                startTime = Time.time;
            }

        }

        void OnDisable()
        {
            // if (player.winState == true)
            // {
               
            // }
            // PlayerPrefs.SetFloat("time", startTime);
        }

        void Update()
        {
            // Set timer text to mm:ss:ms, where any value less than 10 is padded with a 0
            // Should be 00:00:00 at the start of the game and increase as time passes
            if (!player.winState)
            {
                TimeSpan t = TimeSpan.FromSeconds(Time.time - currentLevelTime + startTime);
                timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Minutes, t.Seconds, t.Milliseconds);
            }
            if (player.deathState == true)
            {
                // playerGameObject.SetActive(false);
                Debug.Log("Player is dead");
                player.animator.SetInteger("playerState", 3);

                // deathPlayer = (GameObject)Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
                // deathPlayer.transform.localScale = new Vector3(playerGameObject.transform.localScale.x, playerGameObject.transform.localScale.y, playerGameObject.transform.localScale.z);

                // player.deathState = false;
                Invoke("ReloadLevel", 1);
                // player.deathState = false;
            }

            // Restart if R key is pressed
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReloadLevel();

            }

            if (player.winState == true)
            {
                if (Application.loadedLevel != 2)
                {
                    PlayerPrefs.SetFloat("time", Time.time - startTime + currentLevelTime);
                    Invoke("LoadNextLevel", 1f);
                }
                else
                {
                    Invoke("LoadMainScreen", 1f);
                }
            }
        }

        public void setCheckpoint(Checkpoint checkpoint)
        {
            lastCheckpoint = checkpoint;
            Debug.Log("Checkpoint set");
        }
        private void ReloadLevel()
        {
            // Remove the deathPlayer object
            if (deathPlayer != null)
            {
                Destroy(deathPlayer);
            }
            if (lastCheckpoint != null)
            {
                Debug.Log("Moving Player");
                Debug.Log(lastCheckpoint.transform.position);
                Debug.Log(playerGameObject.transform.position);

                playerGameObject.transform.position = lastCheckpoint.transform.position;
            }
            else
            {
                playerGameObject.transform.position = spawnPoint.transform.position;

            }
            // Reset all clocks
            MultiClockController[] clocks = FindObjectsOfType<MultiClockController>();
            foreach (MultiClockController clock in clocks)
            {
                clock.ResetClock();
            }
            player.deathState = false;
            playerGameObject.SetActive(true);
            player.unfreezePlayer();
            // player.playerState = PlayerController.State.Idle;

        }

        private void FreezePlayer()
        {
            player.freezePlayer();
        }
        private void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void LoadMainScreen()
        {
            SceneManager.LoadScene(0);
        }
    }
}
