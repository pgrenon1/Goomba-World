using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Goomba : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Stop()
    {
        transform.DOKill();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var shockwave = other.GetComponent<Shockwave>();

        if (shockwave != null)
        {
            Destroy(gameObject);
        }
    }
}
