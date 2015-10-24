using UnityEngine;
using System.Collections;

[AddComponentMenu("")]
public class VectorArrayInterpolation : MonoBehaviour 
{
	public Vector3[] vectorData;
	public float lineResolution;
	
	public Transform animatedObject;
	
	private float parameter = 0;
	
	void Update( )
	{
		//Calculate a continously changing parameter in the interval 0..1
		parameter = Mathf.PingPong( Time.realtimeSinceStartup, 1 );
		
		//Calculate the node index corresponding to the current spline parameter
		int nodeIndex = Mathf.FloorToInt( (vectorData.Length-1) * parameter );
		
		//Calculate a spline segment's length, assuming that all segment have the same length
		float segmentLength = 1f / (vectorData.Length-1);
		
		//Calculate the current segment parameter
		float segmentParameter = (parameter - (nodeIndex * segmentLength)) / segmentLength;
		
		//Create a new Hermite interpolator
		SplineInterpolator splineInterpolator = new HermiteInterpolator( );
		
		//Calculate the position on the spline and assign it to the transform-component of the animated object
		Vector3 positionOnSpline = splineInterpolator.InterpolateVector( segmentParameter, nodeIndex, false, vectorData, 0 );
		
		animatedObject.transform.position = positionOnSpline;
	}
	
	//Draw the spline in the scene view
	void OnDrawGizmos( )
	{
		SplineInterpolator splineInterpolator = new HermiteInterpolator( );
		
		float invertedLineRes = 1f/lineResolution;
		
		for( int i = 0; i < vectorData.Length; i++ )
		{
			for( float parameter = 0; parameter <= 1-invertedLineRes; parameter += invertedLineRes )
			{
				Vector3 position1 = splineInterpolator.InterpolateVector( parameter, i, false, vectorData, 0 );
				Vector3 position2 = splineInterpolator.InterpolateVector( parameter + invertedLineRes, i, false, vectorData, 0 );
				
				Gizmos.DrawLine( position1, position2 );
			}
		}
	}
}
