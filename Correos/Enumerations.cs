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
}
