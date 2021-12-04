using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    
    CinemachineVirtualCamera camera;
    // Start is called before the first frame update

    public bool rotateCamera;

    Vector3 newRotation = new Vector3(50f, 0f, 0f);
    float rotationElapsedTime, rotationTime;

    Quaternion cameraInitRotation;

    void Start()
    {
        camera = GetComponent<CinemachineVirtualCamera>();
        cameraInitRotation = camera.transform.rotation;
        rotateCamera = false;
        rotationTime = 2f;
        rotationElapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateCamera)
        {
            camera.transform.eulerAngles = Vector3.Lerp(cameraInitRotation.eulerAngles, newRotation, Mathf.SmoothStep(0, 1, rotationElapsedTime / rotationTime));
            rotationElapsedTime += Time.deltaTime;
        }
        if (rotationElapsedTime > rotationTime)
        {
            rotationElapsedTime = 0f;
            rotateCamera = false;
        }
    }
}
