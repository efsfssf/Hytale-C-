using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Core;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;

namespace HytaleClient.Interface.InGame.Hud.Abilities
{
	// Token: 0x020008D5 RID: 2261
	internal class SignatureAbility : Element
	{
		// Token: 0x060041B6 RID: 16822 RVA: 0x000C37DD File Offset: 0x000C19DD
		public SignatureAbility(InGameView inGameView, Desktop desktop, Element parent) : base(desktop, parent)
		{
			this.InGameView = inGameView;
		}

		// Token: 0x060041B7 RID: 16823 RVA: 0x000C37FC File Offset: 0x000C19FC
		public void Build()
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/Abilities/SignatureAbility.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			document.TryResolveNamedValue<float>(this.Desktop.Provider, "BackgroundSignatureSize", out this._backgroundSignatureSize);
			document.TryResolveNamedValue<float>(this.Desktop.Provider, "BackgroundSignatureScaleSize", out this._backgroundSignatureScaleSize);
			this._signatureProgressBar = uifragment.Get<CircularProgressBar>("SignatureProgressBar");
			this._signatureShadowProgressBar = uifragment.Get<CircularProgressBar>("ShadowSignatureProgressBar");
			this._signatureIcon = uifragment.Get<Group>("SignatureIcon");
			this._signatureReadyProgressBar = uifragment.Get<Group>("SignatureReadyProgressBar");
			this._backgroundSignatureReady = uifragment.Get<Group>("BackgroundSignatureReady");
			this._backgroundSignatureNotReady = uifragment.Get<Group>("BackgroundSignatureNotReady");
			this._overlaySignatureError = uifragment.Get<Group>("OverlaySignatureError");
			this._overlaySignatureError.Visible = false;
			this._overlaySignatureError.Parent.Layout(null, true);
			this._inputBidingLabel = uifragment.Get<Label>("InputBinding");
			this._inputBidingsMouse.Add(InputBinding.GetMouseBoundInputLabel(Input.MouseButton.SDL_BUTTON_LEFT), uifragment.Get<Group>("MouseLeft"));
			this._inputBidingsMouse.Add(InputBinding.GetMouseBoundInputLabel(Input.MouseButton.SDL_BUTTON_MIDDLE), uifragment.Get<Group>("MouseMiddle"));
			this._inputBidingsMouse.Add(InputBinding.GetMouseBoundInputLabel(Input.MouseButton.SDL_BUTTON_RIGHT), uifragment.Get<Group>("MouseRight"));
			this._inputBidingContainer = uifragment.Get<Group>("InputBindingContainer");
			this.AnimateSignatureReady();
		}

		// Token: 0x060041B8 RID: 16824 RVA: 0x000C398C File Offset: 0x000C1B8C
		private void SetIconOpacity(float opacity)
		{
			this._signatureIcon.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(opacity * 255f));
			this._signatureIcon.Layout(null, true);
		}

		// Token: 0x060041B9 RID: 16825 RVA: 0x000C39DC File Offset: 0x000C1BDC
		protected override void OnMounted()
		{
			this.UpdateInputBiding();
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			InGameView inGameView = this.InGameView;
			ClientEntityStatValue clientEntityStatValue;
			if (inGameView == null)
			{
				clientEntityStatValue = null;
			}
			else
			{
				GameInstance instance = inGameView.InGame.Instance;
				if (instance == null)
				{
					clientEntityStatValue = null;
				}
				else
				{
					PlayerEntity localPlayer = instance.LocalPlayer;
					clientEntityStatValue = ((localPlayer != null) ? localPlayer.GetEntityStat(DefaultEntityStats.SignatureEnergy) : null);
				}
			}
			ClientEntityStatValue clientEntityStatValue2 = clientEntityStatValue;
			bool flag = clientEntityStatValue2 == null;
			if (!flag)
			{
				bool flag2 = clientEntityStatValue2.AsPercentage() < 1f;
				if (flag2)
				{
					this.SignatureNotReady();
				}
				else
				{
					this.SignatureReady();
				}
			}
		}

		// Token: 0x060041BA RID: 16826 RVA: 0x000C3A6B File Offset: 0x000C1C6B
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060041BB RID: 16827 RVA: 0x000C3A88 File Offset: 0x000C1C88
		protected virtual void Animate(float deltaTime)
		{
			bool flag = this._targetSignaturePercentage != this._renderSignaturePercentage;
			if (flag)
			{
				this._renderSignaturePercentage = MathHelper.Lerp(this._renderSignaturePercentage, this._targetSignaturePercentage, deltaTime * 10f);
				this.AnimateSignature();
			}
			bool flag2 = (double)this._renderSignaturePercentage > (double)this._targetSignaturePercentage * 0.98;
			if (flag2)
			{
				this._targetShadowSignaturePercentage = this._renderSignaturePercentage;
			}
			bool flag3 = this._targetShadowSignaturePercentage != this._renderShadowSignaturePercentage;
			if (flag3)
			{
				this._renderShadowSignaturePercentage = MathHelper.Lerp(this._renderShadowSignaturePercentage, this._targetShadowSignaturePercentage, deltaTime * 10f);
				this.AnimateShadowSignature();
			}
			bool flag4 = this._renderShadowSignaturePercentage > 0.99f && !this._isSignatureReady;
			if (flag4)
			{
				this.SignatureReady();
			}
			else
			{
				bool flag5 = this._renderShadowSignaturePercentage < 0.99f && this._isSignatureReady;
				if (flag5)
				{
					this.SignatureNotReady();
				}
			}
			bool flag6 = this._targetBackgroundSignatureReadyOpacity != this._renderBackgroundSignatureReadyOpacity;
			if (flag6)
			{
				this._renderBackgroundSignatureReadyOpacity = MathHelper.Lerp(this._renderBackgroundSignatureReadyOpacity, this._targetBackgroundSignatureReadyOpacity, deltaTime * 10f);
				this.AnimateSignatureReady();
			}
			bool flag7 = this._errorTargetPercentage != this._errorRenderPercentage;
			if (flag7)
			{
				this._errorRenderPercentage = MathHelper.Lerp(this._errorRenderPercentage, this._errorTargetPercentage, deltaTime * 10f);
				this.AnimateError();
			}
			this.AnimateSignatureReadyScale(deltaTime);
		}

		// Token: 0x060041BC RID: 16828 RVA: 0x000C3C0C File Offset: 0x000C1E0C
		protected void AnimateError()
		{
			this._overlaySignatureError.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(this._errorRenderPercentage * 255f));
			this._overlaySignatureError.Parent.Layout(null, true);
			float num = 0.1f;
			bool flag = this._errorAnimationCount > 4;
			if (flag)
			{
				this.HideErrorOverlay();
			}
			else
			{
				bool flag2 = this._errorRenderPercentage > 1f - num && this._errorTargetPercentage == 1f;
				if (flag2)
				{
					this._errorTargetPercentage = 0.4f;
					this._errorAnimationCount++;
				}
				bool flag3 = this._errorRenderPercentage < 0.4f + num && this._errorTargetPercentage == 0.4f;
				if (flag3)
				{
					this._errorTargetPercentage = 1f;
					this._errorAnimationCount++;
				}
			}
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x000C3D04 File Offset: 0x000C1F04
		private void AnimateSignatureReadyScale(float deltaTime)
		{
			bool animateSignatureBackgroundScaleUp = this._animateSignatureBackgroundScaleUp;
			if (animateSignatureBackgroundScaleUp)
			{
				this._animateSignatureBackgroundScaleDown = false;
				float value = (float)this._backgroundSignatureReady.Anchor.Height.GetValueOrDefault();
				this._backgroundSignatureReady.Anchor.Width = (this._backgroundSignatureReady.Anchor.Height = new int?((int)MathHelper.Lerp(value, this._backgroundSignatureScaleSize, deltaTime * 20f)));
				int? width = this._backgroundSignatureReady.Anchor.Width;
				double? num = (width != null) ? new double?((double)width.GetValueOrDefault()) : null;
				double num2 = 0.95 * (double)this._backgroundSignatureScaleSize;
				bool flag = num.GetValueOrDefault() > num2 & num != null;
				if (flag)
				{
					this._animateSignatureBackgroundScaleUp = false;
					this._animateSignatureBackgroundScaleDown = true;
				}
				this._backgroundSignatureReady.Layout(null, true);
			}
			bool animateSignatureBackgroundScaleDown = this._animateSignatureBackgroundScaleDown;
			if (animateSignatureBackgroundScaleDown)
			{
				this._animateSignatureBackgroundScaleUp = false;
				float value2 = (float)this._backgroundSignatureReady.Anchor.Height.GetValueOrDefault();
				this._backgroundSignatureReady.Anchor.Width = (this._backgroundSignatureReady.Anchor.Height = new int?((int)MathHelper.Lerp(value2, this._backgroundSignatureSize, deltaTime * 20f)));
				int? width = this._backgroundSignatureReady.Anchor.Width;
				double? num = (width != null) ? new double?((double)width.GetValueOrDefault()) : null;
				double num2 = 1.01 * (double)this._backgroundSignatureSize;
				bool flag2 = num.GetValueOrDefault() < num2 & num != null;
				if (flag2)
				{
					this._backgroundSignatureReady.Anchor.Width = (this._backgroundSignatureReady.Anchor.Height = new int?((int)this._backgroundSignatureSize));
					this._animateSignatureBackgroundScaleDown = false;
				}
				this._backgroundSignatureReady.Layout(null, true);
			}
		}

		// Token: 0x060041BE RID: 16830 RVA: 0x000C3F24 File Offset: 0x000C2124
		private void AnimateSignatureReady()
		{
			this._backgroundSignatureReady.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(this._renderBackgroundSignatureReadyOpacity * 255f));
			this._backgroundSignatureNotReady.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)((1f - this._renderBackgroundSignatureReadyOpacity) * 255f));
			this._backgroundSignatureNotReady.Layout(null, true);
			this._backgroundSignatureReady.Layout(null, true);
		}

		// Token: 0x060041BF RID: 16831 RVA: 0x000C3FC8 File Offset: 0x000C21C8
		private void SignatureReady()
		{
			this._isSignatureReady = true;
			this.SetIconOpacity(1f);
			this._signatureReadyProgressBar.Visible = true;
			this._signatureReadyProgressBar.Parent.Layout(null, true);
			this._targetBackgroundSignatureReadyOpacity = 1f;
			this._animateSignatureBackgroundScaleUp = true;
			this.SetInputBidingsOpacity(1f);
		}

		// Token: 0x060041C0 RID: 16832 RVA: 0x000C4030 File Offset: 0x000C2230
		private void SignatureNotReady()
		{
			this._isSignatureReady = false;
			this.SetIconOpacity(0.4f);
			this._signatureReadyProgressBar.Visible = false;
			this._signatureReadyProgressBar.Parent.Layout(null, true);
			this._targetBackgroundSignatureReadyOpacity = 0f;
			this.SetInputBidingsOpacity(0.4f);
		}

		// Token: 0x060041C1 RID: 16833 RVA: 0x000C4090 File Offset: 0x000C2290
		private void SetInputBidingsOpacity(float opacity)
		{
			this._inputBidingLabel.Style.TextColor.SetA((byte)(opacity * 255f));
			foreach (string key in this._inputBidingsMouse.Keys)
			{
				this._inputBidingsMouse[key].Background.Color.SetA((byte)(opacity * 255f));
			}
			this._inputBidingContainer.Layout(null, true);
		}

		// Token: 0x060041C2 RID: 16834 RVA: 0x000C4140 File Offset: 0x000C2340
		protected void AnimateSignature()
		{
			this._signatureProgressBar.Value = this._renderSignaturePercentage;
			this._signatureProgressBar.Layout(null, true);
		}

		// Token: 0x060041C3 RID: 16835 RVA: 0x000C4178 File Offset: 0x000C2378
		protected void AnimateShadowSignature()
		{
			this._signatureShadowProgressBar.Value = this._renderShadowSignaturePercentage;
			this._signatureShadowProgressBar.Layout(null, true);
		}

		// Token: 0x060041C4 RID: 16836 RVA: 0x000C41B0 File Offset: 0x000C23B0
		public void UpdateInputBiding()
		{
			bool flag = this._inputBidingLabel == null;
			if (!flag)
			{
				foreach (string key in this._inputBidingsMouse.Keys)
				{
					this._inputBidingsMouse[key].Visible = false;
				}
				this._inputBidingLabel.Visible = false;
				bool flag2 = this.InGameView == null;
				if (!flag2)
				{
					InputBinding tertiaryItemAction = this.InGameView.Interface.App.Settings.InputBindings.TertiaryItemAction;
					bool flag3 = tertiaryItemAction == null;
					if (!flag3)
					{
						bool flag4 = Enumerable.Contains<string>(Enumerable.ToArray<string>(this._inputBidingsMouse.Keys), tertiaryItemAction.BoundInputLabel);
						if (flag4)
						{
							Group group = this._inputBidingsMouse[tertiaryItemAction.BoundInputLabel];
							bool flag5 = group == null;
							if (!flag5)
							{
								group.Visible = true;
								base.Layout(null, true);
							}
						}
						else
						{
							this._inputBidingLabel.Text = tertiaryItemAction.BoundInputLabel;
							this._inputBidingLabel.Style.FontSize = (float)Math.Max(13 - Enumerable.ToArray<char>(tertiaryItemAction.BoundInputLabel).Length, 8);
							this._inputBidingLabel.Visible = true;
							base.Layout(null, true);
						}
					}
				}
			}
		}

		// Token: 0x060041C5 RID: 16837 RVA: 0x000C4334 File Offset: 0x000C2534
		public void OnSignatureEnergyStatChanged(ClientEntityStatValue entityStatValue)
		{
			bool flag = entityStatValue == null;
			if (flag)
			{
				this._targetSignaturePercentage = 0f;
				this._renderSignaturePercentage = 0f;
				this.AnimateSignature();
				this._targetShadowSignaturePercentage = 0f;
				this._renderShadowSignaturePercentage = 0f;
				this.AnimateShadowSignature();
			}
			else
			{
				this._targetSignaturePercentage = ((entityStatValue.Max > 0f) ? entityStatValue.AsPercentage() : 0f);
			}
		}

		// Token: 0x060041C6 RID: 16838 RVA: 0x000C43A8 File Offset: 0x000C25A8
		public void ShowErrorOverlay()
		{
			this._overlaySignatureError.Visible = true;
			this._overlaySignatureError.Parent.Layout(null, true);
			this._errorTargetPercentage = 0.4f;
			this._errorRenderPercentage = 1f;
			this._errorAnimationCount = 0;
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x000C43FC File Offset: 0x000C25FC
		public void HideErrorOverlay()
		{
			this._overlaySignatureError.Visible = false;
			this._overlaySignatureError.Parent.Layout(null, true);
			this._errorTargetPercentage = 1f;
			this._errorRenderPercentage = 1f;
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x000C4448 File Offset: 0x000C2648
		public void OnSignatureAction()
		{
			ClientEntityStatValue entityStat = this.InGameView.InGame.Instance.LocalPlayer.GetEntityStat(DefaultEntityStats.SignatureEnergy);
			bool flag = entityStat.AsPercentage() < 1f;
			if (flag)
			{
				this.ShowErrorOverlay();
			}
		}

		// Token: 0x04002001 RID: 8193
		private const float _signatureAnimationVelocity = 10f;

		// Token: 0x04002002 RID: 8194
		private const float _backgroundAnimationVelocity = 20f;

		// Token: 0x04002003 RID: 8195
		private InGameView InGameView;

		// Token: 0x04002004 RID: 8196
		private Dictionary<string, Group> _inputBidingsMouse = new Dictionary<string, Group>();

		// Token: 0x04002005 RID: 8197
		private Label _inputBidingLabel;

		// Token: 0x04002006 RID: 8198
		private Group _inputBidingContainer;

		// Token: 0x04002007 RID: 8199
		private float _targetSignaturePercentage;

		// Token: 0x04002008 RID: 8200
		private float _renderSignaturePercentage;

		// Token: 0x04002009 RID: 8201
		private CircularProgressBar _signatureProgressBar;

		// Token: 0x0400200A RID: 8202
		private float _targetShadowSignaturePercentage;

		// Token: 0x0400200B RID: 8203
		private float _renderShadowSignaturePercentage;

		// Token: 0x0400200C RID: 8204
		private CircularProgressBar _signatureShadowProgressBar;

		// Token: 0x0400200D RID: 8205
		private Group _signatureIcon;

		// Token: 0x0400200E RID: 8206
		private Group _signatureReadyProgressBar;

		// Token: 0x0400200F RID: 8207
		private Group _backgroundSignatureReady;

		// Token: 0x04002010 RID: 8208
		private Group _backgroundSignatureNotReady;

		// Token: 0x04002011 RID: 8209
		private Group _overlaySignatureError;

		// Token: 0x04002012 RID: 8210
		private int _errorAnimationCount;

		// Token: 0x04002013 RID: 8211
		private const int _errorAnimationTotal = 4;

		// Token: 0x04002014 RID: 8212
		private const float _maxErrorAnimationPercentage = 1f;

		// Token: 0x04002015 RID: 8213
		private const float _minErrorAnimationPercentage = 0.4f;

		// Token: 0x04002016 RID: 8214
		private const float _errorAnimationSpeed = 10f;

		// Token: 0x04002017 RID: 8215
		protected float _errorTargetPercentage;

		// Token: 0x04002018 RID: 8216
		protected float _errorRenderPercentage;

		// Token: 0x04002019 RID: 8217
		private bool _isSignatureReady;

		// Token: 0x0400201A RID: 8218
		private float _targetBackgroundSignatureReadyOpacity;

		// Token: 0x0400201B RID: 8219
		private float _renderBackgroundSignatureReadyOpacity;

		// Token: 0x0400201C RID: 8220
		private float _backgroundSignatureSize;

		// Token: 0x0400201D RID: 8221
		private float _backgroundSignatureScaleSize;

		// Token: 0x0400201E RID: 8222
		private bool _animateSignatureBackgroundScaleUp;

		// Token: 0x0400201F RID: 8223
		private bool _animateSignatureBackgroundScaleDown;
	}
}
