using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public enum RotationAxes
    {
        MouseXAndY = 0, 
        MouseX = 1, 
        MouseY = 2
    }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    private float _rotationX = 0;

    private GameObject _canvasUI;
    void Start()
    {
        _canvasUI = GameObject.Find("CanvasUI");
        Rigidbody body = GetComponent<Rigidbody>(); 
        if (body != null)
            body.freezeRotation = true;
    }

    void Update()
    {
        if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        } else if (axes == RotationAxes.MouseY)
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert; 
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
            float rotationY = transform.localEulerAngles.y; 
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
            if (_canvasUI.GetComponent<CanvasManager>() != null)
            {
                if (_rotationX > 65)
                {

                    _canvasUI.GetComponent<CanvasManager>().canDrawInventory = true;
                }
                else
                {
                    _canvasUI.GetComponent<CanvasManager>().canDrawInventory = false;
                }
            }
        } else
        {
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert; 
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert); 
            float delta = Input.GetAxis("Mouse X") * sensitivityHor; 
            float rotationY = transform.localEulerAngles.y + delta; 
            transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }

    }
}
