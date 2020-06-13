using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EXPMetal.Utils
{
    public class ComparerCalculator : MonoBehaviour
    {
        public static bool AtRightHeight(Transform attackerTransform, Transform targetTransform, float yAttackRange)
        {
            return targetTransform.position.y > attackerTransform.position.y - yAttackRange && targetTransform.position.y < attackerTransform.position.y + yAttackRange;
        }
        public static bool IsFarFrom(Transform attackerTransform, Transform targetTransform, float xAttackRange)
        {
            return (attackerTransform.position - targetTransform.position).sqrMagnitude > xAttackRange*xAttackRange;
        }
        public static bool InRange(float min, float max, float number)
        {
            return (number >= min && number <= max);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <param name="targetPosition"></param>
        /// <param name="animRotationError">defines error of looking when looking </param>
        /// <param name="rotationZ"></param>
        /// <param name="direction">1:looking right, -1: looking left</param>
        public static void CalculateRotation(Vector2 currentPosition, Vector2 targetPosition, float animRotationError, out float rotationZ, int direction)
        {
            Vector2 difference = currentPosition - targetPosition;
            float rawRotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            rotationZ = direction > 0 ? rawRotationZ + animRotationError : rawRotationZ;
        }

    }
}