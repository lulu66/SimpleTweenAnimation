using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MEngine
{
	public class TweenAnimsManager : MonoBehaviour
	{
		[SerializeReference] protected List<Es_PlayAnimationCommand> animationCommandList;

		public List<Es_PlayAnimationCommand> AnimationCommandList
		{
			get { return animationCommandList; }
		}
		public Transform TargetTransform
		{
			get { return transform; }
		}

		public void OnStart()
		{
			if (animationCommandList == null)
				return;
			for (int i = 0; i < animationCommandList.Count; i++)
			{
				var command = animationCommandList[i];
				command.onStart(transform);
			}
		}

		public void OnUpdate()
		{
			if (animationCommandList == null)
				return;
			for (int i = 0; i < animationCommandList.Count; i++)
			{
				var command = animationCommandList[i];
				command.onUpdate(Time.deltaTime);
			}
		}

		public void ResetAllCommands()
		{
			if (animationCommandList == null)
				return;

			for (int i = 0; i < animationCommandList.Count; i++)
			{
				var command = animationCommandList[i];
				command.resetTween();
			}
		}
	}
}

