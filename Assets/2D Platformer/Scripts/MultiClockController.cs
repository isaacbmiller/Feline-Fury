using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArrowDirection = ArrowController.ArrowDirection;


public class MultiClockController : MonoBehaviour
{
    public int numArrows = 5;
    private State state;

    private Dictionary<ArrowDirection, GameObject> ArrowDictionary = new Dictionary<ArrowDirection, GameObject>();

    private ArrowDirection currentDirection;
    private MiniArrowController miniArrowController;

    private int currentArrow = 0;
    private List<ArrowDirection> chosenDirections = new List<ArrowDirection>();
    private float lastActivatedTime;

    private bool shouldLaunch = true;
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
        miniArrowController = GetComponentInChildren<MiniArrowController>();
        miniArrowController.maxArrows = numArrows;

        setState(State.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.PickingDirection:
                bool up = Input.GetAxisRaw("Vertical") > 0;
                bool down = Input.GetAxisRaw("Vertical") < 0;
                bool right = Input.GetAxis("Horizontal") > 0;
                bool left = Input.GetAxis("Horizontal") < 0;
                // Debug.Log("up: " + up + " down: " + down + " right: " + right + " left: " + left);
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

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    chosenDirections.Add(currentDirection);
                    currentArrow++;
                    updateMiniArrows();
                    if (currentArrow == numArrows)
                    {
                        lastActivatedTime = Time.time;
                        setState(State.Shooting);
                    }
                }
                break;
            case State.Shooting:

                break;
            case State.Inactive:
                if (lastActivatedTime + 1.5f < Time.time)
                {
                    // Debug.Log("Clock is now active");
                    setState(State.Idle);
                }
                break;
        }
    }
    // 8 normal arrow dirs

    public enum State
    {
        Idle,
        PickingDirection,
        Shooting,
        Inactive
    }

    public bool getShouldLaunch()
    {
        return shouldLaunch;
    }

    public void updateMiniArrows()
    {
        miniArrowController.updateArrows(this.chosenDirections);
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
                transform.Find("ClockIcon").gameObject.SetActive(true);
                transform.Find("ClockIcon").gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                chosenDirections.Clear();
                miniArrowController.updateArrows(chosenDirections);
                currentArrow = 0;
                shouldLaunch = true;
                break;
            case State.PickingDirection:
                transform.Find("ClockIcon").gameObject.SetActive(false);
                foreach (GameObject arrow in ArrowDictionary.Values)
                {
                    arrow.GetComponent<ArrowController>().SetActive(true);
                }
                ArrowDictionary[currentDirection].GetComponent<ArrowController>().SetFocused(true);
                miniArrowController.updateArrows(chosenDirections);
                break;
            case State.Shooting:
                // Debug.Log("Shooting");
                foreach (GameObject arrow in ArrowDictionary.Values)
                {
                    arrow.GetComponent<ArrowController>().SetActive(false);
                }
                break;
            case State.Inactive:
                foreach (GameObject arrow in ArrowDictionary.Values)
                {
                    arrow.GetComponent<ArrowController>().SetActive(false);
                }
                // Set the color of the clock icon to gray
                transform.Find("ClockIcon").gameObject.SetActive(true);
                transform.Find("ClockIcon").gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                shouldLaunch = false;
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
        // Create a sum vector of each arrow
        Vector2 sumVector = new Vector2(0, 0);
        foreach (ArrowDirection direction in chosenDirections)
        {
            sumVector += ArrowController.GetDirectionVector(direction);
        }
        // Scale based on the log of the number of arrows
        // sumVector.Normalize();
        // sumVector = sumVector;
        return sumVector;
    }
    public void ResetClock()
    {
        setState(State.Idle);
        
    }
}
