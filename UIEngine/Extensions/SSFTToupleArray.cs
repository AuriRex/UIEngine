using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Configuration;

namespace UIEngine.Extensions
{
    public static class SSFTToupleArray
    {

        public static (string, string, float, Type)[] WithFiltersApplied(this (string, string, float, Type)[] self, List<PluginConfig.FilterExclusion> filters)
        {
            List<(string, string, float, Type)> listToRemove = new List<(string, string, float, Type)>();
            List<(string, string, float, Type)> list = new List<(string, string, float, Type)>();

            foreach (var exclusion in filters)
            {
                switch(exclusion.ExclusionType)
                {
                    case PluginConfig.FilterExclusion.EXCLUSION_TYPE_PROPERTY:
                        foreach(var prop in self)
                        {
                            if (prop.Item2.Equals(exclusion.ExclusionTarget))
                                listToRemove.Add(prop);
                        }
                        break;
                    default:
                    case PluginConfig.FilterExclusion.EXCLUSION_TYPE_CHILD_GAMEOBJECT:
                        foreach (var prop in self)
                        {
                            if (prop.Item1.Equals(exclusion.ExclusionTarget))
                                listToRemove.Add(prop);
                        }
                        break;
                }
            }

            foreach(var exclusion in self)
            {
                if (!listToRemove.Contains(exclusion))
                    list.Add(exclusion);
            }

            return list.ToArray();
        }

    }
}
