#region License
/* Copyright (C) 2007 Cristian Libardo
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, write to the Free
 * Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
 * 02110-1301 USA, or see the FSF site: http://www.fsf.org.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.Web.Configuration;
using System.ComponentModel;

namespace N2.Configuration
{
	/// <summary>Implementation of a N2 configuration source that reads settings from the N2ConfigurationSectionHandler from web.config.</summary>
	[Obsolete("Replaced by castle windsor configuration")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class DefaultConfiguration : IConfiguration
	{
		public DefaultConfiguration()
		{
			throw new N2Exception("Replaced by castle windsor configuration. Read upgrade.txt for further information.");
		}

		public DefaultConfiguration(N2ConfigurationSectionHandler configSection)
		{
			throw new N2Exception("Replaced by castle windsor configuration. Read upgrade.txt for further information.");
		}
	}
}