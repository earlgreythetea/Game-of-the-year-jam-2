using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    
    //Переменные для проверки на определенные свойства активных предметов/объектов
    public bool isCollectible;
    public bool isOpening;
    public bool isOpeningParent;
    public bool isDoorPanel;
    public bool opened;

    private Animator _animator;
    private ItemPlacer _placer;
    private GameObject _manager;
    private GameObject _player;
    //Для панелей двери
    private int _objectX;
    private int _objectY;
    private string _doorArray;
    
    //Доступен ли предмет к поднятию
    public bool canTake = false;

    void Start()
    {
        _placer = GetComponentInParent<ItemPlacer>();
        _player = GameObject.Find("Player");
        _manager = GameObject.Find("RoomGenerator");
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
        GameObject door = _manager.GetComponent<RoomGenerator>().GetGameObjectFromArray(_doorArray, _objectX, _objectY);
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
    public void CollectItem(GameObject destroyingObject) //Добавление предмета
    {
        ItemBehavoir item = gameObject.GetComponent<ItemBehavoir>();
        for (int i = 0; i < _player.GetComponent<PlayerBehavoir>().inventory.Length; i++)
        {
            if (_player.GetComponent<PlayerBehavoir>().inventory[i].name == "")
            {
                _player.GetComponent<PlayerBehavoir>().inventory[i].name = item.itemName;
                _player.GetComponent<PlayerBehavoir>().inventory[i].id = item.id;
                _player.GetComponent<PlayerBehavoir>().inventory[i].prefab = this.gameObject;
                _player.GetComponent<PlayerBehavoir>().inventory[i].size = item.size;
                _player.GetComponent<PlayerBehavoir>().inventory[i].capacityMax = item.capacityMax;
                _player.GetComponent<PlayerBehavoir>().inventory[i].capacityCurrent = item.capacityCurrent;
                _player.GetComponent<PlayerBehavoir>().inventory[i].breakOnZeroCapacity = item.breakOnZeroCapacity;
                _player.GetComponent<PlayerBehavoir>().inventory[i].healingPower = item.healingPower;
                _player.GetComponent<PlayerBehavoir>().inventory[i].boostingPower = item.boostingPower;
                _player.GetComponent<PlayerBehavoir>().inventory[i].damage = item.damage;
                Destroy(destroyingObject);
                i = _player.GetComponent<PlayerBehavoir>().inventory.Length;
                Debug.Log("Item collected!");
            }
        }
    }
}
