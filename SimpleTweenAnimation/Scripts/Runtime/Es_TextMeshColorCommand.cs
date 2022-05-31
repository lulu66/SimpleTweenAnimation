using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
[CommandInfo("TextMeshColor","")]
public class Es_TextMeshColorCommand : Es_PlayAnimationCommand
{
	[SerializeField] protected AnimationCurve curve_xTime_yProgressValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
	[SerializeField] Color startColor;
	[SerializeField] Color endColor;
	[SerializeField] float delay = 0;
	[SerializeField] float duration = 0;

	private CurveType curveType;
	private TMP_Text mesh;
	private SpriteRenderer sr;
	private Color originColor;
	public event Action<Es_TextMeshColorCommand> onComplete;

	public Es_TextMeshColorCommand() : base()
	{
		key = "Text Mesh Color";
	}

	public Es_TextMeshColorCommand(AnimationCurve progressCurve, Color startcolor, Color endcolor, float delay, float duration) : base()
	{
		key = "Text Mesh Color";
		curve_xTime_yProgressValue = progressCurve;
		startColor = startcolor;
		endColor = endcolor;
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
		mesh = rootTransform.GetComponent<TMP_Text>();
		sr = rootTransform.GetComponent<SpriteRenderer>();
		if (mesh)
		{
			if (mesh.enableVertexGradient)
				mesh.enableVertexGradient = false;//关闭渐变
			originColor = mesh.color;
		}
		else if (sr)
		{
			originColor = sr.color;
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
			Color deltaC = endColor - startColor;
			if (mesh)
			{
				mesh.color = startColor + deltaC * curveValue;
				mesh.SetVerticesDirty();
			}
			else if (sr)
			{
				Color resultColor = startColor + deltaC * curveValue;
				sr.color = new Color(resultColor.r, resultColor.g, resultColor.b, sr.color.a);
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

	public override void resetTween()
	{
		base.resetTween();
		if (mesh)
		{
			mesh.color = originColor;
		}
		else if (sr)
		{
			sr.color = originColor;
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
