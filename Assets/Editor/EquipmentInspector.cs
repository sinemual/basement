using System;
using Client.ECS.CurrentGame.PlayerEquipment.Components;
using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration.Editor;
using UnityEditor;

namespace Client.ECS.CurrentGame.Player
{
    class EquipmentInspector :  IEcsComponentInspector
    {
        Type IEcsComponentInspector.GetFieldType()
        {
            return typeof(Equipment);
        }

        void IEcsComponentInspector.OnGUI(string label, object value, EcsWorld world, ref EcsEntity entityId)
        {
            var dic = value is Equipment ? (Equipment)value : default;
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (var element in dic.Value)
                EditorGUILayout.LabelField($"{element.Key}:", $"{element.Value}");
            EditorGUI.indentLevel--;
        }
    }
}