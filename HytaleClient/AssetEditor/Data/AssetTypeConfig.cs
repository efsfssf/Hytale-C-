using System;
using System.Collections.Generic;
using HytaleClient.Data;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Protocol;
using Newtonsoft.Json.Linq;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BD9 RID: 3033
	public class AssetTypeConfig
	{
		// Token: 0x170013DD RID: 5085
		// (get) Token: 0x06005F87 RID: 24455 RVA: 0x001EDC4F File Offset: 0x001EBE4F
		public bool IsJson
		{
			get
			{
				return this.EditorType == 3 || this.EditorType == 2 || this.EditorType == 6 || this.EditorType == 4;
			}
		}

		// Token: 0x170013DE RID: 5086
		// (get) Token: 0x06005F88 RID: 24456 RVA: 0x001EDC78 File Offset: 0x001EBE78
		// (set) Token: 0x06005F89 RID: 24457 RVA: 0x001EDC80 File Offset: 0x001EBE80
		public string FileExtension
		{
			get
			{
				return this._fileExtension;
			}
			set
			{
				this._fileExtension = value;
				this.FileExtensionLowercase = value.ToLowerInvariant();
			}
		}

		// Token: 0x170013DF RID: 5087
		// (get) Token: 0x06005F8A RID: 24458 RVA: 0x001EDC96 File Offset: 0x001EBE96
		// (set) Token: 0x06005F8B RID: 24459 RVA: 0x001EDC9E File Offset: 0x001EBE9E
		public string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
				this.PathLowercase = value.ToLowerInvariant();
			}
		}

		// Token: 0x06005F8C RID: 24460 RVA: 0x001EDCB4 File Offset: 0x001EBEB4
		public bool HasFeature(AssetTypeConfig.EditorFeature feature)
		{
			return this.EditorFeatures != null && this.EditorFeatures.Contains(feature);
		}

		// Token: 0x04003B70 RID: 15216
		public string Id;

		// Token: 0x04003B71 RID: 15217
		public string Name;

		// Token: 0x04003B72 RID: 15218
		public Image IconImage;

		// Token: 0x04003B73 RID: 15219
		public PatchStyle Icon;

		// Token: 0x04003B74 RID: 15220
		public bool IsColoredIcon;

		// Token: 0x04003B75 RID: 15221
		public AssetTreeFolder AssetTree;

		// Token: 0x04003B76 RID: 15222
		public AssetEditorEditorType EditorType;

		// Token: 0x04003B77 RID: 15223
		public bool IsVirtual;

		// Token: 0x04003B78 RID: 15224
		public string IdProvider;

		// Token: 0x04003B79 RID: 15225
		public string[] InternalAssetIds;

		// Token: 0x04003B7A RID: 15226
		private string _fileExtension;

		// Token: 0x04003B7B RID: 15227
		public string FileExtensionLowercase;

		// Token: 0x04003B7C RID: 15228
		private string _path;

		// Token: 0x04003B7D RID: 15229
		public string PathLowercase;

		// Token: 0x04003B7E RID: 15230
		public bool HasIdField;

		// Token: 0x04003B7F RID: 15231
		public SchemaNode Schema;

		// Token: 0x04003B80 RID: 15232
		public JObject BaseJsonAsset;

		// Token: 0x04003B81 RID: 15233
		public List<AssetTypeConfig.EditorFeature> EditorFeatures;

		// Token: 0x04003B82 RID: 15234
		public AssetTypeConfig.PreviewType Preview;

		// Token: 0x04003B83 RID: 15235
		public List<AssetTypeConfig.Button> SidebarButtons;

		// Token: 0x04003B84 RID: 15236
		public List<AssetTypeConfig.Button> CreateButtons;

		// Token: 0x04003B85 RID: 15237
		public List<AssetTypeConfig.RebuildCacheType> RebuildCaches;

		// Token: 0x02000FE2 RID: 4066
		public class Button
		{
			// Token: 0x060069EA RID: 27114 RVA: 0x0021F886 File Offset: 0x0021DA86
			public Button(string textId, string action)
			{
				this.TextId = textId;
				this.Action = action;
			}

			// Token: 0x04004C3E RID: 19518
			public readonly string TextId;

			// Token: 0x04004C3F RID: 19519
			public readonly string Action;
		}

		// Token: 0x02000FE3 RID: 4067
		public enum EditorFeature
		{
			// Token: 0x04004C41 RID: 19521
			WeatherDaytimeBar,
			// Token: 0x04004C42 RID: 19522
			WeatherPreviewLocal
		}

		// Token: 0x02000FE4 RID: 4068
		public enum PreviewType
		{
			// Token: 0x04004C44 RID: 19524
			None,
			// Token: 0x04004C45 RID: 19525
			Item,
			// Token: 0x04004C46 RID: 19526
			Model
		}

		// Token: 0x02000FE5 RID: 4069
		public enum RebuildCacheType
		{
			// Token: 0x04004C48 RID: 19528
			BlockTextures,
			// Token: 0x04004C49 RID: 19529
			Models,
			// Token: 0x04004C4A RID: 19530
			ModelTextures,
			// Token: 0x04004C4B RID: 19531
			MapGemoetry,
			// Token: 0x04004C4C RID: 19532
			ItemIcons
		}
	}
}
