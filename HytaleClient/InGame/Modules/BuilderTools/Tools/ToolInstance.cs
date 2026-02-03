using System;
using HytaleClient.Data.Items;
using HytaleClient.InGame.Modules.BuilderTools.Tools.Brush;
using HytaleClient.InGame.Modules.BuilderTools.Tools.Client;

namespace HytaleClient.InGame.Modules.BuilderTools.Tools
{
	// Token: 0x02000980 RID: 2432
	internal class ToolInstance
	{
		// Token: 0x06004D39 RID: 19769 RVA: 0x0014B3EF File Offset: 0x001495EF
		public ToolInstance(ClientItemStack itemStack, BuilderTool builderTool, ClientTool clientTool, BrushData brushData)
		{
			this.ItemStack = itemStack;
			this.BuilderTool = builderTool;
			this.ClientTool = clientTool;
			this.BrushData = brushData;
		}

		// Token: 0x04002878 RID: 10360
		public readonly ClientItemStack ItemStack;

		// Token: 0x04002879 RID: 10361
		public readonly BuilderTool BuilderTool;

		// Token: 0x0400287A RID: 10362
		public readonly ClientTool ClientTool;

		// Token: 0x0400287B RID: 10363
		public readonly BrushData BrushData;
	}
}
