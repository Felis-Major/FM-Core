using Daniell.Runtime.Helpers.GUIDSystem;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Daniell.Editor.GUIDSystem
{
    public static class GUIDGenerator
    {
        /// <summary>
        /// Regenerate GUIDs for all fields marked with the GUID Attribute
        /// </summary>
        [MenuItem("Daniell/GUID Generator/Regenerate Scene GUIDs")]
        public static void RegenerateSceneGUIDs()
        {
            string generatedGUIDsLog = "";

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (FieldInfo field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        if (field.GetCustomAttribute<GUIDAttribute>() != null)
                        {
                            var instances = GameObject.FindObjectsOfType(type);
                            foreach (var v in instances)
                            {
                                field.SetValue(v, GenerateGUID());
                                EditorUtility.SetDirty(v);
                                generatedGUIDsLog += $"Generated new GUID for {v.name} -> {type.Name} Component\n";
                            }
                        }
                    }
                }
            }

            Debug.Log($"GUIDs successfully regenerated.\n{generatedGUIDsLog}");
        }

        /// <summary>
        /// Generate a random GUID
        /// </summary>
        /// <returns>GUID as a string</returns>
        public static string GenerateGUID()
        {
            return GUID.Generate().ToString();
        }
    }
}