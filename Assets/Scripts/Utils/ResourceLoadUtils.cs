using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public static class ResourceLoadUtils
    {
        public static List<T> GetAllScriptableObjects<T>(string path) where T : ScriptableObject
        {
            return Resources.LoadAll<T>(nameof(ScriptableObjects) + path).ToList();
        }
    }
}
