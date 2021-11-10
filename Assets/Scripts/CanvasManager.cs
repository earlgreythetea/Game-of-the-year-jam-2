using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] TMP_Text textMesh;
    public bool canDraw;
    public string drawingText;
    
    // Start is called before the first frame update
    void Start()
    {
         
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
    }
    public void DrawText() //Изменение текста в TextMeshPro
    {
        textMesh.text = drawingText;
    }
}
