using System;
using UnityEngine;

namespace Gameplay
{
    public class PlayerAnimationsHandler : MonoBehaviour
    {
        private const string IsInMotion = "IsInMotion";
        private const string JumpTrigger = "JumpTrigger";
        private const string LandedParameter = "IsLanded";

        [SerializeField]
        private float moveTreshold = 0.1f;
        
        [SerializeField]
        private Animator _animator;

        public void SetMove(float moveForce) => 
            _animator.SetBool(IsInMotion, Math.Abs(moveForce) > moveTreshold);

        public void Jump() => _animator.SetTrigger(JumpTrigger);
        
        public void SetLanded(bool value) => _animator.SetBool(LandedParameter, value);
    }
}