using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public List<Sprite> backgrounds;

	// Use this for initialization
	void Start () {
        int randomInt = Random.Range(0, backgrounds.Count);
        GetComponent<SpriteRenderer>().sprite = backgrounds[randomInt];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
