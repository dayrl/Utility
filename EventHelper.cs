#region License and Copyright

/*
 * Dotnet Commons Reflection 
 *
 *
 * This library is free software; you can redistribute it and/or modify it 
 * under the terms of the GNU Lesser General Public License as published by 
 * the Free Software Foundation; either version 2.1 of the License, or 
 * (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but 
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License 
 * for more details. 
 *
 * You should have received a copy of the GNU Lesser General Public License 
 * along with this library; if not, write to the 
 * Free Software Foundation, Inc., 
 * 
 * 59 Temple Place, 
 * Suite 330, 
 * Boston, 
 * MA 02111-1307 
 * USA 
 * 
 */

#endregion

using System;
using System.Diagnostics;
using System.Reflection;

namespace Zdd.Utility
{
    /// <summary>	
    /// This class contains methods that will help to assign event handlers at runtime. These
    /// methods are very useful in dealing with objects at runtime. Delegates can be added to 
    /// events at runtime. Delegates wired to events of an object can be "copied" and wired up
    /// to a similar object, etc. These powerful utility operations are not part of the .Net Base 
    /// Class Library. It is necessary 
    /// </summary>	
    public static class EventHelper
    {
        /// <summary>
        /// Assigns a delegate to an event handler
        /// </summary>
        /// <param name="targetObj">target object whose event is to be wired up </param>
        /// <param name="eventName">the name of the event</param>
        /// <param name="delegateObj">the object that contains the delegate method</param>
        /// <param name="methodName">name of the delegate method</param>
        /// <param name="ignoreCase">ignore case sensitivity in the method name</param>		
        public static void AddToEvent(object targetObj, string eventName, object delegateObj, string methodName,
                                      bool ignoreCase)
        {
            if (targetObj == null)
                throw new ArgumentNullException("targetObj");

            if ((eventName == null) || (eventName.Length < 1))
                throw new ArgumentException("eventName cannot be null or empty");

            if (delegateObj == null)
                throw new ArgumentNullException("delegateObj");

            if ((methodName == null) || (methodName.Length < 1))
                throw new ArgumentException("methodName cannot be null or empty");

            // Get the event Information to wire up
            EventInfo ei = GetEventInfo(targetObj, eventName, ignoreCase);

            if (ei != null)
            {
                try
                {
                    // try to create a delegate of the event delegate type, with the target event object and method as the event sink.
                    Delegate eventHandler = Delegate.CreateDelegate(ei.EventHandlerType, delegateObj, methodName);
                    AddToEvent(targetObj, eventName, eventHandler, ignoreCase);
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Unable to assign event handler to: " + targetObj.ToString() + " (" + eventName +
                                    ":" + methodName + "):  " + e.Message);
                    throw e;
                }
            }
            else
            {
                Trace.WriteLine("Unable to acquire event information for: " + targetObj.ToString() + " (" + eventName +
                                ":" + methodName + ")");
            }
        }

        /// <summary>
        /// Assigns a delegate to an event of a specified object
        /// </summary>
        /// <param name="targetObj">target object whose event is to be wired up </param>
        /// <param name="eventName">the name of the event</param>
        /// <param name="eventHandler">Encapsulates a method or methods to be 
        /// invoked when the event is raised by the target</param>
        /// <param name="ignoreCase"></param>
        public static void AddToEvent(object targetObj, string eventName, Delegate eventHandler, bool ignoreCase)
        {
            if (targetObj == null)
                throw new ArgumentNullException("targetObj");

            if ((eventName == null) || (eventName.Length < 1))
                throw new ArgumentException("eventName cannot be null or empty");

            if (eventHandler == null)
                throw new ArgumentNullException("eventHandler");

            EventInfo ei = GetEventInfo(targetObj, eventName, ignoreCase);

            if (ei != null)
            {
                ei.AddEventHandler(targetObj, eventHandler);
            }
            else
            {
                Trace.WriteLine("Unable to acquire event information for: " + targetObj.ToString() + " (" + eventName +
                                ":" + eventHandler.Method.Name + ")");
            }
        }

        /// <summary>
        /// Copy event handlers from one object to another given an event name.
        /// </summary>
        /// <param name="source">Source object to copy from</param>
        /// <param name="target">target object to copy event handlers to</param>
        /// <param name="eventName">events to copy event handlers to</param>
        public static void CopyEventHandlers(object source, object target, string eventName)
        {
            if (source == null) throw new ArgumentNullException("source");

            if (target == null) throw new ArgumentNullException("target");

            // check if event exist
            if (!HasEvent(source, eventName, false))
                throw new ArgumentException(
                    String.Format("The '{0}' object does not have a '{1}' event", source.GetType().FullName, eventName));

            if (!HasEvent(target, eventName, false))
                throw new ArgumentException(
                    String.Format("The '{0}' object does not have a '{1}' event", target.GetType().FullName, eventName));

            Delegate targetDelegate = GetEventDelegateFromObject(target, eventName);
            targetDelegate = MulticastDelegate.Combine(targetDelegate, GetEventDelegateFromObject(source, eventName));
            GetFieldInfo(target, eventName).SetValue(target, targetDelegate);
        }

        /// <summary>
        /// Copy event handlers from one object to another given an <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="eventInfo"></param>
        public static void CopyEventHandlers(object source, object target, EventInfo eventInfo)
        {
            string eventName = eventInfo.Name;
            CopyEventHandlers(source, target, eventName);
        }

        /// <summary>
        /// Copy all event handlers from one object to another
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <exception cref="ArgumentNullException">If source parameter is <see lang="null" />; and/or
        /// target parameter is also <see lang="null" />  </exception>
        public static void CopyEventHandlers(object source, object target)
        {
            if (source == null) throw new ArgumentNullException("source");

            if (target == null) throw new ArgumentNullException("target");

            EventInfo[] events = source.GetType().GetEvents();

            foreach (EventInfo e in events)
            {
                if (!HasEvent(target, e.Name, false))
                    continue;

                CopyEventHandlers(source, target, e);
            }
        }

        /// <summary>
        /// Get the delegate of an event of an object
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static Delegate GetEventDelegateFromObject(object source, string eventName)
        {
            return (Delegate) GetFieldInfo(source, eventName).GetValue(source);
        }

        /// <summary>
        /// Get the field info of an event of an object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventName">Name of the event.</param>
        public static FieldInfo GetFieldInfo(object source, string eventName)
        {
            return source.GetType().GetField(eventName,
                                             BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <summary>
        /// Get the EventInfo instance for the supplied object and event name.
        /// </summary>
        /// <param name="srcObj">The object on which we want to find the named event.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="ignoreCase">flag to indicate if it is case insensitive</param>
        /// <returns>The <see cref="System.Reflection.EventInfo"/>  instance or <see langword="null" />.</returns>
        public static EventInfo GetEventInfo(object srcObj, string eventName, bool ignoreCase)
        {
            if (srcObj == null)
                throw new ArgumentNullException("srcObj");

            BindingFlags bindingAttrs = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            if (!ignoreCase)
            {
                EventInfo ei = srcObj.GetType().GetEvent(eventName, bindingAttrs);
                return ei;
            }

            EventInfo[] eventInfos = srcObj.GetType().GetEvents(bindingAttrs);

            foreach (EventInfo ei in eventInfos)
            {
                if (ei.Name.ToLower().Equals(eventName.ToLower()))
                    return ei;
            }
            return null;
        }

        /// <summary>
        /// Get all the delegates wired into an event
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"><see cref="System.Reflection.EventInfo"/> of an event</param>
        /// <returns>an array of <see cref="System.Delegate" /> wired to the event</returns>
        public static Delegate[] GetDelegates(object source, EventInfo e)
        {
            return GetDelegates(source, e.Name);
        }

        /// <summary>
        /// Get all the delegates wired into an event
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eventName">Name of an event to search</param>
        /// <returns>an array of <see cref="System.Delegate" /> wired to the event</returns>
        public static Delegate[] GetDelegates(object source, string eventName)
        {
            Delegate srcDelegate = GetEventDelegateFromObject(source, eventName);
            return srcDelegate.GetInvocationList();
        }

        /// <summary>
        /// Determine if an object has a particular event based on the name
        /// </summary>
        /// <param name="srcObj">source object to inspect</param>
        /// <param name="eventName">name of the event</param>
        /// <param name="ignoreCase">flag to indicate if it is case insensitive</param>
        /// <returns>true if the object has a specific event, false otherwise</returns>
        public static bool HasEvent(object srcObj, string eventName, bool ignoreCase)
        {
            if (srcObj == null)
                throw new ArgumentNullException("srcObj");

            BindingFlags bindingAttrs = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            if (!ignoreCase)
            {
                return (srcObj.GetType().GetEvent(eventName, bindingAttrs) != null);
            }

            EventInfo[] eventInfos = srcObj.GetType().GetEvents(bindingAttrs);

            foreach (EventInfo ei in eventInfos)
            {
                if (ei.Name.ToLower().Equals(eventName.ToLower()))
                    return true;
            }

            return false;
        }
    }
}