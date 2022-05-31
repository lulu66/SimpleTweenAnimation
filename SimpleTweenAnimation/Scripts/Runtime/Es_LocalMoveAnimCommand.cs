using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CommandInfo("Local Move", "")]
[Serializable]
public class Es_LocalMoveAnimCommand : Es_PlayAnimationCommand
{

	[SerializeField] Vector3 startVector = Vector3.zero;
	[SerializeField] Vector3 endVector = Vector3.zero;

	[SerializeField] bool openRandomX;
	[SerializeField] bool openRandomAngle;
	[SerializeField] bool openRandomAngleArray;

	[SerializeField] float leftRandomX;
	[SerializeField] float rightRandomX;

	[SerializeField] float offsetFromUpY;
	[SerializeField] float offsetFromDownY;
	[SerializeField] float offsetEnableAngle;

	[SerializeField] int minAngle;
	[SerializeField] int maxAngle;

	[SerializeField] int minRadius;
	[SerializeField] int maxRadius;

	[SerializeField]
	public int[] angleArray;

	[SerializeField] float delay = 0;

	[SerializeField] float duration = 0;

	[SerializeField] AnimationCurve curve_xTime_yProgressValue = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

	private Vector3 originVector = Vector3.zero;

	private CurveType curveType;

	public event Action<Es_LocalMoveAnimCommand> onComplete;

	// 位移动画方向 1：朝左，2：朝右
	public int m_moveDir = 2;
	// Y方向位移 0：不偏移 1：朝上 2 朝下
	public int m_moveY = 0;

	public Es_LocalMoveAnimCommand():base()
	{
		key = "Local Move";
	}

	public Es_LocalMoveAnimCommand(Vector3 startvector, Vector3 endvector, bool open_randomx, bool open_random_angle, bool open_random_anglearrray,
		float left_randomx, float right_randomx, float offset_from_upy, float offset_from_downy, float offset_enable_angle,
		int minangle, int maxangle, int minradius, int maxradius, int[] angleArray, float delay, float duration, AnimationCurve progressCurve) : base()
	{
		key = "Local Move";
		this.curve_xTime_yProgressValue = progressCurve;
		this.startVector = startvector;
		this.endVector = endvector;
		this.openRandomX = open_randomx;
		this.openRandomAngle = open_random_angle;
		this.openRandomAngleArray = open_random_anglearrray;
		this.leftRandomX = left_randomx;
		this.rightRandomX = right_randomx;
		this.offsetFromUpY = offset_from_upy;
		this.offsetFromDownY = offset_from_downy;
		this.offsetEnableAngle = offset_enable_angle;
		this.minAngle = minangle;
		this.maxAngle = maxangle;
		this.minRadius = minradius;
		this.maxRadius = maxradius;
		this.delay = delay;
		this.angleArray = angleArray;
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
		Vector3 deltaV;

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
			deltaV = endVector - startVector;
			Vector3 curPos = startVector + deltaV * curveValue;
			float resultY = curPos.y;
			if (m_moveY == 1)
			{
				resultY = curPos.y + offsetFromUpY;
			}
			else if (m_moveY == 2)
			{
				resultY = curPos.y - offsetFromDownY;
			}
			rootTransform.localPosition = new Vector3(m_moveDir == 2 ? curPos.x : -curPos.x, resultY, curPos.z);
		}
		else
		{
			IsStartTween = false;
			return;
		}

		if(percent >= 1)
		{
			IsStartTween = false;
			if(onComplete != null)
			{
				onComplete(this);
			}
		}
	}

	public override void playTween(float randomDelta = 0)
	{
		if(openRandomX)
		{
			float randomValue = UnityEngine.Random.Range(leftRandomX, rightRandomX);
			endVector.x = startVector.x = randomValue;
		}
		if(openRandomAngle)
		{
			int randomAngle = 0;
			if (openRandomAngleArray)
			{
				int index = UnityEngine.Random.Range(0, angleArray.Length);
				if (index == randomDelta)
				{
					index = index == 0 ? angleArray.Length - 1 : 0;
				}
				randomAngle = angleArray[index];
				int otherRandomAngle = index;
			}
			else
			{
				//随机一个X值
				randomAngle = UnityEngine.Random.Range(minAngle, maxAngle);
				int randomCount = 0;
				while (Mathf.Abs(randomAngle - randomDelta) < 10)
				{
					//随机次数限制，避免程序卡死
					if (randomCount > 10)
					{
						break;
					}
					randomAngle = UnityEngine.Random.Range(minAngle, maxAngle);
					++randomCount;
				}
				int otherRandomAngle = randomAngle;
			}

			float randomRadius = UnityEngine.Random.Range(minRadius, maxRadius);
			endVector.x = startVector.x + randomRadius * (float)Mathf.Sin(randomAngle * Mathf.Deg2Rad);
			endVector.y = startVector.y + randomRadius * (float)Mathf.Cos(randomAngle * Mathf.Deg2Rad);

		}
		IsStartTween = true;
	}

	public override void resetTween()
	{
		base.resetTween();

		if (rootTransform!=null)
			rootTransform.localPosition = originVector;
	}
}
