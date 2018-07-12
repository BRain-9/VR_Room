using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HotKeys : EditorWindow
{

	[MenuItem("HotKeys/SwitchObjects _`")]
	private static void Switch(){
		for (int i = 0; i < Selection.gameObjects.Length; i++){
			Selection.gameObjects[i].SetActive(!Selection.gameObjects[i].activeInHierarchy);
		}
	}
}
