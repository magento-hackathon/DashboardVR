using UnityEngine;
using System.Collections;

public class ClockFingerController : MonoBehaviour {

	private float speed = 10.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {



		transform.Rotate( new Vector3( 0f, Time.deltaTime * speed, 0f ) );
	}
}
