﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.OpenMM8.Scripts.Gameplay
{
    public partial class UiMgr
    {
        public enum CharDetailState
        {
            None,
            Stats,
            Inventory,
            Skills,
            Awards
        }

        public class CharDetailUIStateArgs
        {
            public CharDetailState EnterState = CharDetailState.None;

            public CharDetailUIStateArgs(CharDetailState enterState)
            {
                EnterState = enterState;
            }
        }

        public class CharDetailUIState : UIState
        {
            private CharDetailState m_State;

            public override bool OnActionPressed(string action)
            {
                if (action == "Escape")
                {
                    UiMgr.Instance.ReturnToGame();

                    return true;
                }
                else if (action == "Stats")
                {
                    SwitchState(CharDetailState.Stats);
                }
                else if (action == "Inventory")
                {
                    SwitchState(CharDetailState.Inventory);
                }
                else if (action == "Skills")
                {
                    SwitchState(CharDetailState.Skills);
                }
                else if (action == "Awards")
                {
                    SwitchState(CharDetailState.Awards);
                }
                else if (action == "NextPlayer")
                {
                    UiMgr.Instance.m_PlayerParty.SelectNextCharacter();
                }

                return false;
            }

            public override bool EnterState(object stateArgs)
            {
                if (UiMgr.Instance.m_PlayerParty.ActiveCharacter == null)
                {
                    UiMgr.Instance.m_PlayerParty.SelectCharacter(0);
                }

                CharDetailUIStateArgs args = (CharDetailUIStateArgs)stateArgs;

                UiMgr.Instance.SetupForPartialUiState(this);

                UiMgr.Instance.m_CharDetailUI.CanvasHolder.enabled = true;
                SwitchState(args.EnterState);

                // Register events
                PlayerParty.OnActiveCharacterChanged += OnActiveCharacterChanged;

                return true;
            }

            public override void LeaveState()
            {
                // Unregister from registered events
                PlayerParty.OnActiveCharacterChanged -= OnActiveCharacterChanged;

                UiMgr.Instance.m_CharDetailUI.CanvasHolder.enabled = false;
            }

            private void OnActiveCharacterChanged(Character chr)
            {
                DisplayDetailState(chr);
            }

            private void DisplayDetailState(Character chr)
            {
                if (chr == null)
                {
                    Logger.LogError("NULL character - cannot display char detail state info");
                    return;
                }

                UiMgr.Instance.m_CharDetailUI.DollBodyImage.sprite = chr.UI.Sprites.DollBody;
                UiMgr.Instance.m_CharDetailUI.DollBodyImage.SetNativeSize();
                UiMgr.Instance.m_CharDetailUI.DollLeftHandImage.sprite = chr.UI.Sprites.DollLeftHandOpen;
                UiMgr.Instance.m_CharDetailUI.DollLeftHandImage.SetNativeSize();
                UiMgr.Instance.m_CharDetailUI.DollRightHandImage.sprite = chr.UI.Sprites.DollRightHandDown;
                UiMgr.Instance.m_CharDetailUI.DollRightHandImage.SetNativeSize();

                switch (m_State)
                {
                    case CharDetailState.Stats:
                        break;

                    case CharDetailState.Inventory:
                        break;

                    case CharDetailState.Skills:
                        break;

                    case CharDetailState.Awards:
                        break;

                    default:
                        Debug.Log("Unknown state: " + m_State.ToString());
                        break;
                }
            }

            private void HideAllSubstates()
            {
                UiMgr.Instance.m_CharDetailUI.StatsUI.Holder.SetActive(false);
                UiMgr.Instance.m_CharDetailUI.SkillsUI.Holder.SetActive(false);
                UiMgr.Instance.m_CharDetailUI.InventoryUI.Holder.SetActive(false);
                UiMgr.Instance.m_CharDetailUI.AwardsUI.Holder.SetActive(false);
            }

            private void SwitchState(CharDetailState newState)
            {
                if (m_State == newState)
                {
                    return;
                }

                m_State = newState;

                HideAllSubstates();

                switch (newState)
                {
                    case CharDetailState.Stats:
                        UiMgr.Instance.m_CharDetailUI.StatsUI.Holder.SetActive(true);
                        break;

                    case CharDetailState.Skills:
                        UiMgr.Instance.m_CharDetailUI.SkillsUI.Holder.SetActive(true);
                        break;

                    case CharDetailState.Inventory:
                        UiMgr.Instance.m_CharDetailUI.InventoryUI.Holder.SetActive(true);
                        break;

                    case CharDetailState.Awards:
                        UiMgr.Instance.m_CharDetailUI.AwardsUI.Holder.SetActive(true);
                        break;

                    case CharDetailState.None:
                        UiMgr.Instance.ReturnToGame();
                        break;

                    default:
                        break;
                }

                DisplayDetailState(UiMgr.Instance.m_PlayerParty.ActiveCharacter);
            }
        }
    } // public partial class UiMgr
} // namespace
