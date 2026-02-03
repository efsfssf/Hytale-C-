using System;
using HytaleClient.Protocol;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BB9 RID: 3001
	internal class CacheRebuildInfo
	{
		// Token: 0x06005DF6 RID: 24054 RVA: 0x001DFA51 File Offset: 0x001DDC51
		public CacheRebuildInfo(AssetEditorRebuildCaches caches, bool appliesToChildProperties)
		{
			this.Caches = caches;
			this.AppliesToChildProperties = appliesToChildProperties;
		}

		// Token: 0x04003AB7 RID: 15031
		public readonly AssetEditorRebuildCaches Caches;

		// Token: 0x04003AB8 RID: 15032
		public readonly bool AppliesToChildProperties;
	}
}
