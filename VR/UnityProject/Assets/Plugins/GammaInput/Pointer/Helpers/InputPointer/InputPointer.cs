using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GammaInput {
	public abstract class InputPointer : BasePointer
	{
		public delegate void InputPointerEventHandler(int inputIndex, InputPointerParameters pointerParams, InputPointer sender);

		public static event InputPointerEventHandler OnInputPointerDown;
		public static event InputPointerEventHandler OnInputPointer;
		public static event InputPointerEventHandler OnInputPointerUp;
		public static event InputPointerEventHandler OnInputPointerUpExternal;

		public delegate void InputPointerUpdateEventHandler(int inputIndex, Ray inputRay, InputPointer sender);
		public event InputPointerUpdateEventHandler OnInputRayUpdate;

		protected delegate void PointerInputDelegate(int inputIndex, InputParameters inputParams, BaseInput input, int raydepth, bool behindGui);
		protected PointerInputDelegate pointerInputDownDelegate;
		protected PointerInputDelegate pointerInputDelegate;
		protected PointerInputDelegate pointerInputUpDelegate;
		protected PointerInputDelegate pointerInputTimeClickDelegate;

		protected new InputPointer eventReceiver {
			get{
				return base.eventReceiver as InputPointer;
			}
		}

		protected override void SetupEventDelegates ()
		{
			base.SetupEventDelegates ();
			
			this.pointerInputDownDelegate = eventReceiver.PointerInputDown;
			this.pointerInputDelegate = eventReceiver.PointerInput;
			this.pointerInputUpDelegate = eventReceiver.PointerInputUp;
			this.pointerInputTimeClickDelegate = eventReceiver.PointerInputTimeClick;
		}

		protected virtual void PointerInputDown(int inputIndex, InputParameters inputParams, BaseInput input, int raydepth, bool behindGui)
		{
			if(pointerInputDownDelegate != null)
				pointerInputDownDelegate(inputIndex, inputParams, input, raydepth, behindGui);
		}

		protected virtual void PointerInput(int inputIndex, InputParameters inputParams, BaseInput input, int raydepth, bool behindGui)
		{
			if(pointerInputDelegate != null)
				pointerInputDelegate(inputIndex, inputParams, input, raydepth, behindGui);
		}

		protected virtual void PointerInputUp(int inputIndex, InputParameters inputParams, BaseInput input, int raydepth, bool behindGui)
		{
			if(pointerInputUpDelegate != null)
				pointerInputUpDelegate(inputIndex, inputParams, input, raydepth, behindGui);
		}

		protected virtual void PointerInputTimeClick(int inputIndex, InputParameters inputParams, BaseInput input, int raydepth, bool behindGui)
		{
			if(pointerInputTimeClickDelegate != null)
				pointerInputTimeClickDelegate(inputIndex, inputParams, input, raydepth, behindGui);
		}

		#region Selected management
		public Dictionary<int, List<PointerParameters>> selectedTarget = new Dictionary<int, List<PointerParameters>>();
		protected bool HasSelected(int inputIndex)
		{
			if(selectedTarget.ContainsKey(inputIndex)){
				return selectedTarget[inputIndex].Count > 0;
			}else{
				return false;
			}
		}
		
		protected bool IsSelected(int inputIndex, RaycastHit hit)
		{
			PointerParameters blah;
			return IsSelected(inputIndex, hit, out blah);
		}
		
		protected bool IsSelected(int inputIndex, RaycastHit hit, out PointerParameters enterParameters)
		{
			if(selectedTarget.ContainsKey(inputIndex)){
				foreach(var pp in selectedTarget[inputIndex]){
					if(pp.target == hit.collider.transform){
						enterParameters = pp;
						return true;
					}
				}
			}
			enterParameters = null;
			return false;
		}
		
		protected void AddSelected(int inputIndex, PointerParameters newParameters)
		{
			if(!selectedTarget.ContainsKey(inputIndex)){
				selectedTarget.Add(inputIndex, new List<PointerParameters>());
			}
			selectedTarget[inputIndex].Add(newParameters);
		}

		protected void RemoveSelected(int inputIndex, RaycastHit hit)
		{
			if(selectedTarget.ContainsKey(inputIndex)){
				for(int i = 0; i < selectedTarget[inputIndex].Count; i++){
					if(selectedTarget[inputIndex][i].target == hit.collider.transform){
						selectedTarget[inputIndex].Remove(selectedTarget[inputIndex][i]);
						return;
					}
				}
			}
		}
		
		protected List<PointerParameters> GetSelected(int inputIndex)
		{
			if(selectedTarget.ContainsKey(inputIndex)){
				return selectedTarget[inputIndex];
			}else{
				return new List<PointerParameters>();
			}
		}
		
		protected void ClearSelected(int inputIndex)
		{
			selectedTarget.Remove(inputIndex);
		}
		#endregion

		#region BroadCasting
		protected void InvokeInputPointerDown(int inputIndex, InputPointerParameters inputPointerParams)
		{
			if(inputPointerParams != null){
				MonoBehaviour[] targetScripts = inputPointerParams.target.GetComponents<MonoBehaviour>();
			
				foreach(var script in targetScripts){
					IInputPointerListener myListener = script as IInputPointerListener;
					if(myListener != null){
						myListener.OnInputPointerDown(inputIndex, inputPointerParams, this);
					}
				}
			}
			
			if(OnInputPointerDown != null)
				OnInputPointerDown(inputIndex, inputPointerParams, this);
		}

		protected void InvokeInputPointer(int inputIndex, InputPointerParameters inputPointerParams)
		{
			if(inputPointerParams != null){
				MonoBehaviour[] targetScripts = inputPointerParams.target.GetComponents<MonoBehaviour>();
				
				foreach(var script in targetScripts){
					IInputPointerListener myListener = script as IInputPointerListener;
					if(myListener != null){
						myListener.OnInputPointer(inputIndex, inputPointerParams, this);
					}
				}
			}
			
			if(OnInputPointer != null)
				OnInputPointer(inputIndex, inputPointerParams, this);
		}

		protected void InvokeInputPointerUp(int inputIndex, InputPointerParameters inputPointerParams)
		{
			if(inputPointerParams != null){
				MonoBehaviour[] targetScripts = inputPointerParams.target.GetComponents<MonoBehaviour>();
				
				foreach(var script in targetScripts){
					IInputPointerListener myListener = script as IInputPointerListener;
					if(myListener != null){
						myListener.OnInputPointerUp(inputIndex, inputPointerParams, this);
					}
				}
			}
			
			if(OnInputPointerUp != null)
				OnInputPointerUp(inputIndex, inputPointerParams, this);
		}

		protected void InvokeInputPointerUpExternal(int inputIndex, InputPointerParameters inputPointerParams)
		{
			if(inputPointerParams != null){
				MonoBehaviour[] targetScripts = inputPointerParams.target.GetComponents<MonoBehaviour>();

				foreach(var script in targetScripts){
					IInputPointerListener myListener = script as IInputPointerListener;
					if(myListener != null){
						myListener.OnInputPointerUpExternal(inputIndex, inputPointerParams, this);
					}
				}
			}

			if(OnInputPointerUpExternal != null)
				OnInputPointerUpExternal(inputIndex, inputPointerParams, this);
		}

		protected void InvokeInputRayUpdate(int inputIndex, Ray inputRay)
		{
			if(OnInputRayUpdate != null)
				OnInputRayUpdate(inputIndex, inputRay, this);
		}
		#endregion
	}
}
