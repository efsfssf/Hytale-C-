using System;
using System.Collections.Generic;
using HytaleClient.AssetEditor.Data;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.AssetEditor.Utils;
using HytaleClient.Data;
using HytaleClient.Data.BlockyModels;
using HytaleClient.Graphics;
using HytaleClient.Graphics.BlockyModels;
using HytaleClient.Graphics.Map;
using HytaleClient.Graphics.Programs;
using HytaleClient.Interface.AssetEditor.Utils;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Math;
using HytaleClient.Protocol;
using NLog;

namespace HytaleClient.AssetEditor.Interface.Previews
{
	// Token: 0x02000B98 RID: 2968
	internal abstract class AssetPreview : RendererPreviewElement
	{
		// Token: 0x06005BAB RID: 23467 RVA: 0x001CAF20 File Offset: 0x001C9120
		private bool EnsureAssetsFetched(HashSet<string> requiredTextures, HashSet<string> requiredModels)
		{
			this.GatherRequiredAssets(requiredTextures, requiredModels);
			this._assetPaths.Clear();
			List<AssetReference> list = new List<AssetReference>();
			foreach (string relativeCommonPath in requiredTextures)
			{
				list.Add(new AssetReference("Texture", AssetPathUtils.GetAssetPathWithCommon(relativeCommonPath)));
				this._assetPaths.Add(AssetPathUtils.GetAssetPathWithCommon(relativeCommonPath));
			}
			foreach (string relativeCommonPath2 in requiredModels)
			{
				list.Add(new AssetReference("Model", AssetPathUtils.GetAssetPathWithCommon(relativeCommonPath2)));
				this._assetPaths.Add(AssetPathUtils.GetAssetPathWithCommon(relativeCommonPath2));
			}
			bool flag = false;
			foreach (AssetReference assetReference in list)
			{
				TrackedAsset trackedAsset;
				bool flag2 = !this._assetEditorOverlay.TrackedAssets.TryGetValue(assetReference.FilePath, out trackedAsset);
				if (flag2)
				{
					flag = true;
					this._assetEditorOverlay.FetchTrackedAsset(assetReference, false);
				}
				else
				{
					bool isLoading = trackedAsset.IsLoading;
					if (isLoading)
					{
						flag = true;
					}
				}
			}
			return !flag;
		}

		// Token: 0x06005BAC RID: 23468 RVA: 0x001CB0A8 File Offset: 0x001C92A8
		private void CreateTextureAtlas(HashSet<string> texturePaths, out Texture texture)
		{
			Dictionary<string, Image> dictionary = new Dictionary<string, Image>();
			foreach (string text in texturePaths)
			{
				string assetPathWithCommon = AssetPathUtils.GetAssetPathWithCommon(text);
				bool flag = dictionary.ContainsKey(assetPathWithCommon);
				if (!flag)
				{
					TrackedAsset trackedAsset;
					bool flag2 = !this._assetEditorOverlay.TrackedAssets.TryGetValue(assetPathWithCommon, out trackedAsset) || !trackedAsset.IsAvailable;
					if (!flag2)
					{
						dictionary[text] = (Image)trackedAsset.Data;
					}
				}
			}
			texture = TextureAtlasUtils.CreateTextureAtlas(dictionary, out this._textureLocations);
			this._textureSizes.Clear();
			foreach (KeyValuePair<string, Image> keyValuePair in dictionary)
			{
				this._textureSizes.Add(keyValuePair.Key, new Point(keyValuePair.Value.Width, keyValuePair.Value.Height));
			}
		}

		// Token: 0x06005BAD RID: 23469 RVA: 0x001CB1D4 File Offset: 0x001C93D4
		protected AssetPreview(AssetEditorOverlay assetEditorOverlay, Element parent) : base(assetEditorOverlay.Desktop, parent)
		{
			this._assetEditorOverlay = assetEditorOverlay;
			this.CameraType = CameraType.Camera2D;
			this.RenderScene = new Action(this.Render);
			this.EnableCameraPositionControls = false;
			this.EnableCameraOrientationControls = true;
			this.EnableCameraScaleControls = false;
		}

		// Token: 0x06005BAE RID: 23470 RVA: 0x001CB246 File Offset: 0x001C9446
		protected override void OnUnmounted()
		{
			base.OnUnmounted();
			this.DisposeRenderer();
		}

		// Token: 0x06005BAF RID: 23471 RVA: 0x001CB258 File Offset: 0x001C9458
		private void DisposeRenderer()
		{
			this._needsUpdateAfterRendererDisposal = true;
			this._model = null;
			Texture textureAtlas = this._textureAtlas;
			if (textureAtlas != null)
			{
				textureAtlas.Dispose();
			}
			this._textureAtlas = null;
			this._fallbackTexture = null;
			this._blockVertexData = null;
			this._gradientTexture = null;
			this._textureSizes.Clear();
			this._textureLocations.Clear();
			AnimatedBlockRenderer animatedBlockRenderer = this._animatedBlockRenderer;
			if (animatedBlockRenderer != null)
			{
				animatedBlockRenderer.Dispose();
			}
			this._animatedBlockRenderer = null;
			ModelRenderer modelRenderer = this._modelRenderer;
			if (modelRenderer != null)
			{
				modelRenderer.Dispose();
			}
			this._modelRenderer = null;
		}

		// Token: 0x06005BB0 RID: 23472 RVA: 0x001CB2EC File Offset: 0x001C94EC
		protected override void OnMouseButtonDown(MouseButtonEvent evt)
		{
			base.OnMouseButtonDown(evt);
			bool flag = (long)evt.Button == 1L;
			if (flag)
			{
				this._isMouseDragging = true;
				this._dragStartMouseX = this.Desktop.MousePosition.X;
				this._dragStartModelAngleY = this._targetModelAngleY;
			}
		}

		// Token: 0x06005BB1 RID: 23473 RVA: 0x001CB33C File Offset: 0x001C953C
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = (long)evt.Button == 1L;
			if (flag)
			{
				this._isMouseDragging = false;
			}
		}

		// Token: 0x06005BB2 RID: 23474 RVA: 0x001CB36C File Offset: 0x001C956C
		protected override void OnMouseMove()
		{
			base.OnMouseMove();
			bool isMouseDragging = this._isMouseDragging;
			if (isMouseDragging)
			{
				this._targetModelAngleY = this._dragStartModelAngleY + (float)(this.Desktop.MousePosition.X - this._dragStartMouseX);
				this._updateUserRotation = true;
			}
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x001CB3BC File Offset: 0x001C95BC
		protected override void Animate(float deltaTime)
		{
			base.Animate(deltaTime);
			bool flag = !this._updateUserRotation;
			if (!flag)
			{
				this._lerpModelAngleY = MathHelper.Lerp(this._lerpModelAngleY, this._targetModelAngleY, MathHelper.Min(1f, 10f * deltaTime));
				this.CameraOrientation.Y = -MathHelper.ToRadians(this._lerpModelAngleY);
				base.UpdateViewMatrices();
				bool flag2 = (double)Math.Abs(this._lerpModelAngleY - this._targetModelAngleY) < 0.1;
				if (flag2)
				{
					this._updateUserRotation = false;
				}
			}
		}

		// Token: 0x06005BB4 RID: 23476 RVA: 0x001CB454 File Offset: 0x001C9654
		private void Render()
		{
			GLFunctions gl = this.Desktop.Graphics.GL;
			bool flag = this._animatedBlockRenderer != null;
			if (flag)
			{
				MapBlockAnimatedProgram mapBlockAnimatedForwardProgram = this.Desktop.Graphics.GPUProgramStore.MapBlockAnimatedForwardProgram;
				Matrix identity = Matrix.Identity;
				gl.Enable(GL.DEPTH_TEST);
				gl.Disable(GL.BLEND);
				gl.UseProgram(mapBlockAnimatedForwardProgram);
				gl.ActiveTexture(GL.TEXTURE0);
				gl.BindTexture(GL.TEXTURE_2D, this._textureAtlas.GLTexture);
				gl.ActiveTexture(GL.TEXTURE1);
				gl.BindTexture(GL.TEXTURE_2D, this._fallbackTexture.GLTexture);
				mapBlockAnimatedForwardProgram.AssertInUse();
				gl.AssertEnabled(GL.DEPTH_TEST);
				mapBlockAnimatedForwardProgram.ViewProjectionMatrix.SetValue(ref this.ViewProjectionMatrix);
				mapBlockAnimatedForwardProgram.ModelMatrix.SetValue(ref identity);
				mapBlockAnimatedForwardProgram.NodeBlock.SetBuffer(this._animatedBlockRenderer.NodeBuffer);
				gl.BindVertexArray(this._animatedBlockRenderer.VertexArray);
				gl.DrawElements(GL.TRIANGLES, this._animatedBlockRenderer.IndicesCount, GL.UNSIGNED_INT, (IntPtr)0);
			}
			else
			{
				bool flag2 = this._modelRenderer != null;
				if (flag2)
				{
					BlockyModelProgram blockyModelForwardProgram = this.Desktop.Graphics.GPUProgramStore.BlockyModelForwardProgram;
					Matrix identity2 = Matrix.Identity;
					gl.Enable(GL.DEPTH_TEST);
					gl.Disable(GL.BLEND);
					gl.UseProgram(blockyModelForwardProgram);
					gl.ActiveTexture(GL.TEXTURE0);
					gl.BindTexture(GL.TEXTURE_2D, this._textureAtlas.GLTexture);
					gl.ActiveTexture(GL.TEXTURE1);
					gl.BindTexture(GL.TEXTURE_2D, this._fallbackTexture.GLTexture);
					gl.ActiveTexture(GL.TEXTURE3);
					gl.BindTexture(GL.TEXTURE_2D, this._gradientTexture.GLTexture);
					blockyModelForwardProgram.AssertInUse();
					gl.AssertEnabled(GL.DEPTH_TEST);
					blockyModelForwardProgram.ViewProjectionMatrix.SetValue(ref this.ViewProjectionMatrix);
					blockyModelForwardProgram.ModelMatrix.SetValue(ref identity2);
					blockyModelForwardProgram.NodeBlock.SetBuffer(this._modelRenderer.NodeBuffer);
					this._modelRenderer.Draw();
				}
			}
			gl.Disable(GL.DEPTH_TEST);
			gl.Enable(GL.BLEND);
		}

		// Token: 0x06005BB5 RID: 23477 RVA: 0x001CB6B4 File Offset: 0x001C98B4
		private void InitializeRenderer()
		{
			this._lerpModelAngleY = (this._targetModelAngleY = -MathHelper.ToDegrees(this.CameraOrientation.Y));
			bool flag = this._animatedBlockRenderer != null;
			if (flag)
			{
				this._animatedBlockRenderer.Dispose();
				this._animatedBlockRenderer = null;
			}
			bool flag2 = this._modelRenderer != null;
			if (flag2)
			{
				this._modelRenderer.Dispose();
				this._modelRenderer = null;
			}
			Point[] atlasSizes = new Point[]
			{
				new Point(this._textureAtlas.Width, this._textureAtlas.Height),
				new Point(this._fallbackTexture.Width, this._fallbackTexture.Height)
			};
			bool flag3 = this._blockVertexData != null;
			if (flag3)
			{
				this._animatedBlockRenderer = new AnimatedBlockRenderer(this._model, atlasSizes, this._blockVertexData, this.Desktop.Graphics, true);
				this._animatedBlockRenderer.UpdatePose();
				this._animatedBlockRenderer.SendDataToGPU();
			}
			else
			{
				bool flag4 = this._model != null;
				if (flag4)
				{
					this._modelRenderer = new ModelRenderer(this._model, atlasSizes, this.Desktop.Graphics, 0U, true);
					this._modelRenderer.UpdatePose();
					this._modelRenderer.SendDataToGPU();
				}
			}
			this.NeedsRendering = true;
		}

		// Token: 0x06005BB6 RID: 23478 RVA: 0x001CB810 File Offset: 0x001C9A10
		public byte[] Capture(int width, int height)
		{
			this._renderTarget.Resize(width, height, false);
			byte[] result = base.RenderIntoRgbaByteArray();
			this._renderTarget.Resize(base.AnchoredRectangle.Width, base.AnchoredRectangle.Height, false);
			return result;
		}

		// Token: 0x06005BB7 RID: 23479 RVA: 0x001CB85C File Offset: 0x001C9A5C
		public void OnTrackedAssetChanged(TrackedAsset trackedAsset)
		{
			bool flag = !this._assetPaths.Contains(trackedAsset.Reference.FilePath);
			if (!flag)
			{
				this.TrySetupRenderer();
			}
		}

		// Token: 0x06005BB8 RID: 23480 RVA: 0x001CB890 File Offset: 0x001C9A90
		public void UpdateCameraSettings(AssetEditorPreviewCameraSettings cameraSettings)
		{
			this._cameraSettings = cameraSettings;
			this.ApplyCameraSettings();
			base.UpdateViewMatrices();
		}

		// Token: 0x06005BB9 RID: 23481 RVA: 0x001CB8A8 File Offset: 0x001C9AA8
		private void ApplyCameraSettings()
		{
			this.CameraScale = this._cameraSettings.ModelScale / 32f;
			this.CameraPosition = new Vector3(this._cameraSettings.CameraPosition.X, this._cameraSettings.CameraPosition.Y, this._cameraSettings.CameraPosition.Z);
			this.CameraOrientation = new Vector3(this._cameraSettings.CameraOrientation.X, this._cameraSettings.CameraOrientation.Y, this._cameraSettings.CameraOrientation.Z);
		}

		// Token: 0x06005BBA RID: 23482 RVA: 0x001CB944 File Offset: 0x001C9B44
		protected void TrySetupRenderer()
		{
			this.DisposeRenderer();
			try
			{
				this.SetupRenderer();
			}
			catch (Exception exception)
			{
				AssetPreview.Logger.Error(exception, "Failed to set up preview.");
				this.DisposeRenderer();
			}
			base.Layout(null, true);
			this._needsUpdateAfterRendererDisposal = false;
		}

		// Token: 0x06005BBB RID: 23483 RVA: 0x001CB9AC File Offset: 0x001C9BAC
		private void SetupRenderer()
		{
			bool flag = !this.IsAssetValid();
			if (!flag)
			{
				HashSet<string> hashSet = new HashSet<string>();
				HashSet<string> requiredModels = new HashSet<string>();
				bool flag2 = !this.EnsureAssetsFetched(hashSet, requiredModels);
				if (!flag2)
				{
					bool flag3 = !this.AreMinimumRequiredAssetsAvailable();
					if (!flag3)
					{
						this.CreateTextureAtlas(hashSet, out this._textureAtlas);
						this.SetupModelData();
						this._fallbackTexture = this.Desktop.Graphics.WhitePixelTexture;
						this._gradientTexture = this._assetEditorOverlay.Interface.App.CharacterPartStore.CharacterGradientAtlas;
						this.ApplyCameraSettings();
						this.InitializeRenderer();
					}
				}
			}
		}

		// Token: 0x06005BBC RID: 23484 RVA: 0x001CBA53 File Offset: 0x001C9C53
		protected virtual bool AreMinimumRequiredAssetsAvailable()
		{
			return true;
		}

		// Token: 0x06005BBD RID: 23485 RVA: 0x001CBA56 File Offset: 0x001C9C56
		protected virtual bool IsAssetValid()
		{
			return true;
		}

		// Token: 0x06005BBE RID: 23486
		protected abstract void SetupModelData();

		// Token: 0x06005BBF RID: 23487
		protected abstract void GatherRequiredAssets(HashSet<string> requiredTextures, HashSet<string> requiredModels);

		// Token: 0x0400394F RID: 14671
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003950 RID: 14672
		protected readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x04003951 RID: 14673
		private AnimatedBlockRenderer _animatedBlockRenderer;

		// Token: 0x04003952 RID: 14674
		private ModelRenderer _modelRenderer;

		// Token: 0x04003953 RID: 14675
		private float _lerpModelAngleY;

		// Token: 0x04003954 RID: 14676
		private float _dragStartModelAngleY;

		// Token: 0x04003955 RID: 14677
		private float _targetModelAngleY;

		// Token: 0x04003956 RID: 14678
		private bool _isMouseDragging;

		// Token: 0x04003957 RID: 14679
		private int _dragStartMouseX;

		// Token: 0x04003958 RID: 14680
		private bool _updateUserRotation;

		// Token: 0x04003959 RID: 14681
		protected Texture _textureAtlas;

		// Token: 0x0400395A RID: 14682
		protected Texture _fallbackTexture;

		// Token: 0x0400395B RID: 14683
		protected Texture _gradientTexture;

		// Token: 0x0400395C RID: 14684
		protected BlockyModel _model;

		// Token: 0x0400395D RID: 14685
		protected ChunkGeometryData _blockVertexData;

		// Token: 0x0400395E RID: 14686
		protected AssetEditorPreviewCameraSettings _cameraSettings;

		// Token: 0x0400395F RID: 14687
		protected Dictionary<string, Point> _textureLocations = new Dictionary<string, Point>();

		// Token: 0x04003960 RID: 14688
		protected Dictionary<string, Point> _textureSizes = new Dictionary<string, Point>();

		// Token: 0x04003961 RID: 14689
		private readonly List<string> _assetPaths = new List<string>();

		// Token: 0x04003962 RID: 14690
		protected bool _needsUpdateAfterRendererDisposal;
	}
}
