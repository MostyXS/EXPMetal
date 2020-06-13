using EXPMetal.Saving;
using MostyProUI.Audio;
using MostyProUI.PrefsControl;
using System.Collections.Generic;
using UnityEngine;

namespace EXPMetal.Control
{
    //Add this script to every pausable character entity
    public class CharacterEntity : MonoBehaviour, ISaveable
    {
        [Tooltip("Parametr for each character entity, changes saveable state for current scene for current character")]

        protected AudioSource mainAudio;
        protected AudioSource walkingSource; //Used for player and sometimes for enemies
        protected float defaultXScale;
        protected Animator animator;
        protected float charactersVolume = 0;
        

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            defaultXScale = Mathf.Abs(transform.localScale.x);
            walkingSource = GetComponent<AudioSource>();
            charactersVolume = PrefsController.CharactersVolume;
            if (!walkingSource) return;
            walkingSource.volume = charactersVolume;
        }
        protected virtual void Start()
        {
            mainAudio = MainAudioSource.Instance.value;
            
        }
        private enum TransformValues
        {
            position,
            scale
        }

        public object CaptureState()
        {
            
            Dictionary<TransformValues, object> transformValues = new Dictionary<TransformValues, object>();

            SerializableVector2 position = new SerializableVector2(transform.position);
            SerializableVector2 scale = new SerializableVector2(transform.localScale);

            transformValues.Add(TransformValues.position, position);
            transformValues.Add(TransformValues.scale, scale);

            return transformValues;
        }
        public void RestoreState(object state)
        {
            Dictionary<TransformValues, object> savedTransformValues = (Dictionary<TransformValues, object>)state;
          
            SerializableVector2 position = (SerializableVector2)savedTransformValues[TransformValues.position];
            SerializableVector2 scale = (SerializableVector2)savedTransformValues[TransformValues.scale];

            transform.position = position.ToVector2();
            transform.localScale = scale.ToVector2();
        }
        public bool CurrentAnimNameIs(string name)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
        }
    }
}
