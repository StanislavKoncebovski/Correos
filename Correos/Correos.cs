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
using System.Text;
using System.Threading.Tasks;

namespace Correos
{
	public static class Correos
	{
		#region Private Members
		/// <summary>
		/// Dictionary of request targets.
		/// Key: Correos name of the method.
		/// Value: The instance of the method.
		/// </summary>
		private static Dictionary<string, MethodInfo>	_requestTargets = new Dictionary<string, MethodInfo>();
		#endregion

		#region Properties
		/// <summary>
		/// The registration policy.
		/// Default: labeled only.
		/// </summary>
		public static RegistrationPolicy RegistrationPolicy {get;set;} = RegistrationPolicy.LabeledOnly;
		#endregion

		#region Public Features
		/// <summary>
		/// Registers the request and the notification targets of a type.
		/// </summary>
		/// <param name="t">The type to register.</param>
		public static void Register(Type t)
		{
			RegisterRequestTargets(t);
		}

		/// <summary>
		/// Unregisters the request and the notification targets of a type.
		/// </summary>
		/// <param name="t">The type to unregister.</param>
		public static void Unregister(Type t)
		{
			
		}
		#endregion

		#region Private auxiliary
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
					continue;
				}
					
				if (!_requestTargets.ContainsKey(key))
				{
					_requestTargets.Add(key, method);
				}
            }
        }
		#endregion
	}
}
