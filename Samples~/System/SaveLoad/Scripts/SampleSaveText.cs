using System.Collections;
using System.Collections.Generic;
using CollieMollie.System;
using TMPro;
using UnityEngine;

public class SampleSaveText : MonoBehaviour, ISaveable
{
    [SerializeField] private TextMeshProUGUI _nameText = null;

    public void LoadState(object state)
    {
        if (state == null) return;
        _nameText.text = (state as SampleGameData).Name;
    }

    public void SaveState(object state)
    {
        if (state == null) return;
        (state as SampleGameData).Name = _nameText.text;
    }
}
