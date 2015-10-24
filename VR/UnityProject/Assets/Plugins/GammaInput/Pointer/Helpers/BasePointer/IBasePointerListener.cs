using UnityEngine;

namespace GammaInput {
// A Target for BasicPointer interaction should implement this interface to ensure to receive events
	public interface IBasePointerListener
	{
		void OnPointerEnter(int inputIndex, BasePointerParameters pointerParams, BasePointer sender);
		void OnPointerOver(int inputIndex, BasePointerParameters pointerParams, BasePointer sender);
		void OnPointerClicked(int inputIndex, BasePointerParameters pointerParams, BasePointer sender);
		void OnPointerExit(int inputIndex, BasePointerParameters pointerParams, BasePointer sender);
	}
}