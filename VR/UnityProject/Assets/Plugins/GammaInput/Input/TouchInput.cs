using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GammaInput;

public class TouchInput : BaseInput
{
	public float clickTime = 0.1f;
	public float doubleClickTime = 0.2f;

	public delegate void TouchEventHandler(int index, TouchInputParameters inputParameters, TouchInput sender);

	public static event TouchEventHandler OnTouchBegan;
	public static event TouchEventHandler OnTouchMoved;
	public static event TouchEventHandler OnTouchStationary;
	public static event TouchEventHandler OnTouchEnded;
	public static event TouchEventHandler OnTouchCanceled;

	public static event TouchEventHandler OnClick;

	public static bool touchSupported {
		get {
			return Input.touchSupported;
		}
	}

	private Dictionary<int, TouchInputParameters> activeInputs = new Dictionary<int, TouchInputParameters>();

	#region MouseInputParameters state Handling
	private void StartInput(Touch touch)
	{
		if(!activeInputs.ContainsKey(touch.fingerId)){
			activeInputs.Add(touch.fingerId, new TouchInputParameters(touch));
		}else{
			EndInput(touch);
			StartInput(touch);
		}
	}
	
	private void UpdateInput(Touch touch)
	{
		if(activeInputs.ContainsKey(touch.fingerId)){
			activeInputs[touch.fingerId].UpdateTouch(touch);
		}else{
			StartInput(touch);
		}
	}
	
	private void EndInput(Touch touch)
	{
		if(activeInputs.ContainsKey(touch.fingerId)){
			activeInputs.Remove(touch.fingerId);
		}
	}
	#endregion

	void Update()
	{
		for(int i=0; i<Input.touchCount; i++){
			switch(Input.touches[i].phase){
			case TouchPhase.Began:
				StartInput(Input.touches[i]);

				if(OnTouchBegan != null)
					OnTouchBegan(i, activeInputs[Input.touches[i].fingerId], this);
				break;
			case TouchPhase.Moved:
				UpdateInput(Input.touches[i]);

				if(OnTouchMoved != null)
					OnTouchMoved(i, activeInputs[Input.touches[i].fingerId], this);
				break;
			case TouchPhase.Stationary:
				UpdateInput(Input.touches[i]);

				if(OnTouchStationary != null)
					OnTouchStationary(i, activeInputs[Input.touches[i].fingerId], this);
				break;
			case TouchPhase.Ended:
				UpdateInput(Input.touches[i]);

				if(activeInputs[Input.touches[i].fingerId].InputStartSince(TimeSpan.FromSeconds(clickTime))){
					if(OnClick != null)
						OnClick(i, activeInputs[Input.touches[i].fingerId], this);
				}

				if(OnTouchEnded != null)
					OnTouchEnded(i, activeInputs[Input.touches[i].fingerId], this);

				EndInput(Input.touches[i]);
				break;
			case TouchPhase.Canceled:
				UpdateInput(Input.touches[i]);

				if(OnTouchCanceled != null)
					OnTouchCanceled(i, activeInputs[Input.touches[i].fingerId], this);

				EndInput(Input.touches[i]);
				break;
			}
		}
	}
}

public class TouchInputParameters : InputParameters
{
	private Touch _touch;
	public Touch touch {
		get {
			return _touch;
		}
	}

	public TouchInputParameters(Touch touch) : base (touch.fingerId, touch.position)
	{
		this._touch = touch;
	}

	public void UpdateTouch(Touch touch)
	{
		this._touch = touch;
		base.UpdatePosition(touch.position);
	}
}
