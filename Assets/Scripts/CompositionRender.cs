using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement //Класс для записи позиции помещения предметов в контейнеры и доп параметры
{
    public GameObject Parent { get; set; } //Родительский GameObject 
    public Vector3 Position { get; set; } //Локальный вектор для размещения
    public bool IsFree { get; set; } //Возможно ли взаимодействие с предметом(используется для Raycast, false если контейнер закрыт)
    public int Size { get; set; } //Максимальный размер предмета, который можно поместить на плейсмент (0 - малый, 1 - средний, 2 - большой)
    public Placement(int size, GameObject parent, Vector3 position, bool isFree)
    {
        this.Size = size;
        this.Parent = parent;
        this.Position = position;
        this.IsFree = isFree;
    }
}
public class CompositionRender : MonoBehaviour
{
    [SerializeField] GameObject prefabItem;


    //Задержка перед освобождением предметов в контейнере(в сек)
    public float freeAlarmTimer = 0.5f;

    

    private List<Placement> _placements = new List<Placement>();
    private List<GameObject> _placedItems = new List<GameObject>();
    private List<GameObject> _placedParents;
    private bool _itemsGenerated = false;
    //Переменные для метода "освобождения" предмета в контейнере (в силу применения RaycastAll)
    private float _freeAlarm = 0f;

    private GameObject _freeParent;
    private Assetter _assetter;
    void Start()
    {
        _assetter = GameObject.Find("Collector").GetComponent<Assetter>();
        //Находим GameObject игрока для определения центральной координаты для оптимизации комнаты

        //Размещение предметов внутри композиции
        StartCoroutine(ItemsPlacer());
    }
    IEnumerator ItemsPlacer() //Размещение происходит с небольшой задержкой, чтобы в _placements появились все записи
    {
        yield return new WaitForSeconds(.1f);
        PlaceItemsOnPlacements();
    }

    void Update()
    {
        //Освобождение предметов происходит через (0.5 сек) для предотвращения одновременного открытия контейнера и поднятия предмета
        if (_freeAlarm > 0f)
        {
            _freeAlarm -= 1f * Time.deltaTime;
        }
        else if (_freeParent != null)
        {
            FreeItems();
            _freeParent = null;
        }
        if (_itemsGenerated)
        {
            if (GetComponent<RenderOptimization>() != null)
            {
                GetComponent<RenderOptimization>().StartOptimizingWithItems(_placedItems);
            }
        }
    }
    void SetActiveChildren(bool state) //Метод не работает, в данный момент не используется
    {
        Transform[] allChildren = gameObject.GetComponentsInChildren<Transform>();
        List<GameObject> childObjects = new List<GameObject>();
        foreach (Transform child in allChildren)
        {
            childObjects.Add(child.gameObject);
        }
        foreach (GameObject child in childObjects)
        {
            child.SetActive(state);
            if (state)
                Debug.Log("state: " + state);
        }
    }
    public void AddPlacementVector(int size, Vector3 vector, GameObject parent, bool isFree) //Добавление нового объекта класса Placement в локальную коллекцию
    {
        _placements.Add(new Placement(size, parent, vector, isFree));
    }
    void FreeItems() //Происходит обход по всем предметам в композиции, все предметы связанные с одним контейнером становятся доступными для использования
    {
        for (int i = 0; i < _placedItems.Count; i++)
        {
            if (_placedParents[i].Equals(_freeParent))
            {
                if (_placedItems[i] != null)
                    if (_placedItems[i].GetComponent<ReactiveTarget>() != null)
                    {
                        _placedItems[i].GetComponent<ReactiveTarget>().canTake = true;
                        //Debug.Log("Freed an object!");
                    }
            }
        }
    }
    public void MakeItemsObtainable (GameObject parent) //Задание задержки перед освобождением предметов в контейнере
    {
        _freeAlarm = freeAlarmTimer;
        _freeParent = parent;
    }
    void PlaceItemsOnPlacements()
    {
        _placedItems = new List<GameObject>();
        _placedParents = new List<GameObject>();
        for (int i = 0; i < _placements.Count; i++)
        {
            GameObject item = Instantiate(ChooseItem(_placements[i]));
            Vector3 mainVector = _placements[i].Position;
            //В вектор обавляется высота предмета для ровного расположения
            mainVector.y += item.GetComponent<Collider>().bounds.max.y;
            item.transform.position = mainVector;
            item.transform.Rotate(0, _placements[i].Parent.transform.eulerAngles.y, 0);//TEST
            item.GetComponent<ReactiveTarget>().canTake = _placements[i].IsFree;

            //В два листа добавляются ссылки на созданный GameObject и его родитель-контейнер (для случаев, когда нужно освободить предмет)
            _placedItems.Add(item);
            _placedParents.Add(_placements[i].Parent);
        }
        _itemsGenerated = true;
    }
    GameObject ChooseItem(Placement placement)
    {
        GameObject item = _assetter.itemSpareParts;
        //GameObject item = _assetter.GetItemsCard()[Random.Range(0, _assetter.GetItemsCard().Length)];
        return item;
    }
}
