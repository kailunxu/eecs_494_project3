using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody player;
    private float moveVelocity = 5;
    public bool haselement = true;
    public GameObject child;
    
    //public static int[, ] flags = new int[2 * SIZE, 2 * SIZE];
    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity += new Vector3(-moveVelocity, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity += new Vector3(moveVelocity, 0, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            velocity += new Vector3(0, moveVelocity, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity += new Vector3(0, -moveVelocity, 0);
        }
        player.velocity = velocity;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("generator"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (checkSameTile(transform.position, other.transform.position))
                {
                    if (!haselement)
                    {
                        pickFromGenerator(other);
                    }
                }
            }
        }
        if (other.CompareTag("cube"))
        {   
                /*
                if (checkSameTile(transform.position, other.transform.position))
                {
                    Debug.Log(other);
                    Instantiate(child, other.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
                }
                */

                
            if (checkSameTile(transform.position, other.transform.position))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Debug.Log("space");
                    if (haselement && !other.GetComponent<CubeController>().elementplaced)
                    {
                        place(other, child);
                        return;
                    }
                    if (!haselement && other.GetComponent<CubeController>().elementplaced)
                    {
                        //other.GetComponentInChildren<Transform>().position = transform.position + new Vector3(0, 1, 0);
                        pick(other);
                        return;
                    }
                }
           
                /*
                if (checkSameTile(transform.position, other.transform.position))
                {
                    Debug.Log(other);
                    Instantiate(child, other.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
                }
                */


            
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (!haselement && other.GetComponent<CubeController>().elementplaced) {
                        Rotate90(other.GetComponent<CubeController>().child.transform);
                        //other.GetComponentInChildren<Transform>().position = transform.position + new Vector3(0, 1, 0);
                        //GetComponent<PlayerMovement>().child.transform.RotateAround(other.transform.position, new Vector3(0, 0, 1), 90);
                    }
                }
            }
        }

    }
    private void place(Collider other, GameObject child)
    {
        other.GetComponent<CubeController>().child = child;
        other.GetComponent<CubeController>().child.transform.position = other.transform.position + new Vector3(0, 0, -1);
        other.GetComponent<CubeController>().child.transform.parent = other.transform;
        haselement = false;
        int currx = Mathf.RoundToInt(other.transform.position.x) + VictoryJudge.SIZE;
        int curry = Mathf.RoundToInt(other.transform.position.y) + VictoryJudge.SIZE;
        if (child.CompareTag("cross"))
        {
            VictoryJudge.flags[currx, curry] = VictoryJudge.ElementDirection.leftup;
        }
        if (child.CompareTag("straight"))
        {
            VictoryJudge.flags[currx, curry] = VictoryJudge.ElementDirection.vertical;
        }
        Debug.Log(VictoryJudge.flags[currx, curry]);
        other.GetComponent<CubeController>().elementplaced = true;
    }
    private void pick(Collider other)
    {
        GetComponent<PlayerMovement>().child = other.GetComponent<CubeController>().child;
        GetComponent<PlayerMovement>().child.transform.position = transform.position;
        GetComponent<PlayerMovement>().child.transform.parent = this.transform;
        haselement = true;
        int currx = Mathf.RoundToInt(other.transform.position.x) + VictoryJudge.SIZE;
        int curry = Mathf.RoundToInt(other.transform.position.y) + VictoryJudge.SIZE;
        VictoryJudge.flags[currx, curry] = VictoryJudge.ElementDirection.none;
        Debug.Log(VictoryJudge.flags[currx, curry]);
        other.GetComponent<CubeController>().elementplaced = false;
    }
    private void pickFromGenerator(Collider other)
    {
        GetComponent<PlayerMovement>().child = Instantiate(other.GetComponent<ElementGenerator>().child, transform.position, Quaternion.identity);

        GetComponent<PlayerMovement>().child.transform.parent = this.transform;
        haselement = true;
    }
    private bool checkSameTile(Vector3 player, Vector3 cube)
    {
        if (Mathf.Round(player.x) != cube.x)
        {
            return false;
        }
        if (Mathf.Round(player.y) != cube.y)
        {
            return false;
        }
        return true;
    }
    private void Rotate90(Transform trans)
    {
        trans.RotateAround(trans.position, new Vector3(0, 0, 1), 90);
        int currx = Mathf.RoundToInt(trans.position.x) + VictoryJudge.SIZE;
        int curry = Mathf.RoundToInt(trans.position.y) + VictoryJudge.SIZE;
        VictoryJudge.flags[currx, curry] = VictoryJudge.Rotate(VictoryJudge.flags[currx, curry]);
        Debug.Log(VictoryJudge.flags[currx, curry]);
    }
}
