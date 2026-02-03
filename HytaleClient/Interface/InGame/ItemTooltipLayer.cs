using System;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame
{
	// Token: 0x0200088A RID: 2186
	internal class ItemTooltipLayer : BaseTooltipLayer
	{
		// Token: 0x06003E75 RID: 15989 RVA: 0x000A7740 File Offset: 0x000A5940
		public ItemTooltipLayer(InGameView inGameView) : base(inGameView.Desktop)
		{
			this._inGameView = inGameView;
			Document document;
			inGameView.Interface.TryGetDocument("InGame/Tooltips/ItemTooltip.ui", out document);
			this._backgroundBorder = document.ResolveNamedValue<int>(this.Desktop.Provider, "TextureBorder");
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._group = uifragment.Get<Group>("Group");
			this._arrow = uifragment.Get<Group>("Arrow");
			this._header = uifragment.Get<Group>("Header");
			this._footer = uifragment.Get<Group>("Footer");
			this._description = uifragment.Get<Group>("Description");
			this._typeLabel = uifragment.Get<Label>("Type");
			this._idLabel = uifragment.Get<Label>("Id");
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x000A7820 File Offset: 0x000A5A20
		public void UpdateTooltip(Point centerPoint, ClientItemStack stack, string name = null, string description = null, string itemStackId = null)
		{
			this._centerPoint = centerPoint;
			Label label = this._header.Find<Label>("Name");
			Label label2 = this._description.Find<Label>("Label");
			Label label3 = this._header.Find<Label>("Quality");
			Label label4 = this._footer.Find<Label>("Durability");
			bool flag = stack == null;
			if (flag)
			{
				label.Text = name;
				bool flag2 = description != null;
				if (flag2)
				{
					label2.Text = description;
					this._description.Visible = true;
				}
				else
				{
					this._description.Visible = false;
				}
				bool flag3 = itemStackId != null;
				if (flag3)
				{
					this._idLabel.Text = this.Desktop.Provider.GetText("ui.items.idLabel", null, true) + " " + itemStackId;
					this._idLabel.Visible = true;
				}
				else
				{
					this._idLabel.Visible = false;
				}
				this._typeLabel.Visible = false;
				this._footer.Visible = false;
				label3.Visible = false;
				label4.Visible = false;
			}
			else
			{
				ClientItemBase clientItemBase;
				bool flag4 = this._inGameView.Items.TryGetValue(stack.Id, out clientItemBase);
				if (flag4)
				{
					ClientItemQuality clientItemQuality = this._inGameView.InGame.Instance.ServerSettings.ItemQualities[clientItemBase.QualityIndex];
					bool flag5 = clientItemQuality.ItemTooltipTexture == null || clientItemQuality.ItemTooltipArrowTexture == null;
					if (flag5)
					{
						throw new Exception(string.Format("Missing texture patches for ItemQuality index {0} tooltips.", clientItemBase.QualityIndex));
					}
					bool flag6 = clientItemBase.Utility != null && clientItemBase.Utility.Usable;
					if (flag6)
					{
						this._typeLabel.Text = this.Desktop.Provider.GetText("ui.items.utility", null, true);
						this._typeLabel.Visible = true;
					}
					else
					{
						bool consumable = clientItemBase.Consumable;
						if (consumable)
						{
							this._typeLabel.Text = this.Desktop.Provider.GetText("ui.items.consumable", null, true);
							this._typeLabel.Visible = true;
						}
						else
						{
							this._typeLabel.Visible = false;
						}
					}
					TextureArea textureArea;
					bool flag7 = this._inGameView.TryMountAssetTexture(clientItemQuality.ItemTooltipTexture, out textureArea);
					if (flag7)
					{
						this._group.Background = new PatchStyle(textureArea)
						{
							Border = this._backgroundBorder
						};
					}
					else
					{
						this._group.Background = new PatchStyle(this.Desktop.Provider.MissingTexture);
					}
					bool showArrow = this.ShowArrow;
					if (showArrow)
					{
						TextureArea textureArea2;
						bool flag8 = this._inGameView.TryMountAssetTexture(clientItemQuality.ItemTooltipArrowTexture, out textureArea2);
						if (flag8)
						{
							this._arrow.Background = new PatchStyle(textureArea2);
						}
						else
						{
							this._arrow.Background = new PatchStyle(this.Desktop.Provider.MissingTexture);
						}
						this._arrow.Visible = true;
					}
					else
					{
						this._arrow.Visible = false;
					}
					label.Text = this.Desktop.Provider.GetText("items." + stack.Id + ".name", null, true);
					label.Style.TextColor = clientItemQuality.TextColor;
					bool visibleQualityLabel = clientItemQuality.VisibleQualityLabel;
					if (visibleQualityLabel)
					{
						label3.Visible = true;
						label3.Text = this.Desktop.Provider.GetText(clientItemQuality.LocalizationKey, null, true);
						label3.Style.TextColor = clientItemQuality.TextColor;
					}
					else
					{
						label3.Visible = false;
					}
					description = this.Desktop.Provider.GetText("items." + stack.Id + ".description", null, false);
					bool flag9 = description != null;
					if (flag9)
					{
						label2.Text = description;
						this._description.Visible = true;
					}
					else
					{
						this._description.Visible = false;
					}
					bool flag10 = stack.MaxDurability > 0.0 && stack.Durability >= 0.0;
					if (flag10)
					{
						label4.Text = this.Desktop.Provider.FormatNumber((int)stack.Durability) + "/" + this.Desktop.Provider.FormatNumber((int)stack.MaxDurability);
						this._footer.Visible = true;
					}
					else
					{
						this._footer.Visible = false;
					}
					bool flag11 = this._inGameView.InGame.Instance.GameMode == 1;
					if (flag11)
					{
						this._idLabel.Text = this.Desktop.Provider.GetText("ui.items.idLabel", null, true) + " " + stack.Id;
						this._idLabel.Visible = true;
					}
					else
					{
						this._idLabel.Visible = false;
					}
				}
				else
				{
					label.Text = "Invalid Item";
					label.Style.TextColor = UInt32Color.FromRGBA(byte.MaxValue, 0, 0, byte.MaxValue);
					this._description.Visible = false;
					this._footer.Visible = false;
					this._typeLabel.Visible = false;
				}
			}
			base.Layout(null, true);
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x000A7DA0 File Offset: 0x000A5FA0
		protected override void UpdatePosition()
		{
			int num = this.ShowArrow ? 30 : 15;
			int num2 = this._group.ContainerRectangle.Width / 2;
			this.Anchor.Left = new int?(this.Desktop.UnscaleRound((float)(this._centerPoint.X - num2)));
			this.Anchor.Top = new int?(this.Desktop.UnscaleRound((float)(this._centerPoint.Y - this._group.ContainerRectangle.Height)) - num);
			int? left = this.Anchor.Left;
			int? num3 = this.Anchor.Right + this.Desktop.UnscaleRound((float)this._group.ContainerRectangle.Width);
			int? num4 = left;
			int num5 = 0;
			bool flag = num4.GetValueOrDefault() < num5 & num4 != null;
			if (flag)
			{
				this.Anchor.Left = new int?(0);
			}
			else
			{
				num4 = num3;
				num5 = this.Desktop.RootLayoutRectangle.Width;
				bool flag2 = num4.GetValueOrDefault() > num5 & num4 != null;
				if (flag2)
				{
					this.Anchor.Left = this.Anchor.Left - num3;
				}
			}
		}

		// Token: 0x04001D5E RID: 7518
		private readonly InGameView _inGameView;

		// Token: 0x04001D5F RID: 7519
		private Point _centerPoint;

		// Token: 0x04001D60 RID: 7520
		private Group _group;

		// Token: 0x04001D61 RID: 7521
		private Group _arrow;

		// Token: 0x04001D62 RID: 7522
		private Group _header;

		// Token: 0x04001D63 RID: 7523
		private Group _description;

		// Token: 0x04001D64 RID: 7524
		private Group _footer;

		// Token: 0x04001D65 RID: 7525
		private Label _idLabel;

		// Token: 0x04001D66 RID: 7526
		private Label _typeLabel;

		// Token: 0x04001D67 RID: 7527
		private int _backgroundBorder;

		// Token: 0x04001D68 RID: 7528
		public bool ShowArrow = true;
	}
}
