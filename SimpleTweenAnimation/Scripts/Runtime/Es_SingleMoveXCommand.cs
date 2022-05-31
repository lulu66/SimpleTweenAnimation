using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("SingleMoveX", "")]
[Serializable]
public class Es_SingleMoveXCommand : Es_PlayAnimationCommand
{
	[SerializeField] AnimationCurve curve_xTime_yProgressValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
	[SerializeField] float starX;
	[SerializeField] float endX;
	[SerializeField] float delay = 0;
	[SerializeField] float duration = 0;


	public CurveType curveType;
	public int m_moveDir = 2;
	public event Action<Es_SingleMoveXCommand> onComplete;

	private Vector3 originVector = Vector3.zero;

	public Es_SingleMoveXCommand():base()
	{
		key = "SingleMoveX";
	}

	public Es_SingleMoveXCommand(AnimationCurve progressCurve, float starx, float endx, float delay, float duration) : base()
	{
		key = "SingleMoveX";
		this.curve_xTime_yProgressValue = progressCurve;
		this.starX = starx;
		this.endX = endx;
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
			offset = endX - starX;
			float curPosX = starX + offset * curveValue;
			if(rootTransform != null)
				rootTransform.localPosition = new Vector3(m_moveDir == 2 ? curPosX : -curPosX, rootTransform.localPosition.y, rootTransform.localPosition.z);
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
