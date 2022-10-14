using System;
using System.Collections;
using System.Collections.Generic;
using CollieMollie.Audio;
using CollieMollie.Helper;
using UnityEngine;

namespace CollieMollie.UI
{
    [RequireComponent(typeof(Animator))]
    public class UIAnimationFeature : MonoBehaviour
    {
        #region Variable Field
        [SerializeField] private bool _isEnabled = true;
        
        [SerializeField] private List<Element> _elements = null;
        #endregion

        #region Public Functions
        public void Change(InteractionState state)
        {
            if (!_isEnabled) return;

            foreach (Element element in _elements)
            {
                if (!element.IsEnabled) continue;
                element.PlayAnimation(this, state);
            }
        }

        #endregion

        [Serializable]
        public class Element
        {
            #region Variabled Field
            public bool IsEnabled = true;
            public Animator Animator = null;
            public AnimatorOverrideController OverrideAnimator = null;
            public UIAnimationPreset Preset = null;

            private IEnumerator _animationAction = null;
            #endregion

            #region Features
            public void PlayAnimation(MonoBehaviour mono, InteractionState state)
            {
                if (_animationAction != null)
                    mono.StopCoroutine(_animationAction);

                UIAnimationPreset.AnimationState animationState = Array.Find(Preset.AnimationStates, x => x.ExecutionState == state);
                if (!animationState.IsValid())
                    animationState = Array.Find(Preset.AnimationStates, x => x.ExecutionState == InteractionState.Default);

                if (animationState.IsValid())
                {
                    if (!animationState.IsEnabled) return;

                    _animationAction = PlayAnimation();
                    mono.StartCoroutine(_animationAction);
                }

                IEnumerator PlayAnimation()
                {
                    OverrideAnimator[state.ToString()] = animationState.Animation;

                    if (Animator.runtimeAnimatorController != OverrideAnimator)
                        Animator.runtimeAnimatorController = OverrideAnimator;

                    if (animationState.ExecutionState != InteractionState.Hovered)
                    {
                        InteractionState[] layerZeroStates = new InteractionState[]
                        {
                            InteractionState.Default,
                            InteractionState.Pressed,
                            InteractionState.Selected,
                            InteractionState.NonInteractive,
                            InteractionState.Show
                        };
                        for (int i = 0; i < layerZeroStates.Length; i++)
                        {
                            if (layerZeroStates[i] == animationState.ExecutionState) continue;
                            Animator.SetBool(layerZeroStates[i].ToString(), false);
                        }
                        Animator.SetBool(animationState.ExecutionState.ToString(), true);
                    }
                    else Animator.SetBool(InteractionState.Hovered.ToString(), true);

                    if (animationState.ExecutionState == InteractionState.Default ||
                        animationState.ExecutionState == InteractionState.Selected)
                        Animator.SetBool(InteractionState.Hovered.ToString(), false);

                    yield return null;
                }
            }
            #endregion
        }
    }
}