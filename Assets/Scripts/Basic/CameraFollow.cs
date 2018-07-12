using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public Vector3 deltaPosition;
    public Transform cam;
    public Vector3 localDeltaPosition;

    public bool rx;
    public bool ry;
    public bool rz;

    public float delta;

    public bool snapPosition;

    // Use this for initialization
    void Awake()
    {

        if (cam == null)
            cam = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (rx || ry || rz)
        {
            Vector3 angles = transform.eulerAngles;
            Vector3 deltaRotation = Quaternion.FromToRotation(cam.forward, transform.forward).eulerAngles;
            if (deltaRotation.x > 180f) deltaRotation.x -= 360f;
            if (deltaRotation.y > 180f) deltaRotation.y -= 360f;
            if (deltaRotation.z > 180f) deltaRotation.z -= 360f;

         //   Debug.Log("deltaRotation: " + deltaRotation);

            if (rx && Mathf.Abs(deltaRotation.x) > delta) angles.x = cam.eulerAngles.x;
            if (ry && Mathf.Abs(deltaRotation.y) > delta) angles.y = cam.eulerAngles.y;
            if (rz && Mathf.Abs(deltaRotation.z) > delta) angles.z = cam.eulerAngles.z;


          //  Debug.Log("angles: " + angles);
            transform.eulerAngles = angles;
        }

        if (snapPosition)
            transform.position = cam.position + localDeltaPosition;

    }
}
