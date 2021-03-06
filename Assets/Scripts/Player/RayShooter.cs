using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
    [SerializeField] Canvas targetCanvas;
    [SerializeField] Animator _animator;
    public float raycastDistance = 2f;

    private Camera _camera;
    private ReactiveTarget target;
    private GameObject hitObject;
    private Ray ray;

    void Start()
    {
        _camera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        AudioListener.volume = 0.5f;

    }
    IEnumerator ActionEnding(float seconds) //Размещение происходит с небольшой задержкой, чтобы в _placements появились все записи
    {
        yield return new WaitForSeconds(seconds);
        _animator.SetBool("IsActionPerforming", false);
    }
    void Update()
    {
        
        Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        ray = _camera.ScreenPointToRay(point);
        RaycastHit[] hits = Physics.RaycastAll(ray, raycastDistance);
        //Передача информации о том, какую фразу отображать при нацеливании на активный предмет/объект
        if (hits.Length > 0)
        {
            bool gotDraw = false;
            foreach (RaycastHit hit in hits)
            {
                hitObject = hit.transform.gameObject;
                target = hitObject.GetComponent<ReactiveTarget>();
                if (target != null)
                {
                    if (hit.distance <= raycastDistance)
                    {
                        targetCanvas.GetComponent<CanvasManager>().canDraw = false;
                        if ((target.isOpening || target.isOpeningParent) && !target.opened)
                        {
                            gotDraw = true;
                            targetCanvas.GetComponent<CanvasManager>().canDraw = true;
                            targetCanvas.GetComponent<CanvasManager>().drawingText = "Open";
                        }
                        if (target.isCollectible && target.canTake)
                        {
                            gotDraw = true;
                            targetCanvas.GetComponent<CanvasManager>().canDraw = true;
                            targetCanvas.GetComponent<CanvasManager>().drawingText = "Grab Item";
                        }
                        if (target.isDoorPanel)
                        {
                            gotDraw = true;
                            targetCanvas.GetComponent<CanvasManager>().canDraw = true;
                            targetCanvas.GetComponent<CanvasManager>().drawingText = "Use";
                        }
                    }
                    else
                        targetCanvas.GetComponent<CanvasManager>().canDraw = false;
                }
                else if (!gotDraw)
                    targetCanvas.GetComponent<CanvasManager>().canDraw = false;
            }
        }
        else
            targetCanvas.GetComponent<CanvasManager>().canDraw = false;
        //Использование предметов/объектов
        if (Input.GetMouseButtonDown(0))
        {
            foreach (RaycastHit hit in hits)
            {
                hitObject = hit.transform.gameObject;
                target = hitObject.GetComponent<ReactiveTarget>();
                if (target != null)
                {
                    if (hit.distance <= raycastDistance)
                    {
                        if (target.isCollectible && target.canTake)
                        {
                            target.CollectItem(hitObject.gameObject);
                            _animator.SetBool("IsActionPerforming", true);
                            StartCoroutine(ActionEnding(0.25f));

                        }
                        if ((target.isOpening || target.isOpeningParent) && !target.opened)
                        {
                            target.SendMessage("OpenObject");
                            _animator.SetBool("IsActionPerforming", true);
                            StartCoroutine(ActionEnding(0.25f));
                        }
                        if (target.isDoorPanel)
                        {
                            target.SendMessage("UseDoorPanel");
                            _animator.SetBool("IsActionPerforming", true);
                            StartCoroutine(ActionEnding(0.25f));
                        }
                    }
                }
            }
        }
    }
}   
