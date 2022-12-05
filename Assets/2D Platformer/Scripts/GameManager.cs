using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public int coinsCounter = 0;

        public GameObject playerGameObject;
        private PlayerController player;
        public GameObject deathPlayerPrefab;
        public Text coinText;

        public GameObject spawnPoint;

        public Checkpoint lastCheckpoint;

        private GameObject deathPlayer;

        void Start()
        {
            player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        void Update()
        {
            if (player.deathState == true)
            {
                playerGameObject.SetActive(false);
                deathPlayer = (GameObject)Instantiate(deathPlayerPrefab, playerGameObject.transform.position, playerGameObject.transform.rotation);
                deathPlayer.transform.localScale = new Vector3(playerGameObject.transform.localScale.x, playerGameObject.transform.localScale.y, playerGameObject.transform.localScale.z);

                player.deathState = false;
                Invoke("ReloadLevel", 1);
            }

            // Restart if R key is pressed
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReloadLevel();

            }

            if (player.winState == true)
            {
                if (Application.loadedLevel == 2)
                {
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
            playerGameObject.SetActive(true);
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
            SceneManager.LoadScene(1);
        }
    }
}
