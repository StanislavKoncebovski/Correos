/***********************************************************************************
* File:         Enumerations.cs                                                    *
* Contents:     Correos Enumerations                                               *
* Author:       Stanislav Koncebovski (stanislav@pikkatech.eu)                     *
* Date:         2023-10-06 1054                                                    *
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
	/// Defines how to register Correos targets.
	/// </summary>
	public enum RegistrationPolicy
	{
		/// <summary>
		/// Only register targets in classes that hold the Correos attribute.
		/// </summary>
		LabeledOnly,

		/// <summary>
		/// Register targets in all classes.
		/// </summary>
		All
	}

	/// <summary>
	/// Defines how empty Correos target names are handled.
	/// </summary>
	public enum EmptyNameHandling
	{
		/// <summary>
		/// If Correos target name is left empty, 
		/// the (short) name of the method itself will be assigned 
		/// as the Correos target name.
		/// </summary>
		AssignMethodName,

		/// <summary>
		/// If Correos target name is left empty, 
		/// the full name of the method itself will be assigned 
		/// as the Correos target name.
		/// </summary>
		AssignFullMethodName,

		/// <summary>
		/// If Correos target name is left empty, the method will be skipped.
		/// </summary>
		Skip
	}
}
