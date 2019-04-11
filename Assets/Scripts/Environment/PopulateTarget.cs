using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateTarget : MonoBehaviour {

	public GameObject[] prefab; // This is our prefab object that will be exposed in the inspector
	public int numberToCreate; // number of objects to create. Exposed in inspector
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Populate()
	{
		GameObject newObj; // Create GameObject instance

		for (int i = 0; i < prefab.Length; i++)
		{
			 // Create new instances of our prefab until we've created as many as we specified
			newObj = (GameObject)Instantiate(prefab[i], transform);
		}
	}
}
