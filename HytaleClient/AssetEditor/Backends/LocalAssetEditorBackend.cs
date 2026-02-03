using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Config;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Core;
using HytaleClient.Data;
using HytaleClient.Data.Audio;
using HytaleClient.Data.Characters;
using HytaleClient.Graphics;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.AssetEditor.Backends
{
	// Token: 0x02000BE2 RID: 3042
	internal class LocalAssetEditorBackend : AssetEditorBackend
	{
		// Token: 0x06005FDF RID: 24543 RVA: 0x001F0AA4 File Offset: 0x001EECA4
		public override void OnValueChanged(PropertyPath path, JToken value)
		{
			string text = path.ToString();
			bool flag = this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes[this.AssetEditorOverlay.CurrentAsset.Type].AssetTree != AssetTreeFolder.Cosmetics;
			if (!flag)
			{
				bool flag2 = value == null;
				if (!flag2)
				{
					bool flag3 = text == "Model";
					if (flag3)
					{
						bool flag4 = this.AssetEditorOverlay.ConfigEditor.Value["Textures"] != null || this.AssetEditorOverlay.ConfigEditor.Value["GreyscaleTexture"] != null;
						if (!flag4)
						{
							this.FindAndAddTextures((string)value, Path.GetFileNameWithoutExtension((string)value), PropertyPath.FromString(""));
						}
					}
					else
					{
						bool flag5 = text.StartsWith("Variants.");
						if (flag5)
						{
							bool flag6 = this.AssetEditorOverlay.ConfigEditor.Value["Variants"] != null;
							if (!flag6)
							{
								bool flag7 = path.Elements.Length != 3 || path.Elements[2] != "Model";
								if (!flag7)
								{
									string fileNameWithoutExtension = Path.GetFileNameWithoutExtension((string)value);
									this.FindAndAddTextures((string)value, fileNameWithoutExtension, PropertyPath.FromString("Variants." + path.Elements[1]));
									string[] array = fileNameWithoutExtension.Split(new char[]
									{
										'_'
									});
									bool flag8 = array.Length != 0;
									if (flag8)
									{
										Array.Resize<string>(ref array, array.Length - 1);
										this.FindAndAddTextures((string)value, string.Join("_", array), PropertyPath.FromString("Variants." + path.Elements[1]));
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06005FE0 RID: 24544 RVA: 0x001F0C84 File Offset: 0x001EEE84
		private void FindAndAddTextures(string modelFile, string baseFileName, PropertyPath basePropertyPath)
		{
			string path = Path.GetDirectoryName(modelFile).Replace(Path.DirectorySeparatorChar, '/');
			string path2 = Path.GetDirectoryName(modelFile).Replace(Path.DirectorySeparatorChar, '/');
			string text = AssetPathUtils.CombinePaths(path, baseFileName + "_Textures");
			string b = baseFileName + "_Greyscale.png";
			List<FileSelector.File> commonFileSelectorFiles = this.AssetEditorOverlay.GetCommonFileSelectorFiles(path2, "", null, null, -1);
			List<FileSelector.File> commonFileSelectorFiles2 = this.AssetEditorOverlay.GetCommonFileSelectorFiles(text, "", null, null, -1);
			JObject jobject = new JObject();
			string text2 = null;
			foreach (FileSelector.File file in commonFileSelectorFiles)
			{
				bool flag = file.IsDirectory || file.Name != b;
				if (!flag)
				{
					text2 = AssetPathUtils.CombinePaths(path, file.Name);
					break;
				}
			}
			foreach (FileSelector.File file2 in commonFileSelectorFiles2)
			{
				bool flag2 = file2.IsDirectory || !file2.Name.EndsWith(".png");
				if (!flag2)
				{
					JObject jobject2 = jobject;
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file2.Name);
					JObject jobject3 = new JObject();
					jobject3.Add("Texture", AssetPathUtils.CombinePaths(text, file2.Name));
					jobject2[fileNameWithoutExtension] = jobject3;
				}
			}
			ConfigEditor configEditor = this.AssetEditorOverlay.ConfigEditor;
			bool flag3 = text2 != null;
			if (flag3)
			{
				PropertyPath? propertyPath;
				configEditor.SetProperty(configEditor.Value, basePropertyPath.GetChild("GreyscaleTexture"), text2, out propertyPath, true, false);
			}
			bool flag4 = jobject.Count > 0;
			if (flag4)
			{
				PropertyPath? propertyPath;
				configEditor.SetProperty(configEditor.Value, basePropertyPath.GetChild("Textures"), jobject, out propertyPath, true, false);
			}
			bool flag5 = configEditor.Value["Name"] == null;
			if (flag5)
			{
				PropertyPath? propertyPath;
				configEditor.SetProperty(configEditor.Value, PropertyPath.FromString("Name"), baseFileName, out propertyPath, true, false);
			}
			configEditor.Layout(null, true);
			this.ValidateJsonAsset(this.AssetEditorOverlay.CurrentAsset, configEditor.Value);
		}

		// Token: 0x06005FE1 RID: 24545 RVA: 0x001F0EF8 File Offset: 0x001EF0F8
		public LocalAssetEditorBackend(AssetEditorOverlay assetEditorOverlay, AssetTreeFolder[] supportedFolders) : base(assetEditorOverlay)
		{
			this._backendLifetimeCancellationToken = this._backendLifetimeCancellationTokenSource.Token;
			base.SupportedAssetTreeFolders = supportedFolders;
		}

		// Token: 0x06005FE2 RID: 24546 RVA: 0x001F0FB8 File Offset: 0x001EF1B8
		protected override void DoDispose()
		{
			foreach (FileSystemWatcher fileSystemWatcher in this._fileSystemWatchers)
			{
				fileSystemWatcher.Dispose();
			}
			this._fileSystemWatchers.Clear();
			this._backendLifetimeCancellationTokenSource.Cancel();
			this._threadCancellationTokenSource.Cancel();
			Thread fileWatcherHandlerThread = this._fileWatcherHandlerThread;
			if (fileWatcherHandlerThread != null)
			{
				fileWatcherHandlerThread.Join();
			}
			foreach (Texture texture in this._iconTextures)
			{
				texture.Dispose();
			}
			this.SaveUnsavedChangesInternal(true);
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x001F1090 File Offset: 0x001EF290
		public override void Initialize()
		{
			Debug.Assert(!this._isInitializingOrInitialized);
			bool isInitializingOrInitialized = this._isInitializingOrInitialized;
			if (!isInitializingOrInitialized)
			{
				this._isInitializingOrInitialized = true;
				this._assetsDirectoryPath = this.AssetEditorOverlay.Interface.App.Settings.AssetsPath;
				this.AssetEditorOverlay.SetFileSaveStatus(AssetEditorOverlay.SaveStatus.Saved, true);
				CancellationToken cancellationToken = this._backendLifetimeCancellationTokenSource.Token;
				Dictionary<string, AssetTypeConfig> assetTypes = null;
				Dictionary<string, SchemaNode> schemas = null;
				IDictionary<string, string> translationMapping = null;
				this.AssetEditorOverlay.SetAssetTreeInitializing(true);
				Action <>9__3;
				Action <>9__4;
				Task.Run(delegate()
				{
					this.InitializeAssetTypeConfigs(out assetTypes, out schemas, out translationMapping);
				}).ContinueWith(delegate(Task t)
				{
					bool isFaulted = t.IsFaulted;
					if (isFaulted)
					{
						LocalAssetEditorBackend.Logger.Error(t.Exception, "Failed to initialize asset types");
					}
					else
					{
						bool isCancellationRequested = cancellationToken.IsCancellationRequested;
						if (!isCancellationRequested)
						{
							this.InitializeAssetMap(assetTypes);
							Engine engine = this.AssetEditorOverlay.Interface.Engine;
							Disposable <>4__this = this;
							Action action;
							if ((action = <>9__3) == null)
							{
								action = (<>9__3 = delegate()
								{
									bool isCancellationRequested2 = cancellationToken.IsCancellationRequested;
									if (!isCancellationRequested2)
									{
										this._translationMessages = translationMapping;
										this.InitializeCommonAssetFileWatcher();
										foreach (AssetTypeConfig assetTypeConfig in assetTypes.Values)
										{
											bool flag = assetTypeConfig.IconImage == null;
											if (!flag)
											{
												Image iconImage = assetTypeConfig.IconImage;
												Texture texture = new Texture(Texture.TextureTypes.Texture2D);
												texture.CreateTexture2D(iconImage.Width, iconImage.Height, iconImage.Pixels, 5, GL.NEAREST, GL.NEAREST, GL.CLAMP_TO_EDGE, GL.CLAMP_TO_EDGE, GL.RGBA, GL.RGBA, GL.UNSIGNED_BYTE, false);
												this._iconTextures.Add(texture);
												assetTypeConfig.Icon = new PatchStyle(new TextureArea(texture, 0, 0, iconImage.Width, iconImage.Height, 1));
												assetTypeConfig.IconImage = null;
											}
										}
										this.AssetEditorOverlay.SetupAssetTypes(schemas, assetTypes);
									}
								});
							}
							engine.RunOnMainThread(<>4__this, action, false, false);
							Action action2;
							if ((action2 = <>9__4) == null)
							{
								action2 = (<>9__4 = delegate()
								{
									this.ValidateAllCosmeticAssets(cancellationToken);
								});
							}
							Task.Run(action2).ContinueWith(delegate(Task task)
							{
								bool flag = !task.IsFaulted;
								if (!flag)
								{
									LocalAssetEditorBackend.Logger.Error(task.Exception, "Failed to validate cosmetic assets");
								}
							});
						}
					}
				}).ContinueWith(delegate(Task t)
				{
					bool flag = !t.IsFaulted;
					if (!flag)
					{
						LocalAssetEditorBackend.Logger.Error(t.Exception, "Failed to initialize backend");
					}
				});
			}
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x001F1178 File Offset: 0x001EF378
		public override string GetButtonText(string messageId)
		{
			int length = "assetTypes.".Length;
			string text;
			bool flag = messageId.Length > length && this._translationMessages.TryGetValue(messageId.Substring(length), out text);
			string result;
			if (flag)
			{
				result = text;
			}
			else
			{
				result = messageId;
			}
			return result;
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x001F11C0 File Offset: 0x001EF3C0
		private void SetupCommonAssetTypes(Dictionary<string, AssetTypeConfig> assetTypes, Dictionary<string, string> fileExtensionAssetTypeMapping)
		{
			LocalAssetEditorBackend.<>c__DisplayClass22_0 CS$<>8__locals1;
			CS$<>8__locals1.fileExtensionAssetTypeMapping = fileExtensionAssetTypeMapping;
			CS$<>8__locals1.assetTypes = assetTypes;
			LocalAssetEditorBackend.<SetupCommonAssetTypes>g__RegisterCommonAssetType|22_0("Model", "Model", ".blockymodel", 2, "Model.png", true, ref CS$<>8__locals1);
			LocalAssetEditorBackend.<SetupCommonAssetTypes>g__RegisterCommonAssetType|22_0("Texture", "Texture", ".png", 5, "Texture.png", true, ref CS$<>8__locals1);
			LocalAssetEditorBackend.<SetupCommonAssetTypes>g__RegisterCommonAssetType|22_0("Animation", "Animation", ".blockyanim", 6, "Animation.png", true, ref CS$<>8__locals1);
			LocalAssetEditorBackend.<SetupCommonAssetTypes>g__RegisterCommonAssetType|22_0("Sound", "Sound", ".ogg", 0, "Audio.png", false, ref CS$<>8__locals1);
			LocalAssetEditorBackend.<SetupCommonAssetTypes>g__RegisterCommonAssetType|22_0("UI", "UI Markup", ".ui", 1, "File.png", false, ref CS$<>8__locals1);
			LocalAssetEditorBackend.<SetupCommonAssetTypes>g__RegisterCommonAssetType|22_0("Language", "Language", ".lang", 5, "File.png", false, ref CS$<>8__locals1);
		}

		// Token: 0x06005FE6 RID: 24550 RVA: 0x001F1294 File Offset: 0x001EF494
		private void InitializeAssetTypeConfigs(out Dictionary<string, AssetTypeConfig> assetTypesOut, out Dictionary<string, SchemaNode> schemasOut, out IDictionary<string, string> translationMapping)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			LocalAssetEditorBackend.<>c__DisplayClass23_0 CS$<>8__locals1;
			CS$<>8__locals1.schemas = new Dictionary<string, SchemaNode>();
			CS$<>8__locals1.assetTypes = new Dictionary<string, AssetTypeConfig>();
			schemasOut = CS$<>8__locals1.schemas;
			assetTypesOut = CS$<>8__locals1.assetTypes;
			translationMapping = Language.LoadServerLanguageFile("assetTypes.lang", this.AssetEditorOverlay.Interface.App.Settings.Language);
			this.SetupCommonAssetTypes(CS$<>8__locals1.assetTypes, this._fileExtensionAssetTypeMapping);
			foreach (string text in Directory.GetFiles(Path.Combine(Paths.EditorData, "CosmeticSchemas")))
			{
				bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					return;
				}
				bool flag = !text.EndsWith(".json");
				if (!flag)
				{
					JObject jObject = JObject.Parse(File.ReadAllText(Path.Combine(Paths.EditorData, "CosmeticSchemas", text)));
					base.LoadSchema(jObject, CS$<>8__locals1.schemas);
				}
			}
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Haircut", "Haircuts.json", CosmeticSchema.CreateHaircutSchema(new string[]
			{
				"/Characters/Haircuts/"
			}), "Haircut", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Overtop", "Overtops.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Cosmetics/Chest/",
				"/Cosmetics/Overtops/"
			}), "Overtop", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Undertop", "Undertops.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Cosmetics/Chest/",
				"/Cosmetics/Undertops/"
			}), "Undertop", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Pants", "Pants.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Cosmetics/Legs/",
				"/Cosmetics/Pants/"
			}), "Pants", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Overpants", "Overpants.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Cosmetics/Overpants/",
				"/Cosmetics/Overpants/"
			}), "Overpants", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("EarAccessory", "EarAccessory.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Cosmetics/Head/",
				"/Cosmetics/Ear_Accessories/"
			}), "Ear Accessory", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Ears", "Ears.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Characters/Body_Attachments/Ears/"
			}), "Ears", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Eyebrows", "Eyebrows.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Characters/Body_Attachments/Eyebrows/"
			}), "Eyebrows", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("FacialHair", "FacialHair.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Characters/Body_Attachments/Beards/"
			}), "Facial Hair", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("HeadAccessory", "HeadAccessory.json", CosmeticSchema.CreateHeadAccessorySchema(new string[]
			{
				"/Cosmetics/Head/",
				"/Cosmetics/Head_Accessories/"
			}), "Head Accessory", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("FaceAccessory", "FaceAccessory.json", CosmeticSchema.CreateHeadAccessorySchema(new string[]
			{
				"/Cosmetics/Head/",
				"/Cosmetics/Face_Accessories/"
			}), "Face Accessory", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Gloves", "Gloves.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Cosmetics/Hands/",
				"/Cosmetics/Gloves/"
			}), "Gloves", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Mouth", "Mouths.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Characters/Body_Attachments/Mouths/"
			}), "Mouth", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Shoes", "Shoes.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Cosmetics/Shoes/",
				"/Cosmetics/Shoes/"
			}), "Shoes", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("SkinFeature", "SkinFeatures.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Characters/Body_Attachments/SkinFeatures/"
			}), "Skin Feature", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Eyes", "Eyes.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Characters/Body_Attachments/Eyes/"
			}), "Eyes", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Face", "Faces.json", CosmeticSchema.CreateCosmeticSchema(new string[]
			{
				"/Characters/Body_Attachments/Faces/"
			}), "Face", "Cosmetic.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("GradientSet", "GradientSets.json", CS$<>8__locals1.schemas["https://schema.hytale.com/cosmetics/GradientSet.json"], "Gradient Set", "Color.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("EyeColor", "EyeColors.json", CS$<>8__locals1.schemas["https://schema.hytale.com/cosmetics/TintColor.json"], "Eye Color", "Color.png", ref CS$<>8__locals1);
			LocalAssetEditorBackend.<InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0("Emote", "Emotes.json", CS$<>8__locals1.schemas["https://schema.hytale.com/cosmetics/Emote.json"], "Emote", "Emote.png", ref CS$<>8__locals1);
		}

		// Token: 0x06005FE7 RID: 24551 RVA: 0x001F1784 File Offset: 0x001EF984
		private bool TryParseAssetType(JObject json, Dictionary<string, SchemaNode> schemas, IDictionary<string, string> translationMapping, out AssetTypeConfig assetTypeConfig)
		{
			assetTypeConfig = null;
			SchemaNode schemaNode = base.LoadSchema(json, schemas);
			JObject jobject = (JObject)json["hytale"];
			bool flag = jobject == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = jobject.ContainsKey("uiEditorIgnore") && (bool)jobject["uiEditorIgnore"];
				if (flag2)
				{
					result = false;
				}
				else
				{
					string text = null;
					bool isVirtual = false;
					bool flag3 = jobject.ContainsKey("path");
					if (flag3)
					{
						text = (string)jobject["path"];
					}
					else
					{
						bool flag4 = jobject.ContainsKey("virtualPath");
						if (flag4)
						{
							text = (string)jobject["virtualPath"];
							isVirtual = true;
						}
					}
					string text2 = Path.Combine(Paths.BuiltInAssets, "Server");
					bool flag5 = text == null || !Paths.IsSubPathOf(Path.Combine(text2, text), text2);
					if (flag5)
					{
						result = false;
					}
					else
					{
						string fileExtension = ".json";
						bool flag6 = jobject.ContainsKey("extension");
						if (flag6)
						{
							fileExtension = (string)jobject["extension"];
						}
						string name;
						bool flag7 = !translationMapping.TryGetValue((string)json["title"] + ".title", out name);
						if (flag7)
						{
							name = (string)json["title"];
						}
						assetTypeConfig = new AssetTypeConfig
						{
							Schema = schemaNode,
							Id = Path.GetFileNameWithoutExtension(schemaNode.Id),
							Name = name,
							Path = AssetPathUtils.CombinePaths("Server", text),
							AssetTree = AssetTreeFolder.Server,
							EditorType = 3,
							FileExtension = fileExtension,
							IsVirtual = isVirtual
						};
						base.ApplySchemaMetadata(assetTypeConfig, jobject);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06005FE8 RID: 24552 RVA: 0x001F194C File Offset: 0x001EFB4C
		public override void SaveUnsavedChanges()
		{
			this.SaveUnsavedChangesInternal(true);
		}

		// Token: 0x06005FE9 RID: 24553 RVA: 0x001F1958 File Offset: 0x001EFB58
		private void ProcessCallback<T>(Action<T, FormattedMessage> action, T value, FormattedMessage error)
		{
			bool isCancellationRequested = this._backendLifetimeCancellationTokenSource.IsCancellationRequested;
			if (!isCancellationRequested)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this.AssetEditorOverlay.Interface, delegate
				{
					action(value, error);
				}, true, false);
			}
		}

		// Token: 0x06005FEA RID: 24554 RVA: 0x001F19C4 File Offset: 0x001EFBC4
		private void ProcessErrorCallback<T>(Action<T, FormattedMessage> action, FormattedMessage error)
		{
			this.ProcessCallback<T>(action, default(T), error);
		}

		// Token: 0x06005FEB RID: 24555 RVA: 0x001F19E4 File Offset: 0x001EFBE4
		private void ProcessSuccessCallback<T>(Action<T, FormattedMessage> action, T value)
		{
			this.ProcessCallback<T>(action, value, null);
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x001F19F4 File Offset: 0x001EFBF4
		private void FetchJsonAsset(AssetTypeConfig assetTypeConfig, AssetReference assetReference, Action<JObject, FormattedMessage> callback, bool fromOpenedTab)
		{
			JObject jobject;
			bool flag = this._unsavedJsonAssets.TryGetValue(assetReference.FilePath, out jobject);
			if (flag)
			{
				this.ProcessSuccessCallback<JObject>(callback, (JObject)jobject.DeepClone());
			}
			else
			{
				string assetId = this.AssetEditorOverlay.GetAssetIdFromReference(assetReference);
				string lastSaveFileHash = null;
				LocalAssetEditorBackend.AssetUndoRedoStacks assetUndoRedoStacks;
				bool flag2 = this._undoRedoStacks.TryGetValue(assetReference.FilePath, out assetUndoRedoStacks);
				if (flag2)
				{
					lastSaveFileHash = assetUndoRedoStacks.SaveFileHash;
				}
				Task.Run(delegate()
				{
					bool flag3 = assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
					if (flag3)
					{
						string path = assetTypeConfig.Path;
						byte[] array = File.ReadAllBytes(Path.Combine(this._assetsDirectoryPath, path));
						string @string = Encoding.UTF8.GetString(array);
						JArray jarray = JArray.Parse(@string);
						bool clearUndoRedoStack = false;
						bool flag4 = fromOpenedTab && lastSaveFileHash != null;
						if (flag4)
						{
							clearUndoRedoStack = (AssetManager.ComputeHash(array) != lastSaveFileHash);
						}
						JObject assetJson = null;
						foreach (JToken jtoken in jarray)
						{
							bool flag5 = (string)jtoken["Id"] != assetId;
							if (!flag5)
							{
								assetJson = (JObject)jtoken;
								break;
							}
						}
						this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this.AssetEditorOverlay.Interface, delegate
						{
							bool clearUndoRedoStack = clearUndoRedoStack;
							if (clearUndoRedoStack)
							{
								this._undoRedoStacks.Remove(assetReference.FilePath);
							}
							bool flag9 = assetJson == null;
							if (flag9)
							{
								this.ProcessErrorCallback<JObject>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.assetNotFound", null));
							}
							else
							{
								this.ProcessSuccessCallback<JObject>(callback, assetJson);
							}
						}, false, false);
					}
					else
					{
						string fullPath = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, assetReference.FilePath));
						string fullPath2 = Path.GetFullPath(this._assetsDirectoryPath);
						bool flag6 = !Paths.IsSubPathOf(fullPath, fullPath2);
						if (flag6)
						{
							throw new Exception("Path must be within assets directory");
						}
						bool flag7 = !File.Exists(fullPath);
						if (flag7)
						{
							this.ProcessErrorCallback<JObject>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.assetNotFound", null));
						}
						else
						{
							byte[] array2 = File.ReadAllBytes(fullPath);
							string string2 = Encoding.UTF8.GetString(array2);
							bool clearUndoRedoStack = false;
							bool flag8 = fromOpenedTab && lastSaveFileHash != null;
							if (flag8)
							{
								clearUndoRedoStack = (AssetManager.ComputeHash(array2) != lastSaveFileHash);
							}
							JObject json;
							try
							{
								json = JObject.Parse(string2);
							}
							catch (Exception)
							{
								this.ProcessErrorCallback<JObject>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.invalidJson", null));
								return;
							}
							this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this.AssetEditorOverlay.Interface, delegate
							{
								bool clearUndoRedoStack = clearUndoRedoStack;
								if (clearUndoRedoStack)
								{
									this._undoRedoStacks.Remove(assetReference.FilePath);
								}
								this.ProcessSuccessCallback<JObject>(callback, json);
							}, false, false);
						}
					}
				}).ContinueWith(delegate(Task t)
				{
					bool isFaulted = t.IsFaulted;
					if (isFaulted)
					{
						LocalAssetEditorBackend.Logger.Error(t.Exception, "Failed to fetch asset");
						this.ProcessErrorCallback<JObject>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.errorOccurredFetching", null));
					}
				});
			}
		}

		// Token: 0x06005FED RID: 24557 RVA: 0x001F1AD0 File Offset: 0x001EFCD0
		private void FetchImageAsset(AssetReference assetReference, Action<Image, FormattedMessage> callback)
		{
			Image value;
			bool flag = this._unsavedImageAssets.TryGetValue(assetReference.FilePath, out value);
			if (flag)
			{
				this.ProcessSuccessCallback<Image>(callback, value);
			}
			else
			{
				string fullPath = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, assetReference.FilePath));
				string fullPath2 = Path.GetFullPath(this._assetsDirectoryPath);
				bool flag2 = !Paths.IsSubPathOf(fullPath, fullPath2);
				if (flag2)
				{
					throw new Exception("Path must be within assets directory");
				}
				Task.Run(delegate()
				{
					bool flag3 = !File.Exists(fullPath);
					if (flag3)
					{
						this.ProcessErrorCallback<Image>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.assetNotFound", null));
					}
					else
					{
						byte[] data = File.ReadAllBytes(fullPath);
						Image value2 = new Image(data);
						this.ProcessSuccessCallback<Image>(callback, value2);
					}
				}).ContinueWith(delegate(Task t)
				{
					bool isFaulted = t.IsFaulted;
					if (isFaulted)
					{
						LocalAssetEditorBackend.Logger.Error(t.Exception, "Failed to fetch asset");
						this.ProcessErrorCallback<Image>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.errorOccurredFetching", null));
					}
				});
			}
		}

		// Token: 0x06005FEE RID: 24558 RVA: 0x001F1B88 File Offset: 0x001EFD88
		private void FetchTextAsset(AssetReference assetReference, Action<string, FormattedMessage> callback)
		{
			string value;
			bool flag = this._unsavedTextAssets.TryGetValue(assetReference.FilePath, out value);
			if (flag)
			{
				this.ProcessSuccessCallback<string>(callback, value);
			}
			else
			{
				string fullPath = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, assetReference.FilePath));
				string fullPath2 = Path.GetFullPath(this._assetsDirectoryPath);
				bool flag2 = !Paths.IsSubPathOf(fullPath, fullPath2);
				if (flag2)
				{
					throw new Exception("Path must be within assets directory");
				}
				Task.Run(delegate()
				{
					bool flag3 = !File.Exists(fullPath);
					if (flag3)
					{
						this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.assetNotFound", null));
					}
					else
					{
						string value2 = File.ReadAllText(fullPath);
						this.ProcessSuccessCallback<string>(callback, value2);
					}
				}).ContinueWith(delegate(Task t)
				{
					bool isFaulted = t.IsFaulted;
					if (isFaulted)
					{
						LocalAssetEditorBackend.Logger.Error(t.Exception, "Failed to fetch asset");
						this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.errorOccurredFetching", null));
					}
				});
			}
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x001F1C40 File Offset: 0x001EFE40
		public override void FetchJsonAssetWithParents(AssetReference rootAssetReference, Action<Dictionary<string, TrackedAsset>, FormattedMessage> callback, bool fromOpenedTab = false)
		{
			LocalAssetEditorBackend.<>c__DisplayClass32_0 CS$<>8__locals1 = new LocalAssetEditorBackend.<>c__DisplayClass32_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.rootAssetReference = rootAssetReference;
			CS$<>8__locals1.callback = callback;
			CS$<>8__locals1.fromOpenedTab = fromOpenedTab;
			bool flag = !this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(CS$<>8__locals1.rootAssetReference.Type, out CS$<>8__locals1.assetTypeConfig);
			if (flag)
			{
				LocalAssetEditorBackend.Logger.Warn("Tried opening asset with unknown type: {0}", CS$<>8__locals1.rootAssetReference.Type);
				FormattedMessage error = FormattedMessage.FromMessageId("ui.assetEditor.errors.unknownAssetType", new Dictionary<string, object>
				{
					{
						"assetType",
						CS$<>8__locals1.rootAssetReference.Type
					}
				});
				this.ProcessErrorCallback<Dictionary<string, TrackedAsset>>(CS$<>8__locals1.callback, error);
			}
			else
			{
				bool flag2 = CS$<>8__locals1.assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
				if (flag2)
				{
					throw new Exception("Single file assets are not supported");
				}
				CS$<>8__locals1.results = new Dictionary<string, TrackedAsset>();
				CS$<>8__locals1.clearUndoRedoStack = false;
				CS$<>8__locals1.lastSaveFileHash = null;
				LocalAssetEditorBackend.AssetUndoRedoStacks assetUndoRedoStacks;
				bool flag3 = this._undoRedoStacks.TryGetValue(CS$<>8__locals1.rootAssetReference.FilePath, out assetUndoRedoStacks);
				if (flag3)
				{
					CS$<>8__locals1.lastSaveFileHash = assetUndoRedoStacks.SaveFileHash;
				}
				CS$<>8__locals1.<FetchJsonAssetWithParents>g__TryLoadAsset|0(CS$<>8__locals1.rootAssetReference, true).ContinueWith(delegate(Task<bool> t)
				{
					bool isFaulted = t.IsFaulted;
					if (isFaulted)
					{
						LocalAssetEditorBackend.Logger.Error(t.Exception, "Failed to fetch asset");
						CS$<>8__locals1.<>4__this.ProcessErrorCallback<Dictionary<string, TrackedAsset>>(CS$<>8__locals1.callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.errorOccurredFetching", null));
					}
					bool flag4 = !t.Result;
					if (!flag4)
					{
						Engine engine = CS$<>8__locals1.<>4__this.AssetEditorOverlay.Interface.Engine;
						Disposable @interface = CS$<>8__locals1.<>4__this.AssetEditorOverlay.Interface;
						Action action;
						if ((action = CS$<>8__locals1.<>9__5) == null)
						{
							action = (CS$<>8__locals1.<>9__5 = delegate()
							{
								bool clearUndoRedoStack = CS$<>8__locals1.clearUndoRedoStack;
								if (clearUndoRedoStack)
								{
									CS$<>8__locals1.<>4__this._undoRedoStacks.Remove(CS$<>8__locals1.rootAssetReference.FilePath);
								}
								CS$<>8__locals1.<>4__this.ProcessSuccessCallback<Dictionary<string, TrackedAsset>>(CS$<>8__locals1.callback, CS$<>8__locals1.results);
							});
						}
						engine.RunOnMainThread(@interface, action, false, false);
					}
				});
			}
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x001F1D7C File Offset: 0x001EFF7C
		public override void FetchAsset(AssetReference assetReference, Action<object, FormattedMessage> callback, bool fromOpenedTab = false)
		{
			AssetTypeConfig assetTypeConfig;
			bool flag = !this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes.TryGetValue(assetReference.Type, out assetTypeConfig);
			if (flag)
			{
				LocalAssetEditorBackend.Logger.Warn("Tried opening asset with unknown type: {0}", assetReference.Type);
				this.ProcessErrorCallback<object>(callback, new FormattedMessage
				{
					MessageId = "ui.assetEditor.errors.unknownAssetType",
					Params = new Dictionary<string, object>
					{
						{
							"assetType",
							assetReference.Type
						}
					}
				});
			}
			else
			{
				try
				{
					switch (assetTypeConfig.EditorType)
					{
					case 1:
						this.FetchTextAsset(assetReference, callback);
						break;
					case 2:
					case 3:
					case 4:
					case 6:
						this.FetchJsonAsset(assetTypeConfig, assetReference, callback, fromOpenedTab);
						break;
					case 5:
						this.FetchImageAsset(assetReference, callback);
						break;
					default:
						throw new Exception("Unhandled file type " + assetTypeConfig.EditorType.ToString());
					}
				}
				catch (Exception exception)
				{
					LocalAssetEditorBackend.Logger.Error(exception, "Failed to fetch asset");
					this.ProcessErrorCallback<object>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.errorOccurredFetching", null));
				}
			}
		}

		// Token: 0x06005FF1 RID: 24561 RVA: 0x001F1EB0 File Offset: 0x001F00B0
		public override void UpdateJsonAsset(AssetReference assetReference, List<ClientJsonUpdateCommand> commands, Action<FormattedMessage> callback = null)
		{
			LocalAssetEditorBackend.AssetUndoRedoStacks assetUndoRedoStacks;
			bool flag = !this._undoRedoStacks.TryGetValue(assetReference.FilePath, out assetUndoRedoStacks);
			if (flag)
			{
				assetUndoRedoStacks = (this._undoRedoStacks[assetReference.FilePath] = new LocalAssetEditorBackend.AssetUndoRedoStacks());
			}
			bool flag2 = assetUndoRedoStacks.RedoStack.Count > 0;
			if (flag2)
			{
				assetUndoRedoStacks.RedoStack.Clear();
			}
			foreach (ClientJsonUpdateCommand item in commands)
			{
				assetUndoRedoStacks.UndoStack.Push(item);
			}
			JObject asset = (JObject)this.AssetEditorOverlay.TrackedAssets[assetReference.FilePath].Data;
			this.UpdateJsonAsset(assetReference, asset);
			bool flag3 = callback != null;
			if (flag3)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					callback(null);
				}, true, false);
			}
		}

		// Token: 0x06005FF2 RID: 24562 RVA: 0x001F1FD4 File Offset: 0x001F01D4
		private void UpdateJsonAsset(AssetReference assetReference, JObject asset)
		{
			this.AssetEditorOverlay.SetFileSaveStatus(AssetEditorOverlay.SaveStatus.Unsaved, true);
			this._unsavedJsonAssets[assetReference.FilePath] = asset;
			this.ValidateJsonAsset(assetReference, asset);
			string type = assetReference.Type;
			string a = type;
			if (a == "ItemCategory")
			{
				this._dropdownDatasetEntriesCache.Remove("ItemCategories");
			}
		}

		// Token: 0x06005FF3 RID: 24563 RVA: 0x001F2038 File Offset: 0x001F0238
		public override void SetOpenEditorAsset(AssetReference assetReference)
		{
			this.SaveUnsavedChangesInternal(true);
		}

		// Token: 0x06005FF4 RID: 24564 RVA: 0x001F2044 File Offset: 0x001F0244
		public override void UpdateAsset(AssetReference assetReference, object data, Action<FormattedMessage> callback = null)
		{
			this.AssetEditorOverlay.SetFileSaveStatus(AssetEditorOverlay.SaveStatus.Unsaved, true);
			Image image = data as Image;
			if (image == null)
			{
				JObject jobject = data as JObject;
				if (jobject == null)
				{
					string text = data as string;
					if (text != null)
					{
						this._unsavedTextAssets[assetReference.FilePath] = text;
					}
				}
				else
				{
					this._unsavedJsonAssets[assetReference.FilePath] = jobject;
				}
			}
			else
			{
				this._unsavedImageAssets[assetReference.FilePath] = image;
			}
			bool flag = callback != null;
			if (flag)
			{
				this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
				{
					callback(null);
				}, true, false);
			}
		}

		// Token: 0x06005FF5 RID: 24565 RVA: 0x001F2110 File Offset: 0x001F0310
		private void ValidateEmote(JObject asset, List<AssetDiagnosticMessage> errors, List<AssetDiagnosticMessage> warnings)
		{
			this.ValidateName(asset, errors, warnings);
			JToken jtoken = asset["Animation"];
			bool flag = JsonUtils.IsNull(jtoken) || jtoken.Type != 8 || string.IsNullOrWhiteSpace((string)jtoken);
			if (flag)
			{
				errors.Add(new AssetDiagnosticMessage("Animation", "An emote must have a animation defined."));
			}
		}

		// Token: 0x06005FF6 RID: 24566 RVA: 0x001F2170 File Offset: 0x001F0370
		private void ValidateCosmetic(JObject asset, List<AssetDiagnosticMessage> errors, List<AssetDiagnosticMessage> warnings, bool requiresBaseColor = true)
		{
			this.ValidateName(asset, errors, warnings);
			bool flag = JsonUtils.IsNull(asset["Model"]) && JsonUtils.IsNull(asset["Variants"]);
			if (flag)
			{
				errors.Add(new AssetDiagnosticMessage("Model", "A cosmetic must have a model or variant defined."));
				errors.Add(new AssetDiagnosticMessage("Variants", "A cosmetic must have a model or variant defined."));
			}
			JArray jarray = asset["Tags"] as JArray;
			bool flag2 = jarray != null;
			if (flag2)
			{
				for (int i = 0; i < jarray.Count; i++)
				{
					JToken jtoken = jarray[i];
					bool flag3 = jtoken == null || jtoken.Type != 8 || ((string)jtoken).Trim() == "";
					if (flag3)
					{
						errors.Add(new AssetDiagnosticMessage("Tags." + i.ToString(), "A valid tag must be specified"));
					}
				}
			}
			JToken jtoken2 = asset["Model"];
			bool flag4 = jtoken2 != null && jtoken2.Type == 8;
			if (flag4)
			{
				JToken jtoken3 = asset["GreyscaleTexture"];
				bool flag5;
				if (jtoken3 == null || jtoken3.Type != 8)
				{
					JObject jobject = asset["Textures"] as JObject;
					flag5 = (jobject == null || jobject.Count == 0);
				}
				else
				{
					flag5 = false;
				}
				bool flag6 = flag5;
				if (flag6)
				{
					errors.Add(new AssetDiagnosticMessage("Textures", "At least one texture must be defined."));
					errors.Add(new AssetDiagnosticMessage("GreyscaleTexture", "At least one texture must be defined."));
				}
				else
				{
					JObject jobject2 = asset["Textures"] as JObject;
					bool flag7 = jobject2 != null;
					if (flag7)
					{
						foreach (KeyValuePair<string, JToken> keyValuePair in jobject2)
						{
							bool flag8 = keyValuePair.Value.Type != 1;
							if (flag8)
							{
								errors.Add(new AssetDiagnosticMessage("Textures." + keyValuePair.Key, "A valid texture must be assigned."));
							}
							else
							{
								JToken jtoken4 = keyValuePair.Value["Texture"];
								bool flag9 = jtoken4 == null || jtoken4.Type != 8 || string.IsNullOrWhiteSpace((string)keyValuePair.Value["Texture"]);
								if (flag9)
								{
									errors.Add(new AssetDiagnosticMessage("Textures." + keyValuePair.Key + ".Texture", "A valid texture must be assigned."));
								}
								bool flag10;
								if (requiresBaseColor)
								{
									JToken jtoken5 = keyValuePair.Value["BaseColor"];
									flag10 = (jtoken5 == null || jtoken5.Type != 2);
								}
								else
								{
									flag10 = false;
								}
								bool flag11 = flag10;
								if (flag11)
								{
									errors.Add(new AssetDiagnosticMessage("Textures." + keyValuePair.Key + ".BaseColor", "A valid preview color must be assigned"));
								}
							}
						}
					}
				}
			}
			JToken jtoken6 = asset["Variants"];
			bool flag12 = jtoken6 != null && jtoken6.Type == 1;
			if (flag12)
			{
				foreach (KeyValuePair<string, JToken> keyValuePair2 in ((JObject)asset["Variants"]))
				{
					bool flag13 = JsonUtils.IsNull(keyValuePair2.Value) || JsonUtils.IsNull(keyValuePair2.Value["Model"]);
					if (flag13)
					{
						errors.Add(new AssetDiagnosticMessage("Variants." + keyValuePair2.Key + ".Model", "This field cannot be empty."));
					}
					bool flag14;
					if (!JsonUtils.IsNull(keyValuePair2.Value))
					{
						JObject jobject3 = keyValuePair2.Value["Textures"] as JObject;
						if (jobject3 == null || jobject3.Count == 0)
						{
							JToken jtoken7 = keyValuePair2.Value["GreyscaleTexture"];
							flag14 = (jtoken7 == null || jtoken7.Type != 8);
						}
						else
						{
							flag14 = false;
						}
					}
					else
					{
						flag14 = true;
					}
					bool flag15 = flag14;
					if (flag15)
					{
						errors.Add(new AssetDiagnosticMessage("Variants." + keyValuePair2.Key + ".Textures", "At least one texture must be defined."));
						errors.Add(new AssetDiagnosticMessage("Variants." + keyValuePair2.Key + ".GreyscaleTexture", "At least one texture must be defined."));
					}
					else
					{
						JObject jobject4 = keyValuePair2.Value["Textures"] as JObject;
						bool flag16 = jobject4 != null;
						if (flag16)
						{
							foreach (KeyValuePair<string, JToken> keyValuePair3 in jobject4)
							{
								bool flag17 = keyValuePair3.Value.Type != 1;
								if (flag17)
								{
									errors.Add(new AssetDiagnosticMessage("Variants." + keyValuePair2.Key + ".Textures." + keyValuePair3.Key, "A valid texture must be assigned."));
								}
								else
								{
									JToken jtoken8 = keyValuePair3.Value["Texture"];
									bool flag18 = jtoken8 == null || jtoken8.Type != 8 || string.IsNullOrWhiteSpace((string)keyValuePair3.Value["Texture"]);
									if (flag18)
									{
										errors.Add(new AssetDiagnosticMessage(string.Concat(new string[]
										{
											"Variants.",
											keyValuePair2.Key,
											".Textures.",
											keyValuePair3.Key,
											".Texture"
										}), "A valid texture must be assigned."));
									}
									bool flag19;
									if (requiresBaseColor)
									{
										JToken jtoken9 = keyValuePair3.Value["BaseColor"];
										flag19 = (jtoken9 == null || jtoken9.Type != 2);
									}
									else
									{
										flag19 = false;
									}
									bool flag20 = flag19;
									if (flag20)
									{
										errors.Add(new AssetDiagnosticMessage(string.Concat(new string[]
										{
											"Variants.",
											keyValuePair2.Key,
											".Textures.",
											keyValuePair3.Key,
											".BaseColor"
										}), "A valid preview color must be assigned"));
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06005FF7 RID: 24567 RVA: 0x001F27F8 File Offset: 0x001F09F8
		private void ValidateEyeColor(JObject asset, List<AssetDiagnosticMessage> errors, List<AssetDiagnosticMessage> warnings)
		{
			bool flag = JsonUtils.IsNull(asset["BaseColor"]);
			if (flag)
			{
				errors.Add(new AssetDiagnosticMessage("BaseColor", "A preview color must be specified"));
			}
		}

		// Token: 0x06005FF8 RID: 24568 RVA: 0x001F2834 File Offset: 0x001F0A34
		private void ValidateGradientSet(JObject asset, List<AssetDiagnosticMessage> errors, List<AssetDiagnosticMessage> warnings)
		{
			bool flag = JsonUtils.IsNull(asset["Gradients"]) || asset["Gradients"].Type != 1 || ((JObject)asset["Gradients"]).Count == 0;
			if (flag)
			{
				errors.Add(new AssetDiagnosticMessage("Gradients", "At least one gradient must be specified"));
			}
			else
			{
				JObject jobject = (JObject)asset["Gradients"];
				foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
				{
					bool flag2 = JsonUtils.IsNull(keyValuePair.Value) || JsonUtils.IsNull(keyValuePair.Value["Texture"]);
					if (flag2)
					{
						errors.Add(new AssetDiagnosticMessage("Gradients." + keyValuePair.Key + ".Texture", "A gradient texture must be specified"));
					}
					bool flag3 = JsonUtils.IsNull(keyValuePair.Value) || JsonUtils.IsNull(keyValuePair.Value["BaseColor"]);
					if (flag3)
					{
						errors.Add(new AssetDiagnosticMessage("Gradients." + keyValuePair.Key + ".BaseColor", "A preview color must be specified"));
					}
				}
			}
		}

		// Token: 0x06005FF9 RID: 24569 RVA: 0x001F29A0 File Offset: 0x001F0BA0
		private void ValidateName(JObject asset, List<AssetDiagnosticMessage> errors, List<AssetDiagnosticMessage> warnings)
		{
			bool flag = JsonUtils.IsNull(asset["Name"]);
			if (flag)
			{
				errors.Add(new AssetDiagnosticMessage("Name", "This field cannot be empty."));
			}
			else
			{
				bool flag2 = ((string)asset["Name"]).Length < 3;
				if (flag2)
				{
					warnings.Add(new AssetDiagnosticMessage("Name", "A name should be at least 3 characters long."));
				}
			}
		}

		// Token: 0x06005FFA RID: 24570 RVA: 0x001F2A10 File Offset: 0x001F0C10
		private AssetDiagnostics GetAssetDiagnostics(AssetReference assetReference, JObject asset)
		{
			bool flag = this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes[assetReference.Type].AssetTree != AssetTreeFolder.Cosmetics;
			AssetDiagnostics result;
			if (flag)
			{
				result = AssetDiagnostics.None;
			}
			else
			{
				List<AssetDiagnosticMessage> list = new List<AssetDiagnosticMessage>();
				List<AssetDiagnosticMessage> list2 = new List<AssetDiagnosticMessage>();
				try
				{
					string type = assetReference.Type;
					string a = type;
					if (!(a == "Cosmetics.Emote"))
					{
						if (!(a == "Cosmetics.EyeColor"))
						{
							if (!(a == "Cosmetics.GradientSet"))
							{
								if (!(a == "Cosmetics.Ears") && !(a == "Cosmetics.Mouth") && !(a == "Cosmetics.Eyes"))
								{
									this.ValidateCosmetic(asset, list, list2, true);
								}
								else
								{
									this.ValidateCosmetic(asset, list, list2, false);
								}
							}
							else
							{
								this.ValidateGradientSet(asset, list, list2);
							}
						}
						else
						{
							this.ValidateEyeColor(asset, list, list2);
						}
					}
					else
					{
						this.ValidateEmote(asset, list, list2);
					}
				}
				catch (Exception exception)
				{
					LocalAssetEditorBackend.Logger.Warn(exception, "Validation error");
					list.Add(new AssetDiagnosticMessage(null, "An error occurred during asset validation"));
				}
				result = new AssetDiagnostics(list.ToArray(), list2.ToArray());
			}
			return result;
		}

		// Token: 0x06005FFB RID: 24571 RVA: 0x001F2B54 File Offset: 0x001F0D54
		private void ValidateJsonAsset(AssetReference assetReference, JObject asset)
		{
			AssetDiagnostics assetDiagnostics = this.GetAssetDiagnostics(assetReference, asset);
			this.AssetEditorOverlay.OnDiagnosticsUpdated(new Dictionary<string, AssetDiagnostics>
			{
				{
					assetReference.FilePath,
					assetDiagnostics
				}
			});
		}

		// Token: 0x06005FFC RID: 24572 RVA: 0x001F2B8C File Offset: 0x001F0D8C
		private void LoadWwiseDropdownEntries()
		{
			Dictionary<string, WwiseResource> upcomingWwiseIds;
			WwiseHeaderParser.Parse(Path.Combine(Paths.BuiltInAssets, "Common/SoundBanks/Wwise_IDs.h"), out upcomingWwiseIds);
			this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
			{
				List<string> list = new List<string>();
				foreach (KeyValuePair<string, WwiseResource> keyValuePair in upcomingWwiseIds)
				{
					bool flag = keyValuePair.Value.Type == WwiseResource.WwiseResourceType.Event;
					if (flag)
					{
						list.Add(keyValuePair.Key);
					}
				}
				this._loadingDropdownDatasets.Remove("WwiseEventIds");
				this._dropdownDatasetEntriesCache["WwiseEventIds"] = list;
				this.AssetEditorOverlay.OnDropdownDatasetUpdated("WwiseEventIds", list);
			}, false, false);
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x001F2BE8 File Offset: 0x001F0DE8
		private void LoadItemCategoriesEntries()
		{
			Dictionary<string, JObject> fileJsonMapping = new Dictionary<string, JObject>();
			string str = UnixPathUtil.ConvertToUnixPath(Path.GetFullPath(Path.Combine(new string[]
			{
				this._assetsDirectoryPath
			})));
			foreach (string text in Directory.GetFiles(Path.Combine(new string[]
			{
				this._assetsDirectoryPath,
				"Server",
				"Item",
				"Category",
				"CreativeLibrary"
			}), "*.json"))
			{
				string key = Paths.StripBasePath(UnixPathUtil.ConvertToUnixPath(text), str + "/");
				try
				{
					JObject value = JObject.Parse(File.ReadAllText(text));
					fileJsonMapping.Add(key, value);
				}
				catch (Exception)
				{
				}
			}
			this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
			{
				List<string> list = new List<string>();
				foreach (KeyValuePair<string, JObject> keyValuePair in fileJsonMapping)
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(keyValuePair.Key);
					JObject value2;
					bool flag = !this._unsavedJsonAssets.TryGetValue(keyValuePair.Key, out value2);
					if (flag)
					{
						value2 = keyValuePair.Value;
					}
					bool flag2;
					if (value2["Children"] != null)
					{
						JToken jtoken = value2["Children"];
						flag2 = (jtoken == null || jtoken.Type != 2);
					}
					else
					{
						flag2 = true;
					}
					bool flag3 = flag2;
					if (!flag3)
					{
						foreach (JToken jtoken2 in value2["Children"])
						{
							bool flag4;
							if (jtoken2 != null)
							{
								if (jtoken2 == null)
								{
									flag4 = true;
								}
								else
								{
									JToken jtoken3 = jtoken2["Id"];
									flag4 = (((jtoken3 != null) ? new JTokenType?(jtoken3.Type) : null).GetValueOrDefault() != 8);
								}
							}
							else
							{
								flag4 = true;
							}
							bool flag5 = flag4;
							if (!flag5)
							{
								list.Add(string.Format("{0}.{1}", fileNameWithoutExtension, jtoken2["Id"]));
							}
						}
					}
				}
				this._loadingDropdownDatasets.Remove("ItemCategories");
				this._dropdownDatasetEntriesCache["ItemCategories"] = list;
				this.AssetEditorOverlay.OnDropdownDatasetUpdated("ItemCategories", list);
			}, false, false);
		}

		// Token: 0x06005FFE RID: 24574 RVA: 0x001F2CF8 File Offset: 0x001F0EF8
		private void LoadDropdownEntries(string dataset)
		{
			Task.Run(delegate()
			{
				string dataset2 = dataset;
				string a = dataset2;
				if (!(a == "WwiseEventIds"))
				{
					if (a == "ItemCategories")
					{
						this.LoadItemCategoriesEntries();
					}
				}
				else
				{
					this.LoadWwiseDropdownEntries();
				}
			}).ContinueWith(delegate(Task t)
			{
				bool flag = !t.IsFaulted;
				if (!flag)
				{
					LocalAssetEditorBackend.Logger.Error(t.Exception, "Failed to fetch dataset");
				}
			});
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x001F2D50 File Offset: 0x001F0F50
		public override bool TryGetDropdownEntriesOrFetch(string dataset, out List<string> entries, object extraValue = null)
		{
			bool flag = base.TryGetDropdownEntriesOrFetch(dataset, out entries, extraValue);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = dataset != "ItemCategories" && dataset != "WwiseEventIds";
				if (flag2)
				{
					entries = new List<string>();
					result = true;
				}
				else
				{
					bool flag3 = this._dropdownDatasetEntriesCache.TryGetValue(dataset, out entries);
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = !this._loadingDropdownDatasets.Contains(dataset);
						if (flag4)
						{
							this._loadingDropdownDatasets.Add(dataset);
							this.LoadDropdownEntries(dataset);
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06006000 RID: 24576 RVA: 0x001F2DE4 File Offset: 0x001F0FE4
		public override void FetchAutoCompleteData(string dataset, string query, Action<HashSet<string>, FormattedMessage> callback)
		{
			AssetReference currentAsset = this.AssetEditorOverlay.CurrentAsset;
			query = query.ToLowerInvariant();
			HashSet<string> hashSet = new HashSet<string>();
			bool flag = this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes[currentAsset.Type].AssetTree == AssetTreeFolder.Cosmetics && dataset == "Cosmetics.Tags";
			if (flag)
			{
				foreach (CharacterPart characterPart in this.GetCharacterParts(currentAsset.Type))
				{
					bool flag2 = characterPart.Tags == null;
					if (!flag2)
					{
						foreach (string text in characterPart.Tags)
						{
							bool flag3 = text == null || text.Trim() == "";
							if (!flag3)
							{
								bool flag4 = query != "" && !text.ToLowerInvariant().Contains(query);
								if (!flag4)
								{
									hashSet.Add(text);
								}
							}
						}
					}
				}
				foreach (JObject jobject in this._unsavedJsonAssets.Values)
				{
					JArray jarray = jobject["Tags"] as JArray;
					bool flag5 = jarray == null;
					if (!flag5)
					{
						foreach (JToken jtoken in jarray)
						{
							bool flag6 = jtoken == null || jtoken.Type != 8;
							if (!flag6)
							{
								string text2 = (string)jtoken;
								bool flag7 = text2.Trim() == "";
								if (!flag7)
								{
									bool flag8 = query != "" && !text2.ToLowerInvariant().Contains(query);
									if (!flag8)
									{
										hashSet.Add(text2);
									}
								}
							}
						}
					}
				}
			}
			this.ProcessSuccessCallback<HashSet<string>>(callback, hashSet);
		}

		// Token: 0x06006001 RID: 24577 RVA: 0x001F3048 File Offset: 0x001F1248
		private List<CharacterPart> GetCharacterParts(string assetType)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(assetType);
			if (num <= 2004342656U)
			{
				if (num <= 324672021U)
				{
					if (num != 87057350U)
					{
						if (num != 259755750U)
						{
							if (num == 324672021U)
							{
								if (assetType == "Cosmetics.HeadAccessory")
								{
									return new List<CharacterPart>(this.AssetEditorOverlay.Interface.App.CharacterPartStore.HeadAccessory);
								}
							}
						}
						else if (assetType == "Cosmetics.Ears")
						{
							return this.AssetEditorOverlay.Interface.App.CharacterPartStore.Ears;
						}
					}
					else if (assetType == "Cosmetics.Undertop")
					{
						return this.AssetEditorOverlay.Interface.App.CharacterPartStore.Undertops;
					}
				}
				else if (num <= 1575334765U)
				{
					if (num != 917743321U)
					{
						if (num == 1575334765U)
						{
							if (assetType == "Cosmetics.Eyes")
							{
								return this.AssetEditorOverlay.Interface.App.CharacterPartStore.Eyes;
							}
						}
					}
					else if (assetType == "Cosmetics.Shoes")
					{
						return this.AssetEditorOverlay.Interface.App.CharacterPartStore.Shoes;
					}
				}
				else if (num != 1974885124U)
				{
					if (num == 2004342656U)
					{
						if (assetType == "Cosmetics.Overtop")
						{
							return this.AssetEditorOverlay.Interface.App.CharacterPartStore.Overtops;
						}
					}
				}
				else if (assetType == "Cosmetics.SkinFeature")
				{
					return this.AssetEditorOverlay.Interface.App.CharacterPartStore.SkinFeatures;
				}
			}
			else if (num <= 2866486861U)
			{
				if (num <= 2452233138U)
				{
					if (num != 2085705907U)
					{
						if (num == 2452233138U)
						{
							if (assetType == "Cosmetics.Mouth")
							{
								return this.AssetEditorOverlay.Interface.App.CharacterPartStore.Mouths;
							}
						}
					}
					else if (assetType == "Cosmetics.FacialHair")
					{
						return this.AssetEditorOverlay.Interface.App.CharacterPartStore.FacialHair;
					}
				}
				else if (num != 2678865939U)
				{
					if (num == 2866486861U)
					{
						if (assetType == "Cosmetics.EarAccessory")
						{
							return this.AssetEditorOverlay.Interface.App.CharacterPartStore.EarAccessory;
						}
					}
				}
				else if (assetType == "Cosmetics.Pants")
				{
					return this.AssetEditorOverlay.Interface.App.CharacterPartStore.Pants;
				}
			}
			else if (num <= 3054032895U)
			{
				if (num != 2934911845U)
				{
					if (num == 3054032895U)
					{
						if (assetType == "Cosmetics.Eyebrows")
						{
							return this.AssetEditorOverlay.Interface.App.CharacterPartStore.Eyebrows;
						}
					}
				}
				else if (assetType == "Cosmetics.Overpants")
				{
					return this.AssetEditorOverlay.Interface.App.CharacterPartStore.Overpants;
				}
			}
			else if (num != 3634689741U)
			{
				if (num == 4079578185U)
				{
					if (assetType == "Cosmetics.Haircut")
					{
						return new List<CharacterPart>(this.AssetEditorOverlay.Interface.App.CharacterPartStore.Haircuts);
					}
				}
			}
			else if (assetType == "Cosmetics.Gloves")
			{
				return this.AssetEditorOverlay.Interface.App.CharacterPartStore.Gloves;
			}
			return null;
		}

		// Token: 0x06006002 RID: 24578 RVA: 0x001F3484 File Offset: 0x001F1684
		public override void CreateAsset(AssetReference assetReference, object data, string buttonId = null, bool openInTab = false, Action<FormattedMessage> callback = null)
		{
			LocalAssetEditorBackend.Logger.Info<string, string>("Creating asset of type {0} in {1}", assetReference.Type, assetReference.FilePath);
			AssetTypeConfig assetTypeConfig = this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes[assetReference.Type];
			bool flag = assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
			if (flag)
			{
				try
				{
					string text = Path.Combine(this._assetsDirectoryPath, assetTypeConfig.Path);
					JArray jarray = JArray.Parse(File.ReadAllText(text));
					JObject jobject = (JObject)data;
					bool flag2 = false;
					for (int i = 0; i < jarray.Count; i++)
					{
						JToken jtoken = jarray[i];
						bool flag3 = (string)jtoken["Id"] != (string)jobject["Id"];
						if (!flag3)
						{
							jtoken[i] = jobject;
							flag2 = true;
							break;
						}
					}
					bool flag4 = !flag2;
					if (flag4)
					{
						jarray.Add(jobject);
					}
					File.WriteAllText(text, jarray.ToString());
				}
				catch (Exception exception)
				{
					LocalAssetEditorBackend.Logger.Error(exception, "Failed to crate asset {0}", new object[]
					{
						assetReference
					});
					if (callback != null)
					{
						callback(FormattedMessage.FromMessageId("ui.assetEditor.errors.failedToCreateAsset", null));
					}
					this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToCreateAsset", null, true));
					return;
				}
			}
			else
			{
				try
				{
					string fullPath = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, assetReference.FilePath));
					string fullPath2 = Path.GetFullPath(this._assetsDirectoryPath);
					bool flag5 = !Paths.IsSubPathOf(fullPath, fullPath2);
					if (flag5)
					{
						throw new Exception("Tried saving asset file outside of asset directory at " + fullPath);
					}
					JObject jobject2 = data as JObject;
					if (jobject2 == null)
					{
						string text2 = data as string;
						if (text2 == null)
						{
							Image image = data as Image;
							if (image != null)
							{
								image.SavePNG(fullPath, 255U, 65280U, 16711680U, 4278190080U);
							}
						}
						else
						{
							File.WriteAllText(fullPath, text2);
						}
					}
					else
					{
						File.WriteAllText(fullPath, jobject2.ToString());
					}
				}
				catch (Exception exception2)
				{
					LocalAssetEditorBackend.Logger.Error(exception2, "Failed to create asset {0}", new object[]
					{
						assetReference
					});
					if (callback != null)
					{
						callback(FormattedMessage.FromMessageId("ui.assetEditor.errors.failedToCreateAsset", null));
					}
					this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToCreateAsset", null, true));
					return;
				}
			}
			if (callback != null)
			{
				callback(null);
			}
			this.AssetEditorOverlay.OnAssetAdded(assetReference, false);
			if (openInTab)
			{
				this.AssetEditorOverlay.OpenCreatedAsset(assetReference, data);
			}
			JObject jobject3 = data as JObject;
			bool flag6 = jobject3 != null;
			if (flag6)
			{
				this.ValidateJsonAsset(assetReference, jobject3);
			}
			bool flag7 = assetReference.Type == "ItemCategory";
			if (flag7)
			{
				this._dropdownDatasetEntriesCache.Remove("ItemCategories");
			}
		}

		// Token: 0x06006003 RID: 24579 RVA: 0x001F37CC File Offset: 0x001F19CC
		public override void DeleteAsset(AssetReference assetReference, bool applyLocally)
		{
			LocalAssetEditorBackend.Logger.Info<string, string>("Deleting asset of type {0} in {1}", assetReference.Type, assetReference.FilePath);
			this._unsavedJsonAssets.Remove(assetReference.FilePath);
			AssetTypeConfig assetTypeConfig = this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes[assetReference.Type];
			bool flag = assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
			if (flag)
			{
				try
				{
					string assetIdFromReference = this.AssetEditorOverlay.GetAssetIdFromReference(assetReference);
					string text = Path.Combine(this._assetsDirectoryPath, assetTypeConfig.Path);
					JArray jarray = JArray.Parse(File.ReadAllText(text));
					for (int i = 0; i < jarray.Count; i++)
					{
						JToken jtoken = jarray[i];
						bool flag2 = (string)jtoken["Id"] != assetIdFromReference;
						if (!flag2)
						{
							jarray.RemoveAt(i);
							break;
						}
					}
					File.WriteAllText(text, jarray.ToString());
				}
				catch (Exception exception)
				{
					LocalAssetEditorBackend.Logger.Error(exception, "Failed to delete asset {0}", new object[]
					{
						assetReference
					});
					this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToDeleteAsset", null, true));
					return;
				}
			}
			else
			{
				try
				{
					string fullPath = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, assetReference.FilePath));
					string fullPath2 = Path.GetFullPath(this._assetsDirectoryPath);
					bool flag3 = !Paths.IsSubPathOf(fullPath, fullPath2);
					if (flag3)
					{
						throw new Exception("Tried removing asset file outside of asset directory at " + fullPath);
					}
					File.Delete(fullPath);
				}
				catch (Exception exception2)
				{
					LocalAssetEditorBackend.Logger.Error(exception2, "Failed to delete asset {0}", new object[]
					{
						assetReference
					});
					this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToDeleteAsset", null, true));
					return;
				}
			}
			this.AssetEditorOverlay.OnAssetDeleted(assetReference);
			string type = assetReference.Type;
			string a = type;
			if (a == "ItemCategory")
			{
				this._dropdownDatasetEntriesCache.Remove("ItemCategories");
			}
			this.AssetEditorOverlay.OnDiagnosticsUpdated(new Dictionary<string, AssetDiagnostics>
			{
				{
					assetReference.FilePath,
					AssetDiagnostics.None
				}
			});
		}

		// Token: 0x06006004 RID: 24580 RVA: 0x001F3A40 File Offset: 0x001F1C40
		public override void RenameAsset(AssetReference assetReference, string newFilePath, bool applyLocally)
		{
			LocalAssetEditorBackend.Logger.Info<string, string, string>("Renaming asset {1} -> {2} ({0})", assetReference.Type, assetReference.FilePath, newFilePath);
			string assetIdFromReference = this.AssetEditorOverlay.GetAssetIdFromReference(assetReference);
			string assetIdFromReference2 = this.AssetEditorOverlay.GetAssetIdFromReference(new AssetReference(assetReference.Type, newFilePath));
			AssetTypeConfig assetTypeConfig = this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes[assetReference.Type];
			this.SaveUnsavedChangesInternal(true);
			bool flag = assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
			if (flag)
			{
				string text = Path.Combine(this._assetsDirectoryPath, assetTypeConfig.Path);
				try
				{
					JArray jarray = JArray.Parse(File.ReadAllText(text));
					foreach (JToken jtoken in jarray)
					{
						bool flag2 = (string)jtoken["Id"] != assetIdFromReference;
						if (!flag2)
						{
							jtoken["Id"] = assetIdFromReference2;
							break;
						}
					}
					File.WriteAllText(text, jarray.ToString());
				}
				catch (Exception exception)
				{
					LocalAssetEditorBackend.Logger.Error(exception, "Failed to rename asset from {0} to {1} in {2}", new object[]
					{
						assetIdFromReference,
						assetIdFromReference2,
						text
					});
					this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToRenameAsset", null, true));
					return;
				}
			}
			else
			{
				string fullPath = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, assetReference.FilePath));
				string fullPath2 = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, newFilePath));
				string fullPath3 = Path.GetFullPath(this._assetsDirectoryPath);
				try
				{
					bool flag3 = !Paths.IsSubPathOf(fullPath, fullPath3);
					if (flag3)
					{
						LocalAssetEditorBackend.Logger.Warn("Tried moving asset file from folder outside of asset directory at " + fullPath);
						this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToRenameAsset", null, true));
						return;
					}
					bool flag4 = !Paths.IsSubPathOf(fullPath2, fullPath3);
					if (flag4)
					{
						LocalAssetEditorBackend.Logger.Warn("Tried moving asset file to folder outside of asset directory at " + fullPath);
						this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToRenameAsset", null, true));
						return;
					}
					File.Move(fullPath, fullPath2);
				}
				catch (Exception exception2)
				{
					LocalAssetEditorBackend.Logger.Error(exception2, "Failed to move file from {0} to {1}", new object[]
					{
						fullPath,
						fullPath2
					});
					this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToRenameAsset", null, true));
					return;
				}
			}
			this.AssetEditorOverlay.OnAssetRenamed(assetReference, new AssetReference(assetReference.Type, newFilePath));
			bool flag5 = assetReference.Type == "ItemCategory";
			if (flag5)
			{
				this._dropdownDatasetEntriesCache.Remove("ItemCategories");
			}
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x001F3D6C File Offset: 0x001F1F6C
		public void SaveImage(string path, Image image)
		{
			image.SavePNG(Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, path)), 255U, 65280U, 16711680U, 4278190080U);
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x001F3D9C File Offset: 0x001F1F9C
		private void SaveUnsavedChangesInternal(bool ignoreErrors)
		{
			bool flag = !ignoreErrors;
			if (flag)
			{
				foreach (KeyValuePair<string, JObject> keyValuePair in this._unsavedJsonAssets)
				{
					AssetDiagnostics assetDiagnostics;
					bool flag2 = this.AssetEditorOverlay.Diagnostics.TryGetValue(keyValuePair.Key, out assetDiagnostics) && assetDiagnostics.Errors != null && assetDiagnostics.Errors.Length != 0;
					if (flag2)
					{
						this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.messages.assetExport.errors", null, true));
						return;
					}
				}
			}
			int num = 0;
			Dictionary<string, JToken> dictionary = new Dictionary<string, JToken>();
			Dictionary<string, JToken> dictionary2 = new Dictionary<string, JToken>();
			Dictionary<string, string> dictionary3 = new Dictionary<string, string>();
			Dictionary<string, Image> dictionary4 = new Dictionary<string, Image>();
			foreach (KeyValuePair<string, JObject> keyValuePair2 in this._unsavedJsonAssets)
			{
				string text;
				bool flag3 = !this.AssetEditorOverlay.AssetTypeRegistry.TryGetAssetTypeFromPath(keyValuePair2.Key, out text);
				if (!flag3)
				{
					AssetTypeConfig assetTypeConfig = this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes[text];
					bool flag4 = assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
					if (flag4)
					{
					}
					bool flag5 = assetTypeConfig.AssetTree == AssetTreeFolder.Cosmetics;
					if (flag5)
					{
						JToken jtoken;
						bool flag6 = !dictionary2.TryGetValue(assetTypeConfig.Path, out jtoken);
						JArray jarray;
						if (flag6)
						{
							string text2 = File.ReadAllText(Path.Combine(this._assetsDirectoryPath, this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes[text].Path));
							jarray = JArray.Parse(text2);
							dictionary2[assetTypeConfig.Path] = jarray;
						}
						else
						{
							jarray = (JArray)jtoken;
						}
						string assetIdFromReference = this.AssetEditorOverlay.GetAssetIdFromReference(new AssetReference(text, keyValuePair2.Key));
						for (int i = 0; i < jarray.Count; i++)
						{
							JToken jtoken2 = jarray[i];
							bool flag7 = (string)jtoken2["Id"] != assetIdFromReference;
							if (!flag7)
							{
								jarray[i] = keyValuePair2.Value;
								num++;
								break;
							}
						}
					}
					else
					{
						string fullPath = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, keyValuePair2.Key));
						string fullPath2 = Path.GetFullPath(this._assetsDirectoryPath);
						bool flag8 = !Paths.IsSubPathOf(fullPath, fullPath2);
						if (flag8)
						{
							LocalAssetEditorBackend.Logger.Warn("Tried saving asset file to folder outside of asset directory at " + fullPath);
							this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.messages.assetExport.failedSaving", null, true));
							return;
						}
						dictionary[keyValuePair2.Key] = keyValuePair2.Value;
						num++;
					}
				}
			}
			this._unsavedJsonAssets.Clear();
			foreach (KeyValuePair<string, Image> keyValuePair3 in this._unsavedImageAssets)
			{
				string fullPath3 = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, keyValuePair3.Key));
				string fullPath4 = Path.GetFullPath(this._assetsDirectoryPath);
				bool flag9 = !Paths.IsSubPathOf(fullPath3, fullPath4);
				if (flag9)
				{
					LocalAssetEditorBackend.Logger.Warn("Tried saving asset file to folder outside of asset directory at " + fullPath3);
					this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.messages.assetExport.failedSaving", null, true));
					return;
				}
				dictionary4[keyValuePair3.Key] = keyValuePair3.Value;
				num++;
			}
			this._unsavedImageAssets.Clear();
			foreach (KeyValuePair<string, string> keyValuePair4 in this._unsavedTextAssets)
			{
				string fullPath5 = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, keyValuePair4.Key));
				string fullPath6 = Path.GetFullPath(this._assetsDirectoryPath);
				bool flag10 = !Paths.IsSubPathOf(fullPath5, fullPath6);
				if (flag10)
				{
					LocalAssetEditorBackend.Logger.Warn("Tried saving asset file to folder outside of asset directory at " + fullPath5);
					this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.messages.assetExport.failedSaving", null, true));
					return;
				}
				dictionary3[keyValuePair4.Key] = keyValuePair4.Value;
				num++;
			}
			this._unsavedTextAssets.Clear();
			bool flag11 = num == 0;
			if (flag11)
			{
				this.AssetEditorOverlay.SetFileSaveStatus(AssetEditorOverlay.SaveStatus.Saved, true);
			}
			else
			{
				foreach (KeyValuePair<string, JToken> keyValuePair5 in dictionary)
				{
					LocalAssetEditorBackend.Logger.Info("{0} has changes. Saving...", keyValuePair5.Key);
					string s = JsonConvert.SerializeObject(keyValuePair5.Value, 1);
					byte[] bytes = Encoding.UTF8.GetBytes(s);
					LocalAssetEditorBackend.AssetUndoRedoStacks assetUndoRedoStacks;
					bool flag12 = this._undoRedoStacks.TryGetValue(keyValuePair5.Key, out assetUndoRedoStacks);
					if (flag12)
					{
						assetUndoRedoStacks.SaveFileHash = AssetManager.ComputeHash(bytes);
					}
					File.WriteAllBytes(Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, keyValuePair5.Key)), bytes);
				}
				foreach (KeyValuePair<string, JToken> keyValuePair6 in dictionary2)
				{
					LocalAssetEditorBackend.Logger.Info("{0} has changes. Saving...", keyValuePair6.Key);
					string s2 = JsonConvert.SerializeObject(keyValuePair6.Value, 1);
					byte[] bytes2 = Encoding.UTF8.GetBytes(s2);
					string text3 = null;
					foreach (KeyValuePair<string, LocalAssetEditorBackend.AssetUndoRedoStacks> keyValuePair7 in this._undoRedoStacks)
					{
						bool flag13 = !keyValuePair7.Key.StartsWith(keyValuePair6.Key + "#");
						if (!flag13)
						{
							bool flag14 = text3 == null;
							if (flag14)
							{
								text3 = AssetManager.ComputeHash(bytes2);
							}
							keyValuePair7.Value.SaveFileHash = text3;
						}
					}
					File.WriteAllBytes(Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, keyValuePair6.Key)), bytes2);
				}
				foreach (KeyValuePair<string, string> keyValuePair8 in dictionary3)
				{
					LocalAssetEditorBackend.Logger.Info("{0} has changes. Saving...", keyValuePair8.Key);
					File.WriteAllText(Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, keyValuePair8.Key)), keyValuePair8.Value);
				}
				foreach (KeyValuePair<string, Image> keyValuePair9 in dictionary4)
				{
					LocalAssetEditorBackend.Logger.Info("{0} has changes. Saving...", keyValuePair9.Key);
					this.SaveImage(keyValuePair9.Key, keyValuePair9.Value);
				}
				this.AssetEditorOverlay.SetFileSaveStatus(AssetEditorOverlay.SaveStatus.Saved, true);
				this.AssetEditorOverlay.ToastNotifications.AddNotification(1, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.messages.assetExport.success", new Dictionary<string, string>
				{
					{
						"count",
						this.AssetEditorOverlay.Interface.FormatNumber(num)
					}
				}, true));
			}
		}

		// Token: 0x06006007 RID: 24583 RVA: 0x001F4640 File Offset: 0x001F2840
		private void ValidateAllCosmeticAssets(CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			Dictionary<string, AssetDiagnostics> diagnosticsMapping = new Dictionary<string, AssetDiagnostics>();
			foreach (KeyValuePair<string, AssetTypeConfig> keyValuePair in this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes)
			{
				bool flag = keyValuePair.Value.AssetTree != AssetTreeFolder.Cosmetics;
				if (!flag)
				{
					string text = File.ReadAllText(Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, keyValuePair.Value.Path)));
					JArray jarray = JArray.Parse(text);
					foreach (JToken jtoken in jarray)
					{
						bool isCancellationRequested = cancellationToken.IsCancellationRequested;
						if (isCancellationRequested)
						{
							break;
						}
						string text2 = (string)jtoken["Id"];
						AssetDiagnostics assetDiagnostics = this.GetAssetDiagnostics(new AssetReference(keyValuePair.Key, text2), (JObject)jtoken);
						diagnosticsMapping[keyValuePair.Value.Path + "#" + text2] = assetDiagnostics;
					}
				}
			}
			this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
			{
				this.AssetEditorOverlay.OnDiagnosticsUpdated(diagnosticsMapping);
			}, false, false);
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x001F47D0 File Offset: 0x001F29D0
		private void InitializeAssetMap(Dictionary<string, AssetTypeConfig> assetTypes)
		{
			List<AssetFile> cosmeticAssetFiles = new List<AssetFile>();
			List<AssetFile> commonAssetFiles = new List<AssetFile>();
			List<AssetFile> serverAssetFiles = new List<AssetFile>();
			Stopwatch stopWatch = Stopwatch.StartNew();
			List<KeyValuePair<string, AssetTypeConfig>> sortedAssetTypes = Enumerable.ToList<KeyValuePair<string, AssetTypeConfig>>(assetTypes);
			sortedAssetTypes.Sort((KeyValuePair<string, AssetTypeConfig> a, KeyValuePair<string, AssetTypeConfig> b) => string.Compare(a.Value.Path, b.Value.Path, StringComparison.InvariantCulture));
			Task task3 = Task.Run(delegate()
			{
				cosmeticAssetFiles = this.LoadAssetFiles(sortedAssetTypes, AssetTreeFolder.Cosmetics, "Cosmetics/CharacterCreator");
			});
			Task task2 = Task.Run(delegate()
			{
				this.LoadCommonAssets(commonAssetFiles);
			});
			Action <>9__4;
			Action <>9__5;
			Task.WhenAll(new Task[]
			{
				task2,
				task3
			}).ContinueWith(delegate(Task task)
			{
				bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
				if (!isCancellationRequested)
				{
					stopWatch.Stop();
					bool isFaulted = task.IsFaulted;
					if (isFaulted)
					{
						LocalAssetEditorBackend.Logger.Error(task.Exception, "Failed to initialize asset list");
						Engine engine = this.AssetEditorOverlay.Interface.Engine;
						Disposable <>4__this = this;
						Action action;
						if ((action = <>9__4) == null)
						{
							action = (<>9__4 = delegate()
							{
								bool isCancellationRequested2 = this._backendLifetimeCancellationToken.IsCancellationRequested;
								if (!isCancellationRequested2)
								{
									this.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.errors.failedToLoadAssetFiles", null, true));
									this.AssetEditorOverlay.SetupAssetFiles(new List<AssetFile>(), new List<AssetFile>(), new List<AssetFile>());
								}
							});
						}
						engine.RunOnMainThread(<>4__this, action, false, false);
					}
					else
					{
						LocalAssetEditorBackend.Logger.Info("Initialized asset list in {0}", stopWatch.Elapsed.TotalMilliseconds / 1000.0);
						Engine engine2 = this.AssetEditorOverlay.Interface.Engine;
						Disposable <>4__this2 = this;
						Action action2;
						if ((action2 = <>9__5) == null)
						{
							action2 = (<>9__5 = delegate()
							{
								bool isCancellationRequested2 = this._backendLifetimeCancellationToken.IsCancellationRequested;
								if (!isCancellationRequested2)
								{
									this.AssetEditorOverlay.SetupAssetFiles(serverAssetFiles, commonAssetFiles, cosmeticAssetFiles);
								}
							});
						}
						engine2.RunOnMainThread(<>4__this2, action2, false, false);
					}
				}
			}, this._threadCancellationToken);
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x001F48A0 File Offset: 0x001F2AA0
		public override void DeleteDirectory(string path, bool applyLocally, Action<string, FormattedMessage> callback)
		{
			string fullPath = Path.GetFullPath(this._assetsDirectoryPath);
			string fullPath2 = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, path));
			bool flag = !Paths.IsSubPathOf(fullPath2, fullPath);
			if (flag)
			{
				this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.deleteDirectoryInvalidPath", null));
			}
			else
			{
				bool flag2 = !Directory.Exists(fullPath2);
				if (flag2)
				{
					this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.deleteDirectoryMissing", null));
				}
				else
				{
					try
					{
						Directory.Delete(fullPath2, true);
						LocalAssetEditorBackend.Logger.Info("Deleted directory {0}", fullPath2);
					}
					catch (Exception exception)
					{
						LocalAssetEditorBackend.Logger.Error(exception, "Failed to create directory " + fullPath2);
						this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.deleteDirectoryOther", null));
						return;
					}
					this.AssetEditorOverlay.OnDirectoryDeleted(path);
					this.ProcessSuccessCallback<string>(callback, path);
				}
			}
		}

		// Token: 0x0600600A RID: 24586 RVA: 0x001F498C File Offset: 0x001F2B8C
		public override void RenameDirectory(string path, string newPath, bool applyLocally, Action<string, FormattedMessage> callback)
		{
			string fullPath = Path.GetFullPath(this._assetsDirectoryPath);
			string fullPath2 = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, path));
			string fullPath3 = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, newPath));
			bool flag = !Paths.IsSubPathOf(fullPath2, fullPath) || !Paths.IsSubPathOf(fullPath3, fullPath);
			if (flag)
			{
				this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.renameDirectoryInvalidPath", null));
			}
			else
			{
				bool flag2 = Directory.Exists(fullPath3);
				if (flag2)
				{
					this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.renameDirectoryExists", null));
				}
				else
				{
					bool flag3 = !Directory.Exists(fullPath2);
					if (flag3)
					{
						this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.renameDirectoryMissing", null));
					}
					else
					{
						try
						{
							Directory.Move(fullPath2, fullPath3);
							LocalAssetEditorBackend.Logger.Info<string, string>("Moved directory {0} to {1}", fullPath2, fullPath3);
						}
						catch (Exception exception)
						{
							LocalAssetEditorBackend.Logger.Error(exception, "Failed to create directory " + fullPath2);
							this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.renameDirectoryOther", null));
							return;
						}
						this.AssetEditorOverlay.OnDirectoryRenamed(path, newPath);
						this.ProcessSuccessCallback<string>(callback, path);
					}
				}
			}
		}

		// Token: 0x0600600B RID: 24587 RVA: 0x001F4AC4 File Offset: 0x001F2CC4
		public override void CreateDirectory(string path, bool applyLocally, Action<string, FormattedMessage> callback)
		{
			string fullPath = Path.GetFullPath(this._assetsDirectoryPath);
			string fullPath2 = Path.GetFullPath(Path.Combine(this._assetsDirectoryPath, path));
			bool flag = !Paths.IsSubPathOf(fullPath2, fullPath);
			if (flag)
			{
				this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.createDirectoryInvalidPath", null));
			}
			else
			{
				bool flag2 = Directory.Exists(fullPath2);
				if (flag2)
				{
					this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.createDirectoryExists", null));
				}
				else
				{
					try
					{
						Directory.CreateDirectory(fullPath2);
						LocalAssetEditorBackend.Logger.Info("Created directory " + fullPath2);
					}
					catch (Exception exception)
					{
						LocalAssetEditorBackend.Logger.Error(exception, "Failed to create directory " + fullPath2);
						this.ProcessErrorCallback<string>(callback, FormattedMessage.FromMessageId("ui.assetEditor.errors.createDirectoryOther", null));
						return;
					}
					this.AssetEditorOverlay.OnDirectoryCreated(path);
					this.ProcessSuccessCallback<string>(callback, path);
				}
			}
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x001F4BB0 File Offset: 0x001F2DB0
		private void InitializeCommonAssetFileWatcher()
		{
			Debug.Assert(ThreadHelper.IsMainThread());
			this.<InitializeCommonAssetFileWatcher>g__CreateFileWatcher|66_0(Path.Combine(this._assetsDirectoryPath, "Common"));
			this.<InitializeCommonAssetFileWatcher>g__CreateFileWatcher|66_0(Path.Combine(this._assetsDirectoryPath, "Server"));
			this._threadCancellationToken = this._threadCancellationTokenSource.Token;
			this._fileWatcherHandlerThread = new Thread(delegate()
			{
				this.ProcessFileWatcherEventQueue(this._threadCancellationToken);
			});
			this._fileWatcherHandlerThread.IsBackground = true;
			this._fileWatcherHandlerThread.Name = "FileWatcherEventQueueHandler";
			this._fileWatcherHandlerThread.Start();
		}

		// Token: 0x0600600D RID: 24589 RVA: 0x001F4C4C File Offset: 0x001F2E4C
		private void ProcessFileWatcherEventQueue(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				FileSystemEventArgs fileSystemEventArgs;
				bool flag = !this._fileWatcherEventQueue.TryDequeue(out fileSystemEventArgs);
				if (!flag)
				{
					WatcherChangeTypes changeType = fileSystemEventArgs.ChangeType;
					WatcherChangeTypes watcherChangeTypes = changeType;
					if (watcherChangeTypes != 1)
					{
						if (watcherChangeTypes != 2)
						{
							if (watcherChangeTypes == 8)
							{
								this.OnAssetFileRenamed((RenamedEventArgs)fileSystemEventArgs, cancellationToken);
							}
						}
						else
						{
							this.OnAssetFileDeleted(fileSystemEventArgs, cancellationToken);
						}
					}
					else
					{
						this.OnAssetFileCreated(fileSystemEventArgs, cancellationToken);
					}
				}
			}
		}

		// Token: 0x0600600E RID: 24590 RVA: 0x001F4CC8 File Offset: 0x001F2EC8
		private void OnAssetFileCreated(FileSystemEventArgs args, CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			bool isDirectory = Directory.Exists(args.FullPath);
			string text = UnixPathUtil.ConvertToUnixPath(Path.GetFullPath(this._assetsDirectoryPath));
			string path = UnixPathUtil.ConvertToUnixPath(args.FullPath).Substring(text.Length).TrimStart(new char[]
			{
				'/'
			}).Normalize(NormalizationForm.FormC);
			try
			{
				bool flag = (File.GetAttributes(args.FullPath) & FileAttributes.Hidden) == FileAttributes.Hidden;
				if (flag)
				{
					return;
				}
			}
			catch (IOException)
			{
				return;
			}
			this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (!isCancellationRequested)
				{
					LocalAssetEditorBackend.Logger.Info("File '{0}' has been created", path);
					string text2 = Path.GetDirectoryName(path).Replace(Path.DirectorySeparatorChar, '/');
					AssetFile assetFile;
					bool flag2 = text2 != "Common" && !this.AssetEditorOverlay.Assets.TryGetAsset(text2, out assetFile, false);
					if (!flag2)
					{
						bool flag3 = this.AssetEditorOverlay.Assets.TryGetAsset(path.Replace(Path.DirectorySeparatorChar, '/'), out assetFile, false);
						if (!flag3)
						{
							bool flag4 = !this.HasPathCompatibleAssetType(path);
							if (!flag4)
							{
								bool isDirectory = isDirectory;
								if (isDirectory)
								{
									this.AssetEditorOverlay.OnDirectoryCreated(path);
									this.LoadAssetsInDirectory(path);
								}
								else
								{
									string type;
									bool flag5 = !this.AssetEditorOverlay.AssetTypeRegistry.TryGetAssetTypeFromPath(path, out type);
									if (!flag5)
									{
										this.AssetEditorOverlay.OnAssetAdded(new AssetReference(type, path), false);
									}
								}
							}
						}
					}
				}
			}, false, false);
		}

		// Token: 0x0600600F RID: 24591 RVA: 0x001F4DA0 File Offset: 0x001F2FA0
		private void OnAssetFileDeleted(FileSystemEventArgs args, CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			string text = UnixPathUtil.ConvertToUnixPath(Path.GetFullPath(this._assetsDirectoryPath));
			string path = UnixPathUtil.ConvertToUnixPath(args.FullPath).Substring(text.Length).TrimStart(new char[]
			{
				'/'
			}).Normalize(NormalizationForm.FormC);
			this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (!isCancellationRequested)
				{
					LocalAssetEditorBackend.Logger.Info("File '{0}' has been deleted", path);
					AssetFile assetFile;
					bool flag = !this.AssetEditorOverlay.Assets.TryGetAsset(path, out assetFile, false);
					if (!flag)
					{
						bool flag2 = !this.HasPathCompatibleAssetType(path);
						if (!flag2)
						{
							bool isDirectory = assetFile.IsDirectory;
							if (isDirectory)
							{
								this.AssetEditorOverlay.OnDirectoryDeleted(path);
							}
							else
							{
								string type;
								bool flag3 = !this.AssetEditorOverlay.AssetTypeRegistry.TryGetAssetTypeFromPath(path, out type);
								if (!flag3)
								{
									this.AssetEditorOverlay.OnAssetDeleted(new AssetReference(type, path));
								}
							}
						}
					}
				}
			}, false, false);
		}

		// Token: 0x06006010 RID: 24592 RVA: 0x001F4E38 File Offset: 0x001F3038
		private void OnAssetFileRenamed(RenamedEventArgs args, CancellationToken cancellationToken)
		{
			Debug.Assert(!ThreadHelper.IsMainThread());
			string text = UnixPathUtil.ConvertToUnixPath(Path.GetFullPath(this._assetsDirectoryPath));
			string pathOld = UnixPathUtil.ConvertToUnixPath(args.OldFullPath).Substring(text.Length).TrimStart(new char[]
			{
				'/'
			}).Normalize(NormalizationForm.FormC);
			string pathNew = UnixPathUtil.ConvertToUnixPath(args.FullPath).Substring(text.Length).TrimStart(new char[]
			{
				'/'
			}).Normalize(NormalizationForm.FormC);
			bool existsOnFileSystem = true;
			bool isHidden = false;
			bool isDirectory = Directory.Exists(args.FullPath);
			try
			{
				isHidden = ((File.GetAttributes(args.FullPath) & FileAttributes.Hidden) == FileAttributes.Hidden);
			}
			catch (FileNotFoundException)
			{
				existsOnFileSystem = false;
			}
			catch (IOException)
			{
			}
			this.AssetEditorOverlay.Interface.Engine.RunOnMainThread(this, delegate
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (!isCancellationRequested)
				{
					string path = Path.GetDirectoryName(pathNew).Replace(Path.DirectorySeparatorChar, '/');
					AssetFile assetFile;
					bool flag = !AssetPathUtils.IsAssetTreeRootDirectory(path) && !this.AssetEditorOverlay.Assets.TryGetDirectory(path, out assetFile, false);
					if (!flag)
					{
						bool isDirectory = isDirectory;
						if (isDirectory)
						{
							LocalAssetEditorBackend.Logger.Info<string, string>("Directory '{0}' has been renamed to '{1}'", pathOld, pathNew);
							bool flag2 = this.HasPathCompatibleAssetType(pathOld);
							bool flag3 = this.HasPathCompatibleAssetType(pathNew);
							bool flag4 = !flag2 && !flag3;
							if (!flag4)
							{
								bool flag5 = this.AssetEditorOverlay.Assets.TryGetDirectory(pathOld, out assetFile, false);
								bool flag6 = this.AssetEditorOverlay.Assets.TryGetAsset(pathNew, out assetFile, false);
								bool flag7 = (flag5 && !flag6 && flag3 && !isHidden) & existsOnFileSystem;
								if (flag7)
								{
									this.AssetEditorOverlay.OnDirectoryRenamed(pathOld, pathNew);
								}
								else
								{
									bool flag8 = flag5;
									if (flag8)
									{
										this.AssetEditorOverlay.OnDirectoryDeleted(pathOld);
									}
									bool flag9 = (!flag6 && flag3 && !isHidden) & existsOnFileSystem;
									if (flag9)
									{
										this.AssetEditorOverlay.OnDirectoryCreated(pathNew);
										this.LoadAssetsInDirectory(pathNew);
									}
								}
							}
						}
						else
						{
							LocalAssetEditorBackend.Logger.Info<string, string>("File '{0}' has been renamed to '{1}'", pathOld, pathNew);
							string text2;
							this.AssetEditorOverlay.AssetTypeRegistry.TryGetAssetTypeFromPath(pathOld, out text2);
							string text3;
							this.AssetEditorOverlay.AssetTypeRegistry.TryGetAssetTypeFromPath(pathNew, out text3);
							bool flag10 = text2 == null && text3 == null;
							if (!flag10)
							{
								bool flag11 = this.AssetEditorOverlay.Assets.TryGetFile(pathOld, out assetFile, false);
								bool flag12 = this.AssetEditorOverlay.Assets.TryGetAsset(pathNew, out assetFile, false);
								bool flag13 = (text3 == text2 && flag11 && !flag12 && !isHidden) & existsOnFileSystem;
								if (flag13)
								{
									this.AssetEditorOverlay.OnAssetRenamed(new AssetReference(text2, pathOld), new AssetReference(text3, pathNew));
								}
								else
								{
									bool flag14 = flag11 && text2 != null;
									if (flag14)
									{
										this.AssetEditorOverlay.OnAssetDeleted(new AssetReference(text2, pathOld));
									}
									bool flag15 = (!flag12 && text3 != null && !isHidden) & existsOnFileSystem;
									if (flag15)
									{
										this.AssetEditorOverlay.OnAssetAdded(new AssetReference(text3, pathNew), false);
									}
								}
							}
						}
					}
				}
			}, false, false);
		}

		// Token: 0x06006011 RID: 24593 RVA: 0x001F4F68 File Offset: 0x001F3168
		private bool HasPathCompatibleAssetType(string path)
		{
			bool flag = path.StartsWith("Common/");
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				foreach (AssetTypeConfig assetTypeConfig in this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes.Values)
				{
					bool flag2 = !assetTypeConfig.IsVirtual && assetTypeConfig.AssetTree != AssetTreeFolder.Cosmetics && path.StartsWith(assetTypeConfig.Path + "/");
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06006012 RID: 24594 RVA: 0x001F5010 File Offset: 0x001F3210
		private void LoadAssetFiles(string basePath, Dictionary<string, AssetTypeConfig> compatibleAssetTypes, List<AssetFile> assetFiles, out int countAdded)
		{
			LocalAssetEditorBackend.<>c__DisplayClass72_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.compatibleAssetTypes = compatibleAssetTypes;
			CS$<>8__locals1.assetFiles = assetFiles;
			CS$<>8__locals1.builtInAssetsPath = Path.GetFullPath(this._assetsDirectoryPath).Replace(Path.DirectorySeparatorChar, '/');
			CS$<>8__locals1.assetCount = 0;
			bool flag = Directory.Exists(Path.Combine(this._assetsDirectoryPath, basePath));
			if (flag)
			{
				this.<LoadAssetFiles>g__Walk|72_0(Path.Combine(this._assetsDirectoryPath, basePath), ref CS$<>8__locals1);
			}
			countAdded = CS$<>8__locals1.assetCount;
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x001F5094 File Offset: 0x001F3294
		private List<AssetFile> LoadAssetFiles(List<KeyValuePair<string, AssetTypeConfig>> assetTypes, AssetTreeFolder assetTree, string basePath)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			List<AssetFile> list = new List<AssetFile>();
			string[] array = (basePath + "/\\").Split(new char[]
			{
				'/'
			});
			Dictionary<string, Dictionary<string, AssetTypeConfig>> dictionary = new Dictionary<string, Dictionary<string, AssetTypeConfig>>();
			foreach (KeyValuePair<string, AssetTypeConfig> keyValuePair in assetTypes)
			{
				Dictionary<string, AssetTypeConfig> dictionary2;
				bool flag = !dictionary.TryGetValue(keyValuePair.Value.Path, out dictionary2);
				if (flag)
				{
					dictionary2 = (dictionary[keyValuePair.Value.Path] = new Dictionary<string, AssetTypeConfig>());
				}
				dictionary2[keyValuePair.Value.FileExtension] = keyValuePair.Value;
			}
			foreach (KeyValuePair<string, AssetTypeConfig> keyValuePair2 in assetTypes)
			{
				bool flag2 = keyValuePair2.Value.AssetTree != assetTree;
				if (!flag2)
				{
					bool flag3 = keyValuePair2.Value.AssetTree == AssetTreeFolder.Cosmetics;
					if (flag3)
					{
						string path = keyValuePair2.Value.Path;
						bool flag4 = !File.Exists(Path.Combine(this._assetsDirectoryPath, path));
						if (!flag4)
						{
							string text = File.ReadAllText(Path.Combine(this._assetsDirectoryPath, path));
							JArray jarray = JArray.Parse(text);
							list.Add(AssetFile.CreateAssetTypeDirectory(keyValuePair2.Value.Name, path, keyValuePair2.Key, path.Split(new char[]
							{
								'/'
							})));
							int count = list.Count;
							int num = 0;
							foreach (JToken jtoken in jarray)
							{
								string text2 = (string)jtoken["Id"];
								string text3 = path + "#" + text2;
								list.Add(AssetFile.CreateFile(text2, text3, keyValuePair2.Key, AssetPathUtils.GetAssetFilePathElements(text3, true)));
								num++;
							}
							list.Sort(count, num, AssetFileComparer.Instance);
						}
					}
					else
					{
						Dictionary<string, AssetTypeConfig> dictionary3;
						bool flag5 = !dictionary.TryGetValue(keyValuePair2.Value.Path, out dictionary3);
						if (!flag5)
						{
							bool flag6 = dictionary3.Count > 1;
							if (flag6)
							{
								dictionary.Remove(keyValuePair2.Value.Path);
							}
							string path2 = keyValuePair2.Value.Path;
							string[] array2 = keyValuePair2.Value.Path.Split(new char[]
							{
								'/'
							});
							bool flag7 = false;
							for (int i = 0; i < array2.Length - 1; i++)
							{
								bool flag8 = flag7 || i >= array.Length - 1 || array[i] != array2[i];
								if (flag8)
								{
									flag7 = true;
									string[] array3 = new string[i + 1];
									Array.Copy(array2, 0, array3, 0, i + 1);
									string path3 = string.Join("/", array3);
									list.Add(AssetFile.CreateDirectory(array2[i], path3, Enumerable.ToArray<string>(array3)));
								}
							}
							array = array2;
							bool flag9 = dictionary3.Count == 1;
							if (flag9)
							{
								list.Add(AssetFile.CreateAssetTypeDirectory(keyValuePair2.Value.Name, path2, keyValuePair2.Key, path2.Split(new char[]
								{
									'/'
								})));
							}
							else
							{
								list.Add(AssetFile.CreateDirectory(Enumerable.Last<string>(array2), path2, path2.Split(new char[]
								{
									'/'
								})));
							}
							int count2 = list.Count;
							int count3;
							this.LoadAssetFiles(path2, dictionary3, list, out count3);
							list.Sort(count2, count3, AssetFileComparer.Instance);
						}
					}
				}
			}
			LocalAssetEditorBackend.Logger.Info<AssetTreeFolder, double>("Loaded {0} assets in {1}s", assetTree, stopwatch.Elapsed.TotalMilliseconds / 1000.0);
			return list;
		}

		// Token: 0x06006014 RID: 24596 RVA: 0x001F54F8 File Offset: 0x001F36F8
		private void WalkCommonAssetDirectory(string path, string builtInAssetsPath, List<AssetFile> assetFiles)
		{
			bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
			if (!isCancellationRequested)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				DirectoryInfo[] directories = directoryInfo.GetDirectories("*.*", 0);
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					bool flag = (directoryInfo2.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
					if (!flag)
					{
						string name = directoryInfo2.Name;
						string text = directoryInfo2.FullName.Replace(Path.DirectorySeparatorChar, '/').Replace(builtInAssetsPath, "").TrimStart(new char[]
						{
							'/'
						}).Normalize(NormalizationForm.FormC);
						assetFiles.Add(AssetFile.CreateDirectory(name, text, text.Split(new char[]
						{
							'/'
						})));
						this.WalkCommonAssetDirectory(directoryInfo2.FullName, builtInAssetsPath, assetFiles);
					}
				}
				FileInfo[] files = directoryInfo.GetFiles("*.*", 0);
				foreach (FileInfo fileInfo in files)
				{
					bool flag2 = (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
					if (!flag2)
					{
						string assetType;
						bool flag3 = this._fileExtensionAssetTypeMapping.TryGetValue(fileInfo.Extension, out assetType);
						if (flag3)
						{
							string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfo.Name);
							string text2 = fileInfo.FullName.Replace(Path.DirectorySeparatorChar, '/').Replace(builtInAssetsPath, "").TrimStart(new char[]
							{
								'/'
							}).Normalize(NormalizationForm.FormC);
							assetFiles.Add(AssetFile.CreateFile(fileNameWithoutExtension, text2, assetType, text2.Split(new char[]
							{
								'/'
							})));
						}
					}
				}
			}
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x001F56AC File Offset: 0x001F38AC
		private void LoadCommonAssets(List<AssetFile> assetFiles)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			string text = Path.Combine(this._assetsDirectoryPath, "Common").Replace(Path.DirectorySeparatorChar, '/');
			bool flag = Directory.Exists(text);
			if (flag)
			{
				string builtInAssetsPath = Path.GetFullPath(this._assetsDirectoryPath).Replace(Path.DirectorySeparatorChar, '/');
				this.WalkCommonAssetDirectory(text, builtInAssetsPath, assetFiles);
			}
			else
			{
				LocalAssetEditorBackend.Logger.Error("Common asset directory does not exist. Skipping...");
			}
			stopwatch.Stop();
			LocalAssetEditorBackend.Logger.Info("Loaded common assets in {0}s", stopwatch.Elapsed.TotalMilliseconds / 1000.0);
			assetFiles.Sort(AssetFileComparer.Instance);
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x001F575C File Offset: 0x001F395C
		private void LoadAssetsInDirectory(string path)
		{
			List<AssetFile> assetFiles = new List<AssetFile>();
			IReadOnlyDictionary<string, AssetTypeConfig> assetTypes = this.AssetEditorOverlay.AssetTypeRegistry.AssetTypes;
			Action <>9__2;
			Task.Run(delegate()
			{
				bool flag = path.StartsWith("Common/");
				if (flag)
				{
					string path2 = Path.Combine(this._assetsDirectoryPath, path).Replace(Path.DirectorySeparatorChar, '/');
					string builtInAssetsPath = Path.GetFullPath(this._assetsDirectoryPath).Replace(Path.DirectorySeparatorChar, '/');
					this.WalkCommonAssetDirectory(path2, builtInAssetsPath, assetFiles);
				}
				else
				{
					Dictionary<string, AssetTypeConfig> dictionary = new Dictionary<string, AssetTypeConfig>();
					foreach (AssetTypeConfig assetTypeConfig in assetTypes.Values)
					{
						bool flag2 = path.StartsWith(assetTypeConfig.Path + "/");
						if (flag2)
						{
							dictionary[assetTypeConfig.FileExtension] = assetTypeConfig;
						}
					}
					int num;
					this.LoadAssetFiles(path, dictionary, assetFiles, out num);
				}
				bool flag3 = assetFiles.Count == 0;
				if (!flag3)
				{
					assetFiles.Sort(AssetFileComparer.Instance);
					Engine engine = this.AssetEditorOverlay.Interface.Engine;
					Disposable <>4__this = this;
					Action action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate()
						{
							bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
							if (!isCancellationRequested)
							{
								this.AssetEditorOverlay.OnDirectoryContentsUpdated(path, assetFiles);
							}
						});
					}
					engine.RunOnMainThread(<>4__this, action, false, false);
				}
			}).ContinueWith(delegate(Task t)
			{
				bool flag = !t.IsFaulted;
				if (!flag)
				{
					LocalAssetEditorBackend.Logger.Error(t.Exception, "Failed to load assets in directory {0}", new object[]
					{
						path
					});
				}
			});
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x001F57C4 File Offset: 0x001F39C4
		public override void UndoChanges(AssetReference assetReference)
		{
			LocalAssetEditorBackend.AssetUndoRedoStacks assetUndoRedoStacks;
			bool flag = !this._undoRedoStacks.TryGetValue(assetReference.FilePath, out assetUndoRedoStacks) || assetUndoRedoStacks.UndoStack.Count == 0;
			if (flag)
			{
				this.AssetEditorOverlay.ToastNotifications.AddNotification(0, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.messages.undoStackEmpty", null, true));
			}
			else
			{
				ClientJsonUpdateCommand clientJsonUpdateCommand = assetUndoRedoStacks.UndoStack.Pop();
				assetUndoRedoStacks.RedoStack.Push(clientJsonUpdateCommand);
				JObject jobject = (JObject)this.AssetEditorOverlay.TrackedAssets[assetReference.FilePath].Data;
				bool flag2 = clientJsonUpdateCommand.Path.Elements.Length == 0 && clientJsonUpdateCommand.Type == 0;
				if (flag2)
				{
					JToken previousValue = clientJsonUpdateCommand.PreviousValue;
					jobject = (JObject)((previousValue != null) ? previousValue.DeepClone() : null);
					this.AssetEditorOverlay.SetTrackedAssetData(assetReference.FilePath, jobject);
				}
				else
				{
					bool flag3 = clientJsonUpdateCommand.FirstCreatedProperty != null;
					if (flag3)
					{
						PropertyPath? propertyPath;
						JToken jtoken;
						this.AssetEditorOverlay.ConfigEditor.RemoveProperty(jobject, clientJsonUpdateCommand.FirstCreatedProperty.Value, out propertyPath, out jtoken, true, false);
					}
					else
					{
						ConfigEditor configEditor = this.AssetEditorOverlay.ConfigEditor;
						JObject root = jobject;
						PropertyPath path = clientJsonUpdateCommand.Path;
						JToken previousValue2 = clientJsonUpdateCommand.PreviousValue;
						PropertyPath? propertyPath;
						configEditor.SetProperty(root, path, (previousValue2 != null) ? previousValue2.DeepClone() : null, out propertyPath, true, clientJsonUpdateCommand.Type == 2);
					}
				}
				this.AssetEditorOverlay.Layout(null, true);
				this.UpdateJsonAsset(assetReference, jobject);
			}
		}

		// Token: 0x06006018 RID: 24600 RVA: 0x001F5948 File Offset: 0x001F3B48
		public override void RedoChanges(AssetReference assetReference)
		{
			LocalAssetEditorBackend.AssetUndoRedoStacks assetUndoRedoStacks;
			bool flag = !this._undoRedoStacks.TryGetValue(assetReference.FilePath, out assetUndoRedoStacks) || assetUndoRedoStacks.RedoStack.Count == 0;
			if (flag)
			{
				this.AssetEditorOverlay.ToastNotifications.AddNotification(0, this.AssetEditorOverlay.Interface.GetText("ui.assetEditor.messages.redoStackEmpty", null, true));
			}
			else
			{
				ClientJsonUpdateCommand clientJsonUpdateCommand = assetUndoRedoStacks.RedoStack.Pop();
				assetUndoRedoStacks.UndoStack.Push(clientJsonUpdateCommand);
				JObject jobject = (JObject)this.AssetEditorOverlay.TrackedAssets[assetReference.FilePath].Data;
				bool flag2 = clientJsonUpdateCommand.Path.Elements.Length == 0 && clientJsonUpdateCommand.Type == 0;
				if (flag2)
				{
					JToken value = clientJsonUpdateCommand.Value;
					jobject = (JObject)((value != null) ? value.DeepClone() : null);
					this.AssetEditorOverlay.SetTrackedAssetData(assetReference.FilePath, jobject);
				}
				else
				{
					JsonUpdateType type = clientJsonUpdateCommand.Type;
					JsonUpdateType jsonUpdateType = type;
					if (jsonUpdateType > 1)
					{
						if (jsonUpdateType == 2)
						{
							PropertyPath? propertyPath;
							JToken jtoken;
							this.AssetEditorOverlay.ConfigEditor.RemoveProperty(jobject, clientJsonUpdateCommand.Path, out propertyPath, out jtoken, true, false);
						}
					}
					else
					{
						ConfigEditor configEditor = this.AssetEditorOverlay.ConfigEditor;
						JObject root = jobject;
						PropertyPath path = clientJsonUpdateCommand.Path;
						JToken value2 = clientJsonUpdateCommand.Value;
						PropertyPath? propertyPath;
						configEditor.SetProperty(root, path, (value2 != null) ? value2.DeepClone() : null, out propertyPath, true, clientJsonUpdateCommand.Type == 1);
					}
				}
				this.AssetEditorOverlay.Layout(null, true);
				this.UpdateJsonAsset(assetReference, jobject);
			}
		}

		// Token: 0x0600601A RID: 24602 RVA: 0x001F5ADC File Offset: 0x001F3CDC
		[CompilerGenerated]
		internal static void <SetupCommonAssetTypes>g__RegisterCommonAssetType|22_0(string type, string name, string fileExtension, AssetEditorEditorType editorType, string icon = "File.png", bool isColoredIcon = false, ref LocalAssetEditorBackend.<>c__DisplayClass22_0 A_6)
		{
			bool flag = A_6.fileExtensionAssetTypeMapping != null;
			if (flag)
			{
				A_6.fileExtensionAssetTypeMapping[fileExtension] = type;
			}
			A_6.assetTypes.Add(type, new AssetTypeConfig
			{
				Name = name,
				Id = type,
				IsColoredIcon = isColoredIcon,
				Icon = new PatchStyle("AssetEditor/AssetIcons/" + icon),
				Path = "Common",
				AssetTree = AssetTreeFolder.Common,
				FileExtension = fileExtension,
				EditorType = editorType
			});
		}

		// Token: 0x0600601B RID: 24603 RVA: 0x001F5B6C File Offset: 0x001F3D6C
		[CompilerGenerated]
		internal static void <InitializeAssetTypeConfigs>g__RegisterCosmeticAssetType|23_0(string cosmeticType, string file, SchemaNode schema, string name, string icon = "Cosmetic.png", ref LocalAssetEditorBackend.<>c__DisplayClass23_0 A_5)
		{
			bool flag = schema.Id == null;
			if (flag)
			{
				schema.Id = "https://schema.hytale.com/cosmetics/" + cosmeticType.ToLowerInvariant() + ".json";
			}
			A_5.schemas[schema.Id] = schema;
			string text = "Cosmetics." + cosmeticType;
			JObject baseJsonAsset = null;
			bool flag2 = cosmeticType == "Haircut" || cosmeticType == "FacialHair" || cosmeticType == "Eyebrows";
			if (flag2)
			{
				JObject jobject = new JObject();
				jobject.Add("GradientSet", "Hair");
				baseJsonAsset = jobject;
			}
			else
			{
				bool flag3 = cosmeticType == "Mouth";
				if (flag3)
				{
					JObject jobject2 = new JObject();
					jobject2.Add("GradientSet", "Skin");
					baseJsonAsset = jobject2;
				}
			}
			A_5.assetTypes.Add(text, new AssetTypeConfig
			{
				HasIdField = true,
				Schema = schema,
				Name = name,
				Id = text,
				Icon = new PatchStyle("AssetEditor/AssetIcons/" + icon),
				BaseJsonAsset = baseJsonAsset,
				Path = "Cosmetics/CharacterCreator/" + file,
				AssetTree = AssetTreeFolder.Cosmetics,
				EditorType = 3,
				FileExtension = ".json"
			});
		}

		// Token: 0x0600601C RID: 24604 RVA: 0x001F5CB8 File Offset: 0x001F3EB8
		[CompilerGenerated]
		private void <InitializeCommonAssetFileWatcher>g__CreateFileWatcher|66_0(string path)
		{
			bool flag = !Directory.Exists(path);
			if (flag)
			{
				LocalAssetEditorBackend.Logger.Warn("Skipping file monitor creation for {0} due to missing directory", path);
			}
			else
			{
				FileSystemWatcher fileSystemWatcher = new FileSystemWatcher
				{
					Path = path,
					IncludeSubdirectories = true,
					NotifyFilter = 51
				};
				fileSystemWatcher.Created += delegate(object _, FileSystemEventArgs args)
				{
					this._fileWatcherEventQueue.Enqueue(args);
				};
				fileSystemWatcher.Deleted += delegate(object _, FileSystemEventArgs args)
				{
					this._fileWatcherEventQueue.Enqueue(args);
				};
				fileSystemWatcher.Renamed += delegate(object _, RenamedEventArgs args)
				{
					this._fileWatcherEventQueue.Enqueue(args);
				};
				fileSystemWatcher.EnableRaisingEvents = true;
				this._fileSystemWatchers.Add(fileSystemWatcher);
			}
		}

		// Token: 0x06006021 RID: 24609 RVA: 0x001F5D90 File Offset: 0x001F3F90
		[CompilerGenerated]
		private void <LoadAssetFiles>g__Walk|72_0(string path, ref LocalAssetEditorBackend.<>c__DisplayClass72_0 A_2)
		{
			foreach (string text in Directory.GetFiles(path))
			{
				bool isCancellationRequested = this._backendLifetimeCancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					return;
				}
				string text2 = text.Replace(Path.DirectorySeparatorChar, '/').Replace(A_2.builtInAssetsPath + "/", "").Normalize(NormalizationForm.FormC);
				string extension = Path.GetExtension(text);
				AssetTypeConfig assetTypeConfig;
				bool flag = A_2.compatibleAssetTypes.TryGetValue(extension, out assetTypeConfig);
				if (flag)
				{
					A_2.assetFiles.Add(AssetFile.CreateFile(Path.GetFileNameWithoutExtension(text2), text2, assetTypeConfig.Id, text2.Split(new char[]
					{
						'/'
					})));
					int assetCount = A_2.assetCount + 1;
					A_2.assetCount = assetCount;
				}
			}
			foreach (string text3 in Directory.GetDirectories(path))
			{
				string text4 = text3.Replace(Path.DirectorySeparatorChar, '/').Replace(A_2.builtInAssetsPath + "/", "").Normalize(NormalizationForm.FormC);
				A_2.assetFiles.Add(AssetFile.CreateDirectory(Path.GetFileName(text4).Normalize(NormalizationForm.FormC), text4, text4.Split(new char[]
				{
					'/'
				})));
				int assetCount = A_2.assetCount + 1;
				A_2.assetCount = assetCount;
				this.<LoadAssetFiles>g__Walk|72_0(text3, ref A_2);
			}
		}

		// Token: 0x04003BCD RID: 15309
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003BCE RID: 15310
		private const string ItemCategoryAssetTypeId = "ItemCategory";

		// Token: 0x04003BCF RID: 15311
		private const string ItemCategoriesDatasetId = "ItemCategories";

		// Token: 0x04003BD0 RID: 15312
		private const string WwiseEventIdsDatasetId = "WwiseEventIds";

		// Token: 0x04003BD1 RID: 15313
		private Dictionary<string, JObject> _unsavedJsonAssets = new Dictionary<string, JObject>();

		// Token: 0x04003BD2 RID: 15314
		private Dictionary<string, string> _unsavedTextAssets = new Dictionary<string, string>();

		// Token: 0x04003BD3 RID: 15315
		private Dictionary<string, Image> _unsavedImageAssets = new Dictionary<string, Image>();

		// Token: 0x04003BD4 RID: 15316
		private Dictionary<string, string> _fileExtensionAssetTypeMapping = new Dictionary<string, string>();

		// Token: 0x04003BD5 RID: 15317
		private Dictionary<string, List<string>> _dropdownDatasetEntriesCache = new Dictionary<string, List<string>>();

		// Token: 0x04003BD6 RID: 15318
		private List<string> _loadingDropdownDatasets = new List<string>();

		// Token: 0x04003BD7 RID: 15319
		private List<Texture> _iconTextures = new List<Texture>();

		// Token: 0x04003BD8 RID: 15320
		private bool _isInitializingOrInitialized;

		// Token: 0x04003BD9 RID: 15321
		private readonly CancellationTokenSource _backendLifetimeCancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04003BDA RID: 15322
		private readonly CancellationToken _backendLifetimeCancellationToken;

		// Token: 0x04003BDB RID: 15323
		private IDictionary<string, string> _translationMessages = new Dictionary<string, string>();

		// Token: 0x04003BDC RID: 15324
		private string _assetsDirectoryPath;

		// Token: 0x04003BDD RID: 15325
		private List<FileSystemWatcher> _fileSystemWatchers = new List<FileSystemWatcher>();

		// Token: 0x04003BDE RID: 15326
		private readonly ConcurrentQueue<FileSystemEventArgs> _fileWatcherEventQueue = new ConcurrentQueue<FileSystemEventArgs>();

		// Token: 0x04003BDF RID: 15327
		private readonly CancellationTokenSource _threadCancellationTokenSource = new CancellationTokenSource();

		// Token: 0x04003BE0 RID: 15328
		private CancellationToken _threadCancellationToken;

		// Token: 0x04003BE1 RID: 15329
		private Thread _fileWatcherHandlerThread;

		// Token: 0x04003BE2 RID: 15330
		private Dictionary<string, LocalAssetEditorBackend.AssetUndoRedoStacks> _undoRedoStacks = new Dictionary<string, LocalAssetEditorBackend.AssetUndoRedoStacks>();

		// Token: 0x02000FE9 RID: 4073
		private class AssetUndoRedoStacks
		{
			// Token: 0x04004C61 RID: 19553
			public readonly DropOutStack<ClientJsonUpdateCommand> UndoStack = new DropOutStack<ClientJsonUpdateCommand>(100);

			// Token: 0x04004C62 RID: 19554
			public readonly DropOutStack<ClientJsonUpdateCommand> RedoStack = new DropOutStack<ClientJsonUpdateCommand>(100);

			// Token: 0x04004C63 RID: 19555
			public string SaveFileHash;
		}
	}
}
