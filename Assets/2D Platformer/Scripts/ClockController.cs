using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArrowDirection = ArrowController.ArrowDirection;


public class ClockController : MonoBehaviour
{

    private State state;

    private Dictionary<ArrowDirection, GameObject> ArrowDictionary = new Dictionary<ArrowDirection, GameObject>();

    private ArrowDirection currentDirection;
    // Start is called before the first frame update
    void Start()
    {

        foreach (Transform child in transform)
        {
            if (child.tag == "Arrow")
            {
                ArrowDictionary.Add(child.GetComponent<ArrowController>().direction, child.gameObject);
            }
        }
        Debug.Log(ArrowDictionary.Count);
        setState(State.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Arrow:
                bool up = Input.GetAxisRaw("Vertical") > 0;
                bool down = Input.GetAxisRaw("Vertical") < 0;
                bool right = Input.GetAxis("Horizontal") > 0;
                bool left = Input.GetAxis("Horizontal") < 0;
                Debug.Log("up: " + up + " down: " + down + " right: " + right + " left: " + left);
                // If the player is moving diagonally, we want to round to the nearest cardinal direction
                if (up && right)
                {
                    setActiveArrow(ArrowDirection.UpRight);
                }
                else if (up && left)
                {
                    setActiveArrow(ArrowDirection.UpLeft);
                }
                else if (down && right)
                {
                    setActiveArrow(ArrowDirection.DownRight);
                }
                else if (down && left)
                {
                    setActiveArrow(ArrowDirection.DownLeft);
                }
                else if (up)
                {
                    setActiveArrow(ArrowDirection.Up);
                }
                else if (down)
                {
                    setActiveArrow(ArrowDirection.Down);
                }
                else if (right)
                {
                    setActiveArrow(ArrowDirection.Right);
                }
                else if (left)
                {
                    setActiveArrow(ArrowDirection.Left);
                }
                break;
            case State.Inactive:
                break;
        }
    }
    // 8 normal arrow dirs

    public enum State
    {
        Idle,
        Arrow,
        Inactive
    }
    public void setState(State newState)
    {
        state = newState;
        switch (state)
        {
            case State.Idle:
                foreach (GameObject arrow in ArrowDictionary.Values)
                {
                    arrow.GetComponent<ArrowController>().SetActive(false);
                }
                // Get tag with name "ClockIcon"
                transform.Find("ClockIcon").gameObject.SetActive(true);
                break;
            case State.Arrow:
                transform.Find("ClockIcon").gameObject.SetActive(false);
                foreach (GameObject arrow in ArrowDictionary.Values)
                {
                    arrow.GetComponent<ArrowController>().SetActive(true);
                }
                ArrowDictionary[currentDirection].GetComponent<ArrowController>().SetFocused(true);
                break;
            case State.Inactive:
                foreach (GameObject arrow in ArrowDictionary.Values)
                {
                    arrow.GetComponent<ArrowController>().SetActive(false);
                }
                // Set the color of the clock icon to gray
                transform.Find("ClockIcon").gameObject.SetActive(true);
                transform.Find("ClockIcon").gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                break;
        }
    }
    public State getState()
    {
        return state;
    }
    public void setActiveArrow(ArrowDirection direction)
    {
        ArrowDictionary[currentDirection].GetComponent<ArrowController>().SetFocused(false);
        currentDirection = direction;
        ArrowDictionary[currentDirection].GetComponent<ArrowController>().SetFocused(true);
    }

    public ArrowDirection GetCurrentDirection()
    {
        return currentDirection;
    }

    public Vector2 GetArrowVector()
    {
        return ArrowController.GetDirectionVector(currentDirection);
    }
}
