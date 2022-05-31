using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CommandInfo("Four Corners Mesh Color", "")]
[Serializable]
public class Es_FourCornersMeshColorCommand : Es_PlayAnimationCommand
{
	[SerializeField] AnimationCurve curve_xTime_yProgressValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
	[SerializeField] Color startLeftUpColor = Color.white;
	[SerializeField] Color startRightDownColor = Color.white;
	[SerializeField] Color startRightUpColor = Color.white;
	[SerializeField] Color startLeftDownColor = Color.white;
	[SerializeField] Color endLeftUpColor = Color.white;
	[SerializeField] Color endRightUpColor = Color.white;
	[SerializeField] Color endLeftDownColor = Color.white;
	[SerializeField] Color endRightDownColor = Color.white;
	[SerializeField] float delay = 0;
	[SerializeField] float duration = 0;

	private CurveType curveType;
	private TMP_Text mesh;

	private Color originLeftUpColor = new Color(1, 1, 1, 1);
	private Color originLeftDownColor = new Color(1, 1, 1, 1);
	private Color originRightUpColor = new Color(1, 1, 1, 1);
	private Color originRightDownColor = new Color(1, 1, 1, 1);

	public event Action<Es_FourCornersMeshColorCommand> onComplete;

	public Es_FourCornersMeshColorCommand() : base()
	{
		key = "Four Corners Mesh Color";

	}

	public Es_FourCornersMeshColorCommand(AnimationCurve progressValue, Color startLeftupColor, Color startRightDownColor,
		Color startRightupColor, Color startLeftDownColor, Color endLeftupColor, Color endRightupColor, Color endLeftdownColor, Color endRightdownColor,
		float delay, float duration) : base()
	{
		key = "Four Corners Mesh Color";
		curve_xTime_yProgressValue = progressValue;
		this.startLeftUpColor = startLeftupColor;
		this.startRightDownColor = startRightDownColor;
		this.startRightUpColor = startRightupColor;
		this.startLeftDownColor = startLeftDownColor;
		this.endLeftUpColor = endLeftupColor;
		this.endRightUpColor = endRightupColor;
		this.endLeftDownColor = endLeftdownColor;
		this.endRightDownColor = endRightdownColor;
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
			if (!mesh.enableVertexGradient)
				mesh.enableVertexGradient = true;//开启渐变
			originLeftDownColor = startLeftDownColor;
			originLeftUpColor = startLeftUpColor;
			originRightDownColor = startRightDownColor;
			originRightUpColor = startRightUpColor;
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
			if (mesh)
			{
				Color deltaA;
				Color LeftUp, LeftDown, RightUp, RightDown;

				deltaA = endLeftUpColor - startLeftUpColor;
				LeftUp = startLeftUpColor + deltaA * curveValue;
				deltaA = endLeftDownColor - startLeftDownColor;
				LeftDown = startLeftDownColor + deltaA * curveValue;
				deltaA = endRightDownColor - startRightDownColor;
				RightDown = startRightDownColor + deltaA * curveValue;
				deltaA = endRightUpColor - startRightUpColor;
				RightUp = startRightUpColor + deltaA * curveValue;

				mesh.colorGradient = new VertexGradient(LeftUp, RightUp, LeftDown, RightDown);
				mesh.SetVerticesDirty();
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
			mesh.colorGradient = new VertexGradient(originLeftUpColor, originRightUpColor, originLeftDownColor, originRightDownColor);
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
