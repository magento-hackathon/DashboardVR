using UnityEngine;
using System.Collections;
using GammaInput;

public class NGuiInput: MonoBehaviour {

	void OnEnable()
	{
		BasePointer.OnPointerEnter += HandleOnPointerOver;
		BasePointer.OnPointerOver += HandleOnPointerOver;
		BasePointer.OnPointerExit += HandleOnPointerExit;
		BasePointer.OnPointerClicked += HandleOnPointerClicked;

		InputPointer.OnInputPointer += HandleOnInputkPointer;
		InputPointer.OnInputPointerUp += HandleOnInputPointerUp;
	}

	void OnDisable()
	{
		BasePointer.OnPointerEnter -= HandleOnPointerOver;
		BasePointer.OnPointerOver -= HandleOnPointerOver;
		BasePointer.OnPointerExit -= HandleOnPointerExit;
		BasePointer.OnPointerClicked -= HandleOnPointerClicked;
		
		InputPointer.OnInputPointer -= HandleOnInputkPointer;
		InputPointer.OnInputPointerUp -= HandleOnInputPointerUp;
	}

	void HandleOnPointerOver (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
	{
		pointerParams.target.SendMessage("OnHover", true, SendMessageOptions.DontRequireReceiver);
	}

	void HandleOnPointerExit (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
	{
		pointerParams.target.SendMessage("OnHover", false, SendMessageOptions.DontRequireReceiver);
	}

	void HandleOnInputkPointer (int inputIndex, InputPointerParameters pointerParams, InputPointer sender)
	{
		pointerParams.target.SendMessage("OnPress", true, SendMessageOptions.DontRequireReceiver);
	}

	void HandleOnInputPointerUp (int inputIndex, InputPointerParameters pointerParams, InputPointer sender)
	{
		pointerParams.target.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
	}

	void HandleOnPointerClicked (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
	{
		pointerParams.target.SendMessage("OnClick", null, SendMessageOptions.DontRequireReceiver);
	}
}
