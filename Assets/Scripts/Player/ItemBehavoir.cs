using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;

public class Item 
{
    public string name { get; set; }
    public int id { get; set; }
    public int size { get; set; }//0 - small, 1 - medium, 2 - large
    public GameObject prefab { get; set; }
    public int capacityMax { get; set; }
    public int capacityCurrent { get; set; }
    public bool breakOnZeroCapacity { get; set; }
    public float healingPower { get; set; }
    public float boostingPower { get; set; }
    public float damage { get; set; }
    public Item(string item_name, int item_id,int item_size, GameObject item_object)
    {
        name = item_name;
        id = item_id;
        size = item_size;
        prefab = item_object;
        capacityMax = 1;
        capacityCurrent = 1;
        breakOnZeroCapacity = false;
        healingPower = 0;
        boostingPower = 0;
    }
    public void SetCapacity(int item_capacity_max,int item_capacity_current,bool is_breakable)
    {
        capacityMax = item_capacity_max;
        capacityCurrent = item_capacity_current;
        breakOnZeroCapacity = is_breakable;
    }
    public void MakeConsumable(float item_healing_power, float item_boosting_power, float item_damage)
    {
        healingPower = item_healing_power;
        boostingPower = item_boosting_power;
        damage = item_damage;
    }
}

public class ItemBehavoir : MonoBehaviour
{
    public string itemName;
    public int id;
    public int size;//0 - small, 1 - medium, 2 - large
    public int capacityMax;
    public int capacityCurrent;
    public bool breakOnZeroCapacity;
    public float healingPower;
    public float boostingPower;
    public float damage;

    void Start()
    {
        
    }
    void Update()
    {

    }
}