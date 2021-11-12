using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] TMP_Text textMesh;

    [SerializeField] TMP_Text textMeshLeftBottom;
    [SerializeField] TMP_Text textMeshRightBottom;
    [SerializeField] TMP_Text textMeshRightUpper;
    [SerializeField] TMP_Text textMeshRightHand;
    [SerializeField] TMP_Text textMeshLeftHand;
    public bool canDraw;
    public bool canDrawInventory;
    public string drawingText;
    private GameObject _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Если курсор нацелен на активный предмет/объект, изменяется текст, иначе стирается
        if (canDraw)
        {
            DrawText();
        }
        else
        {
            textMesh.text = "";
        }
        if (canDrawInventory)
        {
            DrawInventoryText();
        }
        else if (!canDraw)
        {
            textMeshLeftBottom.text = "";
            textMeshRightBottom.text = "";
            textMeshRightUpper.text = "";
            textMeshLeftHand.text = "";
            textMeshRightHand.text = "";
        }
    }
    public void DrawInventoryText()
    {
        string leftBottomName = "Левый карман\n";
        string rightBottomName = "Правый карман\n";
        string rightUpperName = "Верхний карман\n";
        string leftHandName = "Левая рука\n";
        string rightHandName = "Правая рука\n";
        if (_player.GetComponent<PlayerBehavoir>().inventory[1].name != "")
            leftBottomName += _player.GetComponent<PlayerBehavoir>().inventory[1].name + "\n";
        else
            leftBottomName += "[Пусто]\n";

        if (_player.GetComponent<PlayerBehavoir>().inventory[2].name != "")
            rightBottomName += _player.GetComponent<PlayerBehavoir>().inventory[2].name + "\n";
        else
            rightBottomName += "[Пусто]\n";

        if (_player.GetComponent<PlayerBehavoir>().inventory[3].name != "")
            rightUpperName += _player.GetComponent<PlayerBehavoir>().inventory[3].name + "\n";
        else
            rightUpperName += "[Пусто]\n";

        //if (_player.GetComponent<PlayerBehavoir>().inventory[4].name != "")
            //leftHandName += _player.GetComponent<PlayerBehavoir>().inventory[4].name + "\n";
        //else
            //leftHandName += "[Пусто]\n";

        if (_player.GetComponent<PlayerBehavoir>().inventory[0].name != "")
            rightHandName += _player.GetComponent<PlayerBehavoir>().inventory[0].name + "\n";
        else
            rightHandName += "[Пусто]\n";
        textMeshLeftBottom.text = leftBottomName + "Взять предмет: [1]";
        textMeshRightBottom.text = rightBottomName + "Взять предмет: [2]";
        textMeshRightUpper.text = rightUpperName + "Взять предмет: [3]";
        textMeshLeftHand.text = leftHandName + "Взять предмет: [Q]";
        textMeshRightHand.text = rightHandName + "Взять предмет: [E]";
        textMeshLeftHand.text = "";
    }
    public void DrawText() //Изменение текста в TextMeshPro
    {
        textMesh.text = drawingText;
    }
}
