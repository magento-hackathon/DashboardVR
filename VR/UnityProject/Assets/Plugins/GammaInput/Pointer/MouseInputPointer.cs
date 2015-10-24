using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GammaInput;

[RequireComponent(typeof(Camera))]
public class MouseInputPointer : InputPointer {
	
	#region MouseInputHandling
	protected override void OnEnable ()
	{
		base.OnEnable ();

		MouseInput.OnButtonDown += HandleOnButtonDown;
		MouseInput.OnButton += HandleOnButton;
		MouseInput.OnButtonUp += HandleOnButtonUp;
		MouseInput.OnClick += HandleOnClick;
	}
	
	protected override void OnDisable ()
	{
		base.OnDisable ();

		MouseInput.OnButtonDown -= HandleOnButtonDown;
		MouseInput.OnButton -= HandleOnButton;
		MouseInput.OnButtonUp -= HandleOnButtonUp;
		MouseInput.OnClick -= HandleOnClick;
	}
	
	void HandleOnButtonDown (int buttonIndex, MouseInputParameters inputParameters, MouseInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as MouseInputPointer).PointerInputDown(buttonIndex, inputParameters, sender, 0, false);
		}
	}
	
	void HandleOnButton (int buttonIndex, MouseInputParameters inputParameters, MouseInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as MouseInputPointer).PointerInput(buttonIndex, inputParameters, sender, 0, false);
		}
	}
	
	void HandleOnButtonUp (int buttonIndex, MouseInputParameters inputParameters, MouseInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as MouseInputPointer).PointerInputUp(buttonIndex, inputParameters, sender, 0, false);
		}
	}
	
	void HandleOnClick (int buttonIndex, MouseInputParameters inputParameters, MouseInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as MouseInputPointer).PointerInputTimeClick(buttonIndex, inputParameters, sender, 0, false);
		}
	}
	#endregion
	
	#region InputEvents
	bool timeClick = false;
	protected override void PointerInputDown (int inputIndex, InputParameters inputParams, BaseInput input, int raydepth, bool behindGui)
	{
		//Handle Raycast for Input
		Ray inputRay = GetComponent<Camera>().ScreenPointToRay(inputParams.position);
		if(raycastAll){
			RaycastHit[] raycastHits = Physics.RaycastAll(inputRay, rayDistance, mask);
			foreach(var hit in raycastHits){
				
				InputPointerParameters ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, inputRay);
				
				AddSelected(inputIndex, ipp);
				InvokeInputPointerDown(inputIndex, ipp);
				
				if(isGUIPointer){
					behindGui = true;
				}
				
				raydepth++;
			}
		}else{
			RaycastHit hit;
			if(Physics.Raycast(inputRay, out hit, rayDistance, mask)){
				
				InputPointerParameters ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, inputRay);

				AddSelected(inputIndex, ipp);
				InvokeInputPointerDown(inputIndex, ipp);
				
				if(isGUIPointer){
					behindGui = true;
				}
				raydepth++;
			}
		}
		timeClick = false;
		
		base.PointerInputDown (inputIndex, inputParams, input, raydepth, behindGui);
	}
	
	protected override void PointerInput (int inputIndex, InputParameters inputParams, BaseInput input, int raydepth, bool behindGui)
	{
		Ray inputRay = GetComponent<Camera>().ScreenPointToRay(inputParams.position);
		if(raycastAll){
			RaycastHit[] raycastHits = Physics.RaycastAll(inputRay, rayDistance, mask);
			foreach(var hit in raycastHits){
				
				var ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, inputRay);

				InvokeInputPointer(inputIndex, ipp);
				
				if(isGUIPointer){
					behindGui = true;
				}
				
				raydepth++;
			}
		}else{
			RaycastHit hit;
			if(Physics.Raycast(inputRay, out hit, rayDistance, mask)){
				
				IsSelected(inputIndex, hit);
				
				var ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, inputRay);

				InvokeInputPointer(inputIndex, ipp);
				
				if(isGUIPointer){
					behindGui = true;
				}
				raydepth++;
			}
		}
		timeClick = false;
		
		base.PointerInput (inputIndex, inputParams, input, raydepth, behindGui);
	}
	
	protected override void PointerInputTimeClick (int inputIndex, InputParameters inputParams, BaseInput input, int raydepth, bool behindGui)
	{
		timeClick = true;
		
		base.PointerInputTimeClick (inputIndex, inputParams, input, raydepth, behindGui);
	}
	
	protected override void PointerInputUp (int inputIndex, InputParameters inputParams, BaseInput input, int raydepth, bool behindGui)
	{
		Ray inputRay = GetComponent<Camera>().ScreenPointToRay(inputParams.position);
		if(raycastAll){
			RaycastHit[] raycastHits = Physics.RaycastAll(inputRay, rayDistance, mask);
			foreach(var hit in raycastHits){
				
				var ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, inputRay);

				if(IsSelected(inputIndex, hit)){
					ipp.inputTimedClick = timeClick;
					InvokePointerClicked(inputIndex, ipp);
					RemoveSelected(inputIndex, hit);
				}
				
				InvokeInputPointerUp(inputIndex, ipp);
				
				if(isGUIPointer){
					behindGui = true;
				}
				raydepth++;
			}
			
			foreach(var remainingpp in GetSelected(inputIndex))
			{
				var ipp = new InputPointerParameters(remainingpp.target, this);
				ipp.SetInput(input, inputParams, inputRay);

				InvokeInputPointerUpExternal(inputIndex, ipp);
			}
		}else{
			RaycastHit hit;
			if(Physics.Raycast(inputRay, out hit, rayDistance, mask)){
				
				var ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, inputRay);

				InvokeInputPointerUp(inputIndex, ipp);
				
				if(IsSelected(inputIndex, hit)){
					ipp.inputTimedClick = timeClick;
					InvokePointerClicked(inputIndex, ipp);
					RemoveSelected(inputIndex, hit);
				}
				
				if(isGUIPointer){
					behindGui = true;
				}
				raydepth++;
			}
			
			foreach(var remainingpp in GetSelected(inputIndex))
			{
				var ipp = new InputPointerParameters(remainingpp.target, this);
				ipp.SetInput(input, inputParams, inputRay);

				InvokeInputPointerUpExternal(inputIndex, ipp);
			}
		}
		
		ClearSelected(inputIndex);
		
		base.PointerInputUp (inputIndex, inputParams, input, raydepth, behindGui);
	}
	#endregion
	
	protected override void PointerUpdate (int raydepth, bool behindGui)
	{
		if(raycastAll){
			RaycastAll(ref raydepth, ref behindGui);
		}else{
			RaycastFirst(ref raydepth, ref behindGui);
		}
		
		base.PointerUpdate (raydepth, behindGui);
	}
	
	private void RaycastAll(ref int raydepth, ref bool behindGui)
	{
		Ray pointerRay = GetComponent<Camera>().ScreenPointToRay(MouseInput.position);
		InvokeInputRayUpdate(0, pointerRay);

		RaycastHit[] raycastHits = Physics.RaycastAll(pointerRay, rayDistance, mask.value);
		List<RaycastHit> tempActives = new List<RaycastHit>();
		foreach(var hit in raycastHits){
			
			PointerParameters pp;
			
			if(!IsActive(hit, out pp)){
				pp = new PointerParameters(hit, raydepth, this, behindGui);
				AddActive(pp);
				
				InvokePointerEnter(0, new BasePointerParameters(hit, raydepth, this, behindGui));
			}else{
				var npp = new BasePointerParameters(hit, raydepth, this, behindGui);
				
				InvokePointerOver(0, npp);
			}
			tempActives.Add(hit);
			
			//Raise raycounter
			raydepth ++;
		}
		
		//If we hit something and we are a GUIpointer then set behindGui true
		if(tempActives.Count > 0 && isGUIPointer){
			behindGui = true;
		}
		
		List<PointerParameters> leftTargets = RemoveActivesExcept(tempActives);
		
		foreach(var tpp in leftTargets){
			InvokePointerExit(0, new BasePointerParameters(tpp.target, this));
		}
	}
	
	private void RaycastFirst(ref int raydepth, ref bool behindGui)
	{
		Ray pointerRay = GetComponent<Camera>().ScreenPointToRay(MouseInput.position);
		InvokeInputRayUpdate(0, pointerRay);

		RaycastHit hit;
		if(Physics.Raycast(pointerRay, out hit, rayDistance, mask.value)){
			// Something hit, handle Enter and Over events
			PointerParameters pp;
			
			if(!IsActive(hit, out pp)){
				if(HasActives()){
					
					InvokePointerExit(0, new BasePointerParameters(activeTarget[0][0].target, this));
					ClearActives();
				}
				
				pp = new PointerParameters(hit, raydepth, this, behindGui);
				AddActive(pp);
				InvokePointerEnter(0, new BasePointerParameters(hit, raydepth, this, behindGui));
			}else{
				var npp = new BasePointerParameters(hit, raydepth, this, behindGui);
				
				InvokePointerOver(0, npp);
			}
			
			raydepth ++;
			if(isGUIPointer){
				behindGui = true;
			}
		}else{
			// Nothing hit, report Exit event if active objects
			if(HasActives()){
				
				InvokePointerExit(0, new BasePointerParameters(activeTarget[0][0].target, this));
				ClearActives();
			}
		}
	}
}