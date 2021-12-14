using UnityEngine;
using UnityEditor;
using System.Collections;

// Source: https://www.youtube.com/watch?v=uoHc-Lz9Lsc&ab_channel=SumeetKhobare

[CustomPropertyDrawer(typeof(ArrayLayout))]
public class Bool2DMatrixPropertyDrawer : PropertyDrawer
{
	private const int size = 4;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.PrefixLabel(position, label);
		Rect newposition = position;
		newposition.y += 18f;
		SerializedProperty data = property.FindPropertyRelative("rows");
		//data.rows[0][]

		for (int i = 0; i < size; i++)
		{
			SerializedProperty row = data.GetArrayElementAtIndex(i).FindPropertyRelative("row");

			newposition.height = 18f;
			newposition.x = 18f;

			row.arraySize = size;
			for (int j = 0; j < size; j++)
			{
				EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(j), GUIContent.none);
				newposition.x += 18f;
			}

			newposition.x = position.x;
			newposition.y += 18f;
		}
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return 18f * (size + 1);
	}
}