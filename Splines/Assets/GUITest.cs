using UnityEngine;
using System.Collections;

public class GUITest : MonoBehaviour 
{

    public ItemPlacer itemPlacer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, 100, 100), "Road Type");

        if (GUI.Button(new Rect(20, 40, 80, 20), "Straight"))
        {
            itemPlacer.SetMode("Straight!");
        }

        if (GUI.Button(new Rect(20, 70, 80, 20), "Curve"))
        {
            itemPlacer.SetMode("Curved!");
        }
    }
}
