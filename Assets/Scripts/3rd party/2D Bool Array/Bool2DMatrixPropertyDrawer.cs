using UnityEngine;
using UnityEditor;
using System.Collections;

// Source: https://www.youtube.com/watch?v=uoHc-Lz9Lsc&ab_channel=SumeetKhobare
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ArrayLayout))]
public class Bool2DMatrixPropertyDrawer : PropertyDrawer
{
	private const float checkboxSize = 18f;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.PrefixLabel(position, label);
		Rect newposition = position;
		newposition.y += checkboxSize;
		SerializedProperty data = property.FindPropertyRelative("rows");

		for (int i = 0; i < ArrayLayout.size; i++)
		{
			SerializedProperty row = data.GetArrayElementAtIndex(i).FindPropertyRelative("row");

			newposition.height = checkboxSize;
			newposition.x = checkboxSize;

			row.arraySize = ArrayLayout.size;
			for (int j = 0; j < ArrayLayout.size; j++)
			{
				EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(j), GUIContent.none);
				newposition.x += checkboxSize;
			}

			newposition.x = position.x;
			newposition.y += checkboxSize;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return checkboxSize * (ArrayLayout.size + 1);
	}
}
# endif