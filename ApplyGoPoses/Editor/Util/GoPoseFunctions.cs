#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Cam.GoGo {
	public enum GoAFKStance {
		All, Crouch, Prone, Stand
	}

	public struct GoAFKAnimatorStateNames
	{
		public string INIT;
		public string LOOPING;
		public string EXIT;
	}

	public enum GoAFKAnimatorState {
		Init, Looping, Exit, All
	}

	public static class GoPoseFunctions
	{
		public static void SetGoPose(SerializedObject so, string propertyPath, AnimationClip clip, float speed=1.0f) { 
			string posePropertyPath = $"{propertyPath}.pose";
			so.FindProperty(posePropertyPath).objectReferenceValue = clip;

			string speedPropertyPath = $"{propertyPath}.speed";
			so.FindProperty(speedPropertyPath).floatValue = speed;
		}

		public static void SetGoPoseIfNull(SerializedObject so, string propertyPath, AnimationClip clip, float speed = 1.0f)
		{
			string posePropertyPath = $"{propertyPath}.pose";
			string speedPropertyPath = $"{propertyPath}.speed";

			if (so.FindProperty(posePropertyPath).objectReferenceValue != null) { 
				return; 
			}

			so.FindProperty(posePropertyPath).objectReferenceValue = clip;
			so.FindProperty(speedPropertyPath).floatValue = speed;
		}

		public static bool ValidateHasMultipleComponents(GameObject go)
		{
			VRCAvatarDescriptor descriptor = FindDescriptorUp(go);
			return descriptor && descriptor.GetComponentsInChildren<GoCustomizePosesBehavior>().Length > 1;
		}


		public static VRCAvatarDescriptor FindDescriptorUp(GameObject go)
		{
			VRCAvatarDescriptor descriptor = go.GetComponent<VRCAvatarDescriptor>();
			if(descriptor) {
				return descriptor;
			}

			GameObject parent = go.transform.parent.gameObject;
			VRCAvatarDescriptor parentDescriptor = parent.GetComponent<VRCAvatarDescriptor>();
			return parentDescriptor ?? FindDescriptorUp(parent.gameObject);
		}


		public static VRCAvatarDescriptor FindDescriptorDown(GameObject go)
		{
			VRCAvatarDescriptor descriptor = go.GetComponent<VRCAvatarDescriptor>();
			if(descriptor) {
				return descriptor;
			}

			for (int i = 0; i < go.transform.childCount; i++) {
				VRCAvatarDescriptor childDescriptor = FindDescriptorDown(go.transform.GetChild(i).gameObject);
				if (childDescriptor) {
					return childDescriptor;
				}
			}

			return null;
		}

		public static AnimatorController GetPlayableLayerFromDescriptor(VRCAvatarDescriptor desc, VRCAvatarDescriptor.AnimLayerType layerType)
		{
			var layer = desc.baseAnimationLayers.Where(l => l.type == layerType).FirstOrDefault();
			return layer.animatorController as AnimatorController;
		}

		public static void UpdateAFKPoseRecursive(AnimatorStateMachine stateMachine, GoSuperAdvancedAFKPoses afkPoses, GoAFKStance stance)
		{
			GoAFKAnimatorStateNames STATE_NAMES = GoPoseConstants.GOGO_STATE_NAME_DICT[stance];

			foreach (var state in stateMachine.states)
			{
				string stateName = state.state.name;
				if (stateName == STATE_NAMES.INIT && afkPoses.AFKPoseInit.pose != null)
				{
					state.state.motion = afkPoses.AFKPoseInit.pose;
					state.state.speed = afkPoses.AFKPoseInit.speed;
				}
				else if (stateName == STATE_NAMES.LOOPING && afkPoses.AFKPoseLooping.pose != null)
				{
					state.state.motion = afkPoses.AFKPoseLooping.pose;
					state.state.speed = afkPoses.AFKPoseLooping.speed;
				}
				else if (stateName == STATE_NAMES.EXIT && afkPoses.AFKPoseExit.pose != null)
				{
					state.state.motion = afkPoses.AFKPoseExit.pose;
					state.state.speed = afkPoses.AFKPoseExit.speed;
				}
			}

			foreach (var sm in stateMachine.stateMachines)
			{
				UpdateAFKPoseRecursive(sm.stateMachine, afkPoses, stance);
			}
		}

		public static void UpdateMenuPose(AnimatorController baseController, GoPose MenuPose)
		{
			if (MenuPose.pose == null) {
				return;
			}

			foreach (var layer in baseController.layers) {
				UpdateMenuPoseRecursive(layer.stateMachine, MenuPose);
			}
		}

		public static void UpdateMenuPoseRecursive(AnimatorStateMachine stateMachine, GoPose MenuPose)
		{
			foreach (var st in stateMachine.states)
			{
				if (st.state.name == GoPoseConstants.AVATAR_3D_THUMBNAIL_STATE_NAME)
				{
					st.state.motion = MenuPose.pose ?? st.state.motion;
					st.state.speed = MenuPose.speed;
					return;
				}
			}

			foreach (var sm in stateMachine.stateMachines)
			{
				UpdateMenuPoseRecursive(sm.stateMachine, MenuPose);
			}
		}
	}
}
#endif