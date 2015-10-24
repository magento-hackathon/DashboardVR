using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GammaInput {
	public abstract class BasePointer : MonoBehaviour {

		public int depth;

		public LayerMask mask;
		public float rayDistance = 1000f;
		public bool isGUIPointer;
		public bool raycastAll = false;

		public delegate void InputPointerEventHandler(int inputIndex, BasePointerParameters pointerParams, BasePointer sender);

		public static event InputPointerEventHandler OnPointerEnter;
		public static event InputPointerEventHandler OnPointerOver;
		public static event InputPointerEventHandler OnPointerClicked;
		public static event InputPointerEventHandler OnPointerExit;
		
		#region eventforwarding
		private static bool dirtyEventChain;
		private static BasePointer eventMaster;
		protected bool isEventMaster {
			get { return this == eventMaster;}
		}

		protected static List<BasePointer> activePointers = new List<BasePointer>();
		
		protected BasePointer eventReceiver;

		protected delegate void PointerUpdateDelegate(int raydepth, bool behindGui);
		protected PointerUpdateDelegate pointerUpdateDelegate;

		private static void SetEventChainDirty()
		{
			dirtyEventChain = true;
		}

		private void SetupEventChain()
		{
			activePointers.Sort((p1, p2)=>{
				if(p1.depth > p2.depth)
					return -1;
				if(p1.depth < p2.depth)
					return 1;
				return 0;
			});

			for(int i=0; i < activePointers.Count-1; i++){
				activePointers[i].eventReceiver = activePointers[i+1];
				activePointers[i].SetupEventDelegates();
			}
		}

		protected virtual void SetupEventDelegates()
		{
			this.pointerUpdateDelegate = eventReceiver.PointerUpdate;
		}

		protected virtual void PointerUpdate(int raydepth, bool behindGui)
		{
			if(pointerUpdateDelegate != null)
				pointerUpdateDelegate(raydepth, behindGui ? true : false);
		}
		#endregion

		protected virtual void OnEnable()
		{
			activePointers.Add(this);
			
			eventMaster = this;
			SetEventChainDirty();
		}
		
		protected virtual void OnDisable()
		{
			if(eventMaster == this){
				if(activePointers.Count > 0){
					eventMaster = activePointers[0];
				}else{
					eventMaster = null;
				}
			}
			activePointers.Remove(this);
			
			SetEventChainDirty();
		}

		private void Update()
		{
			if(dirtyEventChain && isEventMaster){
				this.SetupEventChain();
			}

			if(isEventMaster){
				activePointers[0].PointerUpdate(0, false);
			}
		}

		#region Active management
		public Dictionary<int, List<PointerParameters>> activeTarget = new Dictionary<int, List<PointerParameters>>();
		protected bool HasActives()
		{
			return HasActives(0);
		}

		protected bool HasActives(int inputIndex)
		{
			if(activeTarget.ContainsKey(inputIndex)){
				if(activeTarget[inputIndex].Count > 0)
					return true;
			}
			return false;
		}

		protected bool IsActive(RaycastHit hit)
		{
			PointerParameters blah;
			return IsActive(hit, out blah);
		}

		protected bool IsActive(int inputIndex, RaycastHit hit)
		{
			PointerParameters blah;
			return IsActive(inputIndex, hit, out blah);
		}

		protected bool IsActive(RaycastHit hit, out PointerParameters enterParameters)
		{
			return IsActive(0, hit, out enterParameters);
		}

		protected bool IsActive(int inputIndex, RaycastHit hit, out PointerParameters enterParameters)
		{
			if(activeTarget.ContainsKey(inputIndex)){
				foreach(var pp in activeTarget[inputIndex]){
					if(pp.target == hit.collider.transform){
						enterParameters = pp;
						return true;
					}
				}
			}
			enterParameters = null;
			return false;
		}
		
		protected void AddActive(PointerParameters newParameters)
		{
			AddActive(0, newParameters);
		}

		protected void AddActive(int inputIndex, PointerParameters newParameters)
		{
			if(!activeTarget.ContainsKey(inputIndex)){
				activeTarget.Add(inputIndex, new List<PointerParameters>());
			}
			activeTarget[inputIndex].Add(newParameters);
		}

		protected List<PointerParameters> RemoveActivesExcept(List<RaycastHit> hits)
		{
			return RemoveActivesExcept(0, hits);
		}

		protected List<PointerParameters> RemoveActivesExcept(int inputIndex, List<RaycastHit> hits)
		{
			List<PointerParameters> removed = new List<PointerParameters>();

			if(!activeTarget.ContainsKey(inputIndex)){
				return removed;
			}

			activeTarget[inputIndex].RemoveAll((pp)=>{
				
				foreach(var hit in hits){
					if(pp.target == hit.collider.transform){
						return false;
					}
				}
				removed.Add(pp);
				return true;
			});
			
			return removed;
		}
		
		protected void ClearActives()
		{
			activeTarget.Clear();
		}
		#endregion

		#region BroadCasting

		protected virtual void InvokePointerEnter(int inputIndex, BasePointerParameters pointerParams)
		{
			MonoBehaviour[] targetScripts = pointerParams.target.GetComponents<MonoBehaviour>();
			
			foreach(var script in targetScripts){
				IBasePointerListener myListener = script as IBasePointerListener;
				if(myListener != null){
					myListener.OnPointerEnter(inputIndex, pointerParams, this);
				}
			}
			
			if(OnPointerEnter != null)
				OnPointerEnter(inputIndex, pointerParams, this);
		}

		protected virtual void InvokePointerOver(int inputIndex, BasePointerParameters pointerParams)
		{
			MonoBehaviour[] targetScripts = pointerParams.target.GetComponents<MonoBehaviour>();
			
			foreach(var script in targetScripts){
				IBasePointerListener myListener = script as IBasePointerListener;
				if(myListener != null){
					myListener.OnPointerOver(inputIndex, pointerParams, this);
				}
			}
			
			if(OnPointerOver != null)
				OnPointerOver(inputIndex, pointerParams, this);
		}

		protected virtual void InvokePointerClicked(int inputIndex, BasePointerParameters pointerParams)
		{
			MonoBehaviour[] targetScripts = pointerParams.target.GetComponents<MonoBehaviour>();
			
			foreach(var script in targetScripts){
				IBasePointerListener myListener = script as IBasePointerListener;
				if(myListener != null){
					myListener.OnPointerClicked(inputIndex, pointerParams, this);
				}
			}
			
			if(OnPointerClicked != null)
				OnPointerClicked(inputIndex, pointerParams, this);
		}

		protected virtual void InvokePointerExit(int inputIndex, BasePointerParameters pointerParams)
		{
			MonoBehaviour[] targetScripts = pointerParams.target.GetComponents<MonoBehaviour>();
			
			foreach(var script in targetScripts){
				IBasePointerListener myListener = script as IBasePointerListener;
				if(myListener != null){
					myListener.OnPointerExit(inputIndex, pointerParams, this);
				}
			}

			if(OnPointerExit != null)
				OnPointerExit(inputIndex, pointerParams, this);
		}

		#endregion
	}
}




