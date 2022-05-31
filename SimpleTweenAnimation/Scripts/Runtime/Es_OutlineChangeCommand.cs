using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CommandInfo("Outline Change", "")]
[Serializable]
public class Es_OutlineChangeCommand :Es_PlayAnimationCommand
{
	[SerializeField] AnimationCurve curve_xTime_yProgressValue;
	[SerializeField] Color startOutlineColor;
	[SerializeField] Color endOutlineColor;
	[SerializeField] bool thickChange;
	[SerializeField] bool softness;
	[SerializeField] float startThick;
	[SerializeField] float endThick;
	[SerializeField] float startSoft;
	[SerializeField] float endSoft;
	[SerializeField] float delay = 0;
	[SerializeField] float duration = 0;

	private CurveType curveType;
	private TMP_Text mesh;
	private Material material;
	private Color originOutline = new Color(1, 1, 1, 1);

	public event Action<Es_OutlineChangeCommand> onComplete;

	public Es_OutlineChangeCommand() : base()
	{
		key = "Outline Change";
	}

	public Es_OutlineChangeCommand(AnimationCurve progressCurve, Color start_outlinecolor, Color end_outlinecolor, bool thick_change,
		bool softness, float startthick, float endthick, float startsoft, float endsoft, float delay, float duration) : base()
	{
		key = "Outline Change";
		this.curve_xTime_yProgressValue = progressCurve;
		this.startOutlineColor = start_outlinecolor;
		this.endOutlineColor = end_outlinecolor;
		this.thickChange = thick_change;
		this.softness = softness;
		this.startThick = startthick;
		this.endThick = endthick;
		this.startSoft = startsoft;
		this.endSoft = endsoft;
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
			if(material == null)
			{
				material = mesh.fontMaterial;
			}
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
			Color deltaA;
			deltaA = endOutlineColor - startOutlineColor;
			if (material)
			{
				material.SetColor("_OutlineColor", startOutlineColor + deltaA * curveValue);
				//if (Application.isPlaying)
				//{
				//    mesh.outlineColor = startOutlineColor + deltaA * curveValue;
				//}
				//else
				//{
				//    material.SetColor("_OutlineColor", startOutlineColor + deltaA * curveValue);
				//}
				if (thickChange)
				{
					float offsetA = endThick - startThick;
					material.SetFloat("_OutlineWidth", startThick + offsetA * curveValue);
				}
				if (softness)
				{
					float offsetB = endSoft - startSoft;
					material.SetFloat("_OutlineSoftness", startSoft + offsetB * curveValue);
				}
			}
			if (mesh)
			{
				mesh.SetMaterialDirty();
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
		if (material)
		{
			material.SetColor("_OutlineColor", originOutline);
			//if(Application.isPlaying)
			//{
			//	mesh.outlineColor = originOutline;
			//}
			//else
			//{
			//	material.SetColor("_OutlineColor", originOutline);
			//}

			if (softness)
			{
				material.SetFloat("_OutlineSoftness", startSoft);
			}
			if (thickChange)
			{
				material.SetFloat("_OutlineWidth", startThick);
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
}
