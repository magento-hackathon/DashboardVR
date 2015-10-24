using UnityEngine;
using GammaInput;
using System.Collections;

namespace GammaInput {
	public class InputPointerParameters : BasePointerParameters
	{
		public BaseInput inputHandler {get; protected set;}
		public InputParameters inputParameters {get; protected set;}
		public Ray inputRay;
		public bool inputTimedClick;

		public InputPointerParameters (RaycastHit hit, int rayDepth, BasePointer pointer, bool behindGUI = false) : base (hit, rayDepth, pointer, behindGUI)
		{
		}

		public InputPointerParameters(Transform target, BasePointer pointer) : base(target, pointer)
		{
		}

		public void SetInput(BaseInput input, InputParameters inputParameters, Ray inputRay)
		{
			this.inputHandler = input;
			this.inputParameters = inputParameters;
			this.inputRay = inputRay;
		}
	}
}