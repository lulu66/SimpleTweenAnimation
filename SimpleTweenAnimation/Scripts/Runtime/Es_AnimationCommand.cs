using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CommandInfo("Animation", "")]
[Serializable]
public class Es_AnimationCommand : Es_PlayAnimationCommand
{

	public Transform relatedTransform;

	private Animation ani;

	public Es_AnimationCommand() : base()
	{
		key = "Animation";
	}

	public override void onStart(Transform rootTransform)
	{
		iIntData = -1;
		timer = 0;
		kStringData = string.Empty;
		Initialized = false;
		this.rootTransform = rootTransform;
		ani = rootTransform.GetComponent<Animation>();
		if(ani == null)
		{
			ani = rootTransform.gameObject.AddComponent<Animation>();
		}
	}

	public override void onUpdate(float deltaTime)
	{
		if (!IsStartTween || !commandEnabled)
			return;
		if (rootTransform == null)
			return;
	}

	public override void playTween(float randomDelta = 0)
	{
		if (ani != null && ani.GetClipCount() > 0)
		{
			ani.Play();
		}
		IsStartTween = true;
	}

	public override void resetTween()
	{
		base.resetTween();
		if (ani != null)
		{
			ani.Stop();
		}
	}
}
