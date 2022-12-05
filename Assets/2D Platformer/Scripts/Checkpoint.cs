using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Checkpoint : MonoBehaviour
    {
        public Sprite checkpointOn;
        public Sprite checkpointOff;

        public GameManager gameManager;

        [HideInInspector]
        public bool isActivated = false;

        // Start is called before the first frame update
        void Start()
        {


        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Checkpoint triggered");
                gameObject.GetComponent<SpriteRenderer>().sprite = checkpointOn;
                isActivated = true;
                gameManager.setCheckpoint(this);
                // GetComponent<SpriteRenderer>().sprite = checkpointOn;
            }
        }
    }
}