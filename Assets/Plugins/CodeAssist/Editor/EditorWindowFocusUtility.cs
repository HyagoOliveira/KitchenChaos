using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Meryel.UnityCodeAssist.Editor
{

    //**--remove this
    //[InitializeOnLoad]
    public class EditorWindowFocusUtility
    {
        private static bool isAppFocused;

        private static HashSet<UnityEngine.Object> selectedObjects;

        private static int dirtyCounter;
        private static Dictionary<GameObject, int> dirtyDict;

        static EditorWindowFocusUtility()
        {
            EditorApplication.update += Update;
            Selection.selectionChanged += OnSelectionChanged;

            selectedObjects = new HashSet<UnityEngine.Object>();

            dirtyDict = new Dictionary<GameObject, int>();
            dirtyCounter = 0;
        }


        static void Update()
        {
            if (!isAppFocused && UnityEditorInternal.InternalEditorUtility.isApplicationActive)
            {
                isAppFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
                OnOnUnityEditorFocusChanged(true);
                Serilog.Log.Debug("On focus gain");
            }
            else if (isAppFocused && !UnityEditorInternal.InternalEditorUtility.isApplicationActive)
            {
                isAppFocused = UnityEditorInternal.InternalEditorUtility.isApplicationActive;
                OnOnUnityEditorFocusChanged(false);
                Serilog.Log.Debug("On focus lost");
            }
        }

        static void OnOnUnityEditorFocusChanged(bool isFocused)
        {
            if (!isFocused)
            {
                OnSelectionChanged();

                Serilog.Log.Debug("exporting {Count} objects", selectedObjects.Count);

                //**--if too many
                foreach (var obj in selectedObjects)
                {
                    if (obj is GameObject go)
                        NetMQInitializer.Publisher.SendGameObject(go);
                    else if (obj is ScriptableObject so)
                        NetMQInitializer.Publisher.SendScriptableObject(so);
                }

                selectedObjects.Clear();
            }
        }



        


        static void OnSelectionChanged()
        {
            //**--limit here, what if too many?
            selectedObjects.UnionWith(Selection.objects);
        }


        public static void MarkAsDirty(GameObject go)
        {
            dirtyCounter++;
            dirtyDict[go] = dirtyCounter;
        }

        static void FlushAllDirty()
        {
            // Sending order is important, must send them in the same order as they are added to/modified in the collection
            // Using dict instead of hashset because of that. Dict value is used as add/modify order

            var sortedDict = from entry in dirtyDict orderby entry.Value descending select entry;

            foreach (var entry in sortedDict)
            {
                var go = entry.Key;
                NetMQInitializer.Publisher.SendGameObject(go);
            }

            dirtyDict.Clear();
            dirtyCounter = 0;
        }

    }

}