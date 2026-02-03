using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Utils;

namespace HytaleClient.AssetEditor.Data
{
	// Token: 0x02000BDA RID: 3034
	public class AssetTypeRegistry
	{
		// Token: 0x170013E0 RID: 5088
		// (get) Token: 0x06005F8E RID: 24462 RVA: 0x001EDCD6 File Offset: 0x001EBED6
		// (set) Token: 0x06005F8F RID: 24463 RVA: 0x001EDCDE File Offset: 0x001EBEDE
		public IReadOnlyDictionary<string, AssetTypeConfig> AssetTypes { get; private set; } = new Dictionary<string, AssetTypeConfig>();

		// Token: 0x06005F90 RID: 24464 RVA: 0x001EDCE7 File Offset: 0x001EBEE7
		public void SetupAssetTypes(IReadOnlyDictionary<string, AssetTypeConfig> assetTypes)
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.AssetTypes = assetTypes;
		}

		// Token: 0x06005F91 RID: 24465 RVA: 0x001EDD00 File Offset: 0x001EBF00
		public bool TryGetAssetTypeFromPath(string filePath, out string assetType)
		{
			filePath = filePath.ToLowerInvariant();
			bool flag = filePath.StartsWith(AssetPathUtils.PathCosmeticsLowercase + "/");
			bool result;
			if (flag)
			{
				string pathWithoutAssetId = AssetPathUtils.GetPathWithoutAssetId(filePath);
				foreach (KeyValuePair<string, AssetTypeConfig> keyValuePair in this.AssetTypes)
				{
					bool flag2 = keyValuePair.Value.AssetTree != AssetTreeFolder.Cosmetics || keyValuePair.Value.PathLowercase != pathWithoutAssetId;
					if (!flag2)
					{
						assetType = keyValuePair.Key;
						return true;
					}
				}
				assetType = null;
				result = false;
			}
			else
			{
				string text = Path.GetExtension(filePath).ToLowerInvariant();
				bool flag3 = text == "";
				if (flag3)
				{
					assetType = null;
					result = false;
				}
				else
				{
					foreach (AssetTypeConfig assetTypeConfig in this.AssetTypes.Values)
					{
						bool flag4 = !filePath.StartsWith(assetTypeConfig.PathLowercase + "/") || assetTypeConfig.FileExtensionLowercase != text;
						if (!flag4)
						{
							assetType = assetTypeConfig.Id;
							return true;
						}
					}
					assetType = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005F92 RID: 24466 RVA: 0x001EDE70 File Offset: 0x001EC070
		public bool TryGetAssetTypesFromDirectoryPath(string filePath, out List<string> assetTypes)
		{
			bool flag = filePath.StartsWith("Common/");
			bool result;
			if (flag)
			{
				assetTypes = null;
				result = false;
			}
			else
			{
				bool flag2 = filePath.StartsWith("Cosmetics/CharacterCreator/");
				if (flag2)
				{
					string pathWithoutAssetId = AssetPathUtils.GetPathWithoutAssetId(filePath);
					foreach (KeyValuePair<string, AssetTypeConfig> keyValuePair in this.AssetTypes)
					{
						bool flag3 = keyValuePair.Value.AssetTree == AssetTreeFolder.Cosmetics && keyValuePair.Value.Path == pathWithoutAssetId;
						if (flag3)
						{
							assetTypes = new List<string>
							{
								keyValuePair.Key
							};
							return true;
						}
					}
					assetTypes = null;
					result = false;
				}
				else
				{
					assetTypes = new List<string>();
					foreach (KeyValuePair<string, AssetTypeConfig> keyValuePair2 in this.AssetTypes)
					{
						bool flag4 = keyValuePair2.Value.AssetTree == AssetTreeFolder.Server && filePath.StartsWith(keyValuePair2.Value.Path + "/");
						if (flag4)
						{
							assetTypes.Add(keyValuePair2.Key);
						}
					}
					result = (assetTypes.Count > 0);
				}
			}
			return result;
		}

		// Token: 0x06005F93 RID: 24467 RVA: 0x001EDFE4 File Offset: 0x001EC1E4
		public void Clear()
		{
			this.AssetTypes = new Dictionary<string, AssetTypeConfig>();
		}
	}
}
