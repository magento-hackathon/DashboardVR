using UnityEngine;
using System.Collections;

public class InteractionManager : MonoBehaviour {

	public static InteractionManager instance {get; private set;}
	public TextMesh infoDisplay;

	void Awake()
	{
		instance = this;
	}

	public void DisplayInfoField(InfoField infoField)
	{
		infoDisplay.text = infoField.displayInfo;
	}
}
