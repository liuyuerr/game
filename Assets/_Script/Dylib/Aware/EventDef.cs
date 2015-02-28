using UnityEngine;
using System.Collections;

namespace Dylib
{
	public class EventDef
	{
		#region CommonEvent
		public const int NONE 								= 0;

		//config
		public const int EVENT_CONFIG_LOAD					= 10;
		public const int EVENT_CONFIG_INIT					= 11;
		public const int EVENT_CONFIG_INIT_COMPLETE			= 12;

		//game
		public const int EVENT_GAME_READY 					= 100;

		// Input
		public const int EVENT_INPUT_MOUSE_LBTNDOWN		    = 100;
		public const int EVENT_INPUT_MOUSE_RBTNDOWN		    = 102;
		public const int EVENT_INPUT_MOUSE_LBTNDBCLICK	    = 103;
		public const int EVENT_INPUT_MOUSE_RBTNDBCLICK	    = 104;
		public const int EVENT_INPUT_KEY_RELEASE		    = 105;
		#endregion

		#region battle
		public const int BATTLE_DEMO						= 999;
		public const int BATTLE_CONTROL 					= 900;
		public const int BATTLE_START						= 901;
		public const int BATTLE_PAUSE 						= 902;
		public const int BATTLE_RESUME 						= 903;
		public const int BATTLE_END							= 904;
		public const int BATTLE_RESULT						= 905;
		public const int BATTLE_EVENT                       = 906;
		#endregion

	}
}

	
	

