﻿using UnityEngine;

namespace Sisus.Debugging
{
	#if DEV_MODE
	[CreateAssetMenu]
	#endif
	public class DebugLogExtensionsProjectSettingsAsset : ScriptableObject, IDebugLogExtensionsProjectSettingsAsset
	{
		private static DebugLogExtensionsProjectSettingsAsset instance;

		private const string ResourcePath = "DebugLogExtensionsProjectSettings";

		public bool useGlobalNamespace = false;
		public bool stripAllCallsFromBuilds = false;
		public bool unlistedChannelsEnabledByDefault = true;
		public DebugChannelInfo[] channels = new DebugChannelInfo[0];
		public KeyConfig toggleView = new KeyConfig(KeyConfig.ToggleViewKey, KeyCode.Insert);
		public IgnoredStackTraceInfo[] hideStackTraceRows = new IgnoredStackTraceInfo[0];

		#if DEV_MODE
		[System.NonSerialized]
		#else
		[SerializeField, HideInInspector]
		#endif
		public bool useGlobalNamespaceDetermined = false;
		
		public bool ignoreUnlistedChannelPrefixes = false;
		public bool autoAddDevUniqueChannels = true;

		public static DebugLogExtensionsProjectSettingsAsset Get()
		{
			if(instance == null)
			{
				instance = Resources.Load<DebugLogExtensionsProjectSettingsAsset>(ResourcePath);

				if(instance == null)
				{
					#if UNITY_EDITOR
					if(!UnityEditor.EditorApplication.isUpdating)
					{
						string path = "Assets/Sisus/Debug.Log Extensions/Resources/" + ResourcePath + ".asset";
						#if DEV_MODE && UNITY_EDITOR
						Debug.LogWarning(ResourcePath + " not found. Creating new instance at "+ path + ".");
						#endif

						instance = CreateInstance<DebugLogExtensionsProjectSettingsAsset>();
						UnityEditor.AssetDatabase.CreateAsset(instance, path);
					}
					#if DEV_MODE
					Debug.LogWarning(ResourcePath + " not found. Returning null because asset database still updating..");
					#endif
					#endif
				}
				#if DEV_MODE && UNITY_EDITOR
				else { Debug.Log("DebugLogExtensionsProjectSettingsAsset.Get - loaded asset", instance); }
				#endif
			}

			return instance;
		}

        public void Apply(DebugLogExtensionsProjectSettings settings)
        {
			#if DEV_MODE
			Debug.Log("DebugLogExtensionsProjectSettingsAsset.Apply(settings)", this);
			#endif

			settings.useGlobalNamespace = useGlobalNamespace;
			settings.stripAllCallsFromBuilds = stripAllCallsFromBuilds;
			settings.unlistedChannelsEnabledByDefault = unlistedChannelsEnabledByDefault;
			settings.channels = channels;
			settings.toggleView = toggleView;
			settings.hideStackTraceRows = hideStackTraceRows;
			settings.useGlobalNamespaceDetermined = useGlobalNamespaceDetermined;
			settings.ignoreUnlistedChannelPrefixes = ignoreUnlistedChannelPrefixes;
			settings.autoAddDevUniqueChannels = autoAddDevUniqueChannels;			
			settings.Apply();
        }

        private void OnEnable()
        {
			instance = this;
		}

        #if UNITY_EDITOR
		private void OnValidate()
		{
			for(int n = channels.Length - 1; n >= 0; n--)
			{
				channels[n].OnValidate();
			}
		}
		#endif
	}
}