using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MEngine;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TweenAnimsManager), true)]
public class TweenAnimManagerEditor : Editor
{
	protected AnimReorderableListAdaptor reorderableListAdaptor;

	private SerializedProperty animationCommandListProp;

	private bool m_bIsPlay = false;

	TweenAnimsManager animManager;
	private float m_animStartTime = 0;

	List<Es_PlayAnimationCommand> anims = new List<Es_PlayAnimationCommand>();

	public void OnEnable()
	{
		if (serializedObject == null)
			return;
		animationCommandListProp = serializedObject.FindProperty("animationCommandList");
		reorderableListAdaptor = new AnimReorderableListAdaptor(target as TweenAnimsManager, animationCommandListProp);

		animManager = target as TweenAnimsManager;

		EditorApplication.update -= OnUpdate;
		EditorApplication.update += OnUpdate;


	}

	public void OnDisable()
	{
		EditorApplication.update -= OnUpdate;
	}

	public override void OnInspectorGUI()
	{

		reorderableListAdaptor.DrawAnimCommandList();

		GUILayout.Space(20);
		if (GUILayout.Button("播放"))
		{
			BeginPlay();
		}

		// 显示重置按钮
		if (GUILayout.Button("重置"))
		{
			StopPlay();
		}
		GUILayout.Space(40);
		if(GUILayout.Button("整体播放"))
		{
			PlayAllAnimations();
		}
		if (GUILayout.Button("整体重置"))
		{
			StopPlayAllAnimations();
		}
		GUILayout.Space(40);
	}


	void PlayAllAnimations()
	{
		if(animManager != null)
		{
			Transform cur = animManager.transform;
			Transform root = cur;
			while (cur != null)
			{
				root = cur;
				cur = cur.parent;
			}

			anims.Clear();
			var allManagers = root.GetComponentsInChildren<TweenAnimsManager>();
			if (allManagers == null)
				return;
			for(int i=0; i<allManagers.Length; i++)
			{
				var curManager = allManagers[i];
				var list = curManager.AnimationCommandList;
				for (int j=0; j< list.Count; j++)
				{
					list[j].onStart(curManager.transform);
				}
				anims.AddRange(list);
			}

			for (int i = 0; i < anims.Count; i++)
			{
				anims[i].playTween();
			}

			m_bIsPlay = true;
			m_animStartTime = Time.realtimeSinceStartup;


		}
	}

	void StopPlayAllAnimations()
	{
		if (animManager != null)
		{
			Transform cur = animManager.transform;
			Transform root = cur;
			while (cur != null)
			{
				root = cur;
				cur = cur.parent;
			}

			anims.Clear();
			var allManagers = root.GetComponentsInChildren<TweenAnimsManager>();
			if (allManagers == null)
				return;
			for (int i = 0; i < allManagers.Length; i++)
			{
				var curManager = allManagers[i];
				var list = curManager.AnimationCommandList;

				anims.AddRange(list);
			}
			for (int i = 0; i < anims.Count; i++)
			{
				anims[i].resetTween();
			}
			m_bIsPlay = false;
		}

	}

	public void OnUpdate()
	{
		float deltaTime = Time.realtimeSinceStartup - m_animStartTime;
		m_animStartTime = Time.realtimeSinceStartup;

		var animList = anims;
		if (m_bIsPlay)
		{
			if (animList != null && animList.Count > 0)
			{
				for (int i = 0; i < animList.Count; i++)
				{
					animList[i].onUpdate(deltaTime);
				}
			}

		}
		EditorApplication.QueuePlayerLoopUpdate();
	}

	public void BeginPlay()
	{
		anims.Clear();
		anims.AddRange(animManager.AnimationCommandList);
		if (anims.Count <= 0)
			return;
		for (int i = 0; i < anims.Count; i++)
		{
			anims[i].onStart(animManager.TargetTransform);
			anims[i].playTween();
		}

		m_bIsPlay = true;
		m_animStartTime = Time.realtimeSinceStartup;

	}

	public void StopPlay()
	{

		anims.Clear();
		anims.AddRange(animManager.AnimationCommandList);
		if (anims.Count <= 0)
			return;

		for (int i = 0; i < anims.Count; i++)
		{
			anims[i].resetTween();
		}


		m_bIsPlay = false;

	}
}
