﻿using UnityEngine;
using System.Collections;
using UnityEngine.Video;
using Assets.OpenMM8.Scripts.Gameplay;
using UnityEngine.Experimental.UIElements;
using System.Collections.Generic;

public delegate void Talk(Character talkerChr, Talkable talkedToObj);
public delegate void EndTalk(Talkable talkedToObj);

public class Talkable : Interactable
{
    static public event Talk OnTalkStart;
    static public event EndTalk OnTalkEnd;

    public string Location;
    public bool IsHouse = false;
    public VideoScene VideoScene = null;
    public List<TalkProperties> TalkProperties = new List<TalkProperties>();

    public override bool CanInteract(GameObject interacter, RaycastHit interactRay)
    {
        if (!enabled)
        {
            return false;
        }

        HostilityChecker ownerHostilityChecker = GetComponent<HostilityChecker>();
        HostilityChecker interacterHostilityChecker = interacter.GetComponent<HostilityChecker>();
        if ((ownerHostilityChecker != null) && (interacterHostilityChecker != null))
        {
            if (ownerHostilityChecker.IsHostileTo(interacter))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Interact(GameObject interacter, RaycastHit interactRay)
    {
        if (!CanInteract(interacter, interactRay))
        {
            return false;
        }

        PlayerParty playerParty = interacter.GetComponent<PlayerParty>();
        Character talker = playerParty.GetMostRecoveredCharacter();

        if (OnTalkStart != null)
        {
            OnTalkStart(talker, this);
        }

        return true;
    }

    public void OnEndInteract()
    {
        if (OnTalkEnd != null)
        {
            OnTalkEnd(this);
        }
    }
}
