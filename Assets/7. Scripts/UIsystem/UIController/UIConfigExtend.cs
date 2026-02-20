using System.Collections.Generic;
using System;
using UnityEngine;
using JS;

public enum UIAnimationType
{
    FadeScale,
    SlideLeft,
    SlideRight,
    SlideTop,
    SlideBottom
}

namespace JS
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Scriptable Objects/UI ConfigExtend", order = 0)]
    public class UIConfigExtend : ScriptableObject
    {
        [Serializable]
        public class UIPairExtend
        {
            public UIBase prefab;                 // 👈 prefab quyết định TYPE
            public CanvasType canvasType;
            public UIAnimationType animationType = UIAnimationType.FadeScale;
            public bool preload = false;
            public bool destroyAfterHide = false;
        }

        [SerializeField] private List<UIPairExtend> uiList = new();

        private Dictionary<Type, UIPairExtend> dict;

        private void BuildDict()
        {
            dict = new Dictionary<Type, UIPairExtend>();
            foreach (var pair in uiList)
            {
                if (pair.prefab == null) continue;

                var type = pair.prefab.GetType();
                if (!dict.ContainsKey(type))
                    dict[type] = pair;
            }
        }

        public UIPairExtend Get(Type uiType)
        {
            if (dict == null)
                BuildDict();

            dict.TryGetValue(uiType, out var pair);
            return pair;
        }

        public List<UIPairExtend> GetAll()
        {
            if (dict == null)
                BuildDict();
            return uiList;
        }
    }
}
