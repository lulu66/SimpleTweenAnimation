using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CommandInfo("UI Blur", "")]
[Serializable]
public class Es_UIBlurCommand : Es_PlayAnimationCommand
{
	[SerializeField] AnimationCurve curve_xTime_yProgressValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

	[SerializeField] float startValue;
	[SerializeField] float endValue;
	[SerializeField] float delay = 0;
	[SerializeField] float duration = 0;

	private CurveType curveType;
	private Material material;
	private float originValue; // 初始值

	public event Action<Es_UIBlurCommand> onComplete;

	public Es_UIBlurCommand() : base()
	{
		key = "UI Blur";
	}

	public Es_UIBlurCommand(AnimationCurve progressCurve, float starvalue, float endvalue, float delay, float duration) : base()
	{
		key = "UI Blur";
		curve_xTime_yProgressValue = progressCurve;
		startValue = starvalue;
		endValue = endvalue;
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

		curveType = CurveType.Time_Progress;
		MaskableGraphic img = rootTransform.GetComponent<RawImage>();
		if (!img)
		{
			img = rootTransform.GetComponent<Image>();
		}
		if (img)
		{
			material = img.material;
			if (material)
			{
				originValue = startValue;
			}
		}

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
			if (material)
			{
				float delta = endValue - startValue;
				material.SetFloat("_Size", startValue + delta * curveValue);
			}
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

	public override void playTween(float randomDelta = 0)
	{
		if (!commandEnabled)
			return;
		if (rootTransform == null)
			return;
		IsStartTween = true;
	}

	public override void resetTween()
	{
		base.resetTween();
		if (material)
		{
			material.SetFloat("_Size", originValue);
		}

	}
}
