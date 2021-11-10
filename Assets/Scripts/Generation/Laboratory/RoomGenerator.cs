using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] GameObject prefabFloor;
    [SerializeField] GameObject prefabCorridor;
    [SerializeField] GameObject prefabRoomDoor;
    [SerializeField] GameObject prefabDoorPanel;
    [SerializeField] GameObject prefabChair;
    [SerializeField] GameObject prefabTable;
    [SerializeField] GameObject prefabCabinetOne;
    [SerializeField] GameObject prefabCabinetTwo;
    [SerializeField] GameObject prefabLocker;
    [SerializeField] GameObject prefabParentItem;
    [SerializeField] GameObject prefabCompOffice;
    [SerializeField] GameObject prefabCompLab;
    [SerializeField] GameObject prefabCompSecurity;
    [SerializeField] GameObject prefabCompStorage;
    [SerializeField] GameObject prefabCompChaotic;
    [SerializeField] GameObject prefabCompTESTING;

    //Длина стороны квадратной комнаты
    public int size;
    //Количество рядов комнат
    public int mapW;
    //Количество столбцов комнат
    public int mapH;
    //Горизонтальный оффсет между комнатами для расположения корридора
    public int offsetW;
    //Вертикальный оффсет между комнатами для расположения корридора
    public int offsetH;

    //Переменные для генерации "хаотичных" комнат (пока не используется), минимальное и максимальное значение
    public int tableMinN;
    public int tableMaxN;
    public int chairMinN;
    public int chairMaxN;
    public int cabinetOneMinN;
    public int cabinetOneMaxN;
    public int cabinetTwoMinN;
    public int cabinetTwoMaxN;
    public int lockerMinN;
    public int lockerMaxN;

    //Размер сетки располагаемых предметов в "хаотичных" комнатах
    public int roomGridW = 10;
    public int roomGridH = 10;

    //Позиция комнаты выхода
    private int _exitX;
    private int _exitY;

    private GameObject[,] roomArray;
    private GameObject[,] corridorArrayHor;
    private GameObject[,] corridorArrayVert;
    private GameObject[,] roomDoorArrayUp;
    private GameObject[,] roomDoorArrayDown;
    private GameObject[,] roomDoorArrayLeft;
    private GameObject[,] roomDoorArrayRight;

    private GameObject[,] doorPanelArrayUp;
    private GameObject[,] doorPanelArrayDown;
    private GameObject[,] doorPanelArrayLeft;
    private GameObject[,] doorPanelArrayRight;

    private GameObject[,] corridorPanelArrayUp;
    private GameObject[,] corridorPanelArrayDown;
    private GameObject[,] corridorPanelArrayLeft;
    private GameObject[,] corridorPanelArrayRight;

    private List<GameObject> interierObjectArray;

    void Start()
    {
        roomArray = new GameObject[mapW, mapH];
        corridorArrayHor = new GameObject[mapW, mapH];
        corridorArrayVert = new GameObject[mapW, mapH];

        roomDoorArrayUp = new GameObject[mapW, mapH];
        roomDoorArrayDown = new GameObject[mapW, mapH];
        roomDoorArrayLeft = new GameObject[mapW, mapH];
        roomDoorArrayRight = new GameObject[mapW, mapH];

        doorPanelArrayUp = new GameObject[mapW, mapH];
        doorPanelArrayDown = new GameObject[mapW, mapH];
        doorPanelArrayLeft = new GameObject[mapW, mapH];
        doorPanelArrayRight = new GameObject[mapW, mapH];

        corridorPanelArrayUp = new GameObject[mapW, mapH];
        corridorPanelArrayDown = new GameObject[mapW, mapH];
        corridorPanelArrayLeft = new GameObject[mapW, mapH];
        corridorPanelArrayRight = new GameObject[mapW, mapH];

        interierObjectArray = new List<GameObject>();
        GenerateLaboratory();
    }


    void Update()
    {

    }
    public GameObject GetGameObjectFromArray(string array, int i, int j) //Нахождение массива дверей для присвоения панели принадлежности
    {
        switch (array)
        {
            case "roomDoorArrayUp":
                {
                    return roomDoorArrayUp[i, j];
                }
            case "roomDoorArrayDown":
                {
                    return roomDoorArrayDown[i, j];
                }
            case "roomDoorArrayLeft":
                {
                    return roomDoorArrayLeft[i, j];
                }
            case "roomDoorArrayRight":
                {
                    return roomDoorArrayRight[i, j];
                }
        }
        return null;
    }
    void GenerateLaboratory() //Генерация экземпляров комнат, композиций, корридоров между комнатами, дверей между комнатами и панелей открытия дверей
    {
        SetExit();
        for (int j = 0; j < mapH; j += 1)
            for (int i = 0; i < mapW; i += 1)
            {
                //Префаб пола и стен
                roomArray[i, j] = Instantiate(prefabFloor) as GameObject;
                roomArray[i, j].transform.position = new Vector3((-mapW / 2 * size + size / 2f) + i * size + i * offsetW
                    , 0, (-mapH / 2 * size + size / 2f) + j * size + j * offsetH);
                ChangeColor(roomArray[i, j]);
                if (i < mapW - 1)
                {
                    //Корридор
                    corridorArrayHor[i, j] = Instantiate(prefabCorridor) as GameObject;
                    corridorArrayHor[i, j].transform.position = new Vector3((-mapW / 2 * size + size + offsetW / 2f) + i * size + i * offsetW
                        , 0, (-mapH / 2 * size + size / 2f) + j * size + j * offsetH);
                    corridorArrayHor[i, j].transform.Rotate(new Vector3(0, 90, 0));
                    ChangeColor(corridorArrayHor[i, j]);
                }
                if (i != _exitX || j != _exitY)
                {
                    //Дверь
                    roomDoorArrayLeft[i, j] = Instantiate(prefabRoomDoor) as GameObject;
                    roomDoorArrayLeft[i, j].transform.position = new Vector3((-mapW / 2 * size + size - 0.25f) + i * size + i * offsetW
                        , 2f, (-mapH / 2 * size + size / 2f) + j * size + j * offsetH);
                }
                if (i < mapW - 1)
                {
                    //Панель в комнате
                    doorPanelArrayLeft[i, j] = Instantiate(prefabDoorPanel) as GameObject;
                    doorPanelArrayLeft[i, j].GetComponent<ReactiveTarget>().SetDoorPanel("roomDoorArrayLeft", i, j);
                    doorPanelArrayLeft[i, j].transform.position = new Vector3((-mapW / 2 * size + size - 1f) + i * size + i * offsetW
                        , 2f, (-mapH / 2 * size + size / 2f) + j * size + j * offsetH - 4f);
                    //Панель в корридоре
                    corridorPanelArrayLeft[i, j] = Instantiate(prefabDoorPanel) as GameObject;
                    corridorPanelArrayLeft[i, j].GetComponent<ReactiveTarget>().SetDoorPanel("roomDoorArrayLeft", i, j);
                    corridorPanelArrayLeft[i, j].transform.position = new Vector3((-mapW / 2 * size + size - 0.25f) + i * size + i * offsetW + offsetW / 4f
                        , 2f, (-mapH / 2 * size + size / 2f) + j * size + j * offsetH + offsetH / 2f - 0.05f);
                    corridorPanelArrayLeft[i, j].transform.Rotate(new Vector3(0, 90, 0));
                }
                if (i != _exitX || j != _exitY)
                {
                    //Дверь
                    roomDoorArrayRight[i, j] = Instantiate(prefabRoomDoor) as GameObject;
                    roomDoorArrayRight[i, j].transform.position = new Vector3((-mapW / 2 * size + 0.25f) + i * size + i * offsetW
                        , 2f, (-mapH / 2 * size + size / 2f) + j * size + j * offsetH);
                }
                if (i > 0)
                {
                    //Панель в комнате
                    doorPanelArrayRight[i, j] = Instantiate(prefabDoorPanel) as GameObject;
                    doorPanelArrayRight[i, j].GetComponent<ReactiveTarget>().SetDoorPanel("roomDoorArrayRight", i, j);
                    doorPanelArrayRight[i, j].transform.position = new Vector3((-mapW / 2 * size + 1f) + i * size + i * offsetW
                        , 2f, (-mapH / 2 * size + size / 2f) + j * size + j * offsetH + 4f);
                    //Панель в корридоре
                    corridorPanelArrayRight[i, j] = Instantiate(prefabDoorPanel) as GameObject;
                    corridorPanelArrayRight[i, j].GetComponent<ReactiveTarget>().SetDoorPanel("roomDoorArrayRight", i, j);
                    corridorPanelArrayRight[i, j].transform.position = new Vector3((-mapW / 2 * size + 0.25f) + i * size + i * offsetW - offsetW / 4f
                        , 2f, (-mapH / 2 * size + size / 2f) + j * size + j * offsetH - offsetH / 2f + 0.05f);
                    corridorPanelArrayRight[i, j].transform.Rotate(new Vector3(0, 90, 0));
                }
                if (j < mapH - 1)
                {
                    //Корридор
                    corridorArrayVert[i, j] = Instantiate(prefabCorridor) as GameObject;
                    corridorArrayVert[i, j].transform.position = new Vector3((-mapW / 2 * size + size / 2f) + i * size + i * offsetW
                        , 0, (-mapH / 2 * size + size + offsetH / 2f) + j * size + j * offsetH);
                    ChangeColor(corridorArrayVert[i, j]);
                }
                if (i != _exitX || j != _exitY)
                {
                    //Дверь
                    roomDoorArrayUp[i, j] = Instantiate(prefabRoomDoor) as GameObject;
                    roomDoorArrayUp[i, j].transform.position = new Vector3((-mapW / 2 * size + size / 2f) + i * size + i * offsetW
                        , 2f, (-mapH / 2 * size + size - 0.25f) + j * size + j * offsetH);
                    roomDoorArrayUp[i, j].transform.Rotate(new Vector3(0, 90, 0));
                }
                if (j < mapH - 1)
                {
                    //Панель в комнате
                    doorPanelArrayUp[i, j] = Instantiate(prefabDoorPanel) as GameObject;
                    doorPanelArrayUp[i, j].GetComponent<ReactiveTarget>().SetDoorPanel("roomDoorArrayUp", i, j);
                    doorPanelArrayUp[i, j].transform.position = new Vector3((-mapW / 2 * size + size / 2f + 4f) + i * size + i * offsetW
                        , 2f, (-mapH / 2 * size + size - 1f) + j * size + j * offsetH);
                    doorPanelArrayUp[i, j].transform.Rotate(new Vector3(0, 90, 0));
                    //Панель в корридоре
                    corridorPanelArrayUp[i, j] = Instantiate(prefabDoorPanel) as GameObject;
                    corridorPanelArrayUp[i, j].GetComponent<ReactiveTarget>().SetDoorPanel("roomDoorArrayUp", i, j);
                    corridorPanelArrayUp[i, j].transform.position = new Vector3((-mapW / 2 * size + size / 2f) + i * size + i * offsetW - offsetW / 2f + 0.05f
                        , 2f, (-mapH / 2 * size + size - 0.25f) + j * size + j * offsetH + offsetH / 4f);
                }
                if (i != _exitX || j != _exitY)
                {
                    //Дверь
                    roomDoorArrayDown[i, j] = Instantiate(prefabRoomDoor) as GameObject;
                    roomDoorArrayDown[i, j].transform.position = new Vector3((-mapW / 2 * size + size / 2f) + i * size + i * offsetW
                        , 2f, (-mapH / 2 * size + 0.25f) + j * size + j * offsetH);
                    roomDoorArrayDown[i, j].transform.Rotate(new Vector3(0, 90, 0));
                }
                if (j > 0)
                {
                    //Панель в комнате
                    doorPanelArrayDown[i, j] = Instantiate(prefabDoorPanel) as GameObject;
                    doorPanelArrayDown[i, j].GetComponent<ReactiveTarget>().SetDoorPanel("roomDoorArrayDown", i, j);
                    doorPanelArrayDown[i, j].transform.position = new Vector3((-mapW / 2 * size + size / 2f - 4f) + i * size + i * offsetW
                        , 2f, (-mapH / 2 * size + 1f) + j * size + j * offsetH);
                    doorPanelArrayDown[i, j].transform.Rotate(new Vector3(0, 90, 0));
                    //Панель в корридоре
                    corridorPanelArrayDown[i, j] = Instantiate(prefabDoorPanel) as GameObject;
                    corridorPanelArrayDown[i, j].GetComponent<ReactiveTarget>().SetDoorPanel("roomDoorArrayDown", i, j);
                    corridorPanelArrayDown[i, j].transform.position = new Vector3((-mapW / 2 * size + size / 2f) + i * size + i * offsetW + offsetW / 2f - 0.05f
                        , 2f, (-mapH / 2 * size + 0.25f) + j * size + j * offsetH - offsetH / 4f);
                }

                GameObject interier = Instantiate(ChooseInterierTemplate()) as GameObject;
                interier.transform.position = new Vector3((-mapW / 2 * size + size / 2f) + i * size + i * offsetW
                    , 0, (-mapH / 2 * size + size / 2f) + j * size + j * offsetH);
            }
        
    }
    void SetExit() //Установка позиции комнаты выхода, может быть только с одного из краев(кроме угловых комнат)
    {
        float posX;
        float posZ;
        Vector3 rotation = Vector3.zero;
        if (Random.Range(0,2) == 1)
        {
            _exitX = Random.Range(1, mapW);
            if (Random.Range(0, 2) == 1)
            {
                _exitY = 0;
                posZ = (-mapH / 2 * size + size / 2f) + _exitY * size + _exitY * offsetH - size / 2f - offsetH / 2f;
            }
            else
            {
                _exitY = mapH - 1;
                posZ = (-mapH / 2 * size + size / 2f) + _exitY * size + _exitY * offsetH + size / 2f + offsetH / 2f;
            }
            posX = (-mapW / 2 * size + size / 2f) + _exitX * size + _exitX * offsetW;
        }
        else
        {
            _exitY = Random.Range(1, mapH);
            rotation = new Vector3(0, 90, 0);
            if (Random.Range(0, 2) == 1)
            {
                _exitX = 0;
                posX = (-mapW / 2 * size + size / 2f) + _exitX * size + _exitX * offsetW - size / 2f - offsetW / 2f;
            }
            else
            {
                _exitX = mapW - 1;
                posX = (-mapW / 2 * size + size / 2f) + _exitX * size + _exitX * offsetW + size / 2f + offsetW / 2f;
            }
            posZ = (-mapH / 2 * size + size / 2f) + _exitY * size + _exitY * offsetH;
        }
        GameObject exitCorridor = Instantiate(prefabCorridor);
        exitCorridor.transform.position = new Vector3(posX, 0, posZ);
        exitCorridor.transform.Rotate(rotation);
    }
    void ChangeColor(GameObject redrawingObject) //Дебаг функция для перекрашивания экземляров префаба комнаты
    {
        redrawingObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        Renderer[] childRenderer = redrawingObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer ren in childRenderer)
        {
            ren.material.color = redrawingObject.GetComponent<Renderer>().material.color;
            //new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
    }
    GameObject ChooseInterierTemplate() //Выбор шаблона композиции при генерации
    {
        List<GameObject> interier = new List<GameObject>() { prefabCompOffice };
        //{ prefabCompChaotic, prefabCompLab, prefabCompOffice, prefabCompSecurity, prefabCompStorage };
        int indexer = Random.Range(0, interier.Count);
        return interier[indexer];
    }
    void FillRoomWithInterier(int i, int j) //Заполнение "хаотичной" комнаты объектами интерьера(не используется)
    {
        bool[,] interierGridIn = new bool[roomGridW, roomGridH];
        bool[,] interierGridOut = new bool[roomGridW, roomGridH];
        for (int n = 0; n < roomGridH; n += 1)
        {
            for (int m = 0; m < roomGridW; m += 1)
            {
                interierGridIn[m, n] = false;
                if (m == 0 || m == roomGridW - 1 || n == 0 || n == roomGridH - 1)
                {
                    interierGridIn[m, n] = true;
                }
            }
        }
        GameObject currentObject;
        //tables
        int objectN = Random.Range(tableMinN, tableMaxN);
        float objectX = (-mapW / 2 * size + size / 2f) + i * size + i * offsetW;
        float objectY = (-mapW / 2 * size + size / 2f) + j * size + j * offsetH;
        for (int n = 0; n < objectN; n++)
        {
            currentObject = PlaceObject(ref interierGridIn, out interierGridOut, objectX, objectY, 0.5f, prefabTable);
            interierObjectArray.Add(currentObject);
        }
        //chairs
        objectN = Random.Range(chairMinN, chairMaxN);

        for (int n = 0; n < objectN; n++)
        {
            currentObject = PlaceObject(ref interierGridIn, out interierGridOut, objectX, objectY, 0.5f, prefabChair);
            interierObjectArray.Add(currentObject);
        }
        //cabinets one
        objectN = Random.Range(cabinetOneMinN, cabinetOneMaxN);

        for (int n = 0; n < objectN; n++)
        {
            currentObject = PlaceObject(ref interierGridIn, out interierGridOut, objectX, objectY, 0.5f, prefabCabinetOne);
            interierObjectArray.Add(currentObject);
        }
        //cabinets two
        objectN = Random.Range(cabinetTwoMinN, cabinetTwoMaxN);

        for (int n = 0; n < objectN; n++)
        {
            currentObject = PlaceObject(ref interierGridIn, out interierGridOut, objectX, objectY, 0.5f, prefabCabinetTwo);
            interierObjectArray.Add(currentObject);
        }
        //lockers
        objectN = Random.Range(lockerMinN, lockerMaxN);

        for (int n = 0; n < objectN; n++)
        {
            currentObject = PlaceObject(ref interierGridIn, out interierGridOut, objectX, objectY, 0.5f, prefabLocker);
            interierObjectArray.Add(currentObject);
        }
    }

    GameObject PlaceObject(ref bool[,] interierArrayIn, out bool[,] interierArrayOut, float objectX, float objectY,float objectZ,GameObject prefabObject)
    {
        GameObject currentObject = Instantiate(prefabObject);
        float posY = 0;
        float posX = 0;
        float posZ = 0;
        int req = 0;
        do
        {
            req++;
            posX = Random.Range(0, roomGridW);
            posZ = Random.Range(0, roomGridH);
        } while (interierArrayIn[(int)posX,(int)posZ] && req < 1000);
        if (req < 1000)
        {
            interierArrayIn[(int)posX, (int)posZ] = true;

            posX = (posX - roomGridW / 2f) * (size / roomGridW);
            posZ = (posZ - roomGridH / 2f) * (size / roomGridH);
            currentObject.transform.position = new Vector3(objectX + posX, objectZ + posY, objectY + posZ);
            //currentObject.transform.Rotate(rotation);
            currentObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            if (prefabObject.Equals(prefabTable))
            {
                Vector3 itemPosition = currentObject.transform.position;
                itemPosition.y += 1f;
                GameObject item = Instantiate(prefabParentItem, itemPosition, Quaternion.identity);
            }
            Renderer[] childRenderer = currentObject.GetComponentsInChildren<Renderer>();
            Transform[] childTransform = currentObject.GetComponentsInChildren<Transform>();
            foreach (Renderer ren in childRenderer)
            {
                ren.material.color = currentObject.GetComponent<Renderer>().material.color;
            }
            foreach (Transform tran in childTransform)
            {
                //tran.Rotate(rotation);
            }
        }
        else
            currentObject = null;
        interierArrayOut = (bool[,])interierArrayIn.Clone();
        return currentObject;
    }
}
