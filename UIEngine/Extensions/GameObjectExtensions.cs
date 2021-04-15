using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UIEngine.Extensions
{
    public static class GameObjectExtensions
    {

        public static GameObject GetChildByName(this GameObject go, string name)
        {
            if (name.Contains('/')) return go.GetChildByRelativePath(name);

            for(int i = 0; i < go.transform.childCount; i ++)
            {
                Transform child = go.transform.GetChild(i);
                if (child.name.Equals(name)) return child.gameObject;
            }
            return null;
        }

        public static GameObject GetChildByRelativePath(this GameObject go, string path)
        {
            var childrenChain = path.Split('/');
            GameObject child = null;
            foreach (string childName in childrenChain)
            {
                if (child == null)
                {
                    child = go.GetChildByName(childName);
                    continue;
                }
                child = child.GetChildByName(childName);
            }
            return child;
        }

        public static T GetComponentOnChild<T>(this GameObject go, string name) where T : Component
        {
            GameObject child = go.GetChildByName(name);

            return child?.GetComponent<T>();
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            var component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();
            return component;
        }

    }
}
