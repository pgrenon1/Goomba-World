using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {

    private Player _player;
    public Player player { get { if (_player == null) { _player = FindObjectOfType<Player>(); } return _player; } }
    public int side;

    // Use this for initialization
    void Start () {
		
	}

    private void OnMouseDown()
    {
        Debug.Log("here");
        player.Dash(side);
    }

    // Update is called once per frame
    void Update ()
    {

	}
}
