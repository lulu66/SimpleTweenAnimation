using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("SingleMoveY", "")]
[Serializable]
public class Es_SingleMoveYCommand : Es_PlayAnimationCommand
{
	[SerializeField] AnimationCurve curve_xTime_yProgressValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
	[SerializeField] float starY;
	[SerializeField] float endY;
	[SerializeField] float offsetFromUpY;
	[SerializeField] float offsetFromDownY;
	[SerializeField] float delay = 0;
	[SerializeField] float duration = 0;
	[SerializeField] float offsetEnableAngle;

	private CurveType curveType;
	public int m_moveY = 0;

	public event Action<Es_SingleMoveYCommand> onComplete;

	private Vector3 originVector = Vector3.zero;

	public Es_SingleMoveYCommand() : base()
	{
		key = "SingleMoveY";
	}

	public Es_SingleMoveYCommand(AnimationCurve progressCurve, float starty, float endy, float offset_from_upy, float offset_from_downy, float delay, float duration,
		float offset_enable_angle) : base()
	{
		key = "SingleMoveY";
		curve_xTime_yProgressValue = progressCurve;
		this.starY = starty;
		this.endY = endy;
		this.offsetFromUpY = offset_from_upy;
		this.offsetFromDownY = offset_from_downy;
		this.delay = delay;
		this.duration = duration;
		this.offsetEnableAngle = offset_enable_angle;
	}

	public override void onStart(Transform rootTransform)
	{
		iIntData = -1;
		timer = 0;
		kStringData = string.Empty;
		Initialized = false;
		this.rootTransform = rootTransform;
		originVector = rootTransform.localPosition;
		curveType = CurveType.Time_Progress;
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
		float offset;
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
			offset = endY - starY;
			float curPosY = starY + offset * curveValue;
			float resultY = curPosY;
			if (m_moveY == 1)
			{
				resultY = curPosY + offsetFromUpY;
			}
			else if (m_moveY == 2)
			{
				resultY = curPosY - offsetFromDownY;
			}
			if(rootTransform != null)
				rootTransform.localPosition = new Vector3(rootTransform.localPosition.x, resultY, rootTransform.localPosition.z);
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
		if (rootTransform)
		{
			rootTransform.localPosition = originVector;
		}
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
