using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public GameObject pipe;
    private int woodNum = 0, steelNum = 0;
    GameObject p;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(woodNum > 0 && steelNum > 0) {
            Instantiate(pipe);
            woodNum = 0;
            steelNum = 0;
            Debug.Log("new pipe");
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("player")) {
            p = other.gameObject;
        }
        if (other.CompareTag("wood"))
        {
            p.GetComponent<PlayerMovement>().haselement = false;
            Destroy(other.gameObject);
            woodNum++;
            Debug.Log("new wood");
        }
        if (other.CompareTag("steel"))
        {
            p.GetComponent<PlayerMovement>().haselement = false;
            Destroy(other.gameObject);
            steelNum++;
            Debug.Log("new steel");
        }
    }
}
