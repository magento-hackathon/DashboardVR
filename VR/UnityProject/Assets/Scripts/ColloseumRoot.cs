using UnityEngine;
using System.Collections;

public class ColloseumRoot : MonoBehaviour {

	public Vector3 upPosition;
	public Vector3 upRotation;

	public Vector3 downPosition;
	public Vector3 downRotation;

	public float animationSpeed;

	public bool headup;
	private float animate = 1;
	private Vector3 startPosition;
	private Quaternion startRotation;

	void Awake()
	{
		startPosition = upPosition;
		startRotation = Quaternion.Euler(upRotation);
	}

	public void AnimateDown()
	{
		headup = false;
		animate = 0;
		startPosition = transform.position;
		startRotation = transform.rotation;
	}

	public void AnimateUp()
	{
		headup = true;
		animate = 0;
		startPosition = transform.position;
		startRotation = transform.rotation;
	}

	void Update()
	{
		if(animate >= 1){
			return;
		}

		animate = Mathf.Clamp01(animate + animationSpeed*Time.deltaTime);

		if(headup){
			transform.position = Vector3.Lerp(startPosition, upPosition, animate);
			transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(upRotation), animate);
		}else{
			transform.position = Vector3.Lerp(startPosition, downPosition, animate);
			transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(downRotation), animate);
		}
	}
}
