using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Core;
using HytaleClient.Data.ClientInteraction;
using HytaleClient.Data.Items;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Interface.InGame.Hud.Abilities
{
	// Token: 0x020008D2 RID: 2258
	internal class BaseAbility : Element
	{
		// Token: 0x06004196 RID: 16790 RVA: 0x000C21E0 File Offset: 0x000C03E0
		public BaseAbility(InGameView inGameView, Desktop desktop, Element parent) : base(desktop, parent)
		{
			this.InGameView = inGameView;
		}

		// Token: 0x06004197 RID: 16791 RVA: 0x000C227C File Offset: 0x000C047C
		public virtual void Build()
		{
			Document document;
			this.Desktop.Provider.TryGetDocument("InGame/Hud/Abilities/Ability.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._inputBidingsMouse.Add(InputBinding.GetMouseBoundInputLabel(Input.MouseButton.SDL_BUTTON_LEFT), uifragment.Get<Group>("MouseLeft"));
			this._inputBidingsMouse.Add(InputBinding.GetMouseBoundInputLabel(Input.MouseButton.SDL_BUTTON_MIDDLE), uifragment.Get<Group>("MouseMiddle"));
			this._inputBidingsMouse.Add(InputBinding.GetMouseBoundInputLabel(Input.MouseButton.SDL_BUTTON_RIGHT), uifragment.Get<Group>("MouseRight"));
			this._inputBidingLabel = uifragment.Get<Label>("InputBinding");
			this._inputBidingContainer = uifragment.Get<Group>("InputBindingContainer");
			this._icons[0] = uifragment.Get<Group>("IconShieldAbility");
			this._icons[1] = uifragment.Get<Group>("IconSwordChargeUpAttack");
			this._cooldownTimer = uifragment.Get<Label>("CooldownTimer");
			this._shadowCooldownTimer = uifragment.Get<Label>("ShadowCooldownTimer");
			this._cooldownTimerContainer = uifragment.Get<Group>("CooldownTimerContainer");
			this._abilityChargesContainer = uifragment.Get<Group>("AbilityChargesContainer");
			this._errorOverlay = uifragment.Get<Group>("ErrorOverlay");
			this._errorOverlay.Visible = false;
			this._chargeProgressBar = uifragment.Get<CircularProgressBar>("ChargeProgressBar");
			this._cooldownTimer.Visible = (this._shadowCooldownTimer.Visible = false);
			this._cooldownBar = uifragment.Get<ProgressBar>("Cooldown");
		}

		// Token: 0x06004198 RID: 16792 RVA: 0x000C23F0 File Offset: 0x000C05F0
		protected void SetIcon(BaseAbility.IconName iconName)
		{
			this._selectedIcon = this._icons[(int)iconName];
			bool flag = this._selectedIcon == null;
			if (!flag)
			{
				this._selectedIcon.Visible = true;
				this._selectedIcon.Layout(null, true);
			}
		}

		// Token: 0x06004199 RID: 16793 RVA: 0x000C2440 File Offset: 0x000C0640
		public void ShowErrorOverlay()
		{
			this._errorOverlay.Visible = true;
			this._errorOverlay.Parent.Layout(null, true);
			this._errorTargetPercentage = 0.4f;
			this._errorRenderPercentage = 1f;
			this._errorAnimationCount = 0;
		}

		// Token: 0x0600419A RID: 16794 RVA: 0x000C2494 File Offset: 0x000C0694
		public void HideErrorOverlay()
		{
			this._errorOverlay.Visible = false;
			this._errorOverlay.Parent.Layout(null, true);
			this._isStaminaErrorEffect = false;
			this._errorTargetPercentage = 0.4f;
			this._errorRenderPercentage = 1f;
		}

		// Token: 0x0600419B RID: 16795 RVA: 0x000C24E8 File Offset: 0x000C06E8
		public void CooldownError(string rootInteractionId)
		{
			GameInstance instance = this.InGameView.InGame.Instance;
			InteractionContext interactionContext = InteractionContext.ForInteraction((instance != null) ? instance.LocalPlayer : null, this._iterationType);
			int num;
			interactionContext.GetRootInteractionId(this.InGameView.InGame.Instance, this._iterationType, out num);
			ClientRootInteraction clientRootInteraction = this.InGameView.InGame.Instance.InteractionModule.RootInteractions[num];
			bool flag = clientRootInteraction.Id != rootInteractionId;
			if (!flag)
			{
				this.ShowErrorOverlay();
			}
		}

		// Token: 0x0600419C RID: 16796 RVA: 0x000C2573 File Offset: 0x000C0773
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
			this.UpdateInputBiding();
			this.SetCharges();
		}

		// Token: 0x0600419D RID: 16797 RVA: 0x000C259D File Offset: 0x000C079D
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x0600419E RID: 16798 RVA: 0x000C25B8 File Offset: 0x000C07B8
		protected virtual void Animate(float deltaTime)
		{
			GameInstance instance = this.InGameView.InGame.Instance;
			bool flag = ((instance != null) ? instance.LocalPlayer : null) == null;
			if (!flag)
			{
				this.SetCooldownPercentages();
				bool flag2 = this._cooldownTargetPercentage != this._cooldownRenderPercentage;
				if (flag2)
				{
					this._cooldownRenderPercentage = MathHelper.Lerp(this._cooldownRenderPercentage, this._cooldownTargetPercentage, deltaTime * this.animationSpeed);
					this.AnimateCooldown();
				}
				bool flag3 = this._errorTargetPercentage != this._errorRenderPercentage;
				if (flag3)
				{
					this._errorRenderPercentage = MathHelper.Lerp(this._errorRenderPercentage, this._errorTargetPercentage, deltaTime * 10f);
					this.AnimateError();
				}
				this.SetChargesPercentage();
				bool flag4 = this._chargeTargetPercentage != this._chargeRenderPercentage;
				if (flag4)
				{
					this._chargeRenderPercentage = MathHelper.Lerp(this._chargeRenderPercentage, this._chargeTargetPercentage, deltaTime * this.animationSpeed);
					this.AnimateChargeProgressBar();
				}
			}
		}

		// Token: 0x0600419F RID: 16799 RVA: 0x000C26B4 File Offset: 0x000C08B4
		private void SetCooldownPercentages()
		{
			bool flag = this._abilityCharges.Count > 1;
			if (flag)
			{
				bool flag2 = this._cooldownRenderPercentage == 0f && !this._cooldownTimer.Visible;
				if (!flag2)
				{
					this._cooldownRenderPercentage = (this._cooldownTargetPercentage = 0f);
					this._cooldownTimer.Visible = (this._shadowCooldownTimer.Visible = false);
					this.SetSelectedIconOpacity(1f);
					this._selectedIcon.Layout(null, true);
					this._cooldownTimerContainer.Layout(null, true);
					this.AnimateCooldown();
				}
			}
			else
			{
				GameInstance instance = this.InGameView.InGame.Instance;
				InteractionContext interactionContext = InteractionContext.ForInteraction((instance != null) ? instance.LocalPlayer : null, this._iterationType);
				int num;
				interactionContext.GetRootInteractionId(this.InGameView.InGame.Instance, this._iterationType, out num);
				ClientRootInteraction clientRootInteraction = this.InGameView.InGame.Instance.InteractionModule.RootInteractions[num];
				float num2 = 0.35f;
				bool flag3 = clientRootInteraction.RootInteraction.Cooldown != null;
				if (flag3)
				{
					num2 = clientRootInteraction.RootInteraction.Cooldown.Cooldown;
				}
				InteractionCooldown cooldown = clientRootInteraction.RootInteraction.Cooldown;
				string text = (cooldown != null) ? cooldown.CooldownId : null;
				bool flag4 = text == null;
				if (flag4)
				{
					text = clientRootInteraction.Id;
				}
				InteractionModule interactionModule = this.InGameView.InGame.Instance.InteractionModule;
				bool flag5 = interactionModule.GetCooldown(text) == null;
				if (flag5)
				{
					bool flag6 = this._cooldownRenderPercentage == 0f;
					if (!flag6)
					{
						this._cooldownRenderPercentage = (this._cooldownTargetPercentage = 0f);
						this._cooldownTimer.Visible = (this._shadowCooldownTimer.Visible = false);
						this.SetSelectedIconOpacity(1f);
						this._selectedIcon.Layout(null, true);
						this._cooldownTimerContainer.Layout(null, true);
						this.AnimateCooldown();
						this.SetChargesStates(this._abilityCharges.Count);
						bool flag7 = !this._isStaminaErrorEffect;
						if (flag7)
						{
							this.HideErrorOverlay();
						}
					}
				}
				else
				{
					float num3 = Math.Min(interactionModule.GetCooldown(text).GetCooldownRemainingTime(), 10000000f);
					string cooldownRemainingTime = this.GetCooldownRemainingTime(num3);
					float num4 = 1f - num3 / num2;
					num4 = Math.Max(num4, 0f);
					bool flag8 = this._cooldownTargetPercentage == num4;
					if (!flag8)
					{
						this._cooldownTargetPercentage = num4;
						this._shadowCooldownTimer.Text = cooldownRemainingTime;
						this._cooldownTimer.Text = cooldownRemainingTime;
						this.SetSelectedIconOpacity(0.4f);
						this._cooldownTimer.Visible = (this._shadowCooldownTimer.Visible = true);
						this._cooldownTimerContainer.Layout(null, true);
					}
				}
			}
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x000C29D8 File Offset: 0x000C0BD8
		private void SetChargesPercentage()
		{
			bool flag = this._abilityCharges.Count <= 1;
			if (flag)
			{
				bool flag2 = this._chargeRenderPercentage == 0f;
				if (!flag2)
				{
					this._chargeRenderPercentage = (this._chargeTargetPercentage = 0f);
					this.AnimateChargeProgressBar();
				}
			}
			else
			{
				GameInstance instance = this.InGameView.InGame.Instance;
				InteractionContext interactionContext = InteractionContext.ForInteraction((instance != null) ? instance.LocalPlayer : null, this._iterationType);
				int num;
				interactionContext.GetRootInteractionId(this.InGameView.InGame.Instance, this._iterationType, out num);
				ClientRootInteraction clientRootInteraction = this.InGameView.InGame.Instance.InteractionModule.RootInteractions[num];
				InteractionCooldown cooldown = clientRootInteraction.RootInteraction.Cooldown;
				string text = (cooldown != null) ? cooldown.CooldownId : null;
				bool flag3 = text == null;
				if (flag3)
				{
					text = clientRootInteraction.Id;
				}
				InteractionModule interactionModule = this.InGameView.InGame.Instance.InteractionModule;
				bool flag4 = interactionModule.GetCooldown(text) == null;
				if (flag4)
				{
					bool flag5 = this._chargeRenderPercentage == 0f;
					if (!flag5)
					{
						this._chargeRenderPercentage = (this._chargeTargetPercentage = 0f);
						this._cooldownTimer.Visible = (this._shadowCooldownTimer.Visible = false);
						this._cooldownTimerContainer.Layout(null, true);
						this.AnimateChargeProgressBar();
						this.SetChargesStates(this._abilityCharges.Count);
					}
				}
				else
				{
					Cooldown cooldown2 = interactionModule.GetCooldown(text);
					float chargeTimer = cooldown2.GetChargeTimer();
					int chargeCount = cooldown2.GetChargeCount();
					bool flag6 = this._errorOverlay.Visible && chargeCount != 0;
					if (flag6)
					{
						bool flag7 = !this._isStaminaErrorEffect;
						if (flag7)
						{
							this.HideErrorOverlay();
						}
					}
					bool flag8 = chargeCount == 0;
					if (flag8)
					{
						this.SetSelectedIconOpacity(0.4f);
					}
					else
					{
						this.SetSelectedIconOpacity(1f);
					}
					this.SetChargesStates(chargeCount);
					float[] charges = cooldown2.GetCharges();
					bool flag9 = chargeCount >= charges.Length;
					float num2;
					float num3;
					if (flag9)
					{
						num2 = cooldown2.GetCooldownMax();
						num3 = Math.Min(cooldown2.GetCooldownRemainingTime(), 10000000f);
					}
					else
					{
						num2 = charges[chargeCount];
						num3 = Math.Min(num2 - chargeTimer, 10000000f);
					}
					float num4 = 1f - num3 / num2;
					num4 = Math.Max(num4, 0f);
					bool flag10 = this._chargeTargetPercentage == num4;
					if (!flag10)
					{
						this._chargeTargetPercentage = num4;
					}
				}
			}
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x000C2C7C File Offset: 0x000C0E7C
		private string GetCooldownRemainingTime(float remainingTime)
		{
			bool flag = remainingTime > 999f;
			string result;
			if (flag)
			{
				this.SetCooldownTimerFontSize(18f);
				result = 999.ToString() + "+";
			}
			else
			{
				bool flag2 = this._cooldownTimer.Style.FontSize != 24f || this._shadowCooldownTimer.Style.FontSize != 24f;
				if (flag2)
				{
					this.SetCooldownTimerFontSize(24f);
				}
				bool flag3 = remainingTime < 0f;
				if (flag3)
				{
					result = "";
				}
				else
				{
					int num = (remainingTime < 1f) ? 10 : 1;
					result = (Math.Truncate((double)(remainingTime * (float)num)) / (double)num).ToString();
				}
			}
			return result;
		}

		// Token: 0x060041A2 RID: 16802 RVA: 0x000C2D48 File Offset: 0x000C0F48
		private void SetCooldownTimerFontSize(float fontSize)
		{
			this._cooldownTimer.Style.FontSize = fontSize;
			this._shadowCooldownTimer.Style.FontSize = fontSize;
			this._cooldownTimerContainer.Layout(null, true);
		}

		// Token: 0x060041A3 RID: 16803 RVA: 0x000C2D90 File Offset: 0x000C0F90
		private void SetChargesStates(int currentCharge)
		{
			for (int i = 0; i < this._abilityCharges.Count; i++)
			{
				bool flag = i > currentCharge - 1;
				if (flag)
				{
					this._abilityCharges[i].SetEmpty();
				}
				else
				{
					this._abilityCharges[i].SetFull();
				}
			}
		}

		// Token: 0x060041A4 RID: 16804 RVA: 0x000C2DEC File Offset: 0x000C0FEC
		public void SetCharges()
		{
			this._abilityCharges = new List<AbilityCharge>();
			bool flag = this._abilityChargesContainer == null;
			if (!flag)
			{
				this._abilityChargesContainer.Clear();
				bool flag2 = this.InGameView.InGame.Instance == null;
				if (!flag2)
				{
					ClientItemBase currentInteractionItem = this.GetCurrentInteractionItem();
					int chargeAmount = this.GetChargeAmount(currentInteractionItem);
					bool flag3 = chargeAmount <= 1;
					if (!flag3)
					{
						for (int i = 0; i < chargeAmount; i++)
						{
							AbilityCharge abilityCharge = new AbilityCharge(this.InGameView, this.Desktop, this._abilityChargesContainer);
							abilityCharge.Build();
							this._abilityCharges.Add(abilityCharge);
							abilityCharge.SetFull();
							bool flag4 = i >= 6;
							if (flag4)
							{
								abilityCharge.Visible = false;
							}
						}
					}
				}
			}
		}

		// Token: 0x060041A5 RID: 16805 RVA: 0x000C2EC8 File Offset: 0x000C10C8
		private ClientItemBase GetCurrentInteractionItem()
		{
			InteractionContext interactionContext = InteractionContext.ForInteraction(this.InGameView.InGame.Instance, this.InGameView.InGame.Instance.InventoryModule, this._iterationType, null);
			int currentInteractionId;
			interactionContext.GetRootInteractionId(this.InGameView.InGame.Instance, this._iterationType, out currentInteractionId);
			ClientItemStack activeHotbarItem = this.InGameView.InGame.Instance.InventoryModule.GetActiveHotbarItem();
			ClientItemBase item = this.InGameView.InGame.Instance.ItemLibraryModule.GetItem((activeHotbarItem != null) ? activeHotbarItem.Id : null);
			bool flag = this.IsCurrentInteractionItem(item, currentInteractionId);
			ClientItemBase result;
			if (flag)
			{
				result = item;
			}
			else
			{
				ClientItemStack utilityItem = this.InGameView.InGame.Instance.InventoryModule.GetUtilityItem(this.InGameView.InGame.Instance.InventoryModule.UtilityActiveSlot);
				ClientItemBase item2 = this.InGameView.InGame.Instance.ItemLibraryModule.GetItem((utilityItem != null) ? utilityItem.Id : null);
				bool flag2 = this.IsCurrentInteractionItem(item2, currentInteractionId);
				if (flag2)
				{
					result = item2;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060041A6 RID: 16806 RVA: 0x000C3004 File Offset: 0x000C1204
		private bool IsCurrentInteractionItem(ClientItemBase item, int currentInteractionId)
		{
			bool flag = item == null || item.Interactions == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int num;
				item.Interactions.TryGetValue(this._iterationType, out num);
				result = (num == currentInteractionId);
			}
			return result;
		}

		// Token: 0x060041A7 RID: 16807 RVA: 0x000C3048 File Offset: 0x000C1248
		private int GetChargeAmount(ClientItemBase item)
		{
			bool flag = item == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = !item.Interactions.ContainsKey(this._iterationType);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					int num = item.Interactions[this._iterationType];
					ClientRootInteraction clientRootInteraction = this.InGameView.InGame.Instance.InteractionModule.RootInteractions[num];
					InteractionCooldown cooldown = clientRootInteraction.RootInteraction.Cooldown;
					int? num2;
					if (cooldown == null)
					{
						num2 = null;
					}
					else
					{
						float[] chargeTimes = cooldown.ChargeTimes;
						num2 = ((chargeTimes != null) ? new int?(chargeTimes.Length) : null);
					}
					int? num3 = num2;
					result = num3.GetValueOrDefault();
				}
			}
			return result;
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x000C30FC File Offset: 0x000C12FC
		private void SetSelectedIconOpacity(float opacity)
		{
			UInt32Color uint32Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(opacity * 255f));
			bool flag = this._selectedIcon.Background.Color.GetA() == uint32Color.GetA();
			if (!flag)
			{
				this._selectedIcon.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(opacity * 255f));
				this._selectedIcon.Layout(null, true);
				this.SetInputBidingsOpacity(opacity);
			}
		}

		// Token: 0x060041A9 RID: 16809 RVA: 0x000C3198 File Offset: 0x000C1398
		private void SetInputBidingsOpacity(float opacity)
		{
			this._inputBidingLabel.Style.TextColor.SetA((byte)(opacity * 255f));
			foreach (string key in this._inputBidingsMouse.Keys)
			{
				this._inputBidingsMouse[key].Background.Color.SetA((byte)(opacity * 255f));
			}
			this._inputBidingLabel.Parent.Layout(null, true);
			this._inputBidingContainer.Layout(null, true);
		}

		// Token: 0x060041AA RID: 16810 RVA: 0x000C3264 File Offset: 0x000C1464
		protected void AnimateCooldown()
		{
			this._cooldownBar.Value = MathHelper.Clamp(this._cooldownRenderPercentage, 0f, 1f);
			this._cooldownBar.Layout(null, true);
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x000C32A8 File Offset: 0x000C14A8
		protected void AnimateError()
		{
			this._errorOverlay.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(this._errorRenderPercentage * 255f));
			this._errorOverlay.Parent.Layout(null, true);
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

		// Token: 0x060041AC RID: 16812 RVA: 0x000C33A0 File Offset: 0x000C15A0
		public void OnStartChain(string rootInteractionId)
		{
			this._lastChainId = rootInteractionId;
			GameInstance instance = this.InGameView.InGame.Instance;
			Entity.UniqueEntityEffect[] array = (instance != null) ? instance.LocalPlayer.EntityEffects : null;
			int i = 0;
			while (i < array.Length)
			{
				Entity.UniqueEntityEffect uniqueEntityEffect = array[i];
				Dictionary<string, int> entityEffectIndicesByIds = this.InGameView.InGame.Instance.EntityStoreModule.EntityEffectIndicesByIds;
				int num;
				bool flag = entityEffectIndicesByIds.TryGetValue("Stamina_Error_State", out num) && uniqueEntityEffect.NetworkEffectIndex == num;
				if (flag)
				{
					GameInstance instance2 = this.InGameView.InGame.Instance;
					InteractionContext interactionContext = InteractionContext.ForInteraction((instance2 != null) ? instance2.LocalPlayer : null, this._iterationType);
					int num2;
					interactionContext.GetRootInteractionId(this.InGameView.InGame.Instance, this._iterationType, out num2);
					ClientRootInteraction clientRootInteraction = this.InGameView.InGame.Instance.InteractionModule.RootInteractions[num2];
					bool flag2 = clientRootInteraction.Id != this._lastChainId;
					if (flag2)
					{
						break;
					}
					this.ShowErrorOverlay();
					this._lastChainId = null;
					this._isStaminaErrorEffect = true;
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x000C34D0 File Offset: 0x000C16D0
		public void OnEffectRemoved(int effectIndex)
		{
			Dictionary<string, int> entityEffectIndicesByIds = this.InGameView.InGame.Instance.EntityStoreModule.EntityEffectIndicesByIds;
			int num;
			bool flag = entityEffectIndicesByIds.TryGetValue("Stamina_Error_State", out num) && effectIndex == num;
			if (flag)
			{
				this._isStaminaErrorEffect = false;
				this._lastChainId = null;
				this.HideErrorOverlay();
			}
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x000C352C File Offset: 0x000C172C
		protected void AnimateChargeProgressBar()
		{
			this._chargeProgressBar.Value = MathHelper.Clamp(this._chargeRenderPercentage, 0f, 1f);
			this._chargeProgressBar.Layout(null, true);
		}

		// Token: 0x060041AF RID: 16815 RVA: 0x000C3570 File Offset: 0x000C1770
		public virtual void UpdateInputBiding()
		{
			bool flag = this._inputBidingLabel == null;
			if (!flag)
			{
				foreach (string key in this._inputBidingsMouse.Keys)
				{
					this._inputBidingsMouse[key].Visible = false;
				}
				this._inputBidingLabel.Visible = false;
				bool flag2 = this._inputBidingKey == null;
				if (!flag2)
				{
					bool flag3 = Enumerable.Contains<string>(Enumerable.ToArray<string>(this._inputBidingsMouse.Keys), this._inputBidingKey.BoundInputLabel);
					if (flag3)
					{
						Group group = this._inputBidingsMouse[this._inputBidingKey.BoundInputLabel];
						bool flag4 = group == null;
						if (!flag4)
						{
							group.Visible = true;
							base.Layout(null, true);
						}
					}
					else
					{
						this._inputBidingLabel.Text = this._inputBidingKey.BoundInputLabel;
						this._inputBidingLabel.Style.FontSize = (float)Math.Max(13 - Enumerable.ToArray<char>(this._inputBidingKey.BoundInputLabel).Length, 8);
						this._inputBidingLabel.Visible = true;
						base.Layout(null, true);
					}
				}
			}
		}

		// Token: 0x04001FDD RID: 8157
		public const string StaminaDepletedEffectId = "Stamina_Error_State";

		// Token: 0x04001FDE RID: 8158
		public const int ChargesMax = 6;

		// Token: 0x04001FDF RID: 8159
		public const int ChargeTimeMax = 999;

		// Token: 0x04001FE0 RID: 8160
		public const int RemainingTimeMax = 10000000;

		// Token: 0x04001FE1 RID: 8161
		public const int TimerFontSizeMax = 24;

		// Token: 0x04001FE2 RID: 8162
		public const int TimerFontSizeMin = 18;

		// Token: 0x04001FE3 RID: 8163
		private const float MaxErrorAnimationPercentage = 1f;

		// Token: 0x04001FE4 RID: 8164
		private const int ErrorAnimationTotal = 4;

		// Token: 0x04001FE5 RID: 8165
		private const float MinErrorAnimationPercentage = 0.4f;

		// Token: 0x04001FE6 RID: 8166
		private const float ErrorAnimationSpeed = 10f;

		// Token: 0x04001FE7 RID: 8167
		public readonly InGameView InGameView;

		// Token: 0x04001FE8 RID: 8168
		protected InputBinding _inputBidingKey;

		// Token: 0x04001FE9 RID: 8169
		private Dictionary<string, Group> _inputBidingsMouse = new Dictionary<string, Group>();

		// Token: 0x04001FEA RID: 8170
		private Label _inputBidingLabel;

		// Token: 0x04001FEB RID: 8171
		private Group _inputBidingContainer;

		// Token: 0x04001FEC RID: 8172
		private Group[] _icons = new Group[Enum.GetNames(typeof(BaseAbility.IconName)).Length];

		// Token: 0x04001FED RID: 8173
		private Group _selectedIcon;

		// Token: 0x04001FEE RID: 8174
		private Label _cooldownTimer;

		// Token: 0x04001FEF RID: 8175
		private Label _shadowCooldownTimer;

		// Token: 0x04001FF0 RID: 8176
		private Group _cooldownTimerContainer;

		// Token: 0x04001FF1 RID: 8177
		private Group _errorOverlay;

		// Token: 0x04001FF2 RID: 8178
		private int _errorAnimationCount;

		// Token: 0x04001FF3 RID: 8179
		protected float _errorTargetPercentage = 0f;

		// Token: 0x04001FF4 RID: 8180
		protected float _errorRenderPercentage = 0f;

		// Token: 0x04001FF5 RID: 8181
		private bool _isStaminaErrorEffect = false;

		// Token: 0x04001FF6 RID: 8182
		private string _lastChainId;

		// Token: 0x04001FF7 RID: 8183
		private ProgressBar _cooldownBar;

		// Token: 0x04001FF8 RID: 8184
		private CircularProgressBar _chargeProgressBar;

		// Token: 0x04001FF9 RID: 8185
		private Group _abilityChargesContainer;

		// Token: 0x04001FFA RID: 8186
		private List<AbilityCharge> _abilityCharges;

		// Token: 0x04001FFB RID: 8187
		protected float _cooldownTargetPercentage = 0f;

		// Token: 0x04001FFC RID: 8188
		protected float _cooldownRenderPercentage = 0f;

		// Token: 0x04001FFD RID: 8189
		protected float _chargeTargetPercentage = 0f;

		// Token: 0x04001FFE RID: 8190
		protected float _chargeRenderPercentage = 0f;

		// Token: 0x04001FFF RID: 8191
		private float animationSpeed = 50f;

		// Token: 0x04002000 RID: 8192
		protected InteractionType _iterationType;

		// Token: 0x02000D86 RID: 3462
		public enum IconName
		{
			// Token: 0x0400424C RID: 16972
			ShieldAbility,
			// Token: 0x0400424D RID: 16973
			SwordChargeUpAttack
		}
	}
}
