using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Grid.HexagonGridGeneration
{
    public static class HexTile
    {
        public const float Radius = 2f;
        private const float InnerRadius = Radius * 0.866025404f;
        
        public static Vector3[] Points = {
            new Vector3(0f, 0f, Radius),
            new Vector3(InnerRadius, 0f, 0.5f * Radius),
            new Vector3(InnerRadius, 0f, -0.5f * Radius),
            new Vector3(0f, 0f, -Radius),
            new Vector3(-InnerRadius, 0f, -0.5f * Radius),
            new Vector3(-InnerRadius, 0f, 0.5f * Radius),
            new Vector3(0f, 0f, Radius)
        };

        public static Vector3 GetCoordinateFromOffset(Vector3 offset)
        {
            var position = Vector3.zero;

            // ReSharper disable once PossibleLossOfFraction
            position.x = ((int)offset.x + (int)offset.z * 0.5f - (int)offset.z / 2) * InnerRadius * 1f;
            position.y = 0f;
            position.z = (int)offset.z * Radius * 0.75f;

            return position;
        }
    }
}