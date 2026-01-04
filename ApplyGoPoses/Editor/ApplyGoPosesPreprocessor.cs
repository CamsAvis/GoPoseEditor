using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase.Editor.BuildPipeline;

namespace Cam.GoGo
{
	public class ApplyGoPosesPreprocessor : IVRCSDKPreprocessAvatarCallback
	{
		public int callbackOrder => 15000;

		// lol i just dont want people's entire avatar build to fail bc of a skill issue
		bool IVRCSDKPreprocessAvatarCallback.OnPreprocessAvatar(GameObject avatar)
		{
			try {
				return Preprocess(avatar);
			} catch {
				Debug.LogError("[ApplyCustomGoGoPosesPreprocessor] An error occurred while applying GoGo customization to the avatar.");
				return true;
			}
		}

		bool Preprocess(GameObject avatar) { 
			ApplyGoPoses[] components = avatar.gameObject.GetComponentsInChildren<ApplyGoPoses>(true);

			if (components.Length > 1) {
				EditorUtility.DisplayDialog("Multiple GoGo customization components found", "Multiple GoGo customization components found on avatar.Please ensure only one exists.", "ok");	
				Debug.LogError("[ApplyCustomGoGoPosesPreprocessor] Multiple GoGo customization components found on avatar. Please ensure only one exists.");
				return false;
			}

			ApplyGoPoses goPoseSettings = components[0];

			var desc = avatar.GetComponent<VRCAvatarDescriptor>();
			if (!desc || !desc.customExpressions) return true;

			Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Found VRCAvatarDescriptor on avatar. Applying GoGo customization.");

			var baseController = GoPoseFunctions.GetPlayableLayerFromDescriptor(desc, VRCAvatarDescriptor.AnimLayerType.Base);
			if(baseController)
			{
				GoPose MenuPose;
				Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Applying custom menu pose.");
				switch (goPoseSettings.MenuPoseMethod)
				{
					case GoMenuPoseMethod.Custom:
						Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Applying custom menu pose.");
						MenuPose = goPoseSettings.MenuPose;
						break;
					default:
						Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Applying default menu pose.");
						MenuPose = new GoPose() { pose = DefaultGoClips.DefaultMenuClip, speed = 1.0f };
						break;
				}

				Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Updating menu pose in base layer.");
				GoPoseFunctions.UpdateMenuPose(baseController, MenuPose);
				Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Menu pose updated.");
			}

			Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Retrieving action layer.");
			var actionController = GoPoseFunctions.GetPlayableLayerFromDescriptor(desc, VRCAvatarDescriptor.AnimLayerType.Action);
			if (actionController)
			{
				Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Action layer found. Applying AFK poses.");
				GoAppliedAFKPoses AppliedAFKPoses = GoAppliedAFKPoses.Default();
				switch (goPoseSettings.AfkPoseMethod)
				{
					case GoAFKCustomizationMethod.Basic:
						Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Applying basic AFK pose.");
						AppliedAFKPoses.SetAFKPosesBasic(
							goPoseSettings.BasicAFKPose_All
						);
						break;
					case GoAFKCustomizationMethod.Advanced:
						Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Applying advanced AFK poses.");
						AppliedAFKPoses.SetAFKPosesAdvanced(
							goPoseSettings.AdvancedAFKPose_Stand,
							goPoseSettings.AdvancedAFKPose_Crouch,
							goPoseSettings.AdvancedAFKPose_Prone
						);
						break;
					case GoAFKCustomizationMethod.SuperAdvanced:
						Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Applying super advanced AFK poses.");
						AppliedAFKPoses.SetAFKPosesSuperAdvanced(
							goPoseSettings.SuperAdvancedAFKPose_Stand,
							goPoseSettings.SuperAdvancedAFKPose_Crouch,
							goPoseSettings.SuperAdvancedAFKPose_Prone
						);
						break;
					default:
						break;
				}

				Debug.Log("[ApplyCustomGoGoPosesPreprocessor] Updating AFK poses in action layer.");
				AppliedAFKPoses.Apply(actionController);
			}

			Debug.Log("[ApplyCustomGoGoPosesPreprocessor] GoGo customization applied successfully.");

			return true;
		}

	}
}
