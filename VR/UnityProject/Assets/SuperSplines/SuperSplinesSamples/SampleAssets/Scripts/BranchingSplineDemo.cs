using UnityEngine;
using System.Collections;

//This scrip shows how to use the BranchingSpline class of the package.
//Branching splines can be defined by registring some splines in the BranchingSpline's spline array.
//SplineNodes that are shared among two or more splines will act as branching points / junctions.

[AddComponentMenu("")]
public class BranchingSplineDemo : MonoBehaviour 
{
	//A reference to the branching spline we're working with
	public BranchingSpline bSpline;
	
	//A BranchingSplineParameter stores our current position on the net of branching splines
	public BranchingSplineParameter bParam = new BranchingSplineParameter( );
	
	void Update( ) 
	{
		if( Input.GetKey( KeyCode.UpArrow ) )
		{
			//In order to correctly handle position changes on the branching spline, we can't "jump" around the spline by simply adding 
			//an offset to bParam.parameter.
			//Instead we need to call the bSpline.Advance method with bParam as a parameter and an offset in game units that will be 
			//added to the BranchingSplineParameter. 
			//Also we have to provide a delegate that will decide which paths to use. It will be called once a junction (shared SplineNode)
			//has been passed.
			bSpline.Advance( bParam, Time.deltaTime * 10f, JunctionController );
		}
		
		//Do the same as above for a negative offset
		if( Input.GetKey( KeyCode.DownArrow ) )
			bSpline.Advance( bParam, -Time.deltaTime * 10f, JunctionController );
		
		transform.position = bSpline.GetPosition( bParam );
	}
	
	//This is our "path decision function". It is called by the bSpline.Advance method once a junction has been passed and we need to decide
	//which path we want to use next.
	//The parameter currentParameter can be used as reference that we will base our decision on.
	//The parameter possiblePaths is a list of BranchingSplinePaths that can be taken. Please note that this list might contain some splines
	//twice. This happens when we hit a junction where a spline can be followed in two directions (e.g. a crossroads).
	//You can query the path's direction using the corresponding field: BranchingSplinePath.direction.
	BranchingSplinePath JunctionController( BranchingSplineParameter currentParameter, System.Collections.Generic.List<BranchingSplinePath> possiblePaths )
	{
		//We just take a random path in this example...
		int randomIndex = (int)(Random.value*possiblePaths.Count);
		
		return possiblePaths[randomIndex];
	}
}
