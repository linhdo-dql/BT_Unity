﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEditor.Compilation;

namespace EasyMobile.Editor
{
    [InitializeOnLoad]
    public class EM_PluginManager : AssetPostprocessor
    {
        #region Init
        private const string PreviousUnsuccessfulDefines = "EM_PreviousUnsuccessfulDefines_Record";
        // This static constructor will automatically run thanks to the InitializeOnLoad attribute.
        static EM_PluginManager()
        {
            EditorApplication.update += Initialize;
            CompilationPipeline.assemblyCompilationFinished += (outputPath, compilerMessages) =>
            {
                bool compilerErrorFound = false;
                foreach (var msg in compilerMessages)
                {
                    if (msg.type == CompilerMessageType.Error && (msg.message.Contains("CS0246") || msg.message.Contains("CS0006")))
                    {
                        compilerErrorFound = true;
                    }
                }
                string currentDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                if (compilerErrorFound)
                {
                    if(!IsDefineResetBefore(outputPath, currentDefine))
                    {
                        Debug.LogWarningFormat("Easy Mobile: Missing type/namespace/dll errors were found in {0}. Attempt to fix by resetting scripting symbols", outputPath);
                    }
                    else
                    {
                        Debug.LogWarningFormat("Easy Mobile: Missing type/namespace/dll errors were found in {0}. Unable to fix by resetting scripting symbols", outputPath);
                    }
                }

                if (compilerErrorFound && !IsDefineResetBefore(outputPath, currentDefine))
                {
                    //try to remove #defind since there was an error while compiling
                    //in case the current define symbols has been resetted before => not reset symbols since it might be other party problems
                    //and to avoid enless recompiling if that was the case
                    SetDefineResetInfo(outputPath, currentDefine, true);
                    GlobalDefineManager.SDS_RemoveDefinesOnAllPlatforms(EM_ScriptingSymbols.GetAllSymbols());
                }

                if(!compilerErrorFound)
                {
                    SetDefineResetInfo(outputPath, currentDefine, false);
                }
            };
        }

        [Serializable]
        private class ProblemDefineInfo
        {
            public string AssemblyOutputPath;
            public string Define;
            public bool Resetted;
        }

        [Serializable]
        private class ProblemDefines
        {
            public List<ProblemDefineInfo> problemDefineInfos = new List<ProblemDefineInfo>();
        }

        private static bool IsDefineResetBefore(string assemblyOutputPath, string define)
        {
            ProblemDefines problemDefines = GetProblemDefines();
            foreach (var defineInfo in problemDefines.problemDefineInfos)
            {
                if(defineInfo.AssemblyOutputPath.Equals(assemblyOutputPath) && defineInfo.Define.Equals(define))
                    return defineInfo.Resetted;
            }
            return false;
        }

        private static void SetDefineResetInfo(string assemblyOutputPath, string define, bool resetted)
        {
            ProblemDefines problemDefines = GetProblemDefines();
            bool foundDefine = false;
            foreach (var defineInfo in problemDefines.problemDefineInfos)
            {
                if(defineInfo.AssemblyOutputPath.Equals(assemblyOutputPath) && defineInfo.Define.Equals(define))
                {
                    defineInfo.Resetted = resetted;
                    foundDefine = true;
                }
            }
            if(!foundDefine)
            {
                problemDefines.problemDefineInfos.Add(new ProblemDefineInfo(){
                    AssemblyOutputPath = assemblyOutputPath,
                    Define = define,
                    Resetted = resetted
                });
            }
            for (int i = problemDefines.problemDefineInfos.Count - 1; i >= 0 ; i--)
            {
                if(!problemDefines.problemDefineInfos[i].Resetted)
                    problemDefines.problemDefineInfos.RemoveAt(i);
            }
            SetProblemDefines(problemDefines);
        }

        private static ProblemDefines GetProblemDefines()
        {
            string jsonString = EditorPrefs.GetString(PreviousUnsuccessfulDefines, "");
            ProblemDefines problemDefines = JsonUtility.FromJson<ProblemDefines>(jsonString);
            if(problemDefines == null)
                problemDefines = new ProblemDefines();
            return problemDefines;
        }

        private static void SetProblemDefines(ProblemDefines problemDefines)
        {
            string jsonString = JsonUtility.ToJson(problemDefines);
            EditorPrefs.SetString(PreviousUnsuccessfulDefines, jsonString);
        }

        private static void Initialize()
        {
            EditorApplication.update -= Initialize;

            // Check if a new version has been imported and perform necessary updating jobs.
            VersionCheck();

            // Define a global symbol indicating the existence of EasyMobile.
            GlobalDefineManager.SDS_AddDefinesOnAllPlatforms(EM_ScriptingSymbols.EasyMobile.Split(';'));

            // Create the EM_Settings scriptable object if it doesn't exist.
            EM_BuiltinObjectCreator.CreateEMSettingsAsset();

            // Regularly check for module prerequisites to avoid issues caused
            // by inadvertent changes, e.g removal of required scripting symbols.
            CheckModules();
        }

        #endregion

        #region Methods

        // Check if a *different* (maybe an older one is being imported!) version has been imported.
        // If yes, import the native package and update the version keys stored in settings file.
        internal static void VersionCheck()
        {
            int savedVersion = EM_ProjectSettings.Instance.GetInt(EM_Constants.PSK_EMVersionInt, -1);

            if (savedVersion != EM_Constants.versionInt)
            {
                // New version detected!
                EM_ProjectSettings.Instance.Set(EM_Constants.PSK_EMVersionString, EM_Constants.versionString);
                EM_ProjectSettings.Instance.Set(EM_Constants.PSK_EMVersionInt, EM_Constants.versionInt);

                // Import the Google Play Services Resolver
                EM_ExternalPluginManager.ImportPlayServicesResolver(false);
            }
            else if (!EM_ExternalPluginManager.IsPlayServicesResolverImported())
            {
                EM_ExternalPluginManager.ImportPlayServicesResolver(false);
            }
        }

        // Makes sure that everything is set up properly so that all modules function as expected.
        internal static void CheckModules()
        {
            foreach (Module mod in Enum.GetValues(typeof(Module)))
            {
                var manager = GetModuleManager(mod);

                if (manager == null)
                    continue;

                // Is this composite module?
                var compManager = manager as CompositeModuleManager;

                if (compManager != null)    // Is really a composite module
                {
                    foreach (Submodule submod in compManager.SelfSubmodules)
                    {
                        if (EM_Settings.IsSubmoduleEnable(submod))
                            compManager.EnableSubmodule(submod);
                        else
                            compManager.DisableSubmodule(submod);
                    }
                }
                else    // Is a normal module
                {
                    if (EM_Settings.IsModuleEnable(mod))
                        manager.EnableModule();
                    else
                        manager.DisableModule();
                }
            }
        }

        internal static ModuleManager GetModuleManager(Module mod)
        {
            switch (mod)
            {
                case Module.Advertising:
                    return ModuleManager_Advertising.Instance;
                case Module.GameServices:
                    return ModuleManager_GameServices.Instance;
                case Module.Gif:
                    return ModuleManager_Gif.Instance;
                case Module.InAppPurchasing:
                    return ModuleManager_InAppPurchasing.Instance;
                case Module.NativeApis:
                    return ModuleManager_NativeAPIs.Instance;
                case Module.Notifications:
                    return ModuleManager_Notifications.Instance;
                case Module.Privacy:
                    return ModuleManager_Privacy.Instance;
                case Module.Sharing:
                    return ModuleManager_Sharing.Instance;
                case Module.Utilities:
                    return ModuleManager_Utilities.Instance;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets all Android permissions required by the given module if it's enabled.
        /// If the module is disable, an empty list will be returned.
        /// </summary>
        /// <returns>The android permissions required by module.</returns>
        /// <param name="mod">Mod.</param>
        internal static List<AndroidPermission> GetAndroidPermissionsRequiredByModule(Module mod)
        {
            var modulePermissions = new List<AndroidPermission>();
            var manager = GetModuleManager(mod);

            if (manager == null)
                return modulePermissions;

            // Is this composite module?
            var compManager = manager as CompositeModuleManager;

            if (compManager != null)    // Is a composite module
            {
                foreach (Submodule submod in compManager.SelfSubmodules)
                {
                    if (EM_Settings.IsSubmoduleEnable(submod) && compManager.AndroidPermissionHolderForSubmodule(submod) != null)
                        modulePermissions.AddRange(compManager.AndroidPermissionHolderForSubmodule(submod).GetAndroidPermissions());
                }
            }
            else    // Is a normal module
            {
                if (EM_Settings.IsModuleEnable(mod) && manager.AndroidPermissionsHolder != null)
                    modulePermissions.AddRange(manager.AndroidPermissionsHolder.GetAndroidPermissions());
            }

            return modulePermissions;
        }

        /// <summary>
        /// Gets all Android permissions required by enabled modules.
        /// </summary>
        /// <returns>The all android permissions required.</returns>
        internal static Dictionary<Module, List<AndroidPermission>> GetAllAndroidPermissionsRequired()
        {
            var allPermissions = new Dictionary<Module, List<AndroidPermission>>();

            foreach (Module mod in Enum.GetValues(typeof(Module)))
            {
                var modulePermissions = GetAndroidPermissionsRequiredByModule(mod);
                if (modulePermissions != null && modulePermissions.Count > 0)
                    allPermissions.Add(mod, modulePermissions);
            }

            return allPermissions;
        }

        /// <summary>
        /// Gets all iOS Info.plist items required by the given module if it's enabled.
        /// If the module is disable, an empty list will be returned.
        /// </summary>
        /// <returns>The IOS info items required by module.</returns>
        /// <param name="mod">Mod.</param>
        internal static List<iOSInfoPlistItem> GetIOSInfoItemsRequiredByModule(Module mod)
        {
            var moduleInfoItems = new List<iOSInfoPlistItem>();
            var manager = GetModuleManager(mod);

            if (manager == null)
                return moduleInfoItems;

            // Is this composite module?
            var compManager = manager as CompositeModuleManager;

            if (compManager != null)    // Is a composite module
            {
                foreach (Submodule submod in compManager.SelfSubmodules)
                {
                    if (EM_Settings.IsSubmoduleEnable(submod) && compManager.iOSInfoItemsHolderForSubmodule(submod) != null)
                        moduleInfoItems.AddRange(compManager.iOSInfoItemsHolderForSubmodule(submod).GetIOSInfoPlistKeys());
                }
            }
            else    // Is a normal module
            {
                if (EM_Settings.IsModuleEnable(mod) && manager.iOSInfoItemsHolder != null)
                    moduleInfoItems.AddRange(manager.iOSInfoItemsHolder.GetIOSInfoPlistKeys());
            }

            return moduleInfoItems;
        }

        /// <summary>
        /// Gets all iOS Info.plist items required by enabled modules.
        /// </summary>
        /// <returns>The all IOS info items required.</returns>
        internal static Dictionary<Module, List<iOSInfoPlistItem>> GetAllIOSInfoItemsRequired()
        {
            var allInfoItems = new Dictionary<Module, List<iOSInfoPlistItem>>();

            foreach (Module mod in Enum.GetValues(typeof(Module)))
            {
                var moduleInfoItems = GetIOSInfoItemsRequiredByModule(mod);

                if (moduleInfoItems != null && moduleInfoItems.Count > 0)
                    allInfoItems.Add(mod, moduleInfoItems);
            }

            return allInfoItems;
        }

        #endregion
    }
}
