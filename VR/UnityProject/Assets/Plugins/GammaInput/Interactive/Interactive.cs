using UnityEngine;
using System.Collections;

namespace GammaInput {
	public abstract class Interactive : MonoBehaviour, IInputPointerListener {

#if UNITY_EDITOR
		[SerializeField]
		private bool debugMode;
#else
		private bool debugMode = false;
#endif

		#region IBasePointerListener implementation

		public virtual void OnPointerEnter (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
		{
			if(debugMode)
				Debug.Log(string.Format("OnPointerEnter: {0}", pointerParams));
		}

		public virtual void OnPointerOver (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
		{

		}
		public virtual void OnPointerClicked (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
		{
			if(debugMode)
				Debug.Log(string.Format("OnPointerClicked: {0}", pointerParams));
		}

		public virtual void OnPointerExit (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
		{
			if(debugMode)
				Debug.Log(string.Format("OnPointerExit: {0}", pointerParams));
		}

		#endregion

		#region IInputPointerListener implementation

		public virtual void OnInputPointerDown (int inputIndex, InputPointerParameters pointerParams, InputPointer sender)
		{
			if(debugMode)
				Debug.Log(string.Format("OnInputPointerDown: {0}", pointerParams));
		}

		public virtual void OnInputPointer (int inputIndex, InputPointerParameters pointerParams, InputPointer sender)
		{

		}

		public virtual void OnInputPointerUp (int inputIndex, InputPointerParameters pointerParams, InputPointer sender)
		{
			if(debugMode)
				Debug.Log(string.Format("OnInputPointerUp: {0}", pointerParams));
		}

		public virtual void OnInputPointerUpExternal (int inputIndex, InputPointerParameters pointerParams, InputPointer sender)
		{
			if(debugMode)
				Debug.Log(string.Format("OnInputPointerUpExternal: {0}", pointerParams));
		}

		#endregion
	}
}
