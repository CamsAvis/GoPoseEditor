#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Cam.GoGo {
	[InitializeOnLoad]
	public static class GoDefaultClips
	{
		public static AnimationClip DefaultMenuClip { get; private set; }
		public static AnimationClip DefaultAfkClip { get; private set; }

		public static AnimationClip DefaultAfkCrouchInitClip { get; private set; }
		public static AnimationClip DefaultAfkProneInitClip { get; private set; }
		public static AnimationClip DefaultAfkStandInitClip { get; private set; }

		public static AnimationClip DefaultAfkStandLoopClip { get; private set; }
		public static AnimationClip DefaultAfkCrouchLoopClip { get; private set; }
		public static AnimationClip DefaultAfkProneLoopClip { get; private set; }

		public static AnimationClip DefaultAfkCrouchExitClip { get; private set; }
		public static AnimationClip DefaultAfkProneExitClip { get; private set; }
		public static AnimationClip DefaultAfkStandExitClip { get; private set; }

		public static AnimationClip DefaultStandingPose => DefaultAfkStandExitClip;
		public static AnimationClip DefaultCrouchingPose => DefaultAfkCrouchExitClip;
		public static AnimationClip DefaultPronePose => DefaultAfkProneExitClip;

		public static GoSuperAdvancedAFKPoses DefaultAFKStandingPoses { get; private set; }
		public static GoSuperAdvancedAFKPoses DefaultAFKCrouchingPoses { get; private set; }
		public static GoSuperAdvancedAFKPoses DefaultAFKProningPoses { get; private set; }


		private static bool _initialized;

		static GoDefaultClips() => Initialize();

		public static void Initialize()
		{
			if (_initialized)
			{
				Debug.Log("[DefaultGoClips] Default GoGo pose clips already initialized");
				return;
			}

			Debug.Log("[DefaultGoClips] Initializing default GoGo pose clips.");

			_initialized = true;

			DefaultMenuClip = LoadClip(GoPoseConstants.DEFEAULT_MENU_POSE_GUID);
			DefaultAfkClip = LoadClip(GoPoseConstants.DEFAULT_AFK_POSE_GUID);

			DefaultAfkCrouchInitClip = LoadClip(GoPoseConstants.DEFAULT_AFK_CROUCH_INIT_GUID);
			DefaultAfkCrouchLoopClip = LoadClip(GoPoseConstants.DEFAULT_AFK_CROUCH_LOOP_GUID);
			DefaultAfkCrouchExitClip = LoadClip(GoPoseConstants.DEFAULT_AFK_CROUCH_EXIT_GUID);

			DefaultAfkProneInitClip = LoadClip(GoPoseConstants.DEFAULT_AFK_PRONE_INIT_GUID);
			DefaultAfkProneLoopClip = LoadClip(GoPoseConstants.DEFAULT_AFK_PRONE_LOOP_GUID);
			DefaultAfkProneExitClip = LoadClip(GoPoseConstants.DEFAULT_AFK_PRONE_EXIT_GUID);

			DefaultAfkStandInitClip = LoadClip(GoPoseConstants.DEFAULT_AFK_STAND_INIT_GUID);
			DefaultAfkStandLoopClip = LoadClip(GoPoseConstants.DEFAULT_AFK_STAND_LOOP_GUID);
			DefaultAfkStandExitClip = LoadClip(GoPoseConstants.DEFAULT_AFK_STAND_EXIT_GUID);

			DefaultAFKStandingPoses = new GoSuperAdvancedAFKPoses
			{
				AFKPoseInit = NewPose(DefaultAfkStandInitClip),
				AFKPoseLooping = NewPose(DefaultAfkStandLoopClip),
				AFKPoseExit = NewPose(DefaultAfkStandExitClip)
			};

			DefaultAFKCrouchingPoses = new GoSuperAdvancedAFKPoses
			{
				AFKPoseInit = NewPose(DefaultAfkCrouchInitClip),
				AFKPoseLooping = NewPose(DefaultAfkCrouchLoopClip),
				AFKPoseExit = NewPose(DefaultAfkCrouchExitClip)
			};

			DefaultAFKProningPoses = new GoSuperAdvancedAFKPoses
			{
				AFKPoseInit = NewPose(DefaultAfkProneInitClip),
				AFKPoseLooping = NewPose(DefaultAfkProneLoopClip),
				AFKPoseExit = NewPose(DefaultAfkProneExitClip)
			};
		}

		private static GoPose NewPose(AnimationClip clip, float speed=1.0f) => new GoPose() { pose = clip, speed = speed };

		private static AnimationClip LoadClip(string guid) => AssetDatabase.LoadAssetAtPath<AnimationClip>(AssetDatabase.GUIDToAssetPath(guid));
	}
}
#endif