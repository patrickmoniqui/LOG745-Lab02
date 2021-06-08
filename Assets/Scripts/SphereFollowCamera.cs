using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFollowCamera : MonoBehaviour
{
    public GameObject Camera;
    public float offsetY = 0f;
    public float offsetX = 0f;
    public float offsetZ = -12f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.transform.position;
        pos.x += offsetX;
        pos.y += offsetY;
        pos.z += offsetZ;
        this.transform.position = pos;
    }
}
