using System.Linq;
using System.Xml.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace Utility.Animation
{
    public static class AnimatorExtensions
    {
        public static bool HasParamter(this Animator @this, string parameter)
        {
            var parameters = @this.parameters;   
            for(var i = 0; i < parameters.Length; ++i)
            {
                if (parameters[i].name == parameter)
                    return true;
            }

            return false;
        }
    }
}