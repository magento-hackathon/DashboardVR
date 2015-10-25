using UnityEngine;
using System.Collections;

public class InfoField : GammaInput.Interactive {

	public enum InfoType {
		Quantity,
		Money
	}

	public string displayInfo;
	public InfoType infoType;

	public override void OnPointerEnter (int inputIndex, GammaInput.BasePointerParameters pointerParams, GammaInput.BasePointer sender)
	{
		base.OnPointerEnter (inputIndex, pointerParams, sender);

		InteractionManager.instance.DisplayInfoField(this);
	}

	public override void OnPointerClicked (int inputIndex, GammaInput.BasePointerParameters pointerParams, GammaInput.BasePointer sender)
	{
		base.OnPointerClicked (inputIndex, pointerParams, sender);

		InteractionManager.instance.MoveToType(this);
	}
}
