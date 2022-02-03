using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SequenceMaterial : MonoBehaviour
{
    [SerializeField]
    private Renderer sequenceRenderer;
    [SerializeField]
    private int materialIndex = 0;
    [SerializeField]
    private Texture2D[] textureSequence;

    // how often in decimals between 0 and 1 we should switch to the next texture
    private float changeRate = 0f;

    private void Awake()
    {
        changeRate = 1f / textureSequence.Length;
    }

    public void UpdateSequenceNormalized(float normalizedTime)
    {
        int sequenceIndex = Mathf.FloorToInt(normalizedTime / changeRate) % textureSequence.Length;

        SetTexture(sequenceIndex);
    }

    private void SetTexture(int index)
    {
        sequenceRenderer.materials[materialIndex].mainTexture = textureSequence[index];
    }
}
