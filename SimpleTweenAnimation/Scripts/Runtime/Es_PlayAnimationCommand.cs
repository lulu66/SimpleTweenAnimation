using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class CommandInfoAttribute:Attribute
{
	public CommandInfoAttribute(string name, string helpText)
	{
		DisplayName = name;
		HelpText = helpText;
	}

	public string DisplayName { get; set; }
	public string HelpText { get; set; }
}

[System.Serializable]
public abstract class Es_PlayAnimationCommand
{
	[HideInInspector][SerializeField] protected string key = "";

	[SerializeField] protected bool commandEnabled = true;

	[NonSerialized]
	public bool Initialized;

	[NonSerialized]
	public bool IsStartTween = false;

	[NonSerialized]
	public int iIntData = -1;

	[NonSerialized]
	protected float timer = 0;
	[NonSerialized]
	public string kStringData = string.Empty;

	[NonSerialized]
	protected Transform rootTransform = null;

	

	public Es_PlayAnimationCommand()
	{

	}

	public virtual void onUpdate(float deltaTime)
	{
		if (!IsStartTween || !commandEnabled)
			return;
		if (rootTransform == null)
			return;

	}

	public virtual void onStart(Transform  rootTransform)
	{
		iIntData = -1;
		timer = 0;
		kStringData = string.Empty;
		Initialized = false;
		this.rootTransform = rootTransform;
	}

	public virtual void resetTween()
	{
		iIntData = -1;
		timer = 0;
		kStringData = string.Empty;

	}

	public virtual void playTween(float randomDelta = 0)
	{
		IsStartTween = true;
	}

	public enum CurveType
	{
		None,
		Time_Value,     // 时间-值   曲线
		Time_Progress,  // 时间-进度 曲线
		Time_Speed,     // 时间-速度 曲线
	}
}



