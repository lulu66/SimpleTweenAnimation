using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using MEngine;

public class AnimCommandPropertyDrawer : MonoBehaviour
{

}

[CustomPropertyDrawer(typeof(Es_LocalMoveAnimCommand))]
public class Es_LocalMoveDrawer:PropertyDrawer
{
	protected float propHeight = 0;
	ReorderableList angleArrayList;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Local Move"), property);

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
		propHeight = 0;
		Rect rect = position;
		rect.y += singleLineHeight;

		rect.width = 500;
		rect.height = singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
		rect.y += singleLineHeight;
		curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);

		rect.y += singleLineHeight;
		SerializedProperty startVectorProp = property.FindPropertyRelative("startVector");
		startVectorProp.vector3Value = EditorGUI.Vector3Field(rect, new GUIContent("开始位置"), startVectorProp.vector3Value);

		rect.y += singleLineHeight;
		SerializedProperty openRandomXProp = property.FindPropertyRelative("openRandomX");
		openRandomXProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("开启X轴随机范围"), openRandomXProp.boolValue);

		rect.y += singleLineHeight;
		SerializedProperty offsetFromUpYProp = property.FindPropertyRelative("offsetFromUpY");
		offsetFromUpYProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("朝上偏移量"), offsetFromUpYProp.floatValue);

		rect.y += singleLineHeight;
		SerializedProperty offsetFromDownYProp = property.FindPropertyRelative("offsetFromDownY");
		offsetFromDownYProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("朝下偏移量"), offsetFromDownYProp.floatValue);

		SerializedProperty offsetEnableAngleProp = property.FindPropertyRelative("offsetEnableAngle");
		rect.y += singleLineHeight;
		offsetEnableAngleProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("偏移量生效在屏幕Y方向的夹角"), offsetEnableAngleProp.floatValue);

		SerializedProperty openRandomAngleProp = property.FindPropertyRelative("openRandomAngle");
		if (openRandomXProp.boolValue)
		{
			openRandomAngleProp.boolValue = false;
			SerializedProperty leftRandomXProp = property.FindPropertyRelative("leftRandomX");
			rect.y += singleLineHeight;
			leftRandomXProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("左X随机范围"), leftRandomXProp.floatValue);

			SerializedProperty rightRandomXProp = property.FindPropertyRelative("rightRandomX");
			rect.y += singleLineHeight;
			rightRandomXProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("右X随机范围"), rightRandomXProp.floatValue);

		}

		rect.y += singleLineHeight;
		openRandomAngleProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("开启随机角度范围"), openRandomAngleProp.boolValue);
		if(openRandomAngleProp.boolValue)
		{
			openRandomXProp.boolValue = false;
			SerializedProperty openRandomAngleArrayProp = property.FindPropertyRelative("openRandomAngleArray");
			rect.y += singleLineHeight;
			openRandomAngleArrayProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("开启随机角度数组"), openRandomAngleArrayProp.boolValue);

			SerializedProperty angleArrayProp = property.FindPropertyRelative("angleArray");
			if (openRandomAngleArrayProp.boolValue)
			{
				rect.y += singleLineHeight;
				if(angleArrayList == null)
				{
					angleArrayList = BuildAngleArrayReorderableList(angleArrayProp);
				}
				angleArrayList.DoList(rect);				
			}

			rect.y += singleLineHeight;
			EditorGUI.BeginDisabledGroup(openRandomAngleArrayProp.boolValue == true);

			if(openRandomAngleArrayProp.boolValue)
			{
				rect.y += angleArrayProp.arraySize * singleLineHeight + 30;
			}
			else
			{
				rect.y += singleLineHeight;
			}
			SerializedProperty minAngleProp = property.FindPropertyRelative("minAngle");
			minAngleProp.intValue = EditorGUI.IntField(rect, new GUIContent("最小随机角度"), minAngleProp.intValue);

			rect.y += singleLineHeight;
			SerializedProperty maxAngleProp = property.FindPropertyRelative("maxAngle");
			maxAngleProp.intValue = EditorGUI.IntField(rect, new GUIContent("最大随机角度"), maxAngleProp.intValue);
			EditorGUI.EndDisabledGroup();

			rect.y += singleLineHeight;
			SerializedProperty minRadiusProp = property.FindPropertyRelative("minRadius");
			minRadiusProp.intValue = EditorGUI.IntField(rect, new GUIContent("最小随机半径"), minRadiusProp.intValue);

			rect.y += singleLineHeight;
			SerializedProperty maxRadiusProp = property.FindPropertyRelative("maxRadius");
			maxRadiusProp.intValue = EditorGUI.IntField(rect, new GUIContent("最大随机半径"), maxRadiusProp.intValue);

		}

		EditorGUI.BeginDisabledGroup(openRandomAngleProp.boolValue == true);
		rect.y += singleLineHeight;
		SerializedProperty endVectorProp = property.FindPropertyRelative("endVector");
		endVectorProp.vector3Value = EditorGUI.Vector3Field(rect, new GUIContent("结束位置"), endVectorProp.vector3Value);
		EditorGUI.EndDisabledGroup();

		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}

	private ReorderableList BuildAngleArrayReorderableList(SerializedProperty property)
	{
		ReorderableList list = new ReorderableList(property.serializedObject, property, true, true, true, true);

		list.drawHeaderCallback = (Rect rect) =>
		{
			EditorGUI.LabelField(rect, "角度数组");
		};
		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			EditorGUI.PropertyField(rect, property.GetArrayElementAtIndex(index), true);
		};
		return list;
	}

}


[CustomPropertyDrawer(typeof(Es_LocalRotationAnimCommand))]
public class Es_LocalRotationDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Local Rotation"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
		rect.y += singleLineHeight;
		curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);

		rect.y += singleLineHeight;
		SerializedProperty startVectorProp = property.FindPropertyRelative("startVector");
		startVectorProp.vector3Value = EditorGUI.Vector3Field(rect, new GUIContent("开始旋转"), startVectorProp.vector3Value);

		rect.y += singleLineHeight;
		SerializedProperty endVectorProp = property.FindPropertyRelative("endVector");
		endVectorProp.vector3Value = EditorGUI.Vector3Field(rect, new GUIContent("结束旋转"), endVectorProp.vector3Value);

		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;

	}
}


[CustomPropertyDrawer(typeof(Es_ScaleAnimCommand))]
public class Es_ScaleDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Scale"), property);
		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		SerializedProperty curveTypeProp = property.FindPropertyRelative("curveType");
		rect.y += singleLineHeight;
		curveTypeProp.enumValueIndex = (int)(Es_PlayAnimationCommand.CurveType)(EditorGUI.EnumPopup(rect, new GUIContent("动画曲线类型"), (Es_PlayAnimationCommand.CurveType)curveTypeProp.enumValueIndex));

		switch (curveTypeProp.enumValueIndex)
		{
			case (int)Es_PlayAnimationCommand.CurveType.Time_Progress:
				{
					rect.y += singleLineHeight;
					SerializedProperty startVectorProp = property.FindPropertyRelative("startVector");
					startVectorProp.vector3Value = EditorGUI.Vector3Field(rect, new GUIContent("开始尺寸"), startVectorProp.vector3Value);

					rect.y += singleLineHeight;
					SerializedProperty endVectorProp = property.FindPropertyRelative("endVector");
					endVectorProp.vector3Value = EditorGUI.Vector3Field(rect, new GUIContent("结束尺寸"), endVectorProp.vector3Value);

					rect.y += singleLineHeight;
					SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
					curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);
				}
				break;
			case (int)Es_PlayAnimationCommand.CurveType.Time_Value:
				{
					rect.y += singleLineHeight;
					SerializedProperty curve_xTime_yValueProp = property.FindPropertyRelative("curve_xTime_yValue");
					curve_xTime_yValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-值"), curve_xTime_yValueProp.animationCurveValue);
					rect.y += singleLineHeight;
					rect.y += singleLineHeight;

				}
				break;
			default:
				rect.y += singleLineHeight;
				rect.y += singleLineHeight;
				rect.y += singleLineHeight;
				break;
		}


		EditorGUI.EndProperty();
		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}
}

[CustomPropertyDrawer(typeof(Es_SingleMoveXCommand))]
public class Es_SingleMoveXDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Single Move X"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
		rect.y += singleLineHeight;
		curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect,new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);

		SerializedProperty starXProp = property.FindPropertyRelative("starX");
		rect.y += singleLineHeight;
		starXProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("X开始位置"), starXProp.floatValue);

		SerializedProperty endXProp = property.FindPropertyRelative("endX");
		rect.y += singleLineHeight;
		endXProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("Y开始位置"), endXProp.floatValue);
		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;

	}
}

[CustomPropertyDrawer(typeof(Es_SingleMoveYCommand))]
public class Es_SingleMoveYDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Single Move Y"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
		rect.y += singleLineHeight;
		curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);

		SerializedProperty starYProp = property.FindPropertyRelative("starY");
		rect.y += singleLineHeight;
		starYProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("Y开始位置"), starYProp.floatValue);

		SerializedProperty endYProp = property.FindPropertyRelative("endY");
		rect.y += singleLineHeight;
		endYProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("Y结束位置"), endYProp.floatValue);

		SerializedProperty offsetFromUpYProp = property.FindPropertyRelative("offsetFromUpY");
		rect.y += singleLineHeight;
		offsetFromUpYProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("朝上偏移量"), offsetFromUpYProp.floatValue);

		SerializedProperty offsetFromDownYProp = property.FindPropertyRelative("offsetFromDownY");
		rect.y += singleLineHeight;
		offsetFromDownYProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("朝下偏移量"), offsetFromDownYProp.floatValue);

		SerializedProperty offsetEnableAngleProp = property.FindPropertyRelative("offsetEnableAngle");
		rect.y += singleLineHeight;
		offsetEnableAngleProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("偏移量生效在屏幕Y方向的夹角"), offsetEnableAngleProp.floatValue);

		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}
}

[CustomPropertyDrawer(typeof(Es_FadeCommand))]
public class Es_FadeDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Fade"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		SerializedProperty animCurveTypeProp = property.FindPropertyRelative("curveType");
		rect.y += singleLineHeight;
		animCurveTypeProp.enumValueIndex = (int)(Es_PlayAnimationCommand.CurveType)EditorGUI.EnumPopup(rect, new GUIContent("动画曲线类型"), (Es_PlayAnimationCommand.CurveType)animCurveTypeProp.enumValueIndex);

		if(animCurveTypeProp.enumValueIndex == (int)Es_PlayAnimationCommand.CurveType.Time_Progress)
		{
			rect.y += singleLineHeight;
			SerializedProperty startValueProp = property.FindPropertyRelative("startValue");
			startValueProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("开始透明度"), startValueProp.floatValue);

			rect.y += singleLineHeight;
			SerializedProperty endValueProp = property.FindPropertyRelative("endValue");
			endValueProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("结束透明度"), endValueProp.floatValue);

			rect.y += singleLineHeight;
			SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
			curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);

		}
		else if(animCurveTypeProp.enumValueIndex == (int)Es_PlayAnimationCommand.CurveType.Time_Value)
		{
			rect.y += singleLineHeight;
			SerializedProperty curve_xTime_yValueProp = property.FindPropertyRelative("curve_xTime_yValue");
			curve_xTime_yValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-值"), curve_xTime_yValueProp.animationCurveValue);
			rect.y += singleLineHeight;
			rect.y += singleLineHeight;

		}
		else
		{
			rect.y += singleLineHeight;
			rect.y += singleLineHeight;
			rect.y += singleLineHeight;
		}

		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}
}

[CustomPropertyDrawer(typeof(Es_FourCornersMeshColorCommand))]
public class Es_FourCornersMeshColorDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Four Corners Mesh Color"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
		rect.y += singleLineHeight;
		curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);

		SerializedProperty startLeftUpColorProp = property.FindPropertyRelative("startLeftUpColor");
		rect.y += singleLineHeight;
		startLeftUpColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("左上Start"), startLeftUpColorProp.colorValue);

		SerializedProperty startLeftDownColorProp = property.FindPropertyRelative("startLeftDownColor");
		rect.y += singleLineHeight;
		startLeftDownColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("左下Start"), startLeftDownColorProp.colorValue);

		SerializedProperty startRightUpColorProp = property.FindPropertyRelative("startRightUpColor");
		rect.y += singleLineHeight;
		startRightUpColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("右上Start"), startRightUpColorProp.colorValue);

		SerializedProperty startRightDownColorProp = property.FindPropertyRelative("startRightDownColor");
		rect.y += singleLineHeight;
		startRightDownColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("右下Start"), startRightDownColorProp.colorValue);

		SerializedProperty endLeftUpColorProp = property.FindPropertyRelative("endLeftUpColor");
		rect.y += singleLineHeight;
		endLeftUpColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("左上End"), endLeftUpColorProp.colorValue);

		SerializedProperty endLeftDownColorProp = property.FindPropertyRelative("endLeftDownColor");
		rect.y += singleLineHeight;
		endLeftDownColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("左下End"), endLeftDownColorProp.colorValue);

		SerializedProperty endRightUpColorProp = property.FindPropertyRelative("endRightUpColor");
		rect.y += singleLineHeight;
		endRightUpColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("右上End"), endRightUpColorProp.colorValue);

		SerializedProperty endRightDownColorProp = property.FindPropertyRelative("endRightDownColor");
		rect.y += singleLineHeight;
		endRightDownColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("右下End"), endRightDownColorProp.colorValue);

		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}
}

[CustomPropertyDrawer(typeof(Es_OutlineChangeCommand))]
public class Es_OutlineChangeDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Outline Change"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
		rect.y += singleLineHeight;
		curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);

		SerializedProperty startOutlineColorProp = property.FindPropertyRelative("startOutlineColor");
		rect.y += singleLineHeight;
		startOutlineColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("开始外边框颜色"), startOutlineColorProp.colorValue);

		SerializedProperty endOutlineColorProp = property.FindPropertyRelative("endOutlineColor");
		rect.y += singleLineHeight;
		endOutlineColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("结束外边框颜色"), endOutlineColorProp.colorValue);

		SerializedProperty thickChangeProp = property.FindPropertyRelative("thickChange");
		rect.y += singleLineHeight;
		thickChangeProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("开启边框粗细渐变（0-1）"), thickChangeProp.boolValue);

		SerializedProperty softnessProp = property.FindPropertyRelative("softness");
		rect.y += singleLineHeight;
		softnessProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("开启边框柔和渐变（0-1）"), softnessProp.boolValue);

		if(thickChangeProp.boolValue)
		{
			SerializedProperty startThickProp = property.FindPropertyRelative("startThick");
			rect.y += singleLineHeight;
			startThickProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("开始粗细"), startThickProp.floatValue);

			SerializedProperty endThickProp = property.FindPropertyRelative("endThick");
			rect.y += singleLineHeight;
			endThickProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("结束粗细"), endThickProp.floatValue);

		}

		if (softnessProp.boolValue)
		{
			SerializedProperty startSoftProp = property.FindPropertyRelative("startSoft");
			rect.y += singleLineHeight;
			startSoftProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("开始柔和度"), startSoftProp.floatValue);

			SerializedProperty endSoftProp = property.FindPropertyRelative("endSoft");
			rect.y += singleLineHeight;
			endSoftProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("结束柔和度"), endSoftProp.floatValue);

		}
		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}


}

[CustomPropertyDrawer(typeof(Es_ShineTextCommand))]
public class Es_ShineTextDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Shine Text"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
		rect.y += singleLineHeight;
		curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);

		SerializedProperty startShineProp = property.FindPropertyRelative("startShine");
		SerializedProperty endShineProp = property.FindPropertyRelative("endShine");
		SerializedProperty showShinePowerProp = property.FindPropertyRelative("showShinePower");

		rect.y += singleLineHeight;
		startShineProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("开始发光颜色"), startShineProp.colorValue);

		rect.y += singleLineHeight;
		endShineProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("结束发光颜色"), endShineProp.colorValue);

		rect.y += singleLineHeight;
		showShinePowerProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("发光强度（0-1）"), showShinePowerProp.boolValue);

		if (showShinePowerProp.boolValue)
		{
			SerializedProperty startPowerProp = property.FindPropertyRelative("startPower");
			SerializedProperty endPowerProp = property.FindPropertyRelative("endPower");

			rect.y += singleLineHeight;
			startPowerProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("开始强度"), startPowerProp.floatValue);

			rect.y += singleLineHeight;
			endPowerProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("结束强度"), endPowerProp.floatValue);

		}

		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}
}

[CustomPropertyDrawer(typeof(Es_ParabolaCommand))]
public class Es_ParabolaDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	GUIStyle style;

	public Es_ParabolaDrawer()
	{
		style = new GUIStyle();
		style.normal.textColor = Color.black;

	}
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Parabola"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		SerializedProperty durationProp = property.FindPropertyRelative("duration");
		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		SerializedProperty curve_xTime_ySpeedProp = property.FindPropertyRelative("curve_xTime_ySpeed");
		SerializedProperty startVectorProp = property.FindPropertyRelative("startVector");
		SerializedProperty parapola_horizontalVelocityProp = property.FindPropertyRelative("parapola_horizontalVelocity");
		SerializedProperty parapola_paramMProp = property.FindPropertyRelative("parapola_paramM");
		SerializedProperty parapola_paramNProp = property.FindPropertyRelative("parapola_paramN");

		rect.y += singleLineHeight;
		curve_xTime_ySpeedProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-速度"), curve_xTime_ySpeedProp.animationCurveValue);

		rect.y += singleLineHeight;
		startVectorProp.vector3Value = EditorGUI.Vector3Field(rect, new GUIContent("开始位置"), startVectorProp.vector3Value);

		rect.y += singleLineHeight;
		parapola_horizontalVelocityProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("抛物线横向基准速度"), parapola_horizontalVelocityProp.floatValue);

		rect.y += singleLineHeight;
		EditorGUI.TextField(rect, "抛物线轨迹方程为：y = (x - m) ^ 2 / -n + m ^ 2 / n", style);

		rect.y += singleLineHeight;
		parapola_paramMProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("抛物线参数 m (填大于0的数)"), parapola_paramMProp.floatValue);

		rect.y += singleLineHeight;
		parapola_paramNProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("抛物线参数 n (填大于0的数)"), parapola_paramNProp.floatValue);

		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}
}

[CustomPropertyDrawer(typeof(Es_UIBlurCommand))]
public class Es_UIBlurDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("UI Blur"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
		SerializedProperty startValueProp = property.FindPropertyRelative("startValue");
		SerializedProperty endValueProp = property.FindPropertyRelative("endValue");
		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		SerializedProperty durationProp = property.FindPropertyRelative("duration");

		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		rect.y += singleLineHeight;
		curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);

		rect.y += singleLineHeight;
		startValueProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("开始模糊度"), startValueProp.floatValue);

		rect.y += singleLineHeight;
		endValueProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("结束模糊度"), endValueProp.floatValue);

		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}
}

[CustomPropertyDrawer(typeof(Es_AnimationCommand))]
public class Es_AnimationDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var manager = property.serializedObject.targetObject as TweenAnimsManager;

		EditorGUI.BeginProperty(position, new GUIContent("Animation"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		rect.y += singleLineHeight;
		if(GUI.Button(rect, "创建Animation"))
		{
			if(manager != null)
			{
				Animation anim = manager.TargetTransform.GetComponent<Animation>();
				if (anim == null)
				{
					anim = manager.gameObject.AddComponent<Animation>();
				}
			}
		}
		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}
}

[CustomPropertyDrawer(typeof(Es_TextMeshColorCommand))]
public class Es_TextMeshColorDrawer : PropertyDrawer
{
	protected float propHeight = 0;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return propHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, new GUIContent("Single Move X"), property);

		propHeight = 0;

		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

		Rect rect = position;
		rect.width = 500;
		rect.height = singleLineHeight;
		rect.y += singleLineHeight;

		SerializedProperty commandEnabledProp = property.FindPropertyRelative("commandEnabled");
		rect.y += singleLineHeight;
		commandEnabledProp.boolValue = EditorGUI.Toggle(rect, new GUIContent("Enabled"), commandEnabledProp.boolValue);

		SerializedProperty curve_xTime_yProgressValueProp = property.FindPropertyRelative("curve_xTime_yProgressValue");
		SerializedProperty startColorProp = property.FindPropertyRelative("startColor");
		SerializedProperty endColorProp = property.FindPropertyRelative("endColor");
		SerializedProperty delayProp = property.FindPropertyRelative("delay");
		SerializedProperty durationProp = property.FindPropertyRelative("duration");

		rect.y += singleLineHeight;
		curve_xTime_yProgressValueProp.animationCurveValue = EditorGUI.CurveField(rect, new GUIContent("动画曲线：时间-进度"), curve_xTime_yProgressValueProp.animationCurveValue);

		rect.y += singleLineHeight;
		startColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("开始颜色"), startColorProp.colorValue);

		rect.y += singleLineHeight;
		endColorProp.colorValue = EditorGUI.ColorField(rect, new GUIContent("开始颜色"), endColorProp.colorValue);

		rect.y += singleLineHeight;
		delayProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("延迟"), delayProp.floatValue);

		rect.y += singleLineHeight;
		durationProp.floatValue = EditorGUI.FloatField(rect, new GUIContent("持续时间"), durationProp.floatValue);

		EditorGUI.EndProperty();

		propHeight = rect.y - position.y + 2 * singleLineHeight;
	}
}

//[CustomPropertyDrawer(typeof(xxx))]
//public class xxxDrawer : PropertyDrawer
//{
//	protected float propHeight = 0;

//	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//	{
//		return propHeight;
//	}

//	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//	{
//		EditorGUI.BeginProperty(position, new GUIContent("Single Move X"), property);

//		propHeight = 0;

//		float singleLineHeight = (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

//		Rect rect = position;
//		rect.width = 500;
//		rect.height = singleLineHeight;

//		EditorGUI.EndProperty();

//		propHeight = rect.y - position.y + 2 * singleLineHeight;
//	}
//}
