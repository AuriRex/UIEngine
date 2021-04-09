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
            for(int i = 0; i < go.transform.childCount; i ++)
            {
                Transform child = go.transform.GetChild(i);
                if (child.name.Equals(name)) return child.gameObject;
            }
            return null;
        }

        public static T GetComponentOnChild<T>(this GameObject go, string name) where T : Component
        {
            GameObject child = go.GetChildByName(name);

            return child?.GetComponent<T>();
        }

    }
}
