#if UNITY_EDITOR
using System.Collections.Generic;

namespace Cam.GoGo {
	public static class GoPoseConstants
	{
		public const string AVATAR_3D_THUMBNAIL_STATE_NAME = "Avatar 3D Thumbnail";

		public const string GO_STANDING_POSE_INIT_STATE_NAME = "Standing Afk Init";
		public const string GO_CROUCHING_POSE_INIT_STATE_NAME = "Crouching Afk Init";
		public const string GO_PRONE_POSE_INIT_STATE_NAME = "Proning Afk Init";

		public const string GO_STANDING_POSE_LOOPING_STATE_NAME = "AFK Standing Loop";
		public const string GO_CROUCHING_POSE_LOOPING_STATE_NAME = "AFK Crouching Loop";
		public const string GO_PRONE_POSE_LOOPING_STATE_NAME = "AFK Proning Loop";

		public const string GO_STANDING_POSE_EXIT_STATE_NAME = "AFK Standing Stop";
		public const string GO_CROUCHING_POSE_EXIT_STATE_NAME = "AFK Crouching Stop";
		public const string GO_PRONE_POSE_EXIT_STATE_NAME = "AFK Proning Stop";


		public const string DEFAULT_AFK_POSE_GUID = "806c242c97b686d4bac4ad50defd1fdb";

		public const string DEFAULT_AFK_CROUCH_INIT_GUID = "a402bc1f4e64dea4e96c08c14725f858";
		public const string DEFAULT_AFK_CROUCH_LOOP_GUID = "806c242c97b686d4bac4ad50defd1fdb";
		public const string DEFAULT_AFK_CROUCH_EXIT_GUID = "36dda18e9c641ca4db19409ee11a80b4";

		public const string DEFAULT_AFK_PRONE_INIT_GUID = "a402bc1f4e64dea4e96c08c14725f858";
		public const string DEFAULT_AFK_PRONE_LOOP_GUID = "806c242c97b686d4bac4ad50defd1fdb";
		public const string DEFAULT_AFK_PRONE_EXIT_GUID = "36dda18e9c641ca4db19409ee11a80b4";

		public const string DEFAULT_AFK_STAND_INIT_GUID = "3fc99a15c5a07d742b4318a270abdb43";
		public const string DEFAULT_AFK_STAND_LOOP_GUID = "e3de79e796f759f43845f862fb99627f";
		public const string DEFAULT_AFK_STAND_EXIT_GUID = "fbeb10f24ed9f2b4abb12eb3f91d6ef1";

		public const string DEFEAULT_MENU_POSE_GUID = "8b4ce8e0f37bce74eb15ee7b628d543c";


		public static readonly Dictionary<GoAFKStance, GoAFKAnimatorStateNames> GOGO_STATE_NAME_DICT = new Dictionary<GoAFKStance, GoAFKAnimatorStateNames> {
			{
				GoAFKStance.Stand, new GoAFKAnimatorStateNames() {
					INIT = GO_STANDING_POSE_INIT_STATE_NAME,
					LOOPING = GO_STANDING_POSE_LOOPING_STATE_NAME,
					EXIT = GO_STANDING_POSE_EXIT_STATE_NAME
				}
			},{
				GoAFKStance.Crouch, new GoAFKAnimatorStateNames() {
					INIT = GO_CROUCHING_POSE_INIT_STATE_NAME,
					LOOPING = GO_CROUCHING_POSE_LOOPING_STATE_NAME,
					EXIT = GO_CROUCHING_POSE_EXIT_STATE_NAME
				}
			},{
				GoAFKStance.Prone, new GoAFKAnimatorStateNames() {
					INIT = GO_PRONE_POSE_INIT_STATE_NAME,
					LOOPING = GO_PRONE_POSE_LOOPING_STATE_NAME,
					EXIT = GO_PRONE_POSE_EXIT_STATE_NAME
				}
			}
		};

	}
}
#endif