using Constants;
using UnityEngine;

namespace Experiment
{
	// TODO MOVE TO MRTK, create new button hints
	public class TutorialButtonHints : MonoBehaviour
    {
/*		public void ShowAll(Hand hand)
		{
			*//*ShowButtonHints(hand);
			ShowTextHints(hand);*//*
			ISteamVR_Action_In action = SteamVR_Input.actionsIn[SteamVRActions.InteractUI];
			if (action.GetActive(hand.handType))
			{
				// ControllerButtonHints.ShowButtonHint(hand, action);
				ControllerButtonHints.ShowTextHint(hand, action, "Select sphere");
			}
		}

		public void HideAll(Hand hand)
		{
			*//*ShowButtonHints(hand);
			ShowTextHints(hand);*//*
			ISteamVR_Action_In action = SteamVR_Input.actionsIn[SteamVRActions.InteractUI];
			if (action.GetActive(hand.handType))
			{
				// ControllerButtonHints.ShowButtonHint(hand, action);
				ControllerButtonHints.HideAllTextHints(hand);
			}
		}*/
	}
}