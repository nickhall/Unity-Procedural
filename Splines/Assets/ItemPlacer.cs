using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemPlacer : MonoBehaviour
{
    public List<Vector3> ClickPoints = new List<Vector3>();
    GameObject mouseCube;

    void Start()
    {
        mouseCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mouseCube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

	void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        RaycastHit worldHit = new RaycastHit();
        bool hitSuccess = false;
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.tag == "GameWorld")
                {
                    mouseCube.transform.position = hit.point;
                    worldHit = hit;
                    hitSuccess = true;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && hitSuccess)
        {
            ClickPoints.Add(worldHit.point);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = worldHit.point;
            cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
	}
}
