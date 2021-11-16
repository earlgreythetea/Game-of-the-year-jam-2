using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOptimization : MonoBehaviour
{
    //Радиус, внутри которого активируются все GameObject
    private float _activationDistance;

    public bool canOptimize = false;
    public bool preventOptimization = false;
    private bool _canOptimizeCointainedItems = false;
    private GameObject _player;
    //Проверка, является ли композиция доступной для активации/деактивации
    private bool _isActive = true;
    private List<GameObject> _placedItems;

    void Start()
    {
        _player = GameObject.Find("Player");
        _activationDistance = _player.GetComponent<PlayerBehavoir>().activationDistance;
    }
    public void StartOptimizingWithItems(List<GameObject> placedItems)
    {
        canOptimize = true;
        _canOptimizeCointainedItems = true;
        _placedItems = placedItems;
    }

    void Update()
    {
        //Оптимизация - при расстоянии между игроком и композицией больше чем activationDistance, все GameObject композиции деактивируются и наоборот
        //Происходит, если в композиции уже размещены все предметы
        if (canOptimize && !preventOptimization)
        {
            if (Vector3.Distance(gameObject.transform.position, _player.gameObject.transform.position) < _activationDistance)
            {
                if (!_isActive)
                {
                    _isActive = true;
                    gameObject.SetActive(true);
                    
                    gameObject.SetActiveRecursively(true);
                    if (_canOptimizeCointainedItems)
                        foreach (GameObject item in _placedItems)
                        {
                            if (item != null)
                            {
                                item.SetActive(true);
                            }
                        }
                    gameObject.GetComponent<Renderer>().enabled = true;
                }
            }
            else
            {
                if (_isActive)
                {
                    _isActive = false;
                    gameObject.SetActiveRecursively(false);
                    if (_canOptimizeCointainedItems)
                        foreach (GameObject item in _placedItems)
                        {
                            if (item != null)
                            {
                                item.SetActive(false);
                            }
                        }
                    gameObject.GetComponent<Renderer>().enabled = false; //.SetActive(false);
                    gameObject.SetActive(true);
                }
            }
        }
    }
}
