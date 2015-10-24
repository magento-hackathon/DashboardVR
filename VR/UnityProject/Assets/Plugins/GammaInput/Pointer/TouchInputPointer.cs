using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GammaInput;

public class TouchInputPointer : InputPointer {

	protected override void OnEnable ()
	{
		base.OnEnable ();

		TouchInput.OnTouchBegan += HandleOnTouchBegan;
		TouchInput.OnTouchMoved += HandleOnTouch;
		TouchInput.OnTouchStationary += HandleOnTouch;
		TouchInput.OnTouchEnded += HandleOnTouchEnded;
		TouchInput.OnTouchCanceled += HandleOnTouchEnded;
		TouchInput.OnClick += HandleOnClick;
	}


	protected override void OnDisable ()
	{
		base.OnDisable ();
		
		TouchInput.OnTouchBegan -= HandleOnTouchBegan;
		TouchInput.OnTouchMoved -= HandleOnTouch;
		TouchInput.OnTouchStationary -= HandleOnTouch;
		TouchInput.OnTouchEnded -= HandleOnTouchEnded;
		TouchInput.OnTouchCanceled -= HandleOnTouchEnded;
		TouchInput.OnClick -= HandleOnClick;
	}

	void HandleOnTouchBegan (int touchIndex, TouchInputParameters inputParameters, TouchInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as TouchInputPointer).PointerInputDown(touchIndex, inputParameters, sender, 0, false);
		}
	}
	
	void HandleOnTouch (int touchIndex, TouchInputParameters inputParameters, TouchInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as TouchInputPointer).PointerInput(touchIndex, inputParameters, sender, 0, false);
		}
	}
	
	void HandleOnTouchEnded (int touchIndex, TouchInputParameters inputParameters, TouchInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as TouchInputPointer).PointerInputUp(touchIndex, inputParameters, sender, 0, false);
		}
	}
	
	void HandleOnClick (int touchIndex, TouchInputParameters inputParameters, TouchInput sender)
	{
		if(isEventMaster){
			(activePointers[0] as TouchInputPointer).PointerInputTimeClick(touchIndex, inputParameters, sender, 0, false);
		}
	}

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

				PointerParameters pp;
				if(!IsActive(inputIndex, hit, out pp)){
					pp = new PointerParameters(hit, raydepth, this, behindGui);
					AddActive(inputIndex, pp);
					
					InvokePointerEnter(inputIndex, ipp);
				}else{					
					InvokePointerOver(inputIndex, ipp);
				}

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

				PointerParameters pp;
				if(!IsActive(inputIndex, hit, out pp)){
					pp = new PointerParameters(hit, raydepth, this, behindGui);
					AddActive(inputIndex, pp);
					InvokePointerEnter(inputIndex, ipp);
				}else{					
					InvokePointerOver(inputIndex, ipp);
				}

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
		InvokeInputRayUpdate(inputIndex, inputRay);

		if(raycastAll){
			RaycastHit[] raycastHits = Physics.RaycastAll(inputRay, rayDistance, mask.value);
			List<RaycastHit> tempActives = new List<RaycastHit>();
			foreach(var hit in raycastHits){

				var ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, inputRay);

				PointerParameters pp;
				if(!IsActive(inputIndex, hit, out pp)){
					pp = new PointerParameters(hit, raydepth, this, behindGui);
					AddActive(inputIndex, pp);
					
					InvokePointerEnter(inputIndex, ipp);
				}else{					
					InvokePointerOver(inputIndex, ipp);
				}
				tempActives.Add(hit);

				InvokeInputPointer(inputIndex, ipp);

				if(isGUIPointer){
					behindGui = true;
				}

				//Raise raycounter
				raydepth ++;
			}
			
			List<PointerParameters> leftTargets = RemoveActivesExcept(inputIndex, tempActives);
			
			foreach(var tpp in leftTargets){
				InvokePointerExit(inputIndex, new BasePointerParameters(tpp.target, this));
			}
		}else{
			RaycastHit hit;
			List<RaycastHit> tempActives = new List<RaycastHit>();

			if(Physics.Raycast(inputRay, out hit, rayDistance, mask)){

				var ipp = new InputPointerParameters(hit, raydepth, this, behindGui);
				ipp.SetInput(input, inputParams, inputRay);
				
				PointerParameters pp;
				if(!IsActive(inputIndex, hit, out pp)){
					pp = new PointerParameters(hit, raydepth, this, behindGui);
					AddActive(inputIndex, pp);
					InvokePointerEnter(inputIndex, ipp);
				}else{					
					InvokePointerOver(inputIndex, ipp);
				}
				tempActives.Add(hit);
				
				InvokeInputPointer(inputIndex, ipp);
				
				if(isGUIPointer){
					behindGui = true;
				}
				
				//Raise raycounter
				raydepth ++;
			}

			List<PointerParameters> leftTargets = RemoveActivesExcept(inputIndex, tempActives);
			foreach(var tpp in leftTargets){
				InvokePointerExit(inputIndex, new BasePointerParameters(tpp.target, this));
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

		List<PointerParameters> leftTargets = RemoveActivesExcept(inputIndex, new List<RaycastHit>(0));
		foreach(var tpp in leftTargets){
			InvokePointerExit(inputIndex, new BasePointerParameters(tpp.target, this));
		}
		
		ClearSelected(inputIndex);
		
		base.PointerInputUp (inputIndex, inputParams, input, raydepth, behindGui);
	}
	#endregion
}
