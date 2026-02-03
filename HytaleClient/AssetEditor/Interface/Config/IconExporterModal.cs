using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Interface.Elements;
using HytaleClient.AssetEditor.Interface.Previews;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Data;
using HytaleClient.Data.Items;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using HytaleClient.Protocol;
using HytaleClient.Utils;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.AssetEditor.Interface.Config
{
	// Token: 0x02000BC2 RID: 3010
	internal class IconExporterModal : Element
	{
		// Token: 0x06005E68 RID: 24168 RVA: 0x001E2EF7 File Offset: 0x001E10F7
		public IconExporterModal(ConfigEditor configEditor) : base(configEditor.Desktop, null)
		{
			this._configEditor = configEditor;
		}

		// Token: 0x06005E69 RID: 24169 RVA: 0x001E2F1C File Offset: 0x001E111C
		public void Build()
		{
			base.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/IconExporterModal.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._container = uifragment.Get<Group>("Container");
			this._previewFrame = uifragment.Get<Group>("PreviewFrame");
			this._previewArea = uifragment.Get<Group>("PreviewArea");
			this._filePathField = uifragment.Get<TextField>("FilePathField");
			uifragment.Get<TextButton>("CancelButton").Activating = new Action(this.Dismiss);
			uifragment.Get<TextButton>("SaveButton").Activating = new Action(this.Export);
			uifragment.Get<TextButton>("ResetButton").Activating = new Action(this.Reset);
			this._copyFromDropdown = new AssetSelectorDropdown(this.Desktop, uifragment.Get<Group>("CopyProperties"), this._configEditor.AssetEditorOverlay)
			{
				AssetType = "Item",
				Style = this._configEditor.FileDropdownBoxStyle,
				FlexWeight = 1,
				ValueChanged = delegate()
				{
					CancellationToken token = this._cancellationTokenSource.Token;
					AssetEditorOverlay assetEditorOverlay = this._configEditor.AssetEditorOverlay;
					string filePath;
					assetEditorOverlay.Assets.TryGetPathForAssetId("Item", this._copyFromDropdown.Value, out filePath, false);
					this.LoadCopyFromAsset(new AssetReference("Item", filePath), token);
				}
			};
			this._modelPreview = new ModelPreview(this._configEditor.AssetEditorOverlay, uifragment.Get<Group>("PreviewArea"));
			this._modelPreview.Visible = false;
			this._blockPreview = new BlockPreview(this._configEditor.AssetEditorOverlay, uifragment.Get<Group>("PreviewArea"));
			this._blockPreview.Visible = false;
			this._rotationXGroup = uifragment.Get<Group>("RotationX");
			this._rotationYGroup = uifragment.Get<Group>("RotationY");
			this._rotationZGroup = uifragment.Get<Group>("RotationZ");
			this._translationXGroup = uifragment.Get<Group>("TranslationX");
			this._translationYGroup = uifragment.Get<Group>("TranslationY");
			this._scaleGroup = uifragment.Get<Group>("Scale");
			this._previewZoomGroup = uifragment.Get<Group>("PreviewZoom");
			this._previewZoomGroup.Find<NumberField>("NumberField").Format.Suffix = "%";
			this.UpdatePreviewZoom(3f, false);
			IconExporterModal.<Build>g__SetupInput|21_0(this._previewZoomGroup, delegate(decimal value)
			{
				this.UpdatePreviewZoom((float)(value / 100m), true);
			});
			IconExporterModal.<Build>g__SetupInput|21_0(this._scaleGroup, delegate(decimal value)
			{
				AssetPreview activePreview = this.GetActivePreview();
				activePreview.CameraScale = (float)value * this._itemScale / 32f;
				activePreview.UpdateViewMatrices();
			});
			IconExporterModal.<Build>g__SetupInput|21_0(this._rotationXGroup, delegate(decimal value)
			{
				AssetPreview activePreview = this.GetActivePreview();
				activePreview.CameraOrientation.Pitch = -MathHelper.ToRadians((float)value);
				activePreview.UpdateViewMatrices();
			});
			IconExporterModal.<Build>g__SetupInput|21_0(this._rotationYGroup, delegate(decimal value)
			{
				AssetPreview activePreview = this.GetActivePreview();
				activePreview.CameraOrientation.Yaw = -MathHelper.ToRadians((float)value);
				activePreview.UpdateViewMatrices();
			});
			IconExporterModal.<Build>g__SetupInput|21_0(this._rotationZGroup, delegate(decimal value)
			{
				AssetPreview activePreview = this.GetActivePreview();
				activePreview.CameraOrientation.Roll = -MathHelper.ToRadians((float)value);
				activePreview.UpdateViewMatrices();
			});
			IconExporterModal.<Build>g__SetupInput|21_0(this._translationXGroup, delegate(decimal value)
			{
				AssetPreview activePreview = this.GetActivePreview();
				activePreview.CameraPosition.X = -(float)value;
				activePreview.UpdateViewMatrices();
			});
			IconExporterModal.<Build>g__SetupInput|21_0(this._translationYGroup, delegate(decimal value)
			{
				AssetPreview activePreview = this.GetActivePreview();
				activePreview.CameraPosition.Y = -(float)value;
				activePreview.UpdateViewMatrices();
			});
		}

		// Token: 0x06005E6A RID: 24170 RVA: 0x001E31F5 File Offset: 0x001E13F5
		protected override void OnMounted()
		{
			this._cancellationTokenSource = new CancellationTokenSource();
		}

		// Token: 0x06005E6B RID: 24171 RVA: 0x001E3203 File Offset: 0x001E1403
		protected override void OnUnmounted()
		{
			this._cancellationTokenSource.Cancel();
			this._modelPreview.Visible = false;
			this._blockPreview.Visible = false;
		}

		// Token: 0x06005E6C RID: 24172 RVA: 0x001E322C File Offset: 0x001E142C
		private AssetPreview GetActivePreview()
		{
			bool visible = this._modelPreview.Visible;
			AssetPreview result;
			if (visible)
			{
				result = this._modelPreview;
			}
			else
			{
				bool visible2 = this._blockPreview.Visible;
				if (visible2)
				{
					result = this._blockPreview;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06005E6D RID: 24173 RVA: 0x001E3270 File Offset: 0x001E1470
		private void LoadCopyFromAsset(AssetReference assetReference, CancellationToken cancellationToken)
		{
			this._configEditor.AssetEditorOverlay.Backend.FetchAsset(assetReference, delegate(object asset, FormattedMessage error)
			{
				bool flag = asset == null || cancellationToken.IsCancellationRequested;
				if (!flag)
				{
					this._configEditor.AssetEditorOverlay.Backend.FetchJsonAssetWithParents(assetReference, delegate(Dictionary<string, TrackedAsset> assets, FormattedMessage fetchParentsError)
					{
						bool flag2 = assets == null || cancellationToken.IsCancellationRequested;
						if (!flag2)
						{
							SchemaNode schema = this._configEditor.AssetEditorOverlay.AssetTypeRegistry.AssetTypes[assetReference.Type].Schema;
							JObject jobject = (JObject)((JObject)asset).DeepClone();
							this._configEditor.AssetEditorOverlay.ApplyAssetInheritance(schema, jobject, assets, schema);
							ClientItemIconProperties clientItemIconProperties;
							bool flag3 = !ItemPreviewUtils.TryGetIconProperties(jobject, out clientItemIconProperties);
							if (!flag3)
							{
								this.SetupInputs(clientItemIconProperties.Rotation.Value, clientItemIconProperties.Translation.Value, clientItemIconProperties.Scale);
								this.Layout(null, true);
							}
						}
					}, false);
				}
			}, false);
		}

		// Token: 0x06005E6E RID: 24174 RVA: 0x001E32C4 File Offset: 0x001E14C4
		private void UpdatePreviewZoom(float zoom, bool doLayout = true)
		{
			this._previewFrame.Anchor.Width = new int?((int)(74f * zoom));
			this._previewFrame.Anchor.Height = new int?((int)(74f * zoom));
			this._previewArea.Anchor.Width = new int?((int)(64f * zoom));
			this._previewArea.Anchor.Height = new int?((int)(64f * zoom));
			if (doLayout)
			{
				this._previewFrame.Parent.Layout(null, true);
			}
		}

		// Token: 0x06005E6F RID: 24175 RVA: 0x001E3368 File Offset: 0x001E1568
		public void OnTrackedAssetsChanged(TrackedAsset trackedAsset)
		{
			bool isMounted = this._modelPreview.IsMounted;
			if (isMounted)
			{
				this._modelPreview.OnTrackedAssetChanged(trackedAsset);
			}
			bool isMounted2 = this._blockPreview.IsMounted;
			if (isMounted2)
			{
				this._blockPreview.OnTrackedAssetChanged(trackedAsset);
			}
		}

		// Token: 0x06005E70 RID: 24176 RVA: 0x001E33B0 File Offset: 0x001E15B0
		public void Open()
		{
			AssetEditorOverlay assetEditorOverlay = this._configEditor.AssetEditorOverlay;
			SchemaNode schema = assetEditorOverlay.AssetTypeRegistry.AssetTypes[assetEditorOverlay.CurrentAsset.Type].Schema;
			JObject jobject = (JObject)this._configEditor.Value.DeepClone();
			assetEditorOverlay.ApplyAssetInheritance(schema, jobject, assetEditorOverlay.TrackedAssets, schema);
			string assetIdFromReference = assetEditorOverlay.GetAssetIdFromReference(assetEditorOverlay.CurrentAsset);
			AssetEditorAppEditor editor = this._configEditor.AssetEditorOverlay.Interface.App.Editor;
			ClientItemIconProperties clientItemIconProperties;
			bool flag = ItemPreviewUtils.TryGetIconProperties(jobject, out clientItemIconProperties);
			if (flag)
			{
				bool flag2 = editor.BlockPreview != null;
				if (flag2)
				{
					this._modelPreview.Visible = false;
					this._blockPreview.Visible = true;
					this._blockPreview.Setup(editor.BlockPreview, editor.PreviewCameraSettings);
					this._filePathField.Value = "Icons/ItemsGenerated/" + assetIdFromReference + ".png";
					this.SetupInputs(clientItemIconProperties.Rotation.Value, clientItemIconProperties.Translation.Value, clientItemIconProperties.Scale);
					this.Desktop.SetLayer(4, this);
					return;
				}
				bool flag3 = editor.ModelPreview != null;
				if (flag3)
				{
					this._blockPreview.Visible = false;
					this._modelPreview.Visible = true;
					this._modelPreview.Setup(editor.ModelPreview, editor.PreviewCameraSettings);
					this._filePathField.Value = "Icons/ItemsGenerated/" + assetIdFromReference + ".png";
					this.SetupInputs(clientItemIconProperties.Rotation.Value, clientItemIconProperties.Translation.Value, clientItemIconProperties.Scale);
					this.Desktop.SetLayer(4, this);
					return;
				}
			}
			this._modelPreview.Visible = false;
			this._blockPreview.Visible = false;
			this._configEditor.AssetEditorOverlay.ToastNotifications.AddNotification(2, this.Desktop.Provider.GetText("ui.assetEditor.iconExporterModal.errors.invalidConfig", null, true));
		}

		// Token: 0x06005E71 RID: 24177 RVA: 0x001E35CC File Offset: 0x001E17CC
		private void SetupInputs(Vector3 rotation, Vector2 translation, float scale)
		{
			AssetEditorPreviewCameraSettings cameraSettings = new AssetEditorPreviewCameraSettings
			{
				ModelScale = scale * this._itemScale,
				CameraPosition = new Vector3f(-translation.X, -translation.Y, 0f),
				CameraOrientation = new Vector3f(-MathHelper.ToRadians(rotation.X), -MathHelper.ToRadians(rotation.Y), -MathHelper.ToRadians(rotation.Z))
			};
			AssetPreview activePreview = this.GetActivePreview();
			if (activePreview != null)
			{
				activePreview.UpdateCameraSettings(cameraSettings);
			}
			this._rotationXGroup.Find<Slider>("Slider").Value = (int)(rotation.X * 1000f);
			this._rotationXGroup.Find<NumberField>("NumberField").Value = (decimal)rotation.X;
			this._rotationYGroup.Find<Slider>("Slider").Value = (int)(rotation.Y * 1000f);
			this._rotationYGroup.Find<NumberField>("NumberField").Value = (decimal)rotation.Y;
			this._rotationZGroup.Find<Slider>("Slider").Value = (int)(rotation.Z * 1000f);
			this._rotationZGroup.Find<NumberField>("NumberField").Value = (decimal)rotation.Z;
			this._translationXGroup.Find<Slider>("Slider").Value = (int)(translation.X * 1000f);
			this._translationXGroup.Find<NumberField>("NumberField").Value = (decimal)translation.X;
			this._translationYGroup.Find<Slider>("Slider").Value = (int)(translation.Y * 1000f);
			this._translationYGroup.Find<NumberField>("NumberField").Value = (decimal)translation.Y;
			this._scaleGroup.Find<Slider>("Slider").Value = (int)(scale * 1000f);
			this._scaleGroup.Find<NumberField>("NumberField").Value = (decimal)scale;
		}

		// Token: 0x06005E72 RID: 24178 RVA: 0x001E37DC File Offset: 0x001E19DC
		private void Reset()
		{
			AssetEditorOverlay assetEditorOverlay = this._configEditor.AssetEditorOverlay;
			SchemaNode schema = assetEditorOverlay.AssetTypeRegistry.AssetTypes[assetEditorOverlay.CurrentAsset.Type].Schema;
			JObject jobject = (JObject)((JObject)assetEditorOverlay.TrackedAssets[this._configEditor.CurrentAsset.FilePath].Data).DeepClone();
			assetEditorOverlay.ApplyAssetInheritance(schema, jobject, assetEditorOverlay.TrackedAssets, schema);
			ClientItemIconProperties clientItemIconProperties;
			bool flag = ItemPreviewUtils.TryGetDefaultIconProperties(jobject, out clientItemIconProperties);
			if (flag)
			{
				this.SetupInputs(clientItemIconProperties.Rotation.Value, clientItemIconProperties.Translation.Value, clientItemIconProperties.Scale);
				base.Layout(null, true);
			}
		}

		// Token: 0x06005E73 RID: 24179 RVA: 0x001E389C File Offset: 0x001E1A9C
		private void HandleCreateOrUpdateCallback(FormattedMessage error)
		{
			bool flag = error != null;
			if (flag)
			{
				this._configEditor.AssetEditorOverlay.ToastNotifications.AddNotification(2, error);
			}
			else
			{
				bool flag2 = this._filePathField.Value.StartsWith("Icons/ItemsGenerated/");
				if (flag2)
				{
					JObject jobject = new JObject();
					jobject.Add("Scale", this._scaleGroup.Find<NumberField>("NumberField").Value);
					string text = "Rotation";
					JArray jarray = new JArray();
					jarray.Add(this._rotationXGroup.Find<NumberField>("NumberField").Value);
					jarray.Add(this._rotationYGroup.Find<NumberField>("NumberField").Value);
					jarray.Add(this._rotationZGroup.Find<NumberField>("NumberField").Value);
					jobject.Add(text, jarray);
					string text2 = "Translation";
					JArray jarray2 = new JArray();
					jarray2.Add(this._translationXGroup.Find<NumberField>("NumberField").Value);
					jarray2.Add(this._translationYGroup.Find<NumberField>("NumberField").Value);
					jobject.Add(text2, jarray2);
					JObject jobject2 = jobject;
					JObject value = this._configEditor.Value;
					ConfigEditor configEditor = this._configEditor;
					PropertyPath path = PropertyPath.FromString("Icon");
					JToken value2 = this._filePathField.Value;
					JToken jtoken = value["Icon"];
					configEditor.OnChangeValue(path, value2, (jtoken != null) ? jtoken.DeepClone() : null, new AssetEditorRebuildCaches
					{
						ItemIcons = true
					}, true, false, true);
					ConfigEditor configEditor2 = this._configEditor;
					PropertyPath path2 = PropertyPath.FromString("IconProperties");
					JToken value3 = jobject2;
					JToken jtoken2 = value["IconProperties"];
					configEditor2.OnChangeValue(path2, value3, (jtoken2 != null) ? jtoken2.DeepClone() : null, null, true, false, false);
					this._configEditor.SubmitPendingUpdateCommands();
					this._configEditor.Layout(null, true);
				}
			}
		}

		// Token: 0x06005E74 RID: 24180 RVA: 0x001E3A90 File Offset: 0x001E1C90
		private void Export()
		{
			string fullPath = Path.GetFullPath(Path.Combine(Paths.BuiltInAssets, "Common"));
			string fullPath2 = Path.GetFullPath(Path.Combine(Paths.BuiltInAssets, "Common", this._filePathField.Value));
			bool flag = !Paths.IsSubPathOf(fullPath2, fullPath);
			if (flag)
			{
				IconExporterModal.Logger.Warn<string, string>("Path must resolve to within common assets directory: {0} in {1}", fullPath2, fullPath);
			}
			else
			{
				AssetPreview activePreview = this.GetActivePreview();
				byte[] pixels = activePreview.Capture(128, 128);
				pixels = BilinearFilter.ApplyFilter(pixels, 128, 64);
				Image data = new Image(64, 64, pixels);
				string assetPathWithCommon = AssetPathUtils.GetAssetPathWithCommon(this._filePathField.Value);
				AssetReference assetReference = new AssetReference("Texture", assetPathWithCommon);
				AssetEditorOverlay assetEditorOverlay = this._configEditor.AssetEditorOverlay;
				AssetFile assetFile;
				bool flag2 = assetEditorOverlay.Assets.TryGetFile(assetReference.FilePath, out assetFile, false);
				if (flag2)
				{
					assetEditorOverlay.Backend.UpdateAsset(assetReference, data, new Action<FormattedMessage>(this.HandleCreateOrUpdateCallback));
				}
				else
				{
					assetEditorOverlay.Backend.CreateAsset(assetReference, data, null, false, new Action<FormattedMessage>(this.HandleCreateOrUpdateCallback));
				}
			}
		}

		// Token: 0x06005E75 RID: 24181 RVA: 0x001E3BB8 File Offset: 0x001E1DB8
		protected internal override void Dismiss()
		{
			this.Desktop.ClearLayer(4);
		}

		// Token: 0x06005E76 RID: 24182 RVA: 0x001E3BC8 File Offset: 0x001E1DC8
		public override Element HitTest(Point position)
		{
			return base.HitTest(position) ?? this;
		}

		// Token: 0x06005E77 RID: 24183 RVA: 0x001E3BD8 File Offset: 0x001E1DD8
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = activate && !this._container.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this.Dismiss();
			}
		}

		// Token: 0x06005E7A RID: 24186 RVA: 0x001E3C88 File Offset: 0x001E1E88
		[CompilerGenerated]
		internal static void <Build>g__SetupInput|21_0(Group container, Action<decimal> valueChanged)
		{
			Slider slider = container.Find<Slider>("Slider");
			NumberField numberField = container.Find<NumberField>("NumberField");
			slider.ValueChanged = delegate()
			{
				numberField.Value = (decimal)((float)slider.Value / 1000f);
				valueChanged(numberField.Value);
			};
			numberField.ValueChanged = delegate()
			{
				slider.Value = (int)(numberField.Value * 1000m);
				slider.Layout(null, true);
				valueChanged(numberField.Value);
			};
		}

		// Token: 0x04003AF0 RID: 15088
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003AF1 RID: 15089
		private const int IconSizeRender = 128;

		// Token: 0x04003AF2 RID: 15090
		private const int IconSizeExport = 64;

		// Token: 0x04003AF3 RID: 15091
		private readonly ConfigEditor _configEditor;

		// Token: 0x04003AF4 RID: 15092
		private Group _container;

		// Token: 0x04003AF5 RID: 15093
		private AssetSelectorDropdown _copyFromDropdown;

		// Token: 0x04003AF6 RID: 15094
		private BlockPreview _blockPreview;

		// Token: 0x04003AF7 RID: 15095
		private ModelPreview _modelPreview;

		// Token: 0x04003AF8 RID: 15096
		private Group _rotationXGroup;

		// Token: 0x04003AF9 RID: 15097
		private Group _rotationYGroup;

		// Token: 0x04003AFA RID: 15098
		private Group _rotationZGroup;

		// Token: 0x04003AFB RID: 15099
		private Group _translationXGroup;

		// Token: 0x04003AFC RID: 15100
		private Group _translationYGroup;

		// Token: 0x04003AFD RID: 15101
		private Group _scaleGroup;

		// Token: 0x04003AFE RID: 15102
		private Group _previewZoomGroup;

		// Token: 0x04003AFF RID: 15103
		private Group _previewFrame;

		// Token: 0x04003B00 RID: 15104
		private Group _previewArea;

		// Token: 0x04003B01 RID: 15105
		private TextField _filePathField;

		// Token: 0x04003B02 RID: 15106
		private CancellationTokenSource _cancellationTokenSource;

		// Token: 0x04003B03 RID: 15107
		private float _itemScale = 1f;
	}
}
