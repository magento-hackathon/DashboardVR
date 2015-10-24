using UnityEngine;
using System.Collections;

[AddComponentMenu("")]
public class AlterTwistMeshModifier : MonoBehaviour 
{
	public SplineTwistModifier meshModifier;
	
	void Update () {
		meshModifier.twistOffset += 0.1f * Time.deltaTime;
	}
}
