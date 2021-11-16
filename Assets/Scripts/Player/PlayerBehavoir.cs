using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavoir : MonoBehaviour
{
    public float activationDistance;
    [SerializeField] Animator _animator;
    public Item[] inventory = new Item[4];

    void Start()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            inventory[i] = new Item("", -1, 0, null);
        }
    }


    void Update()
    {
        if (inventory[0].name != "")
        {
            _animator.SetBool("HasItemInHand", true);
        }
        else
        {
            _animator.SetBool("HasItemInHand", false);
        }
    }
}
