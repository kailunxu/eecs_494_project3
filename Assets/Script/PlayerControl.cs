using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Gamepad gp;
    Rigidbody rb;
    public GameObject child;
    public bool haselement = true;

    public float speed = 2;
    public float force = 10;
    public float carryForceMultiplier = 1.5f;

    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode holdKey = KeyCode.Space;
    public KeyCode rotate = KeyCode.R;

    //public PieceColor pieceColor;

    // Start is called before the first frame update
    void Start()
    {
    // gp = Gamepad.current;
    rb = GetComponent<Rigidbody>();
    }

    private void Update() {
    if (gp != null) {
        handleMovements(gp.leftStick.ReadValue());
    } else {
        handleMovements(getKeyboardAxis());
    }
    }
    Vector2 getKeyboardAxis() {
    Vector2 v = new Vector2(
        ((Input.GetKey(leftKey)) ? -1 : 0) + ((Input.GetKey(rightKey)) ? 1 : 0),
        ((Input.GetKey(downKey)) ? -1 : 0) + ((Input.GetKey(upKey)) ? 1 : 0)
    );
    if (v != Vector2.zero) v.Normalize();
    return v;
    }

    public float rotateTorque = 3f;
    float _prev_deltaZ = 0f;
    void handleMovements(Vector2 input) {
    rb.AddForce(force * input * Time.fixedDeltaTime);
    if (rb.velocity.magnitude > speed) {
        rb.velocity = rb.velocity.normalized * speed;
    }

    if (input.magnitude > .2f) {
        float targetDirection = directionOf(input);
    
        float _deltaZ = Mathf.Repeat((targetDirection - transform.rotation.eulerAngles.z) + 180, 360) - 180;
        // Debug.Log(_deltaZ);
        if (_prev_deltaZ * _deltaZ < 0) {
        rb.angularVelocity = Vector3.zero;
        // Debug.Log("stopped");
        } else {
        rb.AddTorque(_deltaZ * Mathf.Abs(_deltaZ) / 180 * Time.fixedDeltaTime * rotateTorque * Vector3.forward);
        }
        _prev_deltaZ = _deltaZ;
    } else {
        _prev_deltaZ = 0;
    }
    }

    static float directionOf(Vector2 v) {
    return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("generator"))
        {
            if (Input.GetKeyDown(holdKey))
            {
                if (checkSameTile(transform.position, other.transform.position))
                {
                    if (!haselement)
                    {
                        Debug.Log(other);
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
                if (Input.GetKeyDown(holdKey))
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



                if (Input.GetKeyDown(rotate))
                {
                    if (!haselement && other.GetComponent<CubeController>().elementplaced)
                    {
                        Rotate90(other.GetComponent<CubeController>().child.transform);
                        //other.GetComponentInChildren<Transform>().position = transform.position + new Vector3(0, 1, 0);
                        //GetComponent<PlayerControl>().child.transform.RotateAround(other.transform.position, new Vector3(0, 0, 1), 90);
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
        GetComponent<PlayerControl>().child = other.GetComponent<CubeController>().child;
        GetComponent<PlayerControl>().child.transform.position = transform.position;
        GetComponent<PlayerControl>().child.transform.parent = this.transform;
        haselement = true;
        int currx = Mathf.RoundToInt(other.transform.position.x) + VictoryJudge.SIZE;
        int curry = Mathf.RoundToInt(other.transform.position.y) + VictoryJudge.SIZE;
        VictoryJudge.flags[currx, curry] = VictoryJudge.ElementDirection.none;
        Debug.Log(VictoryJudge.flags[currx, curry]);
        other.GetComponent<CubeController>().elementplaced = false;
    }
    private void pickFromGenerator(Collider other)
    {
        GetComponent<PlayerControl>().child = Instantiate(other.GetComponent<ElementGenerator>().child, transform.position, Quaternion.identity);

        GetComponent<PlayerControl>().child.transform.parent = this.transform;
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
