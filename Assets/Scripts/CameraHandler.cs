using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    float camSpeed = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.right + Vector3.forward) * camSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        transform.position += (Vector3.right - Vector3.forward) * camSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
    }
}
