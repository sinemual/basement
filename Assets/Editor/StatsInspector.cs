using System;
using Data;
using Leopotam.Ecs;
using Leopotam.Ecs.UnityIntegration.Editor;
using UnityEditor;

namespace Client.ECS.CurrentGame.Player
{
    class StatsInspector :  IEcsComponentInspector
    {
        Type IEcsComponentInspector.GetFieldType()
        {
            return typeof(Stats);
        }

        void IEcsComponentInspector.OnGUI(string label, object value, EcsWorld world, ref EcsEntity entityId)
        {
            var component = value is Stats ? (Stats)value : default;
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            foreach (var stat in component.Value)
                EditorGUILayout.LabelField($"{stat.Key}:", $"{stat.Value}");
            EditorGUI.indentLevel--;
        }
    }
}