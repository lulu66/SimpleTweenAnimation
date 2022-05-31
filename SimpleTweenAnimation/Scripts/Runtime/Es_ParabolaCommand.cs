using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("Parabola", "")]
[Serializable]
public class Es_ParabolaCommand : Es_PlayAnimationCommand
{
	[SerializeField] float delay = 0;
	[SerializeField] float duration = 0;
	[SerializeField] AnimationCurve curve_xTime_ySpeed = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
	[SerializeField] Vector3 startVector;
	[SerializeField] float parapola_horizontalVelocity;
	[SerializeField] float parapola_paramM;
	[SerializeField] float parapola_paramN;

	private float m_parapola_xMoveDistance = 0;
	private CurveType curveType;
	private Vector3 originVector;
	public int m_moveDir = 2;
	public event Action<Es_ParabolaCommand> onComplete;

	public Es_ParabolaCommand() : base()
	{
		key = "Parabola";
	}

	public Es_ParabolaCommand(AnimationCurve speedCurve, Vector3 starvector, float velocity, float paramM, float paramN, float delay, float duration) : base()
	{
		key = "Parabola";
		this.curve_xTime_ySpeed = speedCurve;
		this.startVector = starvector;
		this.parapola_horizontalVelocity = velocity;
		this.parapola_paramM = paramM;
		this.parapola_paramN = paramN;
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
		m_parapola_xMoveDistance = 0f;
		curveType = CurveType.Time_Speed;
		originVector = rootTransform.localPosition;

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
			case CurveType.Time_Speed:
				curveValue = curve_xTime_ySpeed.Evaluate(percent);
				break;
		}

		if (curveType == CurveType.Time_Speed)
		{
			float x, y, curMoveDistance, curMoveTime;

			// 当前帧的移动时间，不能超过 duration
			curMoveTime = deltaTime;
			if (timer - delay > duration)
			{
				curMoveTime -= (timer - delay - duration);
			}

			// 当前帧移动距离
			curMoveDistance = curveValue * parapola_horizontalVelocity * curMoveTime;

			// 总移动距离，因为需求想实现变速的抛物线（变速用 AnimationCurve 来配置），所以移动距离不能单纯的用 速度 * 时间 来获得，必须每一帧的 位移 来叠加求得
			m_parapola_xMoveDistance += curMoveDistance;

			// 带入抛物线方程求解坐标，x 是横向距离，y是纵向距离
			x = m_parapola_xMoveDistance;
			y = Mathf.Pow(x - parapola_paramM, 2) / -parapola_paramN + Mathf.Pow(parapola_paramM, 2) / parapola_paramN;

			// 根据抛物线方向给对象坐标赋值
			if (m_moveDir == 1)
			{
				// 朝左
				rootTransform.localPosition = new Vector3(-x, y, 0);
			}
			else
			{
				// 朝右
				rootTransform.localPosition = new Vector3(x, y, 0);
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
		if (rootTransform == null)
			return;
		rootTransform.localPosition = originVector;
		m_parapola_xMoveDistance = 0f;

	}
}
