using System.Collections;
using UnityEngine;
using GammaInput;

namespace GammaInput {
	public class BasePointerParameters : PointerParameters
	{		
		public BasePointerParameters(RaycastHit hit, int rayDepth, BasePointer pointer, bool behindGUI = false) : base(hit, rayDepth, pointer, behindGUI)
		{

		}
		
		public BasePointerParameters(Transform target, BasePointer pointer) : base(target, pointer)
		{

		}
		
		public BasePointerParameters(BasePointerParameters copyParameters) : this (copyParameters.raycastHit, copyParameters.rayDepth, copyParameters.pointer, copyParameters.behindGUI)
		{
			
		}
	}
}