namespace Utility.Time
{
    using UnityEngine;
    public struct TimeSince
    {
        private float _time;
 
        public static implicit operator float(TimeSince ts)
        {
            return Time.time - ts._time;
        }
 
        public static implicit operator TimeSince(float ts)
        {
            return new TimeSince { _time = Time.time - ts };
        }
    }
}