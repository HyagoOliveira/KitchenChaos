using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


namespace Meryel.UnityCodeAssist.Editor
{
    public class StatusWindow : EditorWindow
    {
        public static void Display()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<StatusWindow>();
            window.Show();

            NetMQInitializer.Publisher.SendConnectionInfo();

            Serilog.Log.Debug("Displaying status window");
        }

        private void OnEnable()
        {
            //**--icon
            //var icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Sprites/Gear.png");
            //titleContent = new GUIContent("Code Assist", icon);
            titleContent = new GUIContent(Assister.Title);
        }

        private void OnGUI()
        {
            var hasAnyClient = NetMQInitializer.Publisher.clients.Any();

            if (hasAnyClient)
            {
                EditorGUILayout.LabelField($"Code Assist is working!");

                foreach (var client in NetMQInitializer.Publisher.clients)
                {
                    EditorGUILayout.LabelField($"Connected to {client.ContactInfo}");
                }
            }
            else
            {
                EditorGUILayout.LabelField($"Code Assist isn't working!");

                EditorGUILayout.LabelField($"No IDE found");
            }

#if MERYEL_UCA_LITE_VERSION

            EditorGUILayout.LabelField($"");
            EditorGUILayout.LabelField($"This is the lite version of Code Assist with limited features.");
            EditorGUILayout.LabelField($"To unlock all of the features, get the full version.");

#endif // MERYEL_UCA_LITE_VERSION


        }
    }

}