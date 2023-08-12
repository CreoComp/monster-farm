using Code.Logic.Buildings;
using Code.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectWorldCanvas : MonoBehaviour
{
    private Camera _camera;
    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI"))
            {
                var canvas = results[0].gameObject.transform.parent.GetComponent<WorldCanvas>();
                if (canvas != null)
                {
                    canvas.transform.parent.GetComponent<Construction>().SetCanvasActive();
                }
            }
        }

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.parent == null)
                return;


            var construction = hit.transform.parent.GetComponent<Construction>();

            if(construction != null)
            {
                construction.SetCanvasActive();
                return;
            }
        }


    }
}
