//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;
//using GammaInput;
//
//public sealed class CardboardPointer : BasePointer {
//
//	//1 Second after Hovering the object a Click is Simulated
//	public TimeSpan clickTimeSpan {get; private set;}
//	private List<Transform> clickLock = null;
//
//	public bool hasTimeClick;
//	[SerializeField]
//	private float clickTime = 1;
//
//	void Awake()
//	{
//		clickTimeSpan = TimeSpan.FromSeconds(clickTime);
//		clickLock = new List<Transform>();
//	}
//
//	protected override void PointerUpdate (int raydepth, bool behindGui)
//	{
//		if(raycastAll){
//			RaycastAll(ref raydepth, ref behindGui);
//		}else{
//			RaycastFirst(ref raydepth, ref behindGui);
//		}
//
//		base.PointerUpdate (raydepth, behindGui);
//	}
//
//	private void RaycastAll(ref int raydepth, ref bool behindGui)
//	{
//		Ray pointerRay = new Ray(transform.position, transform.forward);
//		RaycastHit[] raycastHits = Physics.RaycastAll(pointerRay, rayDistance, mask.value);
//		List<RaycastHit> tempActives = new List<RaycastHit>();
//		foreach(var hit in raycastHits){
//
//			PointerParameters pp;
//
//			if(!IsActive(hit, out pp)){
//				pp = new PointerParameters(hit, raydepth, this, behindGui);
//				AddActive(pp);
//
//				InvokePointerEnter(0, new BasePointerParameters(hit, raydepth, this, behindGui));
//			}else{
//				var npp = new BasePointerParameters(hit, raydepth, this, behindGui);
//
//				InvokePointerOver(0, npp);
//
//				if(hasTimeClick && (npp.time - pp.time) > clickTimeSpan && !clickLock.Contains(npp.target)){
//					clickLock.Add(npp.target);
//					InvokePointerClicked(0, npp);
//				}
//
//				if(Cardboard.SDK.Triggered){
//					InvokePointerClicked(0, npp);
//				}
//			}
//			tempActives.Add(hit);
//
//			//Raise raycounter
//			raydepth ++;
//		}
//
//		//If we hit something and we are a GUIpointer then set behindGui true
//		if(tempActives.Count > 0 && isGUIPointer){
//			behindGui = true;
//		}
//
//		List<PointerParameters> leftTargets = RemoveActivesExcept(tempActives);
//
//		foreach(var tpp in leftTargets){
//			InvokePointerExit(0, new BasePointerParameters(tpp.target, this));
//			if(clickLock.Contains(tpp.target)){
//				clickLock.Remove(tpp.target);
//			}
//		}
//	}
//
//	private void RaycastFirst(ref int raydepth, ref bool behindGui)
//	{
//		Ray pointerRay = new Ray(transform.position, transform.forward);
//		RaycastHit hit;
//		if(Physics.Raycast(pointerRay, out hit, rayDistance, mask.value)){
//			// Something hit, handle Enter and Over events
//			PointerParameters pp;
//
//			if(!IsActive(hit, out pp)){
//				if(HasActives()){
//
//					InvokePointerExit(0, new BasePointerParameters(activeTarget[0][0].target, this));
//					if(clickLock.Contains(activeTarget[0][0].target)){
//						clickLock.Remove(activeTarget[0][0].target);
//					}
//					ClearActives();
//				}
//
//				pp = new PointerParameters(hit, raydepth, this, behindGui);
//				AddActive(pp);
//				InvokePointerEnter(0, new BasePointerParameters(hit, raydepth, this, behindGui));
//			}else{
//				var npp = new BasePointerParameters(hit, raydepth, this, behindGui);
//
//				InvokePointerOver(0, npp);
//
//				if(hasTimeClick && (npp.time - pp.time) > clickTimeSpan && !clickLock.Contains(npp.target)){
//					clickLock.Add(npp.target);
//					InvokePointerClicked(0, npp);
//				}
//
//				if(Cardboard.SDK.Triggered){
//					InvokePointerClicked(0, npp);
//				}
//			}
//
//			raydepth ++;
//			if(isGUIPointer){
//				behindGui = true;
//			}
//		}else{
//			// Nothing hit, report Exit event if active objects
//			if(HasActives()){
//
//				InvokePointerExit(0, new BasePointerParameters(activeTarget[0][0].target, this));
//				if(clickLock.Contains(activeTarget[0][0].target)){
//					clickLock.Remove(activeTarget[0][0].target);
//				}
//				ClearActives();
//			}
//		}
//	}
//}
