using UnityEngine;

//This class animates a gameobject along the spline at a specific speed. 
[AddComponentMenu("SuperSplines/Animation/Regular Animator")]
public class SplineAnimator : MonoBehaviour
{
	public Spline spline;
	
	public WrapMode wrapMode = WrapMode.Clamp;
	
	public float speed = 1f;
	public float offSet = 0f;
	
	public float passedTime = 0f;
	
	void Update( ) 
	{
		passedTime += Time.deltaTime * speed;
		
		float clampedParam = WrapValue( passedTime + offSet, 0f, 1f, wrapMode );
		
		transform.position = spline.GetPositionOnSpline( clampedParam );
		transform.rotation = spline.GetOrientationOnSpline( clampedParam );
	}
	
	private float WrapValue( float v, float start, float end, WrapMode wMode )
	{
		switch( wMode )
		{
		case WrapMode.Clamp:
		case WrapMode.ClampForever:
			return Mathf.Clamp( v, start, end );
		case WrapMode.Default:
		case WrapMode.Loop:
			return Mathf.Repeat( v, end - start ) + start;
		case WrapMode.PingPong:
			return Mathf.PingPong( v, end - start ) + start;
		default:
			return v;
		}
	}
}
