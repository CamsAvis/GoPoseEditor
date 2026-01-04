#if UNITY_EDITOR
using UnityEditor.Animations;
using UnityEngine;

namespace Cam.GoGo {
	public class GoAppliedAFKPoses
	{
		public GoPose AFKPose_StandInit;
		public GoPose AFKPose_StandLooping;
		public GoPose AFKPose_StandExit;

		public GoPose AFKPose_CrouchInit;
		public GoPose AFKPose_CrouchLooping;
		public GoPose AFKPose_CrouchExit;

		public GoPose AFKPose_ProneInit;
		public GoPose AFKPose_ProneLooping;
		public GoPose AFKPose_ProneExit;

		static GoPose GP(AnimationClip clip, float speed=1.0f) => new GoPose() { pose = clip, speed = speed };

		public static GoAppliedAFKPoses Default() {
			return new GoAppliedAFKPoses()
			{
				AFKPose_StandInit = GP(DefaultGoClips.DefaultAfkStandInitClip),
				AFKPose_StandLooping = GP(DefaultGoClips.DefaultAfkStandLoopClip),
				AFKPose_StandExit = GP(DefaultGoClips.DefaultAfkStandExitClip),
				AFKPose_CrouchInit = GP(DefaultGoClips.DefaultAfkCrouchInitClip),
				AFKPose_CrouchLooping = GP(DefaultGoClips.DefaultAfkCrouchLoopClip),
				AFKPose_CrouchExit = GP(DefaultGoClips.DefaultAfkCrouchExitClip),
				AFKPose_ProneInit = GP(DefaultGoClips.DefaultAfkProneInitClip),
				AFKPose_ProneLooping = GP(DefaultGoClips.DefaultAfkProneLoopClip),
				AFKPose_ProneExit = GP(DefaultGoClips.DefaultAfkProneExitClip)
			};
		}

		public void SetAFKPosesBasic(GoPose basicPose)
		{
			AFKPose_StandInit = AFKPose_StandLooping = basicPose;
			AFKPose_StandExit = GP(DefaultGoClips.DefaultStandingPose);

			AFKPose_CrouchInit = AFKPose_CrouchLooping = basicPose;
			AFKPose_CrouchExit = GP(DefaultGoClips.DefaultCrouchingPose);

			AFKPose_ProneInit = AFKPose_ProneLooping = basicPose;
			AFKPose_ProneExit = GP(DefaultGoClips.DefaultPronePose);		
		}

		public void SetAFKPosesAdvanced(GoPose AdvancedAFKPose_Stand, GoPose AdvancedAFKPose_Crouch, GoPose AdvancedAFKPose_Prone)
		{
			AFKPose_StandInit = AFKPose_StandLooping = AdvancedAFKPose_Stand;
			AFKPose_StandExit = GP(DefaultGoClips.DefaultStandingPose);

			AFKPose_CrouchInit = AFKPose_CrouchLooping = AdvancedAFKPose_Crouch;
			AFKPose_CrouchExit = GP(DefaultGoClips.DefaultCrouchingPose);

			AFKPose_ProneInit = AFKPose_ProneLooping = AdvancedAFKPose_Prone;
			AFKPose_ProneExit = GP(DefaultGoClips.DefaultPronePose);		
		}

		public void SetAFKPosesSuperAdvanced(GoSuperAdvancedAFKPoses StandPoses, GoSuperAdvancedAFKPoses CrouchPoses, GoSuperAdvancedAFKPoses PronePoses) {
			AFKPose_StandInit = StandPoses.AFKPoseInit;
			AFKPose_StandLooping = StandPoses.AFKPoseLooping;
			AFKPose_StandExit = StandPoses.AFKPoseExit;

			AFKPose_CrouchInit = CrouchPoses.AFKPoseInit;
			AFKPose_CrouchLooping = CrouchPoses.AFKPoseLooping;
			AFKPose_CrouchExit = CrouchPoses.AFKPoseExit;

			AFKPose_ProneInit = PronePoses.AFKPoseInit;
			AFKPose_ProneLooping = PronePoses.AFKPoseLooping;
			AFKPose_ProneExit = PronePoses.AFKPoseExit;
		}

		public void Apply(AnimatorController controller)
		{
			AnimatorControllerLayer[] layers = controller.layers;
			bool standSuccess = false;
			bool crouchSuccess = false;
			bool proneSuccess = false;

			foreach (var layer in layers)
			{
				if(standSuccess && crouchSuccess && proneSuccess) {
					Debug.Log("[ApplyCustomGoGoPosesPreprocessor] [GoAppliedAFKPoses] All AFK poses applied successfully. Exiting.");
					return;
				}

				Debug.Log("[ApplyCustomGoGoPosesPreprocessor] [GoAppliedAFKPoses] Attempting to apply AFK poses to layer: " + layer.name);

				if(!standSuccess) {
					standSuccess = UpdateAFKPosesRecursive(layer.stateMachine, GoAFKStance.Stand, AFKPose_StandInit, AFKPose_StandLooping, AFKPose_StandExit);
					if(standSuccess) {
						Debug.Log("[ApplyCustomGoGoPosesPreprocessor] [GoAppliedAFKPoses] Stand AFK poses applied");
					}
				}

				if (!crouchSuccess) {
					crouchSuccess = UpdateAFKPosesRecursive(layer.stateMachine, GoAFKStance.Crouch, AFKPose_CrouchInit, AFKPose_CrouchLooping, AFKPose_CrouchExit);
					if (crouchSuccess) {
						Debug.Log("[ApplyCustomGoGoPosesPreprocessor] [GoAppliedAFKPoses] Crouch AFK poses applied");
					}
				}

				if (!proneSuccess) {
					proneSuccess = UpdateAFKPosesRecursive(layer.stateMachine, GoAFKStance.Prone, AFKPose_ProneInit, AFKPose_ProneLooping, AFKPose_ProneExit);
					if (proneSuccess) {
						Debug.Log("[ApplyCustomGoGoPosesPreprocessor] [GoAppliedAFKPoses] Prone AFK poses applied");
					}
				}
			}

			if(!standSuccess || !crouchSuccess || !proneSuccess) {
				Debug.LogWarning("[ApplyCustomGoGoPosesPreprocessor] [GoAppliedAFKPoses] Warning: Not all AFK poses were applied successfully.");
			} else {
				Debug.Log("[ApplyCustomGoGoPosesPreprocessor] [GoAppliedAFKPoses] All AFK poses applied successfully.");
			}

			Debug.Log("[ApplyCustomGoGoPosesPreprocessor] [GoAppliedAFKPoses] AFK pose application complete.");
		}

		public bool UpdateAFKPosesRecursive(AnimatorStateMachine stateMachine, GoAFKStance stance, GoPose init, GoPose looping, GoPose exit)
		{
			bool initSet = false;
			bool loopingSet = false;
			bool exitSet = false;

			UpdateAFKPosesRecursive(stateMachine, stance, init, looping, exit, ref initSet, ref loopingSet, ref exitSet);
			return initSet && loopingSet && exitSet;
		}

		public void UpdateAFKPosesRecursive(AnimatorStateMachine stateMachine, GoAFKStance stance, GoPose init, GoPose looping, GoPose exit, ref bool initSet, ref bool loopingSet, ref bool exitSet) {
			GoAFKAnimatorStateNames STATE_NAMES = GoPoseConstants.GOGO_STATE_NAME_DICT[stance];

			if(initSet && loopingSet && exitSet) {
				return;
			}

			foreach (var state in stateMachine.states)
			{
				string stateName = state.state.name;
				if(stateName == STATE_NAMES.INIT)
				{
					state.state.motion = init.pose;
					state.state.speed = init.speed;
					initSet = true;
				}
				else if (stateName == STATE_NAMES.LOOPING)
				{
					state.state.motion = looping.pose;
					state.state.speed = looping.speed;
					loopingSet = true;
				}
				else if (stateName == STATE_NAMES.EXIT)
				{
					state.state.motion = exit.pose;
					state.state.speed = exit.speed;
					exitSet = true;
				}
			}

			foreach (var sm in stateMachine.stateMachines) {
				UpdateAFKPosesRecursive(sm.stateMachine, stance, init, looping, exit, ref initSet, ref loopingSet, ref exitSet);
			}
		}
	}
}
#endif