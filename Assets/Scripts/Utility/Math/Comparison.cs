using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility.Math
{
    public static class Comparison
    {
        public static bool IntDistanceGreaterThan(int a, int b, int distance)
        {
            var absA = Mathf.Abs(a);
            var absB = Mathf.Abs(b);

            return Mathf.Abs(absA - absB) > distance;
        }
        
        public static List<int> GetRangeBetween(int A, int B)
        {
            List<int> val;
            
            var dist = Mathf.Abs(A - B) - 1;
            if (B < A)
                val = Enumerable.Range(B + 1, dist).ToList();
            else
                val = Enumerable.Range(A + 1, dist).ToList();
            return val;
        }
    }
}