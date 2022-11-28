using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    float camSpeed = 25, zoomSpeed = 50, originalHeight, camHeight, maxHeight = 20, minHeight = 3;
    // Start is called before the first frame update
    void Start()
    {
        originalHeight = transform.position.y;
        camHeight = originalHeight;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.right + Vector3.forward) * camSpeed * Time.deltaTime * Input.GetAxis("Vertical");
        transform.position += (Vector3.right - Vector3.forward) * camSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        camHeight -= zoomSpeed * Time.deltaTime * Input.mouseScrollDelta.y;
        camHeight = Mathf.Clamp(camHeight, minHeight, maxHeight);

        if (Input.GetMouseButtonDown(2))
        {
            camHeight = originalHeight;
        }
        
        transform.position = new Vector3(transform.position.x, camHeight, transform.position.z);
    }
}
