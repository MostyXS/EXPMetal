using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MostyProUI
{
    //Add to default scene Canvas(Defines menu and dead menu)
    public class MainCanvas : MonoBehaviour
    {

        private void Awake()
        {
            Transform = transform;
        }
        
        public static Transform Transform
        {
            get; private set;
        }


    }
}