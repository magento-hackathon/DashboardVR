using UnityEngine;
using System;
using System.Timers;

public class GammaTimerHelper : MonoBehaviour {
	
	private static GameObject helperGO;
	
	public DateTime endingTime {get; private set;}
	public TimeSpan remainingTime {get; private set;}
	private Action elapsedCallback;
	private bool timerUp = false;


	public static GammaTimerHelper CreateHelper()
	{
		if(helperGO == null)
			helperGO = new GameObject("GammaTimeHelper");
		
		GammaTimerHelper helper = helperGO.AddComponent<GammaTimerHelper>();
		DontDestroyOnLoad(helper);
		return helper;
	}
	
	public void StartTimer(TimeSpan elapsed, Action elapsedCallback)
	{
		this.endingTime = DateTime.UtcNow + elapsed;
		this.elapsedCallback = elapsedCallback;
		this.timerUp = false;
	}
	
	void Update()
	{
		if(!timerUp && DateTime.UtcNow > endingTime)
		{
			timerUp = true;
			if(elapsedCallback != null){
				elapsedCallback();
			}
		}
	}
}