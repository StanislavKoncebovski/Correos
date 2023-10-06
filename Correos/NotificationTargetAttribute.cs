/***********************************************************************************
* File:         NotificationTargetAttribute.cs                                     *
* Contents:     Class NotificationTargetAttribute                                  *
* Author:       Stanislav Koncebovski (stanislav@pikkatech.eu)                     *
* Date:         2023-10-06 1104                                                    *
* Version:      1.0                                                                *
* Copyright:    pikkatech.eu (www.pikkatech.eu)                                    *
***********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Correos
{
	/// <summary>
	/// Attribute to label methods that shall be addressed as notification targets.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class NotificationTargetAttribute: Attribute
	{
		#region Properties
		/// <summary>
		/// The name of the Correos attribute for the method.
		/// </summary>
        public string Name {get;set;} = "";

		/// <summary>
		/// The description of the Correos attribute for the method (optional).
		/// </summary>
        public string Description {get;set;} = "";
		#endregion

		#region Construction
		/// <summary>
		/// Data constructor.
		/// Creates an instance of NotificationTargetAttribute from a name and a description.
		/// </summary>
		/// <param name="name">The name of the request target attribute.</param>
		/// <param name="description">The optional description.</param>
		public NotificationTargetAttribute(string name, string description = "")
		{
			this.Name = name;
			this.Description = description;
		}

		/// <summary>
		/// Default constructor.
		/// Creates an instance of request target attribute with empty Correos name and description.
		/// </summary>
		public NotificationTargetAttribute()
		{
		}
		#endregion
	}
}
