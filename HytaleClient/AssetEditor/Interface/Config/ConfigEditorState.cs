using System;
using System.Collections.Generic;
using HytaleClient.Math;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BBE RID: 3006
	public class ConfigEditorState
	{
		// Token: 0x04003AE4 RID: 15076
		public Dictionary<PropertyPath, bool> UncollapsedProperties = new Dictionary<PropertyPath, bool>();

		// Token: 0x04003AE5 RID: 15077
		public Point ScrollOffset;

		// Token: 0x04003AE6 RID: 15078
		public PropertyPath? ActiveCategory;
	}
}
