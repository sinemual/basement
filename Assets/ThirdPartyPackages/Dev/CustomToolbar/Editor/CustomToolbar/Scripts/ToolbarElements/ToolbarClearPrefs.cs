using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityToolbarExtender;

[Serializable]
internal class ToolbarClearPrefs : BaseToolbarElement {
	private static GUIContent clearPlayerPrefsBtn;

	public override string NameInList => "[Button] Clear player data";

	public override void Init() {
		clearPlayerPrefsBtn = EditorGUIUtility.IconContent("SaveFromPlay");
		clearPlayerPrefsBtn.tooltip = "Clear player data";
	}

	protected override void OnDrawInList(Rect position) {

	}

	protected override void OnDrawInToolbar() {
		if (GUILayout.Button(clearPlayerPrefsBtn, ToolbarStyles.commandButtonStyle)) {
			if (Directory.Exists(Path.Combine(Application.persistentDataPath, "SaveData")))
				Directory.Delete(Path.Combine(Application.persistentDataPath, "SaveData"), true);
			PlayerPrefs.DeleteAll();
			Debug.Log("Clear Player Data");
		}
	}
}
