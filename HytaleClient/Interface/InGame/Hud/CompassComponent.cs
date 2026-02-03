using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Graphics;
using HytaleClient.Graphics.Fonts;
using HytaleClient.InGame.Modules.Camera.Controllers;
using HytaleClient.InGame.Modules.WorldMap;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008B6 RID: 2230
	internal class CompassComponent : InterfaceComponent
	{
		// Token: 0x060040A4 RID: 16548 RVA: 0x000BB6A8 File Offset: 0x000B98A8
		public CompassComponent(InGameView view) : base(view.Interface, view.HudContainer)
		{
			this.InGameView = view;
			this.Anchor = new Anchor
			{
				Top = new int?(10),
				Height = new int?(21),
				Width = new int?(500)
			};
			this.Background = new PatchStyle("InGame/Hud/CompassBackground.png");
		}

		// Token: 0x060040A5 RID: 16549 RVA: 0x000BB780 File Offset: 0x000B9980
		protected override void ApplyStyles()
		{
			base.ApplyStyles();
			this._pointFont = this.Desktop.Provider.GetFontFamily("Default").RegularFont;
			this._compassMaskTextureArea = this.Desktop.Provider.MakeTextureArea("InGame/Hud/CompassMask.png");
		}

		// Token: 0x060040A6 RID: 16550 RVA: 0x000BB7D0 File Offset: 0x000B99D0
		protected override void LayoutSelf()
		{
			this._maskOffset = this.Desktop.ScaleRound(4f);
			this._maskRectangle = new Rectangle(this._anchoredRectangle.X, this._anchoredRectangle.Y - this._maskOffset, this._anchoredRectangle.Width, this._anchoredRectangle.Height + this._maskOffset);
			this._pointStagger = (int)((float)this._rectangleAfterPadding.Width * 0.4f);
			this._effectiveWidth = this._pointStagger * 4;
			this._halfEffectiveWidth = this._effectiveWidth / 2;
			this._halfVisibleWidth = this._rectangleAfterPadding.Width / 2;
		}

		// Token: 0x060040A7 RID: 16551 RVA: 0x000BB884 File Offset: 0x000B9A84
		protected override void PrepareForDrawSelf()
		{
			base.PrepareForDrawSelf();
			bool flag = !this.InGameView.InGame.Instance.IsPlaying;
			if (!flag)
			{
				this.Desktop.Batcher2D.PushMask(this._compassMaskTextureArea, this._maskRectangle, this.Desktop.ViewportRectangle);
				float num = 14f * this.Desktop.Scale;
				ICameraController controller = this.InGameView.InGame.Instance.CameraModule.Controller;
				Vector3 position = controller.Position;
				float yaw = controller.Rotation.Yaw;
				float num2 = (yaw + 3.1415927f) / 6.2831855f * (float)this._effectiveWidth - (float)((int)num / 2);
				int num3 = -(this._pointStagger * 2);
				foreach (string text in this._points)
				{
					float x = (float)(this._rectangleAfterPadding.X + num3) + num2 - (float)this._effectiveWidth + (float)this._halfVisibleWidth;
					this.Desktop.Batcher2D.RequestDrawText(this._pointFont, num, text, new Vector3(x, (float)(this._maskRectangle.Y + 1), 0f), UInt32Color.White, true, false, 0f);
					num3 += this._pointStagger;
				}
				int num4 = 0;
				SortedList<int, CompassComponent.DrawableCompassMarker> sortedList = new SortedList<int, CompassComponent.DrawableCompassMarker>();
				foreach (CompassComponent.CompassMarker compassMarker in this._markers.Values)
				{
					Vector3 vector = new Vector3(compassMarker.Position.X, compassMarker.Position.Y, compassMarker.Position.Z);
					float num5 = Vector3.Distance(position, vector);
					bool flag2 = num5 > 3000f;
					if (!flag2)
					{
						float num6 = compassMarker.Position.X - position.X;
						float num7 = compassMarker.Position.Z - position.Z;
						float num8 = (float)Math.Atan2((double)(-(double)num6), (double)(-(double)num7));
						float num9 = (yaw - num8) / 3.1415927f * (float)this._halfEffectiveWidth;
						bool flag3 = num9 < (float)(-(float)this._halfEffectiveWidth);
						if (flag3)
						{
							num9 += (float)this._effectiveWidth;
						}
						else
						{
							bool flag4 = num9 > (float)this._halfEffectiveWidth;
							if (flag4)
							{
								num9 -= (float)this._effectiveWidth;
							}
						}
						num9 += (float)this._halfVisibleWidth;
						float num10 = CompassComponent.NormalizeToScale(0f, 3000f, (float)compassMarker.Icon.TextureArea.Rectangle.Width * this.Desktop.Scale, 0f, num5);
						float num11 = CompassComponent.NormalizeToScale(0f, 3000f, (float)compassMarker.Icon.TextureArea.Rectangle.Height * this.Desktop.Scale, 0f, num5);
						float num12 = ((float)this._anchoredRectangle.Height - num11) / 2f;
						num9 -= num10 / 2f;
						int num13 = -(int)(num5 * 1000000f) + num4;
						CompassComponent.DrawableCompassMarker drawableCompassMarker = new CompassComponent.DrawableCompassMarker
						{
							Icon = compassMarker.Icon,
							Position = new Vector3((float)this._rectangleAfterPadding.X + num9, (float)this._maskRectangle.Y + num12, 0f),
							Width = num10,
							Height = num11
						};
						sortedList.Add(num13, drawableCompassMarker);
						num4++;
					}
				}
				foreach (CompassComponent.DrawableCompassMarker drawableCompassMarker2 in sortedList.Values)
				{
					this.Desktop.Batcher2D.RequestDrawPatch(drawableCompassMarker2.Icon, drawableCompassMarker2.Position, drawableCompassMarker2.Width, drawableCompassMarker2.Height, this.Desktop.Scale);
				}
				this.Desktop.Batcher2D.PopMask();
			}
		}

		// Token: 0x060040A8 RID: 16552 RVA: 0x000BBCD0 File Offset: 0x000B9ED0
		protected override void OnUnmounted()
		{
			this._markers.Clear();
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x000BBCDF File Offset: 0x000B9EDF
		public void ResetState()
		{
			this._markers.Clear();
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x000BBCF0 File Offset: 0x000B9EF0
		private static float NormalizeToScale(float oldScaleMin, float oldScaleMax, float newScaleMin, float newScaleMax, float value)
		{
			return newScaleMin + (newScaleMax - newScaleMin) * ((value - oldScaleMin) / (oldScaleMax - oldScaleMin));
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x000BBD10 File Offset: 0x000B9F10
		public void OnWorldMapMarkerAdded(WorldMapModule.MapMarker marker)
		{
			this.AddMarker(marker, false);
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x000BBD1B File Offset: 0x000B9F1B
		public void OnWorldMapMarkerUpdated(WorldMapModule.MapMarker marker)
		{
			this.UpdateMarker(marker);
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x000BBD28 File Offset: 0x000B9F28
		public void OnWorldMapMarkerRemoved(WorldMapModule.MapMarker marker)
		{
			bool flag = marker == null;
			if (!flag)
			{
				this.RemoveMarker(marker.Id);
			}
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x000BBD4D File Offset: 0x000B9F4D
		private void OnContextMarkerSelected(WorldMapModule.MarkerSelection marker)
		{
			this.SelectContextMarker(marker);
		}

		// Token: 0x060040AF RID: 16559 RVA: 0x000BBD57 File Offset: 0x000B9F57
		private void OnContextMarkerDeselected()
		{
			this.DeselectContextMarker();
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x000BBD60 File Offset: 0x000B9F60
		private void AddMarker(WorldMapModule.MapMarker marker, bool isSelected = false)
		{
			bool flag = this._markers.ContainsKey(marker.Id);
			if (flag)
			{
				Trace.WriteLine("Tried to load a marker with ID " + marker.Id + " but it already exists.", "CompassComponent");
			}
			else
			{
				CompassComponent.CompassMarker compassMarker = new CompassComponent.CompassMarker
				{
					Id = marker.Id,
					MarkerImage = marker.MarkerImage,
					Position = new Vector3(marker.X, marker.Y, marker.Z),
					Icon = this.CreateMarkerTexturePatch(marker.MarkerImage)
				};
				if (isSelected)
				{
					compassMarker.Icon.Color = CompassComponent.MarkerSelectionColor;
				}
				this._markers.Add(marker.Id, compassMarker);
			}
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x000BBE1B File Offset: 0x000BA01B
		private void UpdateMarker(WorldMapModule.MapMarker marker)
		{
			this.RemoveMarker(marker.Id);
			this.AddMarker(marker, false);
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x000BBE34 File Offset: 0x000BA034
		private void RemoveMarker(string markerId)
		{
			this._markers.Remove(markerId);
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x000BBE44 File Offset: 0x000BA044
		private TexturePatch CreateMarkerTexturePatch(string markerImage)
		{
			TextureArea missingTexture;
			this.InGameView.TryMountAssetTexture("UI/WorldMap/MapMarkers/" + markerImage, out missingTexture);
			bool flag = missingTexture == null;
			if (flag)
			{
				missingTexture = this.Desktop.Provider.MissingTexture;
			}
			return this.Desktop.MakeTexturePatch(new PatchStyle(missingTexture));
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x000BBE9C File Offset: 0x000BA09C
		public void SelectContextMarker(WorldMapModule.MarkerSelection marker)
		{
			this.DeselectContextMarker();
			bool flag = marker.MapMarker != null && this._markers.ContainsKey(marker.MapMarker.Id);
			if (flag)
			{
				this._markers[marker.MapMarker.Id].Icon.Color = CompassComponent.MarkerSelectionColor;
			}
			else
			{
				bool flag2 = marker.Type == WorldMapModule.MarkerSelectionType.Coordinates;
				if (flag2)
				{
					this.AddMarker(new WorldMapModule.MapMarker
					{
						Id = "Coordinates",
						MarkerImage = "Coordinate.png",
						X = (float)marker.Coordinates.X,
						Y = 0f,
						Z = (float)marker.Coordinates.Y
					}, true);
				}
			}
			this._selectedMarker = marker;
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x000BBF68 File Offset: 0x000BA168
		public void DeselectContextMarker()
		{
			bool flag = this._selectedMarker.MapMarker != null && this._markers.ContainsKey(this._selectedMarker.MapMarker.Id);
			if (flag)
			{
				this._markers[this._selectedMarker.MapMarker.Id].Icon.Color = CompassComponent.MarkerDefaultColor;
			}
			else
			{
				bool flag2 = this._selectedMarker.Type == WorldMapModule.MarkerSelectionType.Coordinates && this._markers.ContainsKey("Coordinates");
				if (flag2)
				{
					this.RemoveMarker("Coordinates");
				}
			}
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x000BC008 File Offset: 0x000BA208
		public void OnAssetsUpdated()
		{
			foreach (CompassComponent.CompassMarker compassMarker in this._markers.Values)
			{
				compassMarker.Icon = this.CreateMarkerTexturePatch(compassMarker.MarkerImage);
			}
		}

		// Token: 0x04001EF8 RID: 7928
		public readonly InGameView InGameView;

		// Token: 0x04001EF9 RID: 7929
		private readonly string[] _points = new string[]
		{
			"N",
			"E",
			"S",
			"W",
			"N",
			"E",
			"S",
			"W",
			"N"
		};

		// Token: 0x04001EFA RID: 7930
		private static readonly UInt32Color MarkerDefaultColor = UInt32Color.White;

		// Token: 0x04001EFB RID: 7931
		private static readonly UInt32Color MarkerSelectionColor = UInt32Color.FromRGBA(242, 206, 5, byte.MaxValue);

		// Token: 0x04001EFC RID: 7932
		private TextureArea _compassMaskTextureArea;

		// Token: 0x04001EFD RID: 7933
		private int _maskOffset;

		// Token: 0x04001EFE RID: 7934
		private Rectangle _maskRectangle;

		// Token: 0x04001EFF RID: 7935
		private Font _pointFont;

		// Token: 0x04001F00 RID: 7936
		private int _pointStagger;

		// Token: 0x04001F01 RID: 7937
		private int _effectiveWidth;

		// Token: 0x04001F02 RID: 7938
		private int _halfEffectiveWidth;

		// Token: 0x04001F03 RID: 7939
		private int _halfVisibleWidth;

		// Token: 0x04001F04 RID: 7940
		private const float Scale = 0.4f;

		// Token: 0x04001F05 RID: 7941
		private const int MarkerScalingMaxDistance = 3000;

		// Token: 0x04001F06 RID: 7942
		private readonly Dictionary<string, CompassComponent.CompassMarker> _markers = new Dictionary<string, CompassComponent.CompassMarker>();

		// Token: 0x04001F07 RID: 7943
		private WorldMapModule.MarkerSelection _selectedMarker;

		// Token: 0x02000D7D RID: 3453
		private class CompassMarker
		{
			// Token: 0x0400421B RID: 16923
			public string Id;

			// Token: 0x0400421C RID: 16924
			public string MarkerImage;

			// Token: 0x0400421D RID: 16925
			public Vector3 Position;

			// Token: 0x0400421E RID: 16926
			public TexturePatch Icon;
		}

		// Token: 0x02000D7E RID: 3454
		private class DrawableCompassMarker
		{
			// Token: 0x0400421F RID: 16927
			public TexturePatch Icon;

			// Token: 0x04004220 RID: 16928
			public Vector3 Position;

			// Token: 0x04004221 RID: 16929
			public float Width;

			// Token: 0x04004222 RID: 16930
			public float Height;
		}
	}
}
