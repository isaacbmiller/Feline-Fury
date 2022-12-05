using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArrowDirection = ArrowController.ArrowDirection;

public class MiniArrowController : MonoBehaviour
{
    private List<ArrowDirection> chosenDirections;
    private List<GameObject> miniArrowContainers;
    private int currentArrow = 0;

    public int maxArrows = 3;

    private State state;
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        // Get all children with the tag "MiniArrow" and add them to miniArrowContainers
        miniArrowContainers = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.tag == "MiniArrow")
            {
                miniArrowContainers.Add(child.gameObject);
            }
        }
        miniArrowContainers.Sort((a, b) => a.name.CompareTo(b.name));

        // Set all mini arrows to inactive
        foreach (GameObject arrow in miniArrowContainers)
        {
            arrow.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void updateArrows(List<ArrowDirection> chosenDirections)
    {
        this.chosenDirections = chosenDirections;
        // Set all mini arrows to inactive
        foreach (GameObject arrow in miniArrowContainers)
        {
            arrow.SetActive(false);
            // Get arrow's child that has an arrow controller
            arrow.GetComponentInChildren<ArrowController>().SetActive(false);
        }
        // Set the first maxArrows mini arrows to active
        for (int i = 0; i < maxArrows; i++)
        {
            miniArrowContainers[i].SetActive(true);
        }
        // Set the mini arrows to the correct direction
        for (int i = 0; i < chosenDirections.Count; i++)
        {
            miniArrowContainers[i].GetComponentInChildren<ArrowController>().setDirection(chosenDirections[i]);
            miniArrowContainers[i].GetComponentInChildren<ArrowController>().SetActive(true);
        }
    }
    public void setState(State state)
    {
        this.state = state;
        switch (state)
        {
            case State.Idle:
                break;
            case State.Active:
                for (int i = 0; i < maxArrows; i++)
                {
                    miniArrowContainers[i].SetActive(true);
                }
                for (int i = 0; i < chosenDirections.Count; i++)
                {
                    miniArrowContainers[i].GetComponent<ArrowController>().direction = chosenDirections[i];
                }
                break;
            case State.Finished:
                foreach (GameObject arrow in miniArrowContainers)
                {
                    arrow.SetActive(false);
                }
                break;
        }
    }
    public enum State
    {
        Active,
        Idle,
        Finished,
    }

}
