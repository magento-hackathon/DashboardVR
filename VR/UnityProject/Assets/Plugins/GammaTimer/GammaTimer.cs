using UnityEngine;
using System;
using System.Timers;

public class GammaTimer {

	public Action OnTimerElapsed {get; set;}

	private TimeSpan remainingTime;
	private GammaTimerHelper timeHelper;
	private bool autodestroy;

	public GammaTimer(TimeSpan time, Action OnTimerElapsed, bool autodestroy = true)
	{
		this.OnTimerElapsed = OnTimerElapsed;
		this.remainingTime = time;
		this.autodestroy = autodestroy;
		this.timeHelper = GammaTimerHelper.CreateHelper();
	}

	public GammaTimer(TimeSpan time) : this(time, null)
	{
	}

	public void Start()
	{
		timeHelper.enabled = true;
		timeHelper.StartTimer(remainingTime, ()=>{
			if(OnTimerElapsed != null)
				OnTimerElapsed();

			if(autodestroy)
				Dispose();
		});
	}

	public void Stop()
	{
		timeHelper.enabled = false;
	}

	public void Dispose()
	{
		if(timeHelper != null && timeHelper.gameObject != null)
		{
#if UNITY_EDITOR
		UnityEngine.Object.DestroyImmediate(timeHelper.gameObject);
#else
		UnityEngine.Object.Destroy(timeHelper.gameObject);
#endif
		}
		OnTimerElapsed = null;
		
	}
}

public class GammaIntervallTimer {

	public Action<int> OnIntervallElpased {get; set;}

	private TimeSpan startIntervallTime;
	private int remainingIntevallCount;
	private bool autodestroy;

	private GammaTimerHelper timeHelper;

	public GammaIntervallTimer(TimeSpan intervallTime, int intevallCount, Action<int> OnIntervallElpased, bool autodestroy = true)
	{
		this.startIntervallTime = intervallTime;
		this.remainingIntevallCount = intevallCount;
		this.OnIntervallElpased = OnIntervallElpased;
		this.autodestroy = autodestroy;
		this.timeHelper = GammaTimerHelper.CreateHelper();
	}

	public void Start()
	{
		StartIntervall();
	}

	private void StartIntervall()
	{
		timeHelper.enabled = true;
		timeHelper.StartTimer(startIntervallTime, ()=>{
			remainingIntevallCount--;

			if(OnIntervallElpased != null)
				OnIntervallElpased(remainingIntevallCount);

			if(remainingIntevallCount == 0){
				if(autodestroy)
					Dispose();
				return;
			}
			StartIntervall();
		});
	}

	public void Stop()
	{
		timeHelper.enabled = false;
	}
	
	public void Dispose()
	{
#if UNITY_EDITOR
		UnityEngine.Object.DestroyImmediate(timeHelper.gameObject);
#else
		UnityEngine.Object.Destroy(timeHelper.gameObject);
#endif
		OnIntervallElpased = null;
	}
}


