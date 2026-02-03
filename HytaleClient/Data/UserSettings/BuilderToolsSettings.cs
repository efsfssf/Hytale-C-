using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000AD0 RID: 2768
	internal class BuilderToolsSettings
	{
		// Token: 0x06005762 RID: 22370 RVA: 0x001A7A90 File Offset: 0x001A5C90
		public BuilderToolsSettings Clone()
		{
			return new BuilderToolsSettings
			{
				ToolFavorites = new Dictionary<string, JObject>(this.ToolFavorites),
				BlockFavorites = new List<string>(this.BlockFavorites),
				useToolReachDistance = this.useToolReachDistance,
				ToolReachDistance = this.ToolReachDistance,
				ToolReachLock = this.ToolReachLock,
				ToolDelayMin = this.ToolDelayMin,
				EnableBrushShapeRendering = this.EnableBrushShapeRendering,
				BrushOpacity = this.BrushOpacity,
				SelectionOpacity = this.SelectionOpacity,
				DisplayLegend = this.DisplayLegend,
				ShowBuilderToolsNotifications = this.ShowBuilderToolsNotifications
			};
		}

		// Token: 0x04003513 RID: 13587
		public Dictionary<string, JObject> ToolFavorites = new Dictionary<string, JObject>();

		// Token: 0x04003514 RID: 13588
		public List<string> BlockFavorites = new List<string>();

		// Token: 0x04003515 RID: 13589
		public bool useToolReachDistance = false;

		// Token: 0x04003516 RID: 13590
		public int ToolReachDistance = 128;

		// Token: 0x04003517 RID: 13591
		public bool ToolReachLock;

		// Token: 0x04003518 RID: 13592
		public int ToolDelayMin = 100;

		// Token: 0x04003519 RID: 13593
		public bool EnableBrushShapeRendering = true;

		// Token: 0x0400351A RID: 13594
		public int BrushOpacity = 30;

		// Token: 0x0400351B RID: 13595
		public int SelectionOpacity = 12;

		// Token: 0x0400351C RID: 13596
		public bool DisplayLegend = true;

		// Token: 0x0400351D RID: 13597
		public bool ShowBuilderToolsNotifications = false;
	}
}
