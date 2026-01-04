using UnityEngine;
using VRC.SDKBase;

namespace Cam.GoGo
{
	[System.Serializable]
	public enum GoAFKCustomizationMethod
	{
		Default, Basic, Advanced, SuperAdvanced
	}

	[System.Serializable]
	public enum GoMenuPoseMethod {
		Default, Custom
	}

	[System.Serializable]
	public struct GoPose
	{
		[SerializeField] public AnimationClip pose;
		[SerializeField] public float speed;
	}

	[System.Serializable]
	public class GoSuperAdvancedAFKPoses
	{
		[SerializeField] public GoPose AFKPoseInit;
		[SerializeField] public GoPose AFKPoseLooping;
		[SerializeField] public GoPose AFKPoseExit;
	}

	// Uses a standardized naming scheme
	// e.g:
	// - {Advanced}AFKPose_{Prone}
	// - {Basic}AFKPose_{All}
	// - {SuperAdvanced}AFKPose_{Stand}.AFKPose{Init}
	[System.Serializable]
	public class GoCustomizePosesBehavior : MonoBehaviour, IEditorOnly
	{
		[SerializeField] public GoAFKCustomizationMethod AfkPoseMethod;
		[SerializeField] public GoMenuPoseMethod MenuPoseMethod = GoMenuPoseMethod.Custom;

		[SerializeField] public GoPose MenuPose;
		[SerializeField] public GoPose BasicAFKPose_All;

		[SerializeField] public GoPose AdvancedAFKPose_Stand;
		[SerializeField] public GoPose AdvancedAFKPose_Crouch;
		[SerializeField] public GoPose AdvancedAFKPose_Prone;

		[SerializeField] public GoSuperAdvancedAFKPoses SuperAdvancedAFKPose_Stand;
		[SerializeField] public GoSuperAdvancedAFKPoses SuperAdvancedAFKPose_Crouch;
		[SerializeField] public GoSuperAdvancedAFKPoses SuperAdvancedAFKPose_Prone;
	}
}