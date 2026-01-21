using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraEffectType
{
    public enum eCamType {snapInteract, lockPos, none};
        [Header("Inscribed")]
    public eCamType _camEffectType = eCamType.none;
    public float effectDuration = 0;
    public GameObject uiGO;
    [Header("Shake strength")]
    public float shakeStrength = 0;
    [Header("Lock Pos")]
    public Vector3 lockedPos;

}
