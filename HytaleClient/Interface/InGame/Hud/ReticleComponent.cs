using System;
using System.Collections.Generic;
using HytaleClient.Data.Items;
using HytaleClient.Graphics;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008C3 RID: 2243
	internal class ReticleComponent : InterfaceComponent
	{
		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x0600410C RID: 16652 RVA: 0x000BDE74 File Offset: 0x000BC074
		// (set) Token: 0x0600410D RID: 16653 RVA: 0x000BDE7C File Offset: 0x000BC07C
		public bool IsReticleVisible { get; private set; }

		// Token: 0x0600410E RID: 16654 RVA: 0x000BDE88 File Offset: 0x000BC088
		public ReticleComponent(InGameView view) : base(view.Interface, view.HudContainer)
		{
			this._inGameView = view;
			this.Interface.RegisterForEventFromEngine<bool>("crosshair.setVisible", new Action<bool>(this.OnSetVisible));
			this.Interface.RegisterForEventFromEngine<InteractionModule.InteractionHintData>("crosshair.setInteractionHint", new Action<InteractionModule.InteractionHintData>(this.OnSetInteractionHint));
			this.Interface.RegisterForEventFromEngine<float>("combat.setChargeProgress", new Action<float>(this.OnSetChargeProgress));
			this.Interface.RegisterForEventFromEngine<bool, float[]>("combat.setShowChargeProgress", new Action<bool, float[]>(this.OnSetShowChargeProgress));
		}

		// Token: 0x0600410F RID: 16655 RVA: 0x000BDF30 File Offset: 0x000BC130
		public void Build()
		{
			base.Clear();
			this._chargeProgressNotches.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/Reticle.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._reticleContainer = uifragment.Get<Element>("Reticle");
			this._serverEventContainer = uifragment.Get<Element>("ServerEvent");
			this._clientEventContainer = uifragment.Get<Element>("ClientEvent");
			this._interactionHintContainer = uifragment.Get<Group>("InteractionHint");
			this._chargeProgressContainer = uifragment.Get<Group>("ChargeProgressContainer");
			this._chargeProgressBar = uifragment.Get<Group>("ChargeProgressBar");
			this._localHitOpacity = document.ResolveNamedValue<float>(this.Desktop.Provider, "LocalHitOpacity");
			this.UpdateReticleImage();
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x000BDFFC File Offset: 0x000BC1FC
		public void ResetState(bool updateReticleImage)
		{
			this.IsReticleVisible = false;
			this._chargeProgressContainer.Visible = false;
			this._interactionHintContainer.Visible = false;
			base.Visible = false;
			this.ResetClientEvent();
			this.ResetServerEvent();
			this._reticleContainer.Visible = true;
			this._reticleContainer.Parent.Layout(null, true);
			if (updateReticleImage)
			{
				this.UpdateReticleImage();
			}
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x000BE078 File Offset: 0x000BC278
		public bool RemoveClientReticle(ItemReticleClientEvent eventKey)
		{
			Group group = this._clientEventContainer.Find<Group>(eventKey.ToString());
			bool flag = group == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._clientEventContainer.Remove(group);
				group.Clear();
				result = true;
			}
			return result;
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x000BE0C4 File Offset: 0x000BC2C4
		public void ResetClientEvent()
		{
			this._clientEventContainer.Clear();
			this._animateClientEvent = false;
			this._clientEventTimer = 0f;
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x000BE0E5 File Offset: 0x000BC2E5
		private void ResetServerEvent()
		{
			this._serverEventContainer.Clear();
			this._serverEventTimer = 0f;
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x000BE100 File Offset: 0x000BC300
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			this._reticleContainer.Visible = true;
			this._reticleContainer.Parent.Layout(null, true);
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x000BE14E File Offset: 0x000BC34E
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			this.ResetClientEvent();
			this.ResetServerEvent();
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x000BE178 File Offset: 0x000BC378
		private void Animate(float deltaTime)
		{
			bool flag = this._serverEventContainer.Children.Count > 0;
			if (flag)
			{
				this._serverEventTimer += deltaTime;
				bool flag2 = this._serverEventTimer >= this._serverEventDuration;
				if (flag2)
				{
					this._serverEventContainer.Clear();
				}
			}
			bool animateClientEvent = this._animateClientEvent;
			if (animateClientEvent)
			{
				this._clientEventTimer += deltaTime;
				bool flag3 = this._clientEventTimer >= 0.3f;
				if (flag3)
				{
					this._clientEventContainer.Clear();
				}
			}
			bool flag4 = this._serverEventContainer.Children.Count == 0 && this._clientEventContainer.Children.Count == 0 && !this._reticleContainer.IsMounted;
			if (flag4)
			{
				this._reticleContainer.Visible = true;
				this._reticleContainer.Parent.Layout(null, true);
			}
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x000BE26E File Offset: 0x000BC46E
		private void OnSetVisible(bool visible)
		{
			this.IsReticleVisible = visible;
			this._inGameView.UpdateReticleVisibility(true);
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x000BE288 File Offset: 0x000BC488
		private void OnSetChargeProgress(float progress)
		{
			Element chargeProgressBar = this._chargeProgressBar;
			float num = progress / 100f;
			int? width = this._chargeProgressContainer.Anchor.Width;
			chargeProgressBar.Anchor.Width = new int?((int)(num * ((width != null) ? new float?((float)width.GetValueOrDefault()) : null)).Value);
			this._chargeProgressBar.Layout(null, true);
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x000BE32C File Offset: 0x000BC52C
		private void OnSetShowChargeProgress(bool show, float[] chargeSteps)
		{
			bool flag = !show;
			if (flag)
			{
				this._chargeProgressContainer.Visible = false;
			}
			else
			{
				foreach (Group child in this._chargeProgressNotches)
				{
					this._chargeProgressContainer.Remove(child);
				}
				this._chargeProgressNotches.Clear();
				foreach (float num in chargeSteps)
				{
					bool flag2 = num == 1f || num == 0f;
					if (!flag2)
					{
						List<Group> chargeProgressNotches = this._chargeProgressNotches;
						Group group = new Group(this.Desktop, this._chargeProgressContainer);
						Anchor anchor = default(Anchor);
						anchor.Width = new int?(1);
						float num2 = num;
						int? width = this._chargeProgressContainer.Anchor.Width;
						anchor.Left = new int?((int)(num2 * ((width != null) ? new float?((float)width.GetValueOrDefault()) : null)).Value);
						group.Anchor = anchor;
						group.Background = new PatchStyle(UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, 130));
						chargeProgressNotches.Add(group);
					}
				}
				this._chargeProgressBar.Anchor.Width = new int?(0);
				this._chargeProgressContainer.Visible = true;
				this._chargeProgressContainer.Layout(new Rectangle?(this._chargeProgressContainer.Parent.RectangleAfterPadding), true);
			}
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x000BE510 File Offset: 0x000BC710
		private void OnSetInteractionHint(InteractionModule.InteractionHintData interactionHint)
		{
			bool flag = interactionHint.Target == InteractionModule.InteractionTargetType.None;
			if (flag)
			{
				this._interactionHintContainer.Visible = false;
			}
			else
			{
				string text = interactionHint.Name;
				bool flag2 = interactionHint.Target == InteractionModule.InteractionTargetType.Block;
				if (flag2)
				{
					text = this.Desktop.Provider.GetText("items." + text + ".name", null, true);
				}
				string[] array = this.Desktop.Provider.GetText(interactionHint.Hint, new Dictionary<string, string>
				{
					{
						"key",
						"KEY"
					},
					{
						"name",
						text
					}
				}, true).Split(new string[]
				{
					"[KEY]"
				}, StringSplitOptions.None);
				bool flag3 = array.Length != 2;
				if (!flag3)
				{
					((Label)this._interactionHintContainer.Children[0]).Text = array[0].Trim();
					((Label)this._interactionHintContainer.Children[1]).Text = this.Interface.App.Settings.InputBindings.BlockInteractAction.BoundInputLabel;
					((Label)this._interactionHintContainer.Children[2]).Text = array[1].Trim();
					this._interactionHintContainer.Visible = true;
					this._interactionHintContainer.Layout(new Rectangle?(this._interactionHintContainer.Parent.RectangleAfterPadding), true);
				}
			}
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x000BE68C File Offset: 0x000BC88C
		private ClientItemReticleConfig GetItemReticleConfig(string itemId = null)
		{
			bool flag = itemId == null;
			if (flag)
			{
				bool flag2 = this._inGameView == null;
				if (flag2)
				{
					return null;
				}
				ClientItemStack hotbarItem = this._inGameView.GetHotbarItem(this._activeHotbarSlot);
				bool flag3 = hotbarItem == null;
				if (flag3)
				{
					return null;
				}
				itemId = hotbarItem.Id;
			}
			ClientItemBase clientItemBase;
			return this._inGameView.Items.TryGetValue(itemId, out clientItemBase) ? this._inGameView.InGame.Instance.ServerSettings.ItemReticleConfigs[clientItemBase.ReticleIndex] : null;
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x000BE720 File Offset: 0x000BC920
		public void UpdateReticleImage()
		{
			ClientItemReticleConfig itemReticleConfig = this.GetItemReticleConfig(null);
			this._reticleContainer.Clear();
			List<string> list = new List<string>();
			bool flag = itemReticleConfig == null || itemReticleConfig.Base == null;
			if (flag)
			{
				list.Add("UI/Reticles/Default.png");
			}
			else
			{
				foreach (string item in itemReticleConfig.Base)
				{
					list.Add(item);
				}
			}
			foreach (string assetPath in list)
			{
				TextureArea missingTexture;
				bool flag2 = !this._inGameView.TryMountAssetTexture(assetPath, out missingTexture);
				if (flag2)
				{
					missingTexture = this.Desktop.Provider.MissingTexture;
				}
				Group group = new Group(this.Desktop, this._reticleContainer);
				group.Anchor = new Anchor
				{
					Width = new int?((int)((float)missingTexture.Texture.Width / (float)missingTexture.Scale)),
					Height = new int?((int)((float)missingTexture.Texture.Height / (float)missingTexture.Scale))
				};
				group.Background = new PatchStyle(missingTexture);
			}
			bool isMounted = this._reticleContainer.IsMounted;
			if (isMounted)
			{
				this._reticleContainer.Layout(null, true);
			}
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x000BE8A8 File Offset: 0x000BCAA8
		public void OnClientEvent(ItemReticleClientEvent eventKey, string itemId = null)
		{
			ClientItemReticleConfig itemReticleConfig = this.GetItemReticleConfig(itemId);
			ClientItemReticle clientItemReticle;
			bool flag = itemReticleConfig == null || itemReticleConfig.ClientEvents.Count == 0 || !itemReticleConfig.ClientEvents.TryGetValue(eventKey, out clientItemReticle);
			if (!flag)
			{
				this.ResetClientEvent();
				this._reticleContainer.Visible = !clientItemReticle.HideBase;
				this._reticleContainer.Parent.Layout(null, true);
				foreach (string assetPath in clientItemReticle.Parts)
				{
					TextureArea missingTexture;
					bool flag2 = !this._inGameView.TryMountAssetTexture(assetPath, out missingTexture);
					if (flag2)
					{
						missingTexture = this.Desktop.Provider.MissingTexture;
					}
					PatchStyle patchStyle = new PatchStyle(missingTexture);
					bool flag3 = eventKey == 0;
					if (flag3)
					{
						patchStyle.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(this._localHitOpacity * 255f));
					}
					Group group = new Group(this.Desktop, this._clientEventContainer);
					group.Anchor = this.GetClientReticleAnchor(eventKey, missingTexture);
					group.Background = patchStyle;
					group.Name = eventKey.ToString();
				}
				ItemReticleClientEvent itemReticleClientEvent = eventKey;
				ItemReticleClientEvent itemReticleClientEvent2 = itemReticleClientEvent;
				if (itemReticleClientEvent2 != null)
				{
					if (itemReticleClientEvent2 == 1)
					{
						this._animateClientEvent = false;
					}
				}
				else
				{
					this._clientEventTimer = 0f;
					this._animateClientEvent = true;
				}
				bool isMounted = this._clientEventContainer.IsMounted;
				if (isMounted)
				{
					this._clientEventContainer.Layout(null, true);
				}
			}
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x000BEA48 File Offset: 0x000BCC48
		private Anchor GetClientReticleAnchor(ItemReticleClientEvent eventKey, TextureArea textureArea)
		{
			Anchor result = new Anchor
			{
				Width = new int?((int)((float)textureArea.Texture.Width / (float)textureArea.Scale)),
				Height = new int?((int)((float)textureArea.Texture.Height / (float)textureArea.Scale))
			};
			float num = 0f;
			bool flag = this._reticleContainer.Children.Count > 0;
			if (flag)
			{
				num = (float)this._reticleContainer.Children[0].AnchoredRectangle.Width / this.Desktop.Scale;
			}
			float num2 = (float)this._clientEventContainer.AnchoredRectangle.Width / this.Desktop.Scale / 2f;
			bool flag2 = eventKey == 2;
			if (flag2)
			{
				result.Left = new int?((int)(num2 - num));
			}
			bool flag3 = eventKey == 3;
			if (flag3)
			{
				result.Right = new int?((int)(num2 - num));
			}
			return result;
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x000BEB4C File Offset: 0x000BCD4C
		public void OnServerEvent(int eventIndex)
		{
			this.ResetServerEvent();
			ClientItemReticleConfig itemReticleConfig = this.GetItemReticleConfig(null);
			ClientItemReticle clientItemReticle;
			bool flag = itemReticleConfig == null || itemReticleConfig.ServerEvents.Count == 0 || !itemReticleConfig.ServerEvents.TryGetValue(eventIndex, out clientItemReticle);
			if (!flag)
			{
				this._reticleContainer.Visible = !clientItemReticle.HideBase;
				this._reticleContainer.Parent.Layout(null, true);
				bool animateClientEvent = this._animateClientEvent;
				if (animateClientEvent)
				{
					this.ResetClientEvent();
				}
				foreach (string assetPath in clientItemReticle.Parts)
				{
					TextureArea missingTexture;
					bool flag2 = !this._inGameView.TryMountAssetTexture(assetPath, out missingTexture);
					if (flag2)
					{
						missingTexture = this.Desktop.Provider.MissingTexture;
					}
					Group group = new Group(this.Desktop, this._serverEventContainer);
					group.Anchor = new Anchor
					{
						Width = new int?((int)((float)missingTexture.Texture.Width / (float)missingTexture.Scale)),
						Height = new int?((int)((float)missingTexture.Texture.Height / (float)missingTexture.Scale))
					};
					group.Background = new PatchStyle(missingTexture);
				}
				this._serverEventDuration = clientItemReticle.Duration;
				bool isMounted = this._serverEventContainer.IsMounted;
				if (isMounted)
				{
					this._serverEventContainer.Layout(null, true);
				}
			}
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x000BECD9 File Offset: 0x000BCED9
		public void OnSetStacks()
		{
			this.UpdateReticleImage();
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x000BECE4 File Offset: 0x000BCEE4
		public void OnSetActiveSlot(int slot)
		{
			this._activeHotbarSlot = slot;
			this.ResetClientEvent();
			this.ResetServerEvent();
			this._reticleContainer.Visible = true;
			this._reticleContainer.Parent.Layout(null, true);
			this.UpdateReticleImage();
		}

		// Token: 0x04001F44 RID: 8004
		private const string DefaultReticleAssetPath = "UI/Reticles/Default.png";

		// Token: 0x04001F45 RID: 8005
		private const float ClientEventTimeout = 0.3f;

		// Token: 0x04001F46 RID: 8006
		private readonly InGameView _inGameView;

		// Token: 0x04001F47 RID: 8007
		private Element _reticleContainer;

		// Token: 0x04001F48 RID: 8008
		private Element _serverEventContainer;

		// Token: 0x04001F49 RID: 8009
		private Element _clientEventContainer;

		// Token: 0x04001F4A RID: 8010
		private Group _interactionHintContainer;

		// Token: 0x04001F4B RID: 8011
		private float _serverEventDuration;

		// Token: 0x04001F4C RID: 8012
		private float _serverEventTimer;

		// Token: 0x04001F4D RID: 8013
		private float _clientEventTimer;

		// Token: 0x04001F4E RID: 8014
		private bool _animateClientEvent;

		// Token: 0x04001F4F RID: 8015
		private float _localHitOpacity;

		// Token: 0x04001F50 RID: 8016
		private Group _chargeProgressContainer;

		// Token: 0x04001F51 RID: 8017
		private Group _chargeProgressBar;

		// Token: 0x04001F52 RID: 8018
		private List<Group> _chargeProgressNotches = new List<Group>();

		// Token: 0x04001F53 RID: 8019
		private int _activeHotbarSlot;
	}
}
