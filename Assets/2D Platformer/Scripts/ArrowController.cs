using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public ArrowDirection direction;
    // Start is called before the first frame update
    void Start()
    {
        // Determine arrow direction based on name
        switch (gameObject.name)
        {
            case "ArrowUp":
                direction = ArrowDirection.Up;
                break;
            case "ArrowDown":
                direction = ArrowDirection.Down;
                break;
            case "ArrowLeft":
                direction = ArrowDirection.Left;
                break;
            case "ArrowRight":
                direction = ArrowDirection.Right;
                break;
            case "ArrowUpLeft":
                direction = ArrowDirection.UpLeft;
                break;
            case "ArrowUpRight":
                direction = ArrowDirection.UpRight;
                break;
            case "ArrowDownLeft":
                direction = ArrowDirection.DownLeft;
                break;
            case "ArrowDownRight":
                direction = ArrowDirection.DownRight;
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFocused(bool focused)
    {
        if (focused)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void setDirection(ArrowDirection direction)
    {
        this.direction = direction;
        int angle = directionToAngle(direction);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public int directionToAngle(ArrowDirection direction)
    {
        switch (direction)
        {
            case ArrowDirection.Up:
                return 0;
            case ArrowDirection.Down:
                return 180;
            case ArrowDirection.Left:
                return 90;
            case ArrowDirection.Right:
                return 270;
            case ArrowDirection.UpLeft:
                return 45;
            case ArrowDirection.UpRight:
                return 315;
            case ArrowDirection.DownLeft:
                return 135;
            case ArrowDirection.DownRight:
                return 225;
            default:
                Debug.LogError("direction not set ArrowController: " + gameObject.name);
                return 0;
        }
    }

    public void SetActive(bool active)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = active;
    }

    public enum ArrowDirection
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
        None


    }

    // send a vector in the direction of the arrow
    public static Vector2 GetDirectionVector(ArrowDirection direction)
    {
        switch (direction)
        {
            case ArrowDirection.Up:
                return Vector2.up;
            case ArrowDirection.Down:
                return Vector2.down;
            case ArrowDirection.Left:
                return Vector2.left;
            case ArrowDirection.Right:
                return Vector2.right;
            case ArrowDirection.UpLeft:
                return new Vector2(-1, 1).normalized;
            case ArrowDirection.UpRight:
                return new Vector2(1, 1).normalized;
            case ArrowDirection.DownLeft:
                return new Vector2(-1, -1).normalized;
            case ArrowDirection.DownRight:
                return new Vector2(1, -1).normalized;
            default:
                return Vector2.zero;
        }
    }

}
