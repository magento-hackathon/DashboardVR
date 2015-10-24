using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GammaInput;

public class GearVRPointer : InputPointer {

	#region MouseInputHandling
	protected override void OnEnable ()
	{
		base.OnEnable ();

		if(isEventMaster){
			MouseInput.OnButtonDown += HandleOnButtonDown;
			MouseInput.OnButton += HandleOnButton;
			MouseInput.OnButtonUp += HandleOnButtonUp;
			MouseInput.OnClick += HandleOnClick;
		}
	}

	protected override void OnDisable ()
	{
		base.OnDisable ();

		if(isEventMaster){
			MouseInput.OnButtonDown -= HandleOnButtonDown;
			MouseInput.OnButton -= HandleOnButton;
			MouseInput.OnButtonUp -= HandleOnButtonUp;
			MouseInput.OnClick -= HandleOnClick;
		}
	}

	void HandleOnButtonDown (int buttonIndex, MouseInputParameters inputParameters, MouseInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as GearVRPointer).PointerInputDown(buttonIndex, inputParameters, sender, 0, false);
		}
	}

	void HandleOnButton (int buttonIndex, MouseInputParameters inputParameters, MouseInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as GearVRPointer).PointerInput(buttonIndex, inputParameters, sender, 0, false);
		}
	}
	
	void HandleOnButtonUp (int buttonIndex, MouseInputParameters inputParameters, MouseInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as GearVRPointer).PointerInputUp(buttonIndex, inputParameters, sender, 0, false);
		}
	}

	void HandleOnClick (int buttonIndex, MouseInputParameters inputParameters, MouseInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as GearVRPointer).PointerInputTimeClick(buttonIndex, inputParameters, sender, 0, false);
		}
	}
	#endregion

	#region InputEvents
	bool timeClick = false;
	protected override void PointerInputDown (int inputIndex, InputParameters inputParams, BaseInput input, int raydepth, bool behindGui)
	{
		//Handle Raycast for Input
		Ray gearRay = new Ray(transform.position, transform.forward);

		if(raycastAll){
			RaycastHit[] raycastHits = Physics.RaycastAll(gearRay, rayDistance, mask);
			foreach(var hit in raycastHits){

				InputPointerParameters ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, gearRay);

				AddSelected(inputIndex, ipp);
				InvokeInputPointerDown(inputIndex, ipp);

				if(isGUIPointer){
					behindGui = true;
				}

				raydepth++;
			}
		}else{
			RaycastHit hit;
			if(Physics.Raycast(gearRay, out hit, rayDistance, mask)){

				InputPointerParameters ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, gearRay);

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
		Ray gearRay = new Ray(transform.position, transform.forward);
		if(raycastAll){
			RaycastHit[] raycastHits = Physics.RaycastAll(gearRay, rayDistance, mask);
			foreach(var hit in raycastHits){

				var ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, gearRay);

				InvokeInputPointer(inputIndex, ipp);
				
				if(isGUIPointer){
					behindGui = true;
				}
				
				raydepth++;
			}
		}else{
			RaycastHit hit;
			if(Physics.Raycast(gearRay, out hit, rayDistance, mask)){

				IsSelected(inputIndex, hit);
				
				var ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, gearRay);

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
		Ray gearRay = new Ray(transform.position, transform.forward);
		if(raycastAll){
			RaycastHit[] raycastHits = Physics.RaycastAll(gearRay, rayDistance, mask);
			foreach(var hit in raycastHits){

				var ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, gearRay);

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
				ipp.SetInput(input, inputParams, gearRay);

				InvokeInputPointerUpExternal(inputIndex, ipp);
			}
		}else{
			RaycastHit hit;
			if(Physics.Raycast(gearRay, out hit, rayDistance, mask)){

				var ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, gearRay);

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
				ipp.SetInput(input, inputParams, gearRay);
				
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
		Ray gearRay = new Ray(transform.position, transform.forward);
		InvokeInputRayUpdate(0, gearRay);

		Debug.DrawRay(gearRay.origin, gearRay.direction*50, Color.red);

		RaycastHit[] raycastHits = Physics.RaycastAll(gearRay, rayDistance, mask.value);
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
		Ray gearRay = new Ray(transform.position, transform.forward);
		InvokeInputRayUpdate(0, gearRay);

		Debug.DrawRay(gearRay.origin, gearRay.direction*50, Color.red);

		RaycastHit hit;
		if(Physics.Raycast(gearRay, out hit, rayDistance, mask.value)){

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
