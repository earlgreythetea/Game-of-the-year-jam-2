using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanelBehavoir : MonoBehaviour
{
    [SerializeField] Texture denyTexture;
    [SerializeField] Texture acceptTexture;
    private Texture _initialTexture; 
    private int _panelType = 0; //0 - free open, 1 - red, 2 - blue, 3 - green, 4 - yellow, 5 - gold
    public static Material mat2;
    public void SetPanel(string type, bool randomize)
    {
        string trueType;
        string[] types = new string[] { "Red", "Blue", "Green" ,"Yellow"};
        if (randomize)
        {
            trueType = types[Random.Range(0,types.Length)];
        }
        else
        {
            trueType = type;
        }
        switch (trueType)
        {
            case "Red":
                {
                    _panelType = 1;
                    _initialTexture = Resources.Load<Texture>("Textures/Panels/paneltexred");
                    break;
                }
            case "Blue":
                {
                    _panelType = 2;
                    _initialTexture = Resources.Load<Texture>("Textures/Panels/paneltexblue");

                    break;
                }
            case "Green":
                {
                    _panelType = 3;
                    _initialTexture = Resources.Load<Texture>("Textures/Panels/paneltexgreen");
                    break;
                }
            case "Yellow":
                {
                    _panelType = 4;
                    _initialTexture = Resources.Load<Texture>("Textures/Panels/paneltexyellow");
                    break;
                }
            case "Gold":
                {
                    _panelType = 5;
                    break;
                }
            case "Free":
            default:
                {
                    _panelType = 0;
                    _initialTexture = Resources.Load<Texture>("Textures/Panels/paneltexwhite");
                    break;
                }
        }
    } 
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.mainTexture = _initialTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
