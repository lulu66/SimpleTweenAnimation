using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("Scale","")]
[Serializable]
public class Es_ScaleAnimCommand : Es_PlayAnimationCommand
{
	[SerializeField] CurveType curveType = CurveType.Time_Progress;

	[SerializeField] Vector3 startVector = Vector3.zero;

	[SerializeField] Vector3 endVector = Vector3.zero;

	[SerializeField] AnimationCurve curve_xTime_yProgressValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

	[SerializeField] AnimationCurve curve_xTime_yValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

	[SerializeField] float delay = 0;

	[SerializeField] float duration = 0;

	private Vector3 originVector;


	public event Action<Es_ScaleAnimCommand> onComplete;

	public Es_ScaleAnimCommand():base()
	{
		key = "Scale";
	}

	public Es_ScaleAnimCommand(CurveType curveType, Vector3 starvector, Vector3 endvector, AnimationCurve progressCurve, AnimationCurve valueCurve,
		float delay, float duration) : base()
	{
		key = "Scale";
		this.curveType = curveType;
		this.startVector = starvector;
		this.endVector = endvector;
		this.curve_xTime_yProgressValue = progressCurve;
		this.curve_xTime_yValue = valueCurve;
		this.delay = delay;
		this.duration = duration;
	}

	public override void onStart(Transform rootTransform)
	{
		iIntData = -1;
		timer = 0;
		kStringData = string.Empty;
		Initialized = false;
		this.rootTransform = rootTransform;
		originVector = rootTransform.localScale;

	}

	public override void onUpdate(float deltaTime)
	{
		//base.onUpdate(deltaTime);

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
			case CurveType.Time_Value:
				{
					curveValue = curve_xTime_yValue.Evaluate(percent);
				}
				break;
		}

		if (curveType == CurveType.Time_Progress)
		{
			deltaV = endVector - startVector;
			rootTransform.localScale = startVector + deltaV * curveValue;
		}
		else if (curveType == CurveType.Time_Value)
		{
			rootTransform.localScale = new Vector3(curveValue, curveValue, curveValue);
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
			rootTransform.localScale = originVector;
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
