using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace ActionCode.ScriptableSettingsProvider.Editor
{
    public static class PreloadedAsset
    {
        public static void Add(Object settings)
        {
            var preloadedAssets = Get();
            if (preloadedAssets.Contains(settings)) return;

            preloadedAssets.Add(settings);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());

            EditorUtility.DisplayDialog(
                title: "Preloaded Asset Addition",
                message: GetMessage(settings, "added into"),
                ok: "Okay"
            );
        }

        public static void Replace(Object oldSettings, Object newSettings)
        {
            var preloadedAssets = Get();
            var index = preloadedAssets.IndexOf(oldSettings);
            var hasOldSettings = index > -1;

            if (hasOldSettings)
            {
                preloadedAssets[index] = newSettings;
                var newName = GetAssetName(newSettings);

                EditorUtility.DisplayDialog(
                    title: "Preloaded Asset Replacement",
                    message: GetMessage(oldSettings, $"replaced by {newName} in"),
                    ok: "Okay"
                );
            }
            else
            {
                preloadedAssets.Add(newSettings);

                EditorUtility.DisplayDialog(
                    title: "Preloaded Asset Addition",
                    message: GetMessage(newSettings, "added into"),
                    ok: "Okay"
                );
            }

            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }

        public static void Remove(Object settings)
        {
            var preloadedAssets = Get();
            if (!preloadedAssets.Contains(settings)) return;

            preloadedAssets.Remove(settings);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());

            EditorUtility.DisplayDialog(
                title: "Preloaded Asset Removal",
                message: GetMessage(settings, "removed from"),
                ok: "Okay"
            );
        }

        public static List<Object> Get() => new List<Object>(PlayerSettings.GetPreloadedAssets());

        private static string GetAssetName(Object settings)
        {
            var path = AssetDatabase.GetAssetPath(settings);
            return Path.GetFileNameWithoutExtension(path);
        }

        private static string GetMessage(Object settings, string complement) =>
            string.Format("{0} was {1} Preloaded Assets.\nCheck it on your Project Settings, Optimization section.",
                GetAssetName(settings), complement);
    }
}