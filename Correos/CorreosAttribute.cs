/***********************************************************************************
* File:         CorreosAttribute.cs                                                *
* Contents:     Class CorreosAttribute                                             *
* Author:       Stanislav Koncebovski (stanislav@pikkatech.eu)                     *
* Date:         2023-10-06 1038                                                    *
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
	/// Attribute to label classes that contain Correos targets.
	/// If the global registration policy is LabeledOnly, 
	/// only those classes holding this attribute will be take into consideration.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class CorreosAttribute: Attribute
	{
        #region Properties
		/// <summary>
		/// The name of the Correos attribute for the class.
		/// </summary>
        public string Name {get;set;} = "";

		/// <summary>
		/// The description of the Correos attribute for the class (optional).
		/// </summary>
        public string Description {get;set;} = "";
		#endregion

		#region Construction
		/// <summary>
		/// Data constructor.
		/// Creates an instance of CorreosAttribute from a name and a description.
		/// </summary>
		/// <param name="name">The name of the Correos attribute.</param>
		/// <param name="description">The optional description.</param>
		public CorreosAttribute(string name, string description = "")
		{
			this.Name = name;
			this.Description = description;
		}

		/// <summary>
		/// Default constructor.
		/// Creates an instance of CorreosAttribute with empty Correos name and description.
		/// </summary>
		public CorreosAttribute()
		{
		}
		#endregion
	}
}
