using UnityEngine;
using System.Collections;

public class InteractionManager : MonoBehaviour {

	public static InteractionManager instance {get; private set;}

	public ColloseumRoot root;

	public TextMesh infoDisplay;

	public GammaInput.BasePointer editorPointer;
	public GammaInput.BasePointer devicePointer;

	void Awake()
	{
		instance = this;

#if UNITY_EDITOR
		editorPointer.enabled = true;
		devicePointer.enabled = false;
#else
		editorPointer.enabled = false;
		devicePointer.enabled = true;
#endif
	}

	public void DisplayInfoField(InfoField infoField)
	{
		infoDisplay.text = infoField.displayInfo;
	}

	public void MoveToType(InfoField infoField)
	{
		if(infoField.infoType == InfoField.InfoType.Money){
			root.AnimateUp();
		}else{
			root.AnimateDown();
		}
	}
}
