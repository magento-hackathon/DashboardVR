using UnityEngine;
using System.Collections;
using GammaInput;
using UnityEngine.EventSystems;

public class UGuiInput : MonoBehaviour {

	private EventSystem _eventSystem;
	private EventSystem eventSystem {
		get {
			if(_eventSystem == null){
				_eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
			}
			return _eventSystem;
		}
	}

	void OnEnable()
	{
		BasePointer.OnPointerEnter += HandleOnPointerEnter;
		BasePointer.OnPointerOver += HandleOnPointerOver;
		BasePointer.OnPointerExit += HandleOnPointerExit;
		BasePointer.OnPointerClicked += HandleOnPointerClicked;

		InputPointer.OnInputPointerDown += HandleOnInputPointerDown;
		InputPointer.OnInputPointer += HandleOnInputPointer;
		InputPointer.OnInputPointerUp += HandleOnInputPointerUp;
	}
	
	void OnDisable()
	{
		BasePointer.OnPointerEnter -= HandleOnPointerOver;
		BasePointer.OnPointerOver -= HandleOnPointerOver;
		BasePointer.OnPointerExit -= HandleOnPointerExit;
		BasePointer.OnPointerClicked -= HandleOnPointerClicked;

		InputPointer.OnInputPointerDown -= HandleOnInputPointerDown;
		InputPointer.OnInputPointer -= HandleOnInputPointer;
		InputPointer.OnInputPointerUp -= HandleOnInputPointerUp;
	}

	void HandleOnPointerEnter (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
	{
		IPointerEnterHandler[] enterHandlers = pointerParams.target.GetComponents<IPointerEnterHandler>();

		foreach(var handler in enterHandlers){
			PointerEventData data = new PointerEventData(null);
			handler.OnPointerEnter(data);
		}
	}

	void HandleOnPointerOver (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
	{

	}
	
	void HandleOnPointerExit (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
	{
		IPointerExitHandler[] enterHandlers = pointerParams.target.GetComponents<IPointerExitHandler>();
		
		foreach(var handler in enterHandlers){
			PointerEventData data = new PointerEventData(null);
			handler.OnPointerExit(data);
		}
	}

	void HandleOnInputPointerDown(int inputIndex, InputPointerParameters pointerParams, InputPointer sender)
	{
		IPointerDownHandler[] enterHandlers = pointerParams.target.GetComponents<IPointerDownHandler>();
		
		foreach(var handler in enterHandlers){
			PointerEventData data = new PointerEventData(eventSystem);

			handler.OnPointerDown(data);
		}
	}
	
	void HandleOnInputPointer (int inputIndex, InputPointerParameters pointerParams, InputPointer sender)
	{

	}
	
	void HandleOnInputPointerUp (int inputIndex, InputPointerParameters pointerParams, InputPointer sender)
	{
		IPointerUpHandler[] enterHandlers = pointerParams.target.GetComponents<IPointerUpHandler>();
		
		foreach(var handler in enterHandlers){
			PointerEventData data = new PointerEventData(eventSystem);
			handler.OnPointerUp(data);
		}
	}
	
	void HandleOnPointerClicked (int inputIndex, BasePointerParameters pointerParams, BasePointer sender)
	{
		IPointerClickHandler[] enterHandlers = pointerParams.target.GetComponents<IPointerClickHandler>();
		
		foreach(var handler in enterHandlers){
			PointerEventData data = new PointerEventData(eventSystem);
			handler.OnPointerClick(data);
		}
	}
}
