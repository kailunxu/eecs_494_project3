using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public GameObject pipe, angled, spawnPlace;
    private int woodNum = 0, steelNum = 0, straightNum = 0;
    GameObject p;
    // Start is called before the first frame update
    void Start()
    {

    }

    void place(GameObject other, GameObject child) {
        other.GetComponent<CubeController>().child = child;
        other.GetComponent<CubeController>().child.transform.position = other.transform.position + new Vector3(0, 0, -1);
        //other.GetComponent<CubeController>().child.transform.parent = other.transform;
        //GetComponent<PlayerMovement>().child.GetComponent<ItemManagement>().holding = false;
        p.GetComponent<PlayerMovement>().haselement = false;
        other.GetComponent<CubeController>().elementplaced = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(straightNum >= 4) {
            //p.GetComponent<PlayerMovement>().child = Instantiate(angled, p.transform.position, Quaternion.identity);
            //p.GetComponent<PlayerMovement>().child.transform.parent = p.transform;
            //p.GetComponent<PlayerMovement>().haselement = true;
            place(spawnPlace, Instantiate(angled));
            straightNum = 0;
            Debug.Log("new angled");
        }
        if(woodNum >= 2 && steelNum >= 2) {

            place(spawnPlace, Instantiate(pipe));

            //p.GetComponent<PlayerMovement>().child = Instantiate(pipe, p.transform.position, Quaternion.identity);
            //p.GetComponent<PlayerMovement>().child.transform.parent = p.transform;
            //p.GetComponent<PlayerMovement>().haselement = true;
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
        if(other.CompareTag("straight")) {
            //if(!other.GetComponent<ItemManagement>().holding) {
                p.GetComponent<PlayerMovement>().haselement = false;
                Destroy(other.gameObject);
                straightNum++;
                Debug.Log("new straight" + straightNum);
            //}
        }
        if (other.CompareTag("wood"))
        {
            //if(!other.GetComponent<ItemManagement>().holding) {
                p.GetComponent<PlayerMovement>().haselement = false;
                Destroy(other.gameObject);
                woodNum++;
                Debug.Log("new wood" + woodNum);
            //}
        }
        if (other.CompareTag("steel"))
        {
            //if(!other.GetComponent<ItemManagement>().holding) {
                p.GetComponent<PlayerMovement>().haselement = false;
                Destroy(other.gameObject);
                steelNum++;
                Debug.Log("new steel" + steelNum);
            //}
        }
    }
}
