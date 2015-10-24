using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GammaInput;

public class MouseInput : BaseInput {

	public float clickTime = 0.1f;
	public float doubleClickTime = 0.3f;
	public float trippleClickTime = 0.5f;
	public float mouseMovedThreshhold = 4f;

	public delegate void MouseInputEventHandler(int buttonIndex, MouseInputParameters inputParameters, MouseInput sender);
	public static event MouseInputEventHandler OnButtonDown;
	public static event MouseInputEventHandler OnButtonUp;
	public static event MouseInputEventHandler OnButton;

	public static event MouseInputEventHandler OnClick;

	public delegate void MouseMovedEventHandler(Vector2 position, Vector2 positionDelta, MouseInput sender);
	public static event MouseMovedEventHandler OnMouseMoved;
	public static event MouseMovedEventHandler OnMouseStationary;

	public delegate void MouseMultiClickEventHandler(int buttonIndex, MouseInputParameters[] inputParameters, MouseInput sender);
	public static event MouseMultiClickEventHandler OnDoubleClick;
	public static event MouseMultiClickEventHandler OnTrippleClick;

	public static Vector2 position {
		get {
			return Input.mousePosition;
		}
	}

	private Vector2 lastMousePosition;

	private Dictionary<int, MouseInputParameters> activeInputs;
	private List<MouseInputParameters> pastInputs;

	void Awake()
	{
		activeInputs = new Dictionary<int, MouseInputParameters>();
		pastInputs = new List<MouseInputParameters>();
	}

	#region MouseInputParameters state Handling
	private void StartInput(int buttonIndex)
	{
		if(!activeInputs.ContainsKey(buttonIndex)){
			activeInputs.Add(buttonIndex, new MouseInputParameters(buttonIndex, Input.mousePosition));
		}else{
			EndInput(buttonIndex);
			StartInput(buttonIndex);
		}
	}

	private void UpdateInput(int buttonIndex)
	{
		if(activeInputs.ContainsKey(buttonIndex)){
			activeInputs[buttonIndex].UpdatePosition(Input.mousePosition);
		}else{
			StartInput(buttonIndex);
		}
	}

	private void EndInput(int buttonIndex)
	{
		if(activeInputs.ContainsKey(buttonIndex)){
			if(OnDoubleClick != null || OnTrippleClick != null){
				pastInputs.Add(activeInputs[buttonIndex]);
			}
			activeInputs.Remove(buttonIndex);
		}
	}
	#endregion

	#region Implementation
	// Update is called once per frame
	void Update () {

		// Check if Someone is Listening for Movement Events, If not dont calc delta
		if(OnMouseMoved != null || OnMouseStationary != null){
			if((MouseInput.position - lastMousePosition).sqrMagnitude > mouseMovedThreshhold*mouseMovedThreshhold){
				if(OnMouseMoved != null)
					OnMouseMoved(Input.mousePosition, (MouseInput.position - lastMousePosition), this);
			}else{
				if(OnMouseStationary!= null)
					OnMouseStationary(Input.mousePosition, (MouseInput.position - lastMousePosition), this);
			}
		}

		// Loop over the 6 possible mouseInput Buttons
		for(int i=0; i <=6; i++){

			if(Input.GetMouseButtonDown(i)){
				StartInput(i);

				if(OnButtonDown != null)
					OnButtonDown(i, activeInputs[i], this);
			}

			if(Input.GetMouseButton(i)){
				UpdateInput(i);

				if(OnButton != null)
						OnButton(i, activeInputs[i], this);
			}

			if(Input.GetMouseButtonUp(i)){
				UpdateInput(i);

				if(activeInputs[i].InputStartSince(TimeSpan.FromSeconds(clickTime))){
					if(OnClick != null)
						OnClick(i, activeInputs[i], this);
				}

				if(OnDoubleClick != null){
					var multiclick = from inputParam in pastInputs where (inputParam.multiclickLocker < 2 && inputParam.inputIndex == i && inputParam.InputStartSince(TimeSpan.FromSeconds(doubleClickTime))) select inputParam;

					if(multiclick.Count() >= 1){
						foreach(var param in multiclick){
							param.multiclickLocker = 2;
						}

						activeInputs[i].multiclickLocker = 2;

						OnDoubleClick(i, multiclick.ToArray(), this);
					}
				}

				if(OnTrippleClick != null){
					var multiclick = from inputParam in pastInputs where (inputParam.multiclickLocker < 3 && inputParam.inputIndex == i && inputParam.InputStartSince(TimeSpan.FromSeconds(trippleClickTime))) select inputParam;

					if(multiclick.Count() >= 2){

						foreach(var param in multiclick){
							param.multiclickLocker = 3;
						}

						activeInputs[i].multiclickLocker = 3;

						OnTrippleClick(i, multiclick.ToArray(), this);
					}
				}

				if(OnButtonUp != null)
					OnButtonUp(i, activeInputs[i], this);

				EndInput(i);
			}
		}

		if(OnDoubleClick != null || OnTrippleClick != null){

			if(OnTrippleClick != null){
				pastInputs.RemoveAll((MouseInputParameters param)=>{
					return !param.InputStartSince(TimeSpan.FromSeconds(trippleClickTime));
				});
			}else{
				pastInputs.RemoveAll((MouseInputParameters param)=>{
					return !param.InputStartSince(TimeSpan.FromSeconds(doubleClickTime));
				});
			}
		}

		lastMousePosition = Input.mousePosition;
	}
	#endregion
}

public class MouseInputParameters : InputParameters
{
	public int multiclickLocker;

	public MouseInputParameters(int buttonIndex, Vector2 position) : base(buttonIndex, position)
	{

	}
}
