using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using N2.Definitions;

namespace N2.Edit.Workflow
{
	public class EditorCollectionBinder : IBinder<CommandContext>
	{
		ItemDefinition definition;
		IEnumerable<ContainableContext> editors;

		public EditorCollectionBinder(ItemDefinition definition, IEnumerable<ContainableContext> editors)
		{
			this.definition = definition;
			this.editors = editors;
		}

		#region IBinder<CommandContext> Members

		public bool UpdateObject(CommandContext value)
		{
			bool wasUpdated = false;
			foreach (var cc in editors)
			{
				var editable = definition.Get(cc.PropertyName) as IEditable;
				if (editable != null)
				{
					editable.UpdateItem(cc);
					wasUpdated |= cc.WasUpdated;
				}
			}
			return wasUpdated;
		}

		public void UpdateInterface(CommandContext value)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
