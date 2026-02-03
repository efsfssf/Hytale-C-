using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Config;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.Characters;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.AssetEditor.Backends
{
	// Token: 0x02000BDF RID: 3039
	internal abstract class AssetEditorBackend : Disposable
	{
		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x06005FB0 RID: 24496 RVA: 0x001EFCA1 File Offset: 0x001EDEA1
		// (set) Token: 0x06005FB1 RID: 24497 RVA: 0x001EFCA9 File Offset: 0x001EDEA9
		public AssetTreeFolder[] SupportedAssetTreeFolders { get; protected set; } = new AssetTreeFolder[0];

		// Token: 0x06005FB2 RID: 24498 RVA: 0x001EFCB2 File Offset: 0x001EDEB2
		protected AssetEditorBackend(AssetEditorOverlay assetEditorOverlay)
		{
			this.AssetEditorOverlay = assetEditorOverlay;
		}

		// Token: 0x170013E4 RID: 5092
		// (get) Token: 0x06005FB3 RID: 24499 RVA: 0x001EFCCF File Offset: 0x001EDECF
		// (set) Token: 0x06005FB4 RID: 24500 RVA: 0x001EFCD7 File Offset: 0x001EDED7
		public bool IsEditingRemotely { get; protected set; }

		// Token: 0x170013E5 RID: 5093
		// (get) Token: 0x06005FB5 RID: 24501 RVA: 0x001EFCE0 File Offset: 0x001EDEE0
		// (set) Token: 0x06005FB6 RID: 24502 RVA: 0x001EFCE8 File Offset: 0x001EDEE8
		public bool IsExportingAssets { get; protected set; }

		// Token: 0x06005FB7 RID: 24503
		public abstract void Initialize();

		// Token: 0x06005FB8 RID: 24504
		public abstract void CreateDirectory(string path, bool applyLocally, Action<string, FormattedMessage> callback);

		// Token: 0x06005FB9 RID: 24505
		public abstract void DeleteDirectory(string path, bool applyLocally, Action<string, FormattedMessage> callback);

		// Token: 0x06005FBA RID: 24506
		public abstract void RenameDirectory(string path, string newPath, bool applyLocally, Action<string, FormattedMessage> callback);

		// Token: 0x06005FBB RID: 24507
		public abstract void FetchAsset(AssetReference assetReference, Action<object, FormattedMessage> action, bool trackUpdates = false);

		// Token: 0x06005FBC RID: 24508
		public abstract void FetchJsonAssetWithParents(AssetReference assetReference, Action<Dictionary<string, TrackedAsset>, FormattedMessage> callback, bool trackUpdates = false);

		// Token: 0x06005FBD RID: 24509
		public abstract void SetOpenEditorAsset(AssetReference assetReference);

		// Token: 0x06005FBE RID: 24510
		public abstract void FetchAutoCompleteData(string dataset, string query, Action<HashSet<string>, FormattedMessage> callback);

		// Token: 0x06005FBF RID: 24511 RVA: 0x001EFCF4 File Offset: 0x001EDEF4
		public virtual bool TryGetDropdownEntriesOrFetch(string dataset, out List<string> entries, object extraValue = null)
		{
			bool flag = dataset == "GradientSets";
			bool result;
			if (flag)
			{
				entries = new List<string>(this.AssetEditorOverlay.Interface.App.CharacterPartStore.GradientSets.Keys);
				entries.Sort();
				result = true;
			}
			else
			{
				bool flag2 = dataset == "GradientIds";
				if (flag2)
				{
					CharacterPartGradientSet characterPartGradientSet;
					bool flag3 = extraValue != null && extraValue is string && this.AssetEditorOverlay.Interface.App.CharacterPartStore.GradientSets.TryGetValue((string)extraValue, out characterPartGradientSet);
					if (flag3)
					{
						entries = new List<string>(characterPartGradientSet.Gradients.Keys);
						entries.Sort();
					}
					else
					{
						entries = new List<string>();
					}
					result = true;
				}
				else
				{
					entries = null;
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005FC0 RID: 24512
		public abstract void CreateAsset(AssetReference assetReference, object data, string buttonId = null, bool openInTab = false, Action<FormattedMessage> callback = null);

		// Token: 0x06005FC1 RID: 24513
		public abstract void UpdateJsonAsset(AssetReference assetReference, List<ClientJsonUpdateCommand> jsonUpdateCommands, Action<FormattedMessage> callback = null);

		// Token: 0x06005FC2 RID: 24514
		public abstract void UpdateAsset(AssetReference assetReference, object data, Action<FormattedMessage> callback = null);

		// Token: 0x06005FC3 RID: 24515
		public abstract void DeleteAsset(AssetReference assetReference, bool applyLocally);

		// Token: 0x06005FC4 RID: 24516
		public abstract void RenameAsset(AssetReference assetReference, string newAssetPath, bool applyLocally);

		// Token: 0x06005FC5 RID: 24517 RVA: 0x001EFDC2 File Offset: 0x001EDFC2
		public virtual void OnValueChanged(PropertyPath path, JToken value)
		{
		}

		// Token: 0x06005FC6 RID: 24518 RVA: 0x001EFDC5 File Offset: 0x001EDFC5
		public virtual void SaveUnsavedChanges()
		{
		}

		// Token: 0x06005FC7 RID: 24519
		public abstract void UndoChanges(AssetReference assetReference);

		// Token: 0x06005FC8 RID: 24520
		public abstract void RedoChanges(AssetReference assetReference);

		// Token: 0x06005FC9 RID: 24521 RVA: 0x001EFDC8 File Offset: 0x001EDFC8
		public virtual void OnEditorOpen(bool isOpen)
		{
		}

		// Token: 0x06005FCA RID: 24522 RVA: 0x001EFDCB File Offset: 0x001EDFCB
		public virtual void OnSidebarButtonActivated(string action)
		{
		}

		// Token: 0x06005FCB RID: 24523 RVA: 0x001EFDCE File Offset: 0x001EDFCE
		public virtual string GetButtonText(string messageId)
		{
			return this.AssetEditorOverlay.Interface.GetText(messageId, null, true);
		}

		// Token: 0x06005FCC RID: 24524 RVA: 0x001EFDE3 File Offset: 0x001EDFE3
		public virtual void ExportAssets(List<AssetReference> assetReferences, Action<List<TimestampedAssetReference>> callback = null)
		{
		}

		// Token: 0x06005FCD RID: 24525 RVA: 0x001EFDE6 File Offset: 0x001EDFE6
		public virtual void SetGameTime(DateTime time, bool paused)
		{
		}

		// Token: 0x06005FCE RID: 24526 RVA: 0x001EFDE9 File Offset: 0x001EDFE9
		public virtual void SetWeatherAndTimeLock(bool locked)
		{
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x001EFDEC File Offset: 0x001EDFEC
		public void ExportAndDiscardAssets(List<AssetReference> assetReferences)
		{
			this.ExportAssets(assetReferences, delegate(List<TimestampedAssetReference> exportedAssets)
			{
				List<TimestampedAssetReference> list = new List<TimestampedAssetReference>();
				foreach (TimestampedAssetReference timestampedAssetReference in exportedAssets)
				{
					bool flag = timestampedAssetReference.Timestamp == null;
					if (!flag)
					{
						list.Add(timestampedAssetReference);
					}
				}
				this.DiscardChanges(list);
			});
		}

		// Token: 0x06005FD0 RID: 24528 RVA: 0x001EFE03 File Offset: 0x001EE003
		public void DiscardChanges(TimestampedAssetReference assetToDiscard)
		{
			this.DiscardChanges(new List<TimestampedAssetReference>
			{
				assetToDiscard
			});
		}

		// Token: 0x06005FD1 RID: 24529 RVA: 0x001EFE1A File Offset: 0x001EE01A
		public virtual void DiscardChanges(List<TimestampedAssetReference> assetsToDiscard)
		{
		}

		// Token: 0x06005FD2 RID: 24530 RVA: 0x001EFE1D File Offset: 0x001EE01D
		public virtual void FetchLastModifiedAssets()
		{
		}

		// Token: 0x06005FD3 RID: 24531 RVA: 0x001EFE20 File Offset: 0x001EE020
		public virtual void OnLanguageChanged()
		{
		}

		// Token: 0x06005FD4 RID: 24532 RVA: 0x001EFE23 File Offset: 0x001EE023
		public virtual void UpdateSubscriptionToModifiedAssetsUpdates(bool subscribe)
		{
		}

		// Token: 0x06005FD5 RID: 24533 RVA: 0x001EFE26 File Offset: 0x001EE026
		public virtual AssetEditorLastModifiedAssets.AssetInfo[] GetLastModifiedAssets()
		{
			return null;
		}

		// Token: 0x06005FD6 RID: 24534 RVA: 0x001EFE2C File Offset: 0x001EE02C
		protected SchemaNode LoadSchema(JObject jObject, Dictionary<string, SchemaNode> schemas)
		{
			Dictionary<string, SchemaNode> dictionary = new Dictionary<string, SchemaNode>();
			SchemaParser schemaParser = new SchemaParser(this.AssetEditorOverlay.Interface.App.Settings.DiagnosticMode);
			SchemaNode schemaNode;
			try
			{
				schemaNode = schemaParser.Parse(jObject, dictionary);
			}
			catch (Exception innerException)
			{
				throw new Exception("Failed to parse schema at path " + schemaParser.CurrentPath, innerException);
			}
			bool flag = schemaNode.Properties != null;
			if (flag)
			{
				SchemaNode schemaNode2;
				bool flag2 = schemaNode.Properties.TryGetValue("$Comment", out schemaNode2);
				if (flag2)
				{
					schemaNode2.IsHidden = false;
				}
				SchemaNode schemaNode3;
				bool flag3 = schemaNode.Properties.TryGetValue("Parent", out schemaNode3) && schemaNode3.IsParentProperty;
				if (flag3)
				{
					KeyValuePair<string, SchemaNode> keyValuePair = Enumerable.FirstOrDefault<KeyValuePair<string, SchemaNode>>(schemaNode.Properties);
					bool flag4 = keyValuePair.Value != null && keyValuePair.Value.SectionStart == null;
					if (flag4)
					{
						keyValuePair.Value.SectionStart = "General";
					}
				}
			}
			AssetEditorBackend.Logger.Info("Loaded schema with id {0}", schemaNode.Id);
			schemas[schemaNode.Id] = schemaNode;
			foreach (KeyValuePair<string, SchemaNode> keyValuePair2 in dictionary)
			{
				SchemaNode schemaNode4;
				bool flag5 = keyValuePair2.Value.Properties != null && keyValuePair2.Value.Properties.TryGetValue("Parent", out schemaNode4) && schemaNode4.IsParentProperty;
				if (flag5)
				{
					KeyValuePair<string, SchemaNode> keyValuePair3 = Enumerable.FirstOrDefault<KeyValuePair<string, SchemaNode>>(keyValuePair2.Value.Properties);
					bool flag6 = keyValuePair3.Value != null && keyValuePair3.Value.SectionStart == null;
					if (flag6)
					{
						keyValuePair3.Value.SectionStart = "General";
					}
				}
				schemas[schemaNode.Id + keyValuePair2.Key] = keyValuePair2.Value;
				AssetEditorBackend.Logger.Info("Loaded definition schema with id {0}", schemaNode.Id + keyValuePair2.Key);
			}
			return schemaNode;
		}

		// Token: 0x06005FD7 RID: 24535 RVA: 0x001F0064 File Offset: 0x001EE264
		protected void ApplySchemaMetadata(AssetTypeConfig assetTypeConfig, JObject meta)
		{
			bool flag = meta.ContainsKey("extension");
			if (flag)
			{
				assetTypeConfig.FileExtension = (string)meta["extension"];
			}
			else
			{
				assetTypeConfig.FileExtension = ".json";
			}
			bool flag2 = meta.ContainsKey("uiTypeIcon");
			if (flag2)
			{
				string fullPath = Path.GetFullPath(Path.Combine(Paths.BuiltInAssets, "Schema", "Icons"));
				string fullPath2 = Path.GetFullPath(Path.Combine(fullPath, (string)meta["uiTypeIcon"]));
				bool flag3 = Paths.IsSubPathOf(fullPath2, fullPath) && File.Exists(fullPath2);
				if (flag3)
				{
					assetTypeConfig.IconImage = new Image(File.ReadAllBytes(fullPath2));
				}
			}
			bool flag4 = meta.ContainsKey("idProvider");
			if (flag4)
			{
				assetTypeConfig.IdProvider = (string)meta["idProvider"];
			}
			bool flag5 = assetTypeConfig.IconImage == null;
			if (flag5)
			{
				assetTypeConfig.Icon = new PatchStyle("AssetEditor/AssetIcons/File.png");
			}
			bool flag6 = meta.ContainsKey("internalKeys");
			if (flag6)
			{
				JArray jarray = (JArray)meta["internalKeys"];
				assetTypeConfig.InternalAssetIds = new string[jarray.Count];
				for (int i = 0; i < jarray.Count; i++)
				{
					assetTypeConfig.InternalAssetIds[i] = (string)jarray[i];
				}
			}
			bool flag7 = meta.ContainsKey("uiEditorFeatures");
			if (flag7)
			{
				assetTypeConfig.EditorFeatures = new List<AssetTypeConfig.EditorFeature>();
				foreach (JToken jtoken in ((JArray)meta["uiEditorFeatures"]))
				{
					AssetTypeConfig.EditorFeature item;
					bool flag8 = !Enum.TryParse<AssetTypeConfig.EditorFeature>((string)jtoken, out item);
					if (!flag8)
					{
						assetTypeConfig.EditorFeatures.Add(item);
					}
				}
			}
			bool flag9 = meta.ContainsKey("uiRebuildCaches");
			if (flag9)
			{
				JArray jarray2 = (JArray)meta["uiRebuildCaches"];
				assetTypeConfig.RebuildCaches = new List<AssetTypeConfig.RebuildCacheType>();
				foreach (JToken jtoken2 in jarray2)
				{
					AssetTypeConfig.RebuildCacheType item2;
					bool flag10 = !Enum.TryParse<AssetTypeConfig.RebuildCacheType>((string)jtoken2, out item2);
					if (!flag10)
					{
						assetTypeConfig.RebuildCaches.Add(item2);
					}
				}
			}
			bool flag11 = meta.ContainsKey("uiEditorPreview");
			if (flag11)
			{
				assetTypeConfig.Preview = (AssetTypeConfig.PreviewType)Enum.Parse(typeof(AssetTypeConfig.PreviewType), (string)meta["uiEditorPreview"]);
			}
			bool flag12 = meta.ContainsKey("uiSidebarButtons");
			if (flag12)
			{
				assetTypeConfig.SidebarButtons = new List<AssetTypeConfig.Button>();
				foreach (JToken jtoken3 in meta["uiSidebarButtons"])
				{
					assetTypeConfig.SidebarButtons.Add(new AssetTypeConfig.Button((string)jtoken3["textId"], (string)jtoken3["buttonId"]));
				}
			}
			bool flag13 = meta.ContainsKey("uiCreateButtons");
			if (flag13)
			{
				assetTypeConfig.CreateButtons = new List<AssetTypeConfig.Button>();
				foreach (JToken jtoken4 in meta["uiCreateButtons"])
				{
					assetTypeConfig.CreateButtons.Add(new AssetTypeConfig.Button((string)jtoken4["textId"], (string)jtoken4["buttonId"]));
				}
			}
		}

		// Token: 0x04003BC0 RID: 15296
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003BC1 RID: 15297
		public const string GradientSetsDatasetId = "GradientSets";

		// Token: 0x04003BC2 RID: 15298
		public const string GradientIdsDatasetId = "GradientIds";

		// Token: 0x04003BC3 RID: 15299
		protected readonly AssetEditorOverlay AssetEditorOverlay;
	}
}
