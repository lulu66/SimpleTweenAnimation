using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CommandInfo("Fade", "")]
[Serializable]
public class Es_FadeCommand : Es_PlayAnimationCommand
{
	[SerializeField] CurveType curveType = CurveType.Time_Progress;
	[SerializeField] AnimationCurve curve_xTime_yProgressValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
	[SerializeField] AnimationCurve curve_xTime_yValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

	[SerializeField] float startValue = 0;
	[SerializeField] float endValue = 0;
	[SerializeField] float delay = 0;
	[SerializeField] float duration = 0;

	private TMP_Text mesh;
	private Material material;
	private SpriteRenderer sr;
	private Image img2D;
	private float originValue; // 初始值

	public event Action<Es_FadeCommand> onComplete;

	public Es_FadeCommand() : base()
	{
		key = "Fade";
	}

	public Es_FadeCommand(CurveType curveType, AnimationCurve progressCurve, AnimationCurve valueCurve, float startValue, 
		float endValue, float delay, float duration) : base()
	{
		key = "Fade";
		this.curveType = curveType;
		this.curve_xTime_yProgressValue = progressCurve;
		this.curve_xTime_yValue = valueCurve;
		this.startValue = startValue;
		this.endValue = endValue;
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
		mesh = rootTransform.GetComponent<TMP_Text>();
		sr = rootTransform.GetComponent<SpriteRenderer>();
		img2D = rootTransform.GetComponent<Image>();
		if (mesh)
		{
			originValue = mesh.alpha;
		}
		else if (sr)
		{
			originValue = sr.color.a;
		}
		else if (img2D)
		{
			originValue = img2D.color.a;
		}

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
			IsStartTween = false;
			if (onComplete != null)
			{
				onComplete(this);
			}
		}

		float curveValue = 0;
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
			float delta = endValue - startValue;
			if (mesh)
			{
				mesh.alpha = startValue + delta * curveValue;
				mesh.SetMaterialDirty();
			}
			else if (sr)
			{
				sr.color = new Color(1, 1, 1, startValue + delta * curveValue);
			}
			else if (img2D)
			{
				img2D.color = new Color(1, 1, 1, startValue + delta * curveValue);
			}
		}
		else if (curveType == CurveType.Time_Value)
		{
			if (mesh)
			{
				mesh.alpha = curveValue;
				mesh.SetMaterialDirty();
			}
			else if (sr)
			{
				sr.color = new Color(1, 1, 1, curveValue);
			}
			else if (img2D)
			{
				img2D.color = new Color(1, 1, 1, curveValue);
			}
		}
		else
		{
			IsStartTween = false;
			return;
		}

		if (percent >= 1)
		{

		}

	}

	public override void resetTween()
	{
		base.resetTween();
		if (mesh)
		{
			mesh.alpha = originValue;
		}
		else if (sr)
		{
			sr.color = new Color(1, 1, 1, originValue);
		}
		else if (img2D)
		{
			img2D.color = new Color(1, 1, 1, originValue);
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
