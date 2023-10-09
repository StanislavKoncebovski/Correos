/***********************************************************************************
* File:         Correos.cs                                                         *
* Contents:     Class Correos                                                      *
* Author:       Stanislav Koncebovski (stanislav@pikkatech.eu)                     *
* Date:         2023-10-06 1053                                                    *
* Version:      1.0                                                                *
* Copyright:    pikkatech.eu (www.pikkatech.eu)                                    *
***********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Correos
{
	public static class Correos
	{
		#region Private Members
		/// <summary>
		/// Dictionary of request targets.
		/// Key: Correos name of the request target.
		/// Value: The instance of the method.
		/// </summary>
		private static Dictionary<string, MethodInfo>		_requestTargets			= new Dictionary<string, MethodInfo>();

		/// <summary>
		/// Dictionary of notification targets.
		/// Key: Correos name of the notification target.
		/// Value: List of methods subsc´ribed to the notification.
		/// </summary>
		private static Dictionary<string, List<MethodInfo>>	_notificationTargets	= new Dictionary<string, List<MethodInfo>>();

		private static Dictionary<string, object> _instances = new Dictionary<string, object>();
		#endregion

		#region Properties
		/// <summary>
		/// The registration policy.
		/// Default: labeled only.
		/// </summary>
		public static RegistrationPolicy RegistrationPolicy {get;set;} = RegistrationPolicy.LabeledOnly;

		/// <summary>
		/// Defines how to handle targets with the Correos name left empty.
		/// </summary>
		public static EmptyNameHandling EmptyNameHandling	{get;set;} = EmptyNameHandling.AssignFullMethodName;
		#endregion

		#region Public Registration / Unregistration.
		/// <summary>
		/// Registers the request and the notification targets of a type.
		/// </summary>
		/// <param name="t">The type to register.</param>
		public static void Register(Type t)
		{
			RegisterRequestTargets(t);
			RegisterNotificationTargets(t);
		}

		/// <summary>
		/// Registers the request and the notification targets for an assembly.
		/// </summary>
		/// <param name="assembly">The assembly to register.</param>
		public static void Register(Assembly assembly)
		{
			RegisterRequestTargets(assembly);
			RegisterNotificationTargets(assembly);
		}

		/// <summary>
		/// Unregisters the request and the notification targets of a type.
		/// </summary>
		/// <param name="t">The type to unregister.</param>
		public static void Unregister(Type t)
		{
			UnregisterRequestTargets(t);
			UnregisterNotificationTargets(t);
		}

		/// <summary>
		/// Unregisters the request and the notification targets of an assembly.
		/// </summary>
		/// <param name="assembly">The assembly to unregister.</param>
		public static void Unregister(Assembly assembly)
		{
			UnregisterRequestTargets(assembly);
			UnregisterNotificationTargets(assembly);
		}

		/// <summary>
		/// Unregisters all requests and notification targets.
		/// </summary>
		public static void UnregisterAll()
		{
			_requestTargets.Clear();
			_notificationTargets.Clear();
		}
		#endregion

		#region Requesting
		/// <summary>
		/// Sending a synchronous request to a request target.
		/// </summary>
		/// <param name="requestTargetName">The name of the request target.</param>
		/// <param name="args">Optional parameters for the request.</param>
		/// <returns>Response to the request.</returns>
		public static object Request(string requestTargetName, params object[] args)
		{
			if (_requestTargets.ContainsKey(requestTargetName))
			{
				MethodInfo methodInfo	= _requestTargets[requestTargetName];
				Type declaringType		= methodInfo.DeclaringType;

				object instance			= null;

				if (!_instances.ContainsKey(declaringType.Name))
				{
					instance = Activator.CreateInstance(declaringType);
					_instances[declaringType.Name] = instance;
				}

				instance = _instances[declaringType.Name];

				return methodInfo.Invoke(instance, args);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Notifies synchronously subscribers to a notification.
		/// </summary>
		/// <param name="notificationName">The name of the notification to raise.</param>
		/// <param name="args">Arguments for the notification.</param>
		public static void Notify(string notificationName, params object[] args)
		{
			if (_notificationTargets.ContainsKey(notificationName))	
			{
				MethodInfo[] methodInfos = _notificationTargets[notificationName].ToArray();

				foreach(MethodInfo methodInfo in methodInfos)
				{
					Type declaringType = methodInfo.DeclaringType;

					object instance = null;

					if (!_instances.ContainsKey(declaringType.Name))
					{
						instance = Activator.CreateInstance(declaringType);
						_instances[declaringType.Name] = instance;
					}

					instance = _instances[declaringType.Name];

					methodInfo.Invoke(instance, args);
				}
			}
		}

		/// <summary>
		/// Sending an asynchronous request to a request target.
		/// </summary>
		/// <param name="requestTargetName">The name of the request target.</param>
		/// <param name="args">Optional parameters for the request.</param>
		/// <returns>Response task to the request. Use its Result property to obtain the result.</returns>
		public static async Task<object> RequestAsync(string methodName, params object[] args)
		{
			return await Task.Run(() => Request(methodName, args));
		}

		/// <summary>
		/// Notifies asynchronously subscribers to a notification.
		/// </summary>
		/// <param name="notificationName">The name of the notification to raise.</param>
		/// <param name="args">Arguments for the notification.</param>
		public static async Task NotifyAsync(string methodName, params object[] args)
		{
			await Task.Run(() => Notify(methodName, args));
		}
		#endregion

		#region Private auxiliary
		/// <summary>
		/// Registers request targets for a type.
		/// </summary>
		/// <param name="t">The type to register its request targets.</param>
		private static void RegisterRequestTargets(Type t)
		{
			if (!t.IsDefined(typeof(CorreosAttribute), false) && Correos.RegistrationPolicy == RegistrationPolicy.LabeledOnly)
			{
				return;
			}

			MethodInfo[] methods = t.GetMethods().Where(m => m.GetCustomAttributes<RequestTargetAttribute>().Any()).ToArray();

            foreach (MethodInfo method in methods)
            {
                string key = method.GetCustomAttribute<RequestTargetAttribute>().Name;

				if (String.IsNullOrEmpty(key))
				{
					switch (EmptyNameHandling)
					{
						case EmptyNameHandling.AssignFullMethodName:
							key = $"{method.DeclaringType.FullName}.{method.Name}";
							break;

						case EmptyNameHandling.AssignMethodName:
							key = method.Name;
							break;

						case EmptyNameHandling.Skip:
						default:
							continue;
					}
				}
					
				if (!_requestTargets.ContainsKey(key))
				{
					_requestTargets.Add(key, method);
				}
            }
        }

		/// <summary>
		/// Registers request targets for an assembly.
		/// </summary>
		/// <param name="assembly">The assembly to register its request targets.</param>
		private static void RegisterRequestTargets(Assembly assembly)
		{
			Type[] targetTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes<RequestTargetAttribute>().Count() > 0).ToArray();

			foreach (Type t in targetTypes)
			{
				RegisterRequestTargets(t);
			}
		}

		/// <summary>
		/// Registers notification targets for a type.
		/// </summary>
		/// <param name="t">The type to register its notification targets.</param>
		private static void RegisterNotificationTargets(Type t)
		{
			if (!t.IsDefined(typeof(CorreosAttribute), false) && Correos.RegistrationPolicy == RegistrationPolicy.LabeledOnly)
			{
				return;
			}

			MethodInfo[] methods = t.GetMethods().Where(m => m.GetCustomAttributes<NotificationTargetAttribute>().Any()).ToArray();

			foreach (MethodInfo method in methods)
			{
				 string key = method.GetCustomAttribute<RequestTargetAttribute>().Name;

				if (!_notificationTargets.ContainsKey(key))
				{
					_notificationTargets[key] = new List<MethodInfo>();
				}

				_notificationTargets[key].Add(method);
			}
		}

		/// <summary>
		/// Registers notification targets for an assembly.
		/// </summary>
		/// <param name="assembly">The assembly to register its notification targets.</param>
		private static void RegisterNotificationTargets(Assembly assembly)
		{
			Type[] targetTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes<NotificationTargetAttribute>().Count() > 0).ToArray();

			foreach (Type t in targetTypes)
			{
				RegisterNotificationTargets(t);
			}
		}

		/// <summary>
		/// Unregisters the request targets of a type.
		/// </summary>
		/// <param name="t">The type to unregister.</param>
		private static void UnregisterRequestTargets(Type t)
		{
			if (!t.IsDefined(typeof(CorreosAttribute), false) && Correos.RegistrationPolicy == RegistrationPolicy.LabeledOnly)
			{
				return;
			}

			MethodInfo[] methods = t.GetMethods().Where(m => m.GetCustomAttributes<RequestTargetAttribute>().Any()).ToArray();

			foreach (MethodInfo method in methods)
			{
				string key = method.GetCustomAttributes<RequestTargetAttribute>().ToArray()[0].Name;

				if (_requestTargets.ContainsKey(key))
				{
					_requestTargets.Remove(key);
				}
			}
		}

		/// <summary>
		/// Unregisters the notification targets of a type.
		/// </summary>
		/// <param name="t">The type to unregister.</param>
		private static void UnregisterNotificationTargets(Type t)
		{
			if (!t.IsDefined(typeof(CorreosAttribute), false) && Correos.RegistrationPolicy == RegistrationPolicy.LabeledOnly)
			{
				return;
			}

			MethodInfo[] methods = t.GetMethods().Where(m => m.GetCustomAttributes<NotificationTargetAttribute>().Any()).ToArray();

			foreach (MethodInfo method in methods)
			{
				string key = method.GetCustomAttributes<NotificationTargetAttribute>().ToArray()[0].Name;

				if (_requestTargets.ContainsKey(key))
				{
					_requestTargets.Remove(key);
				}
			}
		}

		/// <summary>
		/// Unregisters the request targets of an assembly.
		/// </summary>
		/// <param name="t">The assembly to unregister.</param>
		private static void UnregisterRequestTargets(Assembly assembly)
		{
			Type[] targetTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes<RequestTargetAttribute>().Count() > 0).ToArray();

			foreach (Type t in targetTypes)
			{
				UnregisterRequestTargets(t);
			}
		}

		/// <summary>
		/// Unregisters the notification targets of an assembly.
		/// </summary>
		/// <param name="t">The assembly to unregister.</param>
		private static void UnregisterNotificationTargets(Assembly assembly)
		{
			Type[] targetTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes<NotificationTargetAttribute>().Count() > 0).ToArray();

			foreach (Type t in targetTypes)
			{
				UnregisterNotificationTargets(t);
			}
		}
		#endregion
	}
}
