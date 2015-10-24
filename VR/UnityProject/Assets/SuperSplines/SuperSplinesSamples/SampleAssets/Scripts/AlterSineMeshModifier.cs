using UnityEngine;
using System.Collections;

[AddComponentMenu("")]
public class AlterSineMeshModifier : MonoBehaviour 
{
	public SplineSineScaleModifier meshModifier;
	
	void Update () {
		meshModifier.offset += 2 * Time.deltaTime;
		
		meshModifier.sinMultiplicator = Mathf.Sin( Time.time ) * 0.75f;
		meshModifier.sinMultiplicator = meshModifier.sinMultiplicator * meshModifier.sinMultiplicator + 0.5f;
	}
}
