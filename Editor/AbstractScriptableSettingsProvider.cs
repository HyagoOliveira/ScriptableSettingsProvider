using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ActionCode.ScriptableSettingsProvider.Editor
{
    /// <summary>
    /// Abstract Provider for Menus created using Scriptable Objects.
    /// </summary>
    /// <typeparam name="T">A ScriptableObject class type.</typeparam>
    public abstract class AbstractScriptableSettingsProvider<T> : AssetSettingsProvider
        where T : ScriptableObject
    {
        /// <summary>
        /// The current Settings used for this project.
        /// </summary>
        public static T CurrentSettings
        {
            get
            {
                EditorBuildSettings.TryGetConfigObject(ConfigName, out T settings);
                return settings;
            }
            set
            {
                var remove = value == null;
                if (remove) EditorBuildSettings.RemoveConfigObject(ConfigName);
                else EditorBuildSettings.AddConfigObject(ConfigName, value, overwrite: true);
            }
        }

        private string searchContext;
        private VisualElement rootElement;
        private readonly bool isPreloadedAsset;

        private static string ConfigName = string.Empty;

        /// <summary>
        /// Creates a new menu at Project Settings.
        /// </summary>
        /// <param name="settingsWindowPath">
        /// Path of the settings in the Settings window. Uses "/" as separator. 
        /// The last token becomes the settings label if none is provided.
        /// <para>Do not prefix it with <b>Project/</b>.</para>
        /// </param>
        /// <param name="isPreloadedAsset">Whether to add/edit this asset into Preloaded Asset list.</param>
        public AbstractScriptableSettingsProvider(string settingsWindowPath, bool isPreloadedAsset = false) :
            base("Project/" + settingsWindowPath, () => CurrentSettings)
        {
            ConfigName = GetConfigName();
            keywords = GetSearchKeywordsFromGUIContentProperties<T>();
            this.isPreloadedAsset = isPreloadedAsset;
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            this.rootElement = rootElement;
            this.searchContext = searchContext;
            base.OnActivate(searchContext, rootElement);
        }

        public override void OnGUI(string searchContext)
        {
            DrawCurrentSettingsGUI();
            EditorGUILayout.Space();

            var invalidSettings = CurrentSettings == null;
            if (invalidSettings) DisplaySettingsCreationGUI();
            else base.OnGUI(searchContext);
        }

        protected abstract string GetConfigName();

        protected void RefreshEditor() => base.OnActivate(searchContext, rootElement);

        private void DrawCurrentSettingsGUI()
        {
            if (isPreloadedAsset)
            {
                const string message = "Your settings is marked as a Preloaded Asset.\n" +
                    "Changes on it will add, remove or replace your Preloaded Asset list, " +
                    "located at Project Settings, Optimization section.";
                EditorGUILayout.HelpBox(message, MessageType.Info, wide: true);
            }

            EditorGUI.BeginChangeCheck();
            EditorGUI.indentLevel++;

            var settings = EditorGUILayout.ObjectField(
                label: "Current Settings",
                obj: CurrentSettings,
                objType: typeof(T),
                allowSceneObjects: false
            ) as T;

            EditorGUI.indentLevel--;
            // Only checks if the Current Settings field has changed using the Inspector window.
            var hasNewSettings = EditorGUI.EndChangeCheck();
            if (hasNewSettings)
            {
                if (isPreloadedAsset)
                {
                    var wasSettingsRemoved = settings == null;
                    if (wasSettingsRemoved) PreloadedAsset.Remove(CurrentSettings);
                    else PreloadedAsset.Replace(CurrentSettings, settings);
                }

                CurrentSettings = settings;
                RefreshEditor();
            }
        }

        private void DisplaySettingsCreationGUI()
        {
            const string message = "You have no Settings available. Would you like to create one?";
            EditorGUILayout.HelpBox(message, MessageType.Info, wide: true);

            var openCreateDialog = GUILayout.Button("Create");
            if (openCreateDialog)
            {
                CurrentSettings = SaveSettingsAsset(isPreloadedAsset);
                RefreshEditor();
            }
        }

        private static T SaveSettingsAsset(bool isPreloaded)
        {
            var name = typeof(T).Name;
            var path = EditorUtility.SaveFilePanelInProject(
                title: "Save " + name,
                defaultName: name,
                extension: "asset",
                message: "Please enter a filename to save the projects settings."
            ).Trim();

            var invalidPath = string.IsNullOrEmpty(path);
            if (invalidPath) return null;

            var settings = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(settings, path);
            if (isPreloaded) PreloadedAsset.Add(settings);
            AssetDatabase.SaveAssets();

            Selection.activeObject = settings;
            return settings;
        }
    }
}