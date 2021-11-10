using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    GameObject manager;
    //Переменные для проверки на определенные свойства активных предметов/объектов
    public bool isCollectible;
    public bool isOpening;
    public bool isOpeningParent;
    public bool isDoorPanel;
    public bool opened;
    private Animator _animator;
    private ItemPlacer _placer;

    //Для панелей двери
    private int _objectX;
    private int _objectY;
    private string _doorArray;
    //Доступен ли предмет к поднятию
    public bool canTake = false;

    void Start()
    {
        _placer = GetComponentInParent<ItemPlacer>();
        manager = GameObject.Find("RoomGenerator");
        if (isOpening)
            _animator = GetComponent<Animator>();
        if (isOpeningParent)
            _animator = GetComponentInParent<Animator>();
    }

    void Update()
    {
        
    }
    public void SetDoorPanel(string array, int i, int j) //Добавление к панели двери координат объекта в сетке, которую она должна открыть (и название сетки)
    {
        _doorArray = array;
        _objectX = i;
        _objectY = j;
    }
    void UseDoorPanel() //Открытие двери
    {
        GameObject door = manager.GetComponent<RoomGenerator>().GetGameObjectFromArray(_doorArray, _objectX, _objectY);
        door.GetComponent<DoorBehavoir>().OpenDoor();
    }
    void OpenObject() //Открытие объекта
    {
        if (!opened)
        {
            opened = true;
            _animator.SetBool("opened", true);
        }
        if (_placer != null)
        {
            if (_placer.gameObject.GetComponentInParent<CompositionRender>() != null)
            {
                _placer.gameObject.GetComponentInParent<CompositionRender>().MakeItemsObtainable(_placer.gameObject);
                //Сквозь объект дверцы можно пройти
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }
    void CollectItem() //Добавление предмета
    {
        Debug.Log("Item collected!");
    }
}
