using System;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BD4 RID: 3028
	public class AssetFile
	{
		// Token: 0x06005F66 RID: 24422 RVA: 0x001ECE39 File Offset: 0x001EB039
		private AssetFile(string displayName, string path, bool isDirectory, string assetType, string[] pathElements)
		{
			this.DisplayName = displayName;
			this.Path = path;
			this.IsDirectory = isDirectory;
			this.AssetType = assetType;
			this.PathElements = pathElements;
		}

		// Token: 0x06005F67 RID: 24423 RVA: 0x001ECE68 File Offset: 0x001EB068
		public static AssetFile CreateFile(string id, string path, string assetType, string[] pathElements)
		{
			return new AssetFile(id, path, false, assetType, pathElements);
		}

		// Token: 0x06005F68 RID: 24424 RVA: 0x001ECE84 File Offset: 0x001EB084
		public static AssetFile CreateDirectory(string id, string path, string[] pathElements)
		{
			return new AssetFile(id, path, true, null, pathElements);
		}

		// Token: 0x06005F69 RID: 24425 RVA: 0x001ECEA0 File Offset: 0x001EB0A0
		public static AssetFile CreateAssetTypeDirectory(string id, string path, string assetType, string[] pathElements)
		{
			return new AssetFile(id, path, true, assetType, pathElements);
		}

		// Token: 0x04003B5C RID: 15196
		public readonly string DisplayName;

		// Token: 0x04003B5D RID: 15197
		public string Path;

		// Token: 0x04003B5E RID: 15198
		public readonly bool IsDirectory;

		// Token: 0x04003B5F RID: 15199
		public string[] PathElements;

		// Token: 0x04003B60 RID: 15200
		public string AssetType;
	}
}
