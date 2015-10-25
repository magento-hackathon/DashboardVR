using UnityEngine;
using System.Collections;

public class InteractionManager : MonoBehaviour {

	public static InteractionManager instance {get; private set;}

	public ColloseumRoot root;

	public TextMesh infoDisplay;

	void Awake()
	{
		instance = this;
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

#if UNITY_EDITOR
	void Update()
	{
		if(Input.GetMouseButton(0)){
			if(root.headup){
				root.AnimateDown();
			}else{
				root.AnimateUp();
			}
		}
	}
#endif
}
