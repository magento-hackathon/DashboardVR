using UnityEngine;
using System;
using System.Collections;

namespace GammaInput {
	public class PointerParameters
	{
		public Transform target {get; protected set;}

		public int combinedDepth {get; protected set;}

		public RaycastHit raycastHit {get; protected set;}
		public int rayDepth {get; protected set;}

		public BasePointer pointer {get; protected set;}

		public bool behindGUI {get; protected set;}

		public DateTime time {get; protected set;}

		public PointerParameters(Transform target, BasePointer pointer) : this(pointer)
		{
			this.target = target;
		}
		
		public PointerParameters(RaycastHit hit, int rayDepth, BasePointer pointer, bool behindGUI = false) : this(pointer)
		{
			this.target = hit.collider.transform;
			this.raycastHit = hit;
			this.rayDepth = rayDepth;

			this.behindGUI = behindGUI;
		}

		private PointerParameters(BasePointer pointer)
		{
			this.pointer = pointer;
			this.time = DateTime.Now;
		}

		public override string ToString ()
		{
			return string.Format ("[PointerParameters: target={0}, raycastHit={1}, rayDepth={2}, pointer={3}, behindGUI={4}]", target, raycastHit, rayDepth, pointer, behindGUI);
		}
	}
}
