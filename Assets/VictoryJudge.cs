using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryJudge : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject start;
    public GameObject end;
    private int startposx;
    private int startposy;
    private int endposx;
    private int endposy;

    public const int SIZE = 10;
    public static ElementDirection[,] flags = new ElementDirection[2 * SIZE, 2 * SIZE];
    public enum ElementDirection { none, horizontal, vertical, leftup, leftdown, rightup, rightdown, end};
    private enum Direction {left, right, up, down};
    void Start()
    {
        startposx = Mathf.RoundToInt(start.transform.position.x) + 1 + SIZE;
        startposy = Mathf.RoundToInt(start.transform.position.y) + SIZE;
        endposx = Mathf.RoundToInt(end.transform.position.x) + SIZE;
        endposy = Mathf.RoundToInt(end.transform.position.y) + SIZE;
        for (int i = 0; i < 2 * SIZE; ++i)
        {
            for (int j = 0; j < 2 * SIZE; ++j)
            {
                flags[i, j] = ElementDirection.none;
            }
        }
        Debug.Log("start: " + startposx + " " + startposy);
        Debug.Log("end: " + endposx + " " + endposy);
        flags[endposx, endposy] = ElementDirection.end;
    }

    // Update is called once per frame
    void Update()
    {
        int currx = startposx;
        int curry = startposy;
        Direction prev = Direction.left;
        bool success = false;
        while (flags[currx, curry] != ElementDirection.none) {
            Debug.Log(currx + " " + curry);
            if (currx == endposx && curry == endposy)
            {
                success = true;
                break;
            }
            if (prev == Direction.left)
            {
                if (flags[currx, curry] == ElementDirection.horizontal)
                {
                    currx += 1;
                    continue;
                }
                if (flags[currx, curry] == ElementDirection.leftdown)
                {
                    prev = Direction.up;
                    curry -= 1;
                    continue;
                }
                if (flags[currx, curry] == ElementDirection.leftup)
                {
                    prev = Direction.down;
                    curry += 1;
                    continue;
                }
                break;
            }
            if (prev == Direction.right)
            {
                if (flags[currx, curry] == ElementDirection.horizontal)
                {
                    currx -= 1;
                    continue;
                }
                if (flags[currx, curry] == ElementDirection.rightdown)
                {
                    prev = Direction.up;
                    curry -= 1;
                    continue;
                }
                if (flags[currx, curry] == ElementDirection.rightup)
                {
                    prev = Direction.down;
                    curry += 1;
                    continue;
                }
                break;
            }
            if (prev == Direction.up)
            {
                if (flags[currx, curry] == ElementDirection.vertical)
                {
                    curry -= 1;
                    continue;
                }
                if (flags[currx, curry] == ElementDirection.rightup)
                {
                    prev = Direction.left;
                    currx += 1;
                    continue;
                }
                if (flags[currx, curry] == ElementDirection.leftup)
                {
                    prev = Direction.right;
                    currx -= 1;
                    continue;
                }
                break;
            }
            if (prev == Direction.down)
            {
                if (flags[currx, curry] == ElementDirection.vertical)
                {
                    curry -= 1;
                    continue;
                }
                if (flags[currx, curry] == ElementDirection.rightdown)
                {
                    prev = Direction.left;
                    currx += 1;
                    continue;
                }
                if (flags[currx, curry] == ElementDirection.leftdown)
                {
                    prev = Direction.right;
                    currx -= 1;
                    continue;
                }
                break;
            }
        }
        if (success == true)
        {
            Debug.Log("success");
        }
    }
    public static ElementDirection Rotate(ElementDirection ed)
    {
        switch (ed) {
            case ElementDirection.horizontal:
                {
                    return ElementDirection.vertical;
                }
            case ElementDirection.vertical:
                {
                    return ElementDirection.horizontal;
                }
            case ElementDirection.leftup:
                {
                    return ElementDirection.leftdown;
                }
            case ElementDirection.leftdown:
                {
                    return ElementDirection.rightdown;
                }
            case ElementDirection.rightdown:
                {
                    return ElementDirection.rightup;
                }
            case ElementDirection.rightup:
                {
                    return ElementDirection.leftup;
                }
            case ElementDirection.none:
                {
                    return ElementDirection.none;
                }
        }
        return ElementDirection.none;
    }
}
