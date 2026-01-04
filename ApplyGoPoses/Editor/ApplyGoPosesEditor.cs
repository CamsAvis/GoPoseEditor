#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Cam.GoGo
{
	[CustomEditor(typeof(ApplyGoPoses))]
	public class ApplyGoPosesEditor : Editor
	{
		SerializedObject soTarget = null;
		bool hasMultipleComponents = false;

		public void OnEnable()
		{
			soTarget = new SerializedObject(target);

			// Menu Pose
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "MenuPose", DefaultGoClips.DefaultMenuClip);

			// Basic Pose
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "BasicAFKPose_All", DefaultGoClips.DefaultAfkCrouchLoopClip);

			// Advanced Poses
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "AdvancedAFKPose_Stand", DefaultGoClips.DefaultAfkStandLoopClip);
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "AdvancedAFKPose_Crouch", DefaultGoClips.DefaultAfkCrouchLoopClip);
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "AdvancedAFKPose_Prone", DefaultGoClips.DefaultAfkProneLoopClip);

			// Super Advanced Poses
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "SuperAdvancedAFKPose_Crouch.AFKPoseInit", DefaultGoClips.DefaultAfkCrouchInitClip);
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "SuperAdvancedAFKPose_Crouch.AFKPoseLooping", DefaultGoClips.DefaultAfkCrouchLoopClip);
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "SuperAdvancedAFKPose_Crouch.AFKPoseExit", DefaultGoClips.DefaultAfkCrouchExitClip);

			GoPoseFunctions.SetGoPoseIfNull(soTarget, "SuperAdvancedAFKPose_Prone.AFKPoseInit", DefaultGoClips.DefaultAfkProneInitClip);
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "SuperAdvancedAFKPose_Prone.AFKPoseLooping", DefaultGoClips.DefaultAfkProneLoopClip);
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "SuperAdvancedAFKPose_Prone.AFKPoseExit", DefaultGoClips.DefaultAfkProneExitClip);

			GoPoseFunctions.SetGoPoseIfNull(soTarget, "SuperAdvancedAFKPose_Stand.AFKPoseInit", DefaultGoClips.DefaultAfkStandInitClip);
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "SuperAdvancedAFKPose_Stand.AFKPoseLooping", DefaultGoClips.DefaultAfkStandLoopClip);
			GoPoseFunctions.SetGoPoseIfNull(soTarget, "SuperAdvancedAFKPose_Stand.AFKPoseExit", DefaultGoClips.DefaultAfkStandExitClip);

			// Check for multiple components
			GoPoseFunctions.ValidateHasMultipleComponents((target as Component).gameObject);

			// Apply changes
			soTarget.ApplyModifiedProperties();
		}

		public override void OnInspectorGUI()
		{
			soTarget.Update();

			EditorGUILayout.LabelField("Modify GoGo Animations", EditorStyles.whiteLargeLabel);

			if(hasMultipleComponents) {
				using (new EditorGUILayout.HorizontalScope()) {
					EditorGUILayout.HelpBox("You have multiple instances of the script on your avatar. Please remove the others.", MessageType.Error);
					IconButton("Refresh", EditorGUIUtility.singleLineHeight * 2);
				}
			}

			using(new EditorGUI.DisabledScope(hasMultipleComponents)) {
				DrawMenuEdit();
				DrawAFKEdit();
			}

			soTarget.ApplyModifiedProperties();
		}

		void DrawAFKEdit()
		{
			GUILayout.Space(20);

			EditorGUI.BeginChangeCheck();
			SerializedProperty poseMethodProperty = soTarget.FindProperty("AfkPoseMethod");
			GoAFKCustomizationMethod afkPoseMethod = (GoAFKCustomizationMethod)poseMethodProperty.enumValueIndex;

			using (new EditorGUILayout.HorizontalScope())
			{
				afkPoseMethod = EditorGUILayout.EnumPopup(afkPoseMethod, GUILayout.Width(150)) as GoAFKCustomizationMethod? ?? GoAFKCustomizationMethod.Default;
				EditorGUILayout.LabelField("AFK Pose Settings", EditorStyles.whiteLargeLabel);
			}

			switch (afkPoseMethod)
			{
				case GoAFKCustomizationMethod.SuperAdvanced	: DrawAFKPoseSuperAdvanced() ; break;
				case GoAFKCustomizationMethod.Advanced		: DrawAFKPoseAdvanced()		 ; break;
				case GoAFKCustomizationMethod.Default		: DrawAFKPoseDefault()		 ; break;
				case GoAFKCustomizationMethod.Basic			: DrawAFKPoseBasic()		 ; break;
			}

			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(target, "Modify AFK Pose Settings");
				soTarget.FindProperty("AfkPoseMethod").enumValueIndex = (int)afkPoseMethod;
			}
		}

		void DrawAFKPoseDefault() {
			return;
		}

		void DrawAFKPoseBasic()
		{
			GUILayout.Space(5);
			EditorGUILayout.HelpBox("Use one, unified AFK pose", MessageType.Info);
			GUILayout.Space(5);

			DrawAFKClipSelection(DefaultGoClips.DefaultAfkClip, GoAFKStance.All, GoAFKAnimatorState.All, GoAFKCustomizationMethod.Basic);
		}

		void DrawAFKPoseAdvanced()
		{
			GUILayout.Space(5);
			EditorGUILayout.HelpBox("Use individual AFK poses for each stance", MessageType.Info);
			GUILayout.Space(5);

			DrawAFKClipSelection(DefaultGoClips.DefaultAfkStandLoopClip, GoAFKStance.Stand, GoAFKAnimatorState.All, GoAFKCustomizationMethod.Advanced);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkCrouchLoopClip, GoAFKStance.Crouch, GoAFKAnimatorState.All, GoAFKCustomizationMethod.Advanced);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkProneLoopClip, GoAFKStance.Prone, GoAFKAnimatorState.All, GoAFKCustomizationMethod.Advanced);
		}

		void DrawAFKPoseSuperAdvanced()
		{
			GUILayout.Space(5);
			EditorGUILayout.HelpBox("Use individual AFK poses for each stance, including animations for transitioning into and out of the AFK state.", MessageType.Info);
			GUILayout.Space(5);

			GUILayout.Label("Standing AFK Poses", EditorStyles.boldLabel);
			GUILayout.Space(2);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkStandInitClip, GoAFKStance.Stand, GoAFKAnimatorState.Init, GoAFKCustomizationMethod.SuperAdvanced);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkStandLoopClip, GoAFKStance.Stand, GoAFKAnimatorState.Looping, GoAFKCustomizationMethod.SuperAdvanced);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkStandExitClip, GoAFKStance.Stand, GoAFKAnimatorState.Exit, GoAFKCustomizationMethod.SuperAdvanced);

			GUILayout.Space(10);

			GUILayout.Label("Crouching AFK Poses", EditorStyles.boldLabel);
			GUILayout.Space(2);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkCrouchInitClip, GoAFKStance.Crouch, GoAFKAnimatorState.Init, GoAFKCustomizationMethod.SuperAdvanced);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkCrouchLoopClip, GoAFKStance.Crouch, GoAFKAnimatorState.Looping, GoAFKCustomizationMethod.SuperAdvanced);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkCrouchExitClip, GoAFKStance.Crouch, GoAFKAnimatorState.Exit, GoAFKCustomizationMethod.SuperAdvanced);

			GUILayout.Space(10);

			GUILayout.Label("Proning AFK Poses", EditorStyles.boldLabel);
			GUILayout.Space(2);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkProneInitClip, GoAFKStance.Prone, GoAFKAnimatorState.Init, GoAFKCustomizationMethod.SuperAdvanced);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkProneLoopClip, GoAFKStance.Prone, GoAFKAnimatorState.Looping, GoAFKCustomizationMethod.SuperAdvanced);
			DrawAFKClipSelection(DefaultGoClips.DefaultAfkProneExitClip, GoAFKStance.Prone, GoAFKAnimatorState.Exit, GoAFKCustomizationMethod.SuperAdvanced);
		}


		void DrawAFKClipSelection(
			AnimationClip defaultPose, 
			GoAFKStance stance = GoAFKStance.All, 
			GoAFKAnimatorState state = GoAFKAnimatorState.All, 
			GoAFKCustomizationMethod customizationMethod=GoAFKCustomizationMethod.Default, 
			bool disabled = false
		) {
			// Uses a standardized naming scheme
			// e.g:
			// - {Advanced}AFKPose_{Prone}
			// - {Basic}AFKPose_{All}
			// - {SuperAdvanced}AFKPose_{Stand}.AFKPose{Init}
			string propertyName = $"{customizationMethod}AFKPose_{stance}";
			if(customizationMethod == GoAFKCustomizationMethod.SuperAdvanced) {
				propertyName += $".AFKPose{state}";
			}

			SerializedProperty afkPoseProp = soTarget.FindProperty(propertyName);
			if (afkPoseProp == null) {
				Debug.LogWarning("Serialized Property is null!");
				return;
			}

			SerializedProperty poseProp = afkPoseProp.FindPropertyRelative("pose");
			SerializedProperty speedProp = afkPoseProp.FindPropertyRelative("speed");

			AnimationClip clip = poseProp.objectReferenceValue as AnimationClip;
			float speed = speedProp.floatValue;

			EditorGUI.BeginChangeCheck();
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.Label($"{stance} AFK Pose ({state})", GUILayout.Width(150));

				GUI.color = Color.white;

				using(new EditorGUI.DisabledScope(disabled))
				{
					clip = (AnimationClip)EditorGUILayout.ObjectField(clip, typeof(AnimationClip), false);
					EditorGUIUtility.IconContent("SpeedScale");
					GUILayout.Label(EditorGUIUtility.IconContent("Animation.Play"), GUILayout.Width(20), GUILayout.Height(20));
					speed = EditorGUILayout.FloatField(speed, GUILayout.Width(25));

					if (IconButton("Refresh", EditorGUIUtility.singleLineHeight))
					{
						clip = defaultPose;
						speed = 1.0f;
					}
				}
			}

			if (EditorGUI.EndChangeCheck()) {
				if (poseProp.objectReferenceValue != null) {
					Undo.RecordObject(poseProp.objectReferenceValue, "Modify AFK Pose Clips");
				}

				poseProp.objectReferenceValue = clip ?? poseProp.objectReferenceValue;
				speedProp.floatValue = speed;
			}
		}

		void DrawMenuEdit()
		{
			GUILayout.Space(20);

			GUILayout.Label("Menu Pose Settings", EditorStyles.whiteLargeLabel);

			EditorGUI.BeginChangeCheck();
			AnimationClip clip = soTarget.FindProperty("MenuPose.pose").objectReferenceValue as AnimationClip;
			float speed = soTarget.FindProperty("MenuPose.speed").floatValue;

			GUILayout.Label("Menu Pose Clip");
			using (new EditorGUILayout.HorizontalScope())
			{
				using (new EditorGUILayout.VerticalScope())
				{
					clip = (AnimationClip)EditorGUILayout.ObjectField(clip, typeof(AnimationClip), false);
					speed = EditorGUILayout.FloatField("Menu Pose Speed", speed);
				}

				if (IconButton("Refresh"))
				{
					clip = DefaultGoClips.DefaultMenuClip;
					speed = 1.0f;
				}
			}

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Modify Menu Pose");
				GoPoseFunctions.SetGoPose(soTarget, "MenuPose", clip, speed);
			}
		}

		bool IconButton(string iconName, float height = -1.0f, float width = 50)
		{
			height = height < 0 ? EditorGUIUtility.singleLineHeight : height;
			return GUILayout.Button(EditorGUIUtility.IconContent("Refresh"), GUILayout.Height(height), GUILayout.Width(width));
		}
	}
}
#endif