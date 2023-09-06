using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{

    public GameObject torch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("UseItem"))
        {
            Instantiate(torch, this.transform.position, Quaternion.identity);
        }
    }
}
