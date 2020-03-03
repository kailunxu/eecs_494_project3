using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool elementplaced = false;
    public GameObject child;
    //Subscription<Place> place;
    void Start()
    {
        //place = EventBus.Subscribe<Place>(_elementPlaced);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void _elementPlaced(GameObject gameobject) {
        elementplaced = true;
        child = GameObject.Instantiate(gameobject, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
    }
    public void _elementRemoved() {
        elementplaced = false;
        Destroy(child);
    }
}
