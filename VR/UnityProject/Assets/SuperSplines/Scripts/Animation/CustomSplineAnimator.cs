using UnityEngine;
using System;

//This class animates a gameobject along the spline at a specific speed. 
[AddComponentMenu("SuperSplines/Animation/Regular Animator")]
public class CustomSplineAnimator : MonoBehaviour
{
	public Spline spline;
	
	public float animationTime = 5f;

	private float startTime;

	public bool animationPlaying {get; private set;}

	public Action onFinishCallback;

	void Start()
	{
		startTime = - animationTime*2;
	}

	void Update( ) 
	{
		if(startTime + animationTime > Time.time){
			transform.position = spline.GetPositionOnSpline(Mathf.Clamp01((Time.time - startTime)/ animationTime));
		}else{
			if(animationPlaying){
				animationPlaying = false;

				if(onFinishCallback != null)
					onFinishCallback();
			}
		}
	}
	
	public void StartAnimation(float ahead = 0)
	{
		startTime = Time.time - ahead;
		animationPlaying = true;
	}

	public void StopAnimation()
	{
		transform.position = spline.GetPositionOnSpline(0);
		startTime = - animationTime*2;
	}
}
