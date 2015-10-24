using UnityEngine;
using System.Collections;

public class InfoField : GammaInput.Interactive {

	public string displayInfo;

	public override void OnPointerEnter (int inputIndex, GammaInput.BasePointerParameters pointerParams, GammaInput.BasePointer sender)
	{
		base.OnPointerEnter (inputIndex, pointerParams, sender);

		InteractionManager.instance.DisplayInfoField(this);
	}
}
