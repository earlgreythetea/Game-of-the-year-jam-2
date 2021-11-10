using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    //0 - малый объект, 1 - средний объект, 2 - крупный объект
    public int[] placementSizes;
    //Доступен ли предмет для использования изначально
    public bool[] placementAvailabilities;
    //Вектор размещения предмета
    public Vector3[] placementVectors;
    //Трансформ для размещения координат x и z 
    [SerializeField] Transform placementTransform;

    void Start()
    {
        AddItemPlacement();
    }

    void Update()
    {
        
    }
    void AddItemPlacement() //Добавление координат размещения предметов в 
    {
        CompositionRender parentRenderer = gameObject.GetComponentInParent<CompositionRender>();
        for (int i = 0; i < placementVectors.Length; i++)
        {
            placementVectors[i] += new Vector3(placementTransform.position.x, 0, placementTransform.position.z);
            //Подсчет координаты высоты (Y) предмета считается от высшей точки Collider контейнера 
            placementVectors[i].y += placementTransform.gameObject.GetComponent<Collider>().bounds.max.y;
            parentRenderer.AddPlacementVector(placementSizes[i], placementVectors[i],gameObject, placementAvailabilities[i]);
        }

    }
}
