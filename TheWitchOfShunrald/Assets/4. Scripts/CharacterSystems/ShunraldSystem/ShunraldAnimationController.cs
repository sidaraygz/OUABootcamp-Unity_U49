using CameraSystem;
using UnityEngine;

namespace Shunrald
{
    public class ShunraldAnimationController : MonoBehaviour
    {
        //private ShunraldController controller;
        private Animator shunraldAnimator;

        private const string isDeath = "isDeath";
        private float initialAnimSpeed, freezeAnimSpeed = 0f,
            lerpSpeed = .2f, elapsedTime = 0f;

        private void Awake()
        {
            GetRequiredComponents();
        }

        private void GetRequiredComponents()
        {
            shunraldAnimator = GetComponent<Animator>();
        }

        // Animate the character
        public void AnimateCharacter(Vector3 _input, float veloX, float veloZ, Animator _animator)
        {
            Vector3 rotatedInput = _input.ToIso();
            //Vector3 rotatedInput = _input;
            veloX = Vector3.Dot(rotatedInput.normalized, transform.right);
            veloZ = Vector3.Dot(rotatedInput.normalized, transform.forward);

            

            _animator.SetFloat("VelocityX", veloX, .1f, Time.deltaTime);
            _animator.SetFloat("VelocityZ", veloZ, .1f, Time.deltaTime);
        }

        // This petrification method must be summoned when our Witch is killed by the Weeping Angel.
        public void PetrificationByAngel()
        {
            while (elapsedTime < lerpSpeed)
            {
                elapsedTime += Time.deltaTime;
                float time = elapsedTime / lerpSpeed;

                shunraldAnimator.speed = Mathf.Lerp(initialAnimSpeed, freezeAnimSpeed, time);
            }

            shunraldAnimator.speed = freezeAnimSpeed;
        }

        public void PlayDeathAnim()
        {
            shunraldAnimator.SetBool(isDeath, true);
        }
    }
}