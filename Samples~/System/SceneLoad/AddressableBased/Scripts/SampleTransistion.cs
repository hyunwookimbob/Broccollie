using System.Collections;
using System.Collections.Generic;
using CollieMollie.System;
using UnityEngine;

public class SampleTransistion : MonoBehaviour
{
    [SerializeField] private SceneAddressableEventChannel sceneEventChannel = null;
    [SerializeField] private SceneAddressablePreset sceneOne = null;

    private void Start()
    {
        sceneEventChannel.RaiseSceneLoadEvent(sceneOne, true);
    }
}