using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CommandInfo("Shine Text", "")]
[Serializable]
public class Es_ShineTextCommand : Es_PlayAnimationCommand
{
	[SerializeField] float delay = 0;

	[SerializeField] float duration = 0;

	[SerializeField] AnimationCurve curve_xTime_yProgressValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

	[SerializeField] Color startShine;
	[SerializeField] Color endShine;
	[SerializeField] bool showShinePower;
	[SerializeField] float startPower;
	[SerializeField] float endPower;

	private CurveType curveType;
	private TMP_Text mesh;
	private Material material;
	private Color originOutline = new Color(1, 1, 1, 1);
	private Color startOutlineColor = new Color(1, 1, 1, 1);

	public event Action<Es_ShineTextCommand> onComplete;

	public Es_ShineTextCommand() : base()
	{
		key = "Shine Text";
	}

	public Es_ShineTextCommand(AnimationCurve progressCurve, Color start_shine, Color end_shine, bool show_shine_power, float start_power,
		float end_power, float delay, float duration) : base()
	{
		key = "Shine Text";
		this.curve_xTime_yProgressValue = progressCurve;
		this.startShine = start_shine;
		this.endShine = end_shine;
		this.showShinePower = show_shine_power;
		this.startPower = start_power;
		this.endPower = end_power;
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
		if (mesh)
		{
			material = mesh.fontMaterial;
			originOutline = startOutlineColor;
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
				Color deltaA;
				deltaA = endShine - startShine;
				material.SetColor("_GlowColor", startShine + deltaA * curveValue);
				if (showShinePower)
				{
					float offsetC = endPower - startPower;
					material.SetFloat("_GlowPower", startPower + offsetC * curveValue);
				}
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
			material.SetColor("_GlowColor", startShine);
			if (showShinePower)
			{
				material.SetFloat("_GlowPower", startPower);
			}
		}

	}

}
