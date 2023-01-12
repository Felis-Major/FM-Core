using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FM.Runtime.Helpers.GUIDSystem
{
    /// <summary>
    /// Attribute used to define a field as a GUID
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GUIDAttribute : PropertyAttribute { }
}
