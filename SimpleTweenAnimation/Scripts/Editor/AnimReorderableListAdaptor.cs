using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using MEngine;

public class AnimReorderableListAdaptor
{
	protected SerializedProperty arrayProperty;
	protected ReorderableList list;
	protected TweenAnimsManager manager;

	private const float HeightHeader = 50f;
	private const float XShiftHeader = 15f;

	public SerializedProperty this[int index]
	{
		get { return arrayProperty.GetArrayElementAtIndex(index); }
	}

	public AnimReorderableListAdaptor(TweenAnimsManager manager, SerializedProperty arrayProperty)
	{
		this.arrayProperty = arrayProperty;
		this.manager = manager;

		list = new ReorderableList(arrayProperty.serializedObject, arrayProperty, true, true, true, true);
		list.drawHeaderCallback = DrawHeader;
		list.drawElementCallback = DrawItem;
		list.drawElementBackgroundCallback = DrawItemBackground;
		list.onAddDropdownCallback = DropDownMenu;
		list.elementHeightCallback = ItemHeight;
		list.onRemoveCallback = RemoveItem;

	}


	public void DrawAnimCommandList()
	{
		arrayProperty.serializedObject.Update();
		list.DoLayoutList();
		arrayProperty.serializedObject.ApplyModifiedProperties();
	}

	private void RemoveItem(ReorderableList list)
	{
		bool canDelete = EditorUtility.DisplayDialog("Warning", "是否要删除该项？", "是", "否");
		if(canDelete)
		{
			ReorderableList.defaultBehaviours.DoRemoveButton(list);
		}
	}

	private void DrawItemBackground(Rect position, int index, bool isActive, bool isFocused)
	{
		if (!isActive || !isFocused)
			return;

		int length = list.serializedProperty.arraySize;

		if (length <= 0)
			return;

		SerializedProperty elementProp = list.serializedProperty.GetArrayElementAtIndex(index);

		float height = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		if(elementProp.isExpanded)
		{
			height = EditorGUI.GetPropertyHeight(elementProp);
		}
		Rect rect = position;
		rect.height = height;
		EditorGUI.DrawRect(rect, Color.gray);
	}
	private void DrawHeader(Rect rect)
	{
		if (rect.width < 0)
			return;
		EditorGUI.LabelField(rect, new GUIContent("动画类型列表"));
	}

	private void DrawItem(Rect position, int index, bool selected, bool focused)
	{
		int size = list.serializedProperty.arraySize;
		if (size <= 0)
			return;
		list.serializedProperty.serializedObject.Update();
		SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
		SerializedProperty displayNameProp = element.FindPropertyRelative("key");
		string actionName = displayNameProp.stringValue;
		Rect elementRect = position;
		elementRect.x += XShiftHeader;
		elementRect.height = HeightHeader;
		element.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(elementRect, true, actionName);
		if (element.isExpanded)
		{
			EditorGUI.indentLevel++;
			EditorGUI.PropertyField(position, element);
			EditorGUI.indentLevel--;
		}
		EditorGUI.EndFoldoutHeaderGroup();
		list.serializedProperty.serializedObject.ApplyModifiedProperties();
	}

	private float ItemHeight(int index)
	{
		int length = list.serializedProperty.arraySize;

		if (length <= 0)
			return 0.0f;

		SerializedProperty elementProp = list.serializedProperty.GetArrayElementAtIndex(index);

		float height = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		if (!elementProp.isExpanded)
			return height;
		height = EditorGUI.GetPropertyHeight(elementProp, true);
		return height;
	}

	private void DropDownMenu(Rect position, ReorderableList list)
	{
		var menu = new GenericMenu();
		List<Type> showTypes = GetNonAbstractTypesSubclassOf<Es_PlayAnimationCommand>();

		for(int i=0; i<showTypes.Count; i++)
		{
			Type type = showTypes[i];
			string actionName = type.Name;
			//bool alreadyHasIt = DoesReordListHaveElementOfType(actionName);
			ReNameActionName(type, ref actionName);
			menu.AddItem(new GUIContent(actionName), false, OnSelected, (object)type);
		}
		menu.ShowAsContext();
	}

	void ReNameActionName(Type commandType, ref string actionName)
	{
		CommandInfoAttribute commandInfoAttr = GetCommandInfo(commandType);
		if(commandInfoAttr != null)
		{
			actionName = commandInfoAttr.DisplayName;
		}
	}

	void OnSelected(System.Object obj)
	{
		Type tweenAnimType = (Type)obj;
		int last = list.serializedProperty.arraySize;
		list.serializedProperty.InsertArrayElementAtIndex(last);
		SerializedProperty lastProp = list.serializedProperty.GetArrayElementAtIndex(last);
		lastProp.managedReferenceValue = Activator.CreateInstance(tweenAnimType);
		arrayProperty.serializedObject.ApplyModifiedProperties();
	}

	//private bool DoesReordListHaveElementOfType(string type)
	//{
	//	for (int i = 0; i < list.serializedProperty.arraySize; ++i)
	//	{
	//		// this works but feels ugly. Type in the array element looks like "managedReference<actualStringType>"
	//		if (list.serializedProperty.GetArrayElementAtIndex(i).type.Contains(type))
	//			return true;
	//	}

	//	return false;
	//}

	private int CompareTypesNames(Type a, Type b)
	{
		return a.Name.CompareTo(b.Name);
	}

	private List<Type> GetNonAbstractTypesSubclassOf<T>(bool sorted = true) where T : class
	{
		Type parentType = typeof(T);
		Assembly assembly = Assembly.GetAssembly(parentType);

		List<Type> types = assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(parentType)).ToList();

		if (sorted)
			types.Sort(CompareTypesNames);

		return types;
	}

	public static CommandInfoAttribute GetCommandInfo(Type commandType)
	{
		CommandInfoAttribute retval = null;

		object[] attributes = commandType.GetCustomAttributes(typeof(CommandInfoAttribute), false);
		foreach (object obj in attributes)
		{
			CommandInfoAttribute commandInfoAttr = obj as CommandInfoAttribute;
			if (commandInfoAttr != null)
			{
				retval = commandInfoAttr;
			}
		}

		return retval;

	}
}
