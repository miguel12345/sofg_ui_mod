using System;
using System.Collections.Specialized;
using Assets.Code;
using UnityEngine;
using UnityEngine.UI;
using Action = Assets.Code.Action;

namespace UIImprovements
{
    public class Utils
    {
        public static string GetUIRectTransformProperties(RectTransform rectTransform)
        {
            // Concatenating all properties into a single string
            string properties = "Anchors Min: " + rectTransform.anchorMin +
                                ", Anchors Max: " + rectTransform.anchorMax +
                                ", Pivot: " + rectTransform.pivot +
                                ", Position: " + rectTransform.localPosition; // Using localPosition

            return properties;
        }

        public static void PrintUITextProperties(Text text)
        {
            var properties = GetUIRectTransformProperties(text.GetComponent<RectTransform>());
            properties += $", Alignment: {text.alignment.ToString()}";
            Debug.LogWarning(properties);
        }

        public static void PrintAllSiblingNames(GameObject go)
        {  
            for (int i = 0; i < go.transform.parent.childCount; i++)
            {
                Debug.LogWarning($"Sibling {i}: {go.transform.parent.GetChild(i).name}");
            }
        }


        public static GameObject CopyAsSiblingIfNotExists(GameObject original, string name)
        {
            var copiedTransform = original.transform.parent.Find(name);

            if(copiedTransform != null) {
                return copiedTransform.gameObject;
            }

            var copy = GameObject.Instantiate(original, original.transform.parent);
            copy.name = name;
            // Get the sibling index of GameObject A
            int originalTextSiblingIndex = original.transform.GetSiblingIndex();

            // Set GameObject B right below GameObject A
            copy.transform.SetSiblingIndex(originalTextSiblingIndex + 1);

            return copy;
        }

        public static void CopyRectTransform(RectTransform source, RectTransform target)
        {
            if (source == null || target == null)
            {
                Debug.LogError("Source or Target RectTransform is null.");
                return;
            }

            // Copy position
            target.anchoredPosition = source.anchoredPosition;
            target.anchoredPosition3D = source.anchoredPosition3D;

            // Copy size
            target.sizeDelta = source.sizeDelta;

            // Copy scale
            target.localScale = source.localScale;

            // Copy rotation
            target.localRotation = source.localRotation;

            // Copy anchor and pivot
            target.anchorMin = source.anchorMin;
            target.anchorMax = source.anchorMax;
            target.pivot = source.pivot;

            // Copy offset if needed
            target.offsetMin = source.offsetMin;
            target.offsetMax = source.offsetMax;
        }

        public static Transform FindChildStrict(Transform parent, string childName){
            var child = parent.Find(childName);
            if(child == null){
                throw new Exception($"Child {childName} not found in {parent.name}");
            }
            return child;
        }
    }
}
