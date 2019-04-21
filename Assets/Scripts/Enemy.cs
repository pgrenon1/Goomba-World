using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour {

    private int RotationSide;
    private int[] sides = { -1, 1 };

	// Use this for initialization
	void Start () {
        RotationSide = sides[Random.Range(0, sides.Length)];
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward * RotationSide, 0.3f);
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
