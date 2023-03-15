using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

//using OdinSerializer;
//using OdinSerializer.Utilities;


#if MY_SERIALIZER
namespace Meryel.UnityCodeAssist.Editor
{

    //**--implement this and get rid of odinserializer.dll
    class MySerializer
    {
        public static void Serialize(Object obj)
        {

        }

        static bool IsTypeCompatible(Type type)
        {
            if (type == null || !(type.IsSubclassOf(typeof(MonoBehaviour)) || type.IsSubclassOf(typeof(ScriptableObject))))
                return false;
            return true;
        }

        void ShowFieldInfo(Type type)//, MonoImporter importer, List<string> names, List<Object> objects, ref bool didModify)
        {
            // Only show default properties for types that support it (so far only MonoBehaviour derived types)
            if (!IsTypeCompatible(type))
                return;

            ShowFieldInfo(type.BaseType);//, importer, names, objects, ref didModify);

            FieldInfo[] infos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (FieldInfo field in infos)
            {
                if (!field.IsPublic)
                {
                    object[] attr = field.GetCustomAttributes(typeof(SerializeField), true);
                    if (attr == null || attr.Length == 0)
                        continue;
                }

                /*
                if (field.FieldType.IsSubclassOf(typeof(Object)) || field.FieldType == typeof(Object))
                {
                    Object oldTarget = importer.GetDefaultReference(field.Name);
                    Object newTarget = EditorGUILayout.ObjectField(ObjectNames.NicifyVariableName(field.Name), oldTarget, field.FieldType, false);

                    names.Add(field.Name);
                    objects.Add(newTarget);

                    if (oldTarget != newTarget)
                        didModify = true;
                }
                */

                if (field.FieldType.IsValueType && field.FieldType.IsPrimitive && !field.FieldType.IsEnum)
                {

                }
                else if (field.FieldType == typeof(string))
                {

                }
            }
        }


        void ShowFieldInfo(Type type, Object obj, List<(string, object)> fields)//, MonoImporter importer, List<string> names, List<Object> objects, ref bool didModify)
        {
            // Only show default properties for types that support it (so far only MonoBehaviour derived types)
            if (!IsTypeCompatible(type))
                return;

            ShowFieldInfo(type.BaseType, obj, fields);//, importer, names, objects, ref didModify);

            FieldInfo[] infos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            foreach (FieldInfo field in infos)
            {
                if (!field.IsPublic)
                {
                    object[] attr = field.GetCustomAttributes(typeof(SerializeField), true);
                    if (attr == null || attr.Length == 0)
                        continue;
                }

                /*
                if (field.FieldType.IsSubclassOf(typeof(Object)) || field.FieldType == typeof(Object))
                {
                    Object oldTarget = importer.GetDefaultReference(field.Name);
                    Object newTarget = EditorGUILayout.ObjectField(ObjectNames.NicifyVariableName(field.Name), oldTarget, field.FieldType, false);

                    names.Add(field.Name);
                    objects.Add(newTarget);

                    if (oldTarget != newTarget)
                        didModify = true;
                }
                */

                if (field.FieldType.IsValueType && field.FieldType.IsPrimitive && !field.FieldType.IsEnum)
                {
                    var val = field.GetValue(obj);
                    fields.Add((field.Name, val));
                }
                else if (field.FieldType == typeof(string))
                {
                    var val = field.GetValue(obj);
                    fields.Add((field.Name, val));
                }
            }
        }



    }
}

#endif