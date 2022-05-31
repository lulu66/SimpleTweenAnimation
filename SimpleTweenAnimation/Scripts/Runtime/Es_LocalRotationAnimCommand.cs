using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("Local Rotation", "")]
[Serializable]
public class Es_LocalRotationAnimCommand : Es_PlayAnimationCommand
{

	[SerializeField] Vector3 endVector = Vector3.zero;

	[SerializeField] Vector3 startVector = Vector3.zero;

	[SerializeField] float delay = 0;

	[SerializeField] float duration = 0;

	[SerializeField] AnimationCurve curve_xTime_yProgressValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

	private CurveType curveType;
	private Vector3 originVector;
	public event Action<Es_LocalRotationAnimCommand> onComplete;

	public Es_LocalRotationAnimCommand():base()
	{
		key = "Local Rotation";
	}

	public Es_LocalRotationAnimCommand(Vector3 endvector, Vector3 startvector, float delay, float duration, AnimationCurve progressCurve) : base()
	{
		key = "Local Rotation";
		this.endVector = endvector;
		this.startVector = startvector;
		this.delay = delay;
		this.duration = duration;
		this.curve_xTime_yProgressValue = progressCurve;
	}

	public override void onStart(Transform  rootTransform)
	{
		iIntData = -1;
		timer = 0;
		kStringData = string.Empty;
		Initialized = false;
		this.rootTransform = rootTransform;
		curveType = CurveType.Time_Progress;
		originVector = rootTransform.localEulerAngles;
	}

	public override void onUpdate(float deltaTime)
	{
		if (!IsStartTween || !commandEnabled)
			return;
		if (rootTransform == null)
			return;

		if (curveType == CurveType.None)
		{
			IsStartTween = false;
			return;
		}
		timer += deltaTime;
		if (timer < delay)
		{
			return;
		}

		float percent = (timer - delay) / duration;
		if (percent > 1)
		{
			percent = 1;
		}

		float curveValue = 0;
		Vector3 deltaV;
		switch (curveType)
		{
			case CurveType.Time_Progress:
				{
					curveValue = curve_xTime_yProgressValue.Evaluate(percent);
				}
				break;
		}

		if (curveType == CurveType.Time_Progress)
		{
			deltaV = endVector - startVector;
			rootTransform.localRotation = Quaternion.Euler(startVector + deltaV * curveValue);
		}
		else
		{
			IsStartTween = false;
			return;
		}

		if (percent >= 1)
		{
			IsStartTween = false;
			if (onComplete != null)
			{
				onComplete(this);
			}
		}
	}

	public override void resetTween()
	{
		base.resetTween();
		if (rootTransform != null)
			rootTransform.localRotation = Quaternion.Euler(originVector);

	}

	public override void playTween(float randomDelta = 0)
	{
		if (!commandEnabled)
			return;
		if (rootTransform == null)
			return;
		IsStartTween = true;
	}
}
