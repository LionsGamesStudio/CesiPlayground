using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Core.UI
{
    [Serializable] public class NovaEvent : UnityEvent
    {
        /// <summary>
        /// Lists all the methods subscribed to the event
        /// </summary>
        public List<object> ListSubscribers()
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            FieldInfo field = typeof(UnityEventBase).GetField("m_Calls", bindingFlags);

            List<object> subscribers = new List<object>();

            if (field != null)
            {
                object callList = field.GetValue(this);
                FieldInfo callsField = callList.GetType().GetField("m_RuntimeCalls", bindingFlags);

                // Get the runtime calls
                if (callsField != null)
                {
                    var runtimeCalls = callsField.GetValue(callList) as System.Collections.IList;

                    if (runtimeCalls != null)
                    {
                        foreach (var call in runtimeCalls)
                        {
                            MethodInfo method = call.GetType().GetMethod("GetMethod");
                            if (method != null)
                            {
                                var methodInfo = method.Invoke(call, null) as MethodInfo;
                                if (methodInfo != null)
                                {
                                    subscribers.Add(methodInfo);
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("No runtime calls found");
                    }
                }
            }

            return subscribers;
        }
    }
}
