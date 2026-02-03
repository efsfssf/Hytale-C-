using System;
using HytaleClient.AssetEditor.Interface.Config;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Backends
{
	// Token: 0x02000BE0 RID: 3040
	public class ClientJsonUpdateCommand
	{
		// Token: 0x06005FDA RID: 24538 RVA: 0x001F04DC File Offset: 0x001EE6DC
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}", new object[]
			{
				"Type",
				this.Type,
				"Path",
				this.Path,
				"Value",
				this.Value
			});
		}

		// Token: 0x04003BC7 RID: 15303
		public JsonUpdateType Type;

		// Token: 0x04003BC8 RID: 15304
		public PropertyPath Path;

		// Token: 0x04003BC9 RID: 15305
		public JToken Value;

		// Token: 0x04003BCA RID: 15306
		public JToken PreviousValue;

		// Token: 0x04003BCB RID: 15307
		public PropertyPath? FirstCreatedProperty;

		// Token: 0x04003BCC RID: 15308
		public AssetEditorRebuildCaches RebuildCaches;
	}
}
