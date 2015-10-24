using UnityEngine;
using System.Collections;


namespace GammaInput {
// A Target for InputPointer interaction should implement this interface to ensure to receive events
	public interface IInputPointerListener : IBasePointerListener {

		void OnInputPointerDown(int inputIndex, InputPointerParameters pointerParams, InputPointer sender);
		void OnInputPointer(int inputIndex, InputPointerParameters pointerParams, InputPointer sender);
		void OnInputPointerUp(int inputIndex, InputPointerParameters pointerParams, InputPointer sender);
		void OnInputPointerUpExternal(int inputIndex, InputPointerParameters pointerParams, InputPointer sender);
	}
}
