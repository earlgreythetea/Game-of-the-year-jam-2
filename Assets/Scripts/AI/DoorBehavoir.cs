using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavoir : MonoBehaviour
{
    
    //Максимальная дистанция при открытии двери
    public float maxOffsetY = -3f;
    //Скорость открытия/закрытия двери
    public float doorSpeed = 1f;
    //Таймер, после которого дверь закроется (в сек)
    public float doorTimerMax = 6f;

    private float _offsetY = 0;
    private float _doorTimer = 0;
    private bool _isOpen = false;
    private Vector3 _initialPosition;

    void Start()
    {
        //Начальная позиция двери
        _initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    public void OpenDoor()
    {
        _isOpen = true;
        _doorTimer = doorTimerMax;
    }
    public void CloseDoor()
    {
        _isOpen = false;
    }

    void Update()
    {
        //Если дверь открыта, позиция по Y сдвинется вниз до maxOffsetY
        if (_isOpen)
        {
            if (_offsetY > maxOffsetY)
            {
                _offsetY -= doorSpeed * Time.deltaTime;
                transform.position = new Vector3(_initialPosition.x, _initialPosition.y + _offsetY, _initialPosition.z);
            }
            if (_doorTimer > 0)
            {
                _doorTimer -= 1 * Time.deltaTime;
            }
            else
            {
                CloseDoor();
            }
        }
        else
        {
            if (_offsetY < 0)
            {
                _offsetY += doorSpeed * Time.deltaTime;
                transform.position = new Vector3(_initialPosition.x, _initialPosition.y + _offsetY, _initialPosition.z);
            }
        }
    }
}
