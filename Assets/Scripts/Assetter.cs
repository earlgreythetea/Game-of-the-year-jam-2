using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assetter : MonoBehaviour
{
    [SerializeField] public GameObject itemCardRed;
    [SerializeField] public GameObject itemCardBlue;
    [SerializeField] public GameObject itemCardGreen;
    [SerializeField] public GameObject itemCardYellow;

    [SerializeField] public GameObject itemCrowbar;
    [SerializeField] public GameObject itemPicklock;
    [SerializeField] public GameObject itemCoolingCapsule;
    [SerializeField] public GameObject itemHandgun;
    [SerializeField] public GameObject itemFirstAidKit;
    [SerializeField] public GameObject itemEnergyDrink;
    [SerializeField] public GameObject itemGeigerCounter;
    [SerializeField] public GameObject itemFlashlight;
    [SerializeField] public GameObject itemSpareParts;

    private GameObject[] itemsCard;

    public GameObject[] GetItemsCard()
    {
        return itemsCard;
    }
    void Start()
    {
        itemsCard = new GameObject[] { itemCardRed , itemCardBlue, itemCardGreen, itemCardYellow };
    }
}
