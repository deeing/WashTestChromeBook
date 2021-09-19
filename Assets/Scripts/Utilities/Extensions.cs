using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NuclearOption.Utilities
{
    public static class Extensions
    {
        #region Arrays

        public static T RandomElement<T>(this T[] arr)
        {
            return arr[Random.Range(0, arr.Length)];
        }

        #endregion

        #region Lists

        public static T RandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        #endregion

        #region floats

        public static void Clamp(this ref float val, float min, float max)
        {
            val.ClampLower(min);
            val.ClampUpper(max);
        }

        public static void ClampLower(this ref float val, float min)
        {
            if (val < min)
            {
                val = min;
            }
        }

        public static void ClampUpper(this ref float val, float max)
        {
            if (val > max)
            {
                val = max;
            }
        }

        #endregion

        #region ints

        public static void Clamp(this ref int val, int min, int max)
        {
            val.ClampLower(min);
            val.ClampUpper(max);
        }

        public static void ClampLower(this ref int val, int min)
        {
            if (val < min)
            {
                val = min;
            }
        }

        public static void ClampUpper(this ref int val, int max)
        {
            if (val > max)
            {
                val = max;
            }
        }

        #endregion

        #region Enums
        // This allows you to add a "description" annotation
        // over enums and to retreive it by calling this method
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
