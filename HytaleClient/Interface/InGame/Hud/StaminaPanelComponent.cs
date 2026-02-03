using System;
using System.Collections.Generic;
using HytaleClient.Data.EntityStats;
using HytaleClient.Data.UserSettings;
using HytaleClient.Graphics;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;
using HytaleClient.Math;
using NLog;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008C5 RID: 2245
	internal class StaminaPanelComponent : InterfaceComponent
	{
		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x06004127 RID: 16679 RVA: 0x000BF278 File Offset: 0x000BD478
		public bool ShouldDisplay
		{
			get
			{
				GameInstance instance = this.InGameView.InGame.Instance;
				PlayerEntity playerEntity = (instance != null) ? instance.LocalPlayer : null;
				bool flag = playerEntity == null;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = this.InGameView.InGame.Instance.GameMode == 1;
					if (flag2)
					{
						result = false;
					}
					else
					{
						ClientEntityStatValue entityStat = this.InGameView.InGame.Instance.LocalPlayer.GetEntityStat(DefaultEntityStats.Stamina);
						bool flag3;
						if (!this.InGameView.Wielding)
						{
							float? num = (entityStat != null) ? new float?(entityStat.Value) : null;
							float? num2 = (entityStat != null) ? new float?(entityStat.Max) : null;
							if (!(num.GetValueOrDefault() < num2.GetValueOrDefault() & (num != null & num2 != null)))
							{
								flag3 = this.ShouldDisplayFlash;
								goto IL_E2;
							}
						}
						flag3 = true;
						IL_E2:
						result = flag3;
					}
				}
				return result;
			}
		}

		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x06004128 RID: 16680 RVA: 0x000BF36B File Offset: 0x000BD56B
		private bool ShouldDisplayFlash
		{
			get
			{
				return this.AlertType > AlertType.None;
			}
		}

		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x06004129 RID: 16681 RVA: 0x000BF378 File Offset: 0x000BD578
		private AlertType AlertType
		{
			get
			{
				bool isErrorActive = this._isErrorActive;
				AlertType result;
				if (isErrorActive)
				{
					result = AlertType.Error;
				}
				else
				{
					bool isOverdrawnAlert = this._isOverdrawnAlert;
					if (isOverdrawnAlert)
					{
						result = AlertType.Overdrawn;
					}
					else
					{
						bool isLowAlert = this._isLowAlert;
						if (isLowAlert)
						{
							result = AlertType.Low;
						}
						else
						{
							result = AlertType.None;
						}
					}
				}
				return result;
			}
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x000BF3B5 File Offset: 0x000BD5B5
		public StaminaPanelComponent(InGameView view) : base(view.Interface, view.HudContainer)
		{
			this.InGameView = view;
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x000BF3DC File Offset: 0x000BD5DC
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/Stamina/StaminaPanel.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._staminaBar = uifragment.Get<Group>("StaminaBar");
			this._staminaBarProgress = uifragment.Get<ProgressBar>("StaminaBarProgress");
			this._staminaBarProgressDrain = uifragment.Get<ProgressBar>("StaminaBarDrainEffect");
			this._staminaBarError = uifragment.Get<Group>("StaminaBarError");
			this._staminaBarErrorOutline = uifragment.Get<Group>("StaminaBarErrorOutline");
			this._debugStaminaInfo = uifragment.Get<Group>("DebugStaminaInfo");
			this._debugStaminaValue = uifragment.Get<Label>("DebugStaminaValue");
			this._debugStaminaRegenRate = uifragment.Get<Label>("DebugStaminaRegenRate");
			this._debugStaminaAlertState = uifragment.Get<Label>("DebugStaminaAlertState");
			this._staminaBarBackground = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "StaminaBarBackground");
			this._staminaBarBackgroundDepleted = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "StaminaBarBackgroundDepleted");
			this._staminaBarBackgroundError = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "StaminaBarBackgroundError");
			this._staminaBarBackgroundErrorOutline = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "StaminaBarBackgroundErrorOutline");
			this._staminaBarBackgroundOverdrawn = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "StaminaBarBackgroundOverdrawn");
			this._staminaBarBackgroundOverdrawnOutline = document.ResolveNamedValue<PatchStyle>(this.Desktop.Provider, "StaminaBarBackgroundOverdrawnOutline");
			this._progressBarAnimationSpeed = document.ResolveNamedValue<float>(this.Desktop.Provider, "ProgressBarAnimationSpeed");
			this._drainProgressBarAnimationSpeed = document.ResolveNamedValue<float>(this.Desktop.Provider, "DrainProgressBarAnimationSpeed");
			this._drainProgressTime = document.ResolveNamedValue<float>(this.Desktop.Provider, "DrainProgressTime");
			this._minimumDrainValue = document.ResolveNamedValue<float>(this.Desktop.Provider, "MinimumDrainValue");
			this._errorAnimationSpeed = document.ResolveNamedValue<float>(this.Desktop.Provider, "ErrorAnimationSpeed");
			this._progressBarErrorThreshold = document.ResolveNamedValue<float>(this.Desktop.Provider, "ProgressBarErrorThreshold");
			this._defaultErrorFlashTimer = document.ResolveNamedValue<float>(this.Desktop.Provider, "ErrorFlashTimer");
			this._defaultStaminaErrorRenderValue = document.ResolveNamedValue<float>(this.Desktop.Provider, "StaminaErrorRenderValue");
			this._lowAlertOutlineOpacityStart = document.ResolveNamedValue<float>(this.Desktop.Provider, "LowAlertOutlineOpacityStart");
			this._lowAlertAnimationSpeed = document.ResolveNamedValue<float>(this.Desktop.Provider, "LowAlertAnimationSpeed");
			this._lowAlertFlashTimer = document.ResolveNamedValue<float>(this.Desktop.Provider, "LowAlertFlashTimer");
			this._overdrawnAlertFlashTimer = document.ResolveNamedValue<float>(this.Desktop.Provider, "OverdrawnAlertFlashTimer");
			this._overdrawnAlertAnimationSpeed = document.ResolveNamedValue<float>(this.Desktop.Provider, "OverdrawnAlertAnimationSpeed");
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x000BF6BF File Offset: 0x000BD8BF
		protected override void OnMounted()
		{
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x000BF6D9 File Offset: 0x000BD8D9
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
			this.StopSound(this._lowAlertPlaybackId);
			this.StopSound(this._overdrawnAlertPlaybackId);
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x000BF70E File Offset: 0x000BD90E
		private void Animate(float deltaTime)
		{
			this.UpdateProgressBarBackground();
			this.UpdateFlash(deltaTime);
			this.AnimateFlash(deltaTime);
			this.UpdateProgressBarValues(deltaTime);
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x000BF730 File Offset: 0x000BD930
		private void UpdateProgressBarBackground()
		{
			AlertType alertType = this.AlertType;
			AlertType alertType2 = alertType;
			if (alertType2 != AlertType.Overdrawn)
			{
				this._staminaBarError.Background = this._staminaBarBackgroundError;
				this._staminaBarErrorOutline.Background = this._staminaBarBackgroundErrorOutline;
			}
			else
			{
				this._staminaBarError.Background = this._staminaBarBackgroundOverdrawn;
				this._staminaBarErrorOutline.Background = this._staminaBarBackgroundOverdrawnOutline;
			}
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x000BF798 File Offset: 0x000BD998
		private void UpdateFlash(float deltaTime)
		{
			bool flag = this.ShouldDisplayFlash && !this._isOutOfStamina;
			if (flag)
			{
				this._flashTimer -= deltaTime;
				bool flag2 = this._flashTimer <= 0f;
				if (flag2)
				{
					this.TriggerFlash();
				}
			}
			else
			{
				this._flashTimer = 0f;
			}
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x000BF7F8 File Offset: 0x000BD9F8
		private void TriggerFlash()
		{
			this._flashTimer = (this._isErrorActive ? this._defaultErrorFlashTimer : (this._isOverdrawnAlert ? this._overdrawnAlertFlashTimer : this._lowAlertFlashTimer));
			this._staminaErrorRenderValue = this._defaultStaminaErrorRenderValue;
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x000BF833 File Offset: 0x000BDA33
		private void AnimateFlash(float deltaTime)
		{
			this.AnimateFlashOutline(deltaTime);
			this.AnimateFlashBar(deltaTime);
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x000BF848 File Offset: 0x000BDA48
		private void AnimateFlashOutline(float deltaTime)
		{
			bool isOutOfStamina = this._isOutOfStamina;
			if (isOutOfStamina)
			{
				this._staminaBarErrorOutline.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				this._staminaBarErrorOutline.Visible = true;
				this._staminaBarErrorOutline.Layout(new Rectangle?(this._staminaBarErrorOutline.Parent.RectangleAfterPadding), true);
			}
			else
			{
				float opacityTargetValue = this.GetOpacityTargetValue();
				bool flag = this._flashOutlineRenderValue != opacityTargetValue;
				if (flag)
				{
					this._flashOutlineRenderValue = MathHelper.Lerp(this._flashOutlineRenderValue, opacityTargetValue, deltaTime * this.GetAnimationSpeed());
					this._staminaBarErrorOutline.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(255f * this._flashOutlineRenderValue));
					this._staminaBarErrorOutline.Visible = true;
					this._staminaBarErrorOutline.Layout(new Rectangle?(this._staminaBarErrorOutline.Parent.RectangleAfterPadding), true);
				}
				else
				{
					bool flag2 = !this.ShouldDisplayFlash && this._staminaBarErrorOutline.Visible;
					if (flag2)
					{
						this._staminaBarErrorOutline.Visible = false;
						this._staminaBarErrorOutline.Layout(null, true);
					}
				}
			}
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x000BF994 File Offset: 0x000BDB94
		private void AnimateFlashBar(float deltaTime)
		{
			bool isOutOfStamina = this._isOutOfStamina;
			if (isOutOfStamina)
			{
				this._staminaBarError.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				this._staminaBarError.Visible = true;
				this._staminaBarError.Layout(new Rectangle?(this._staminaBarError.Parent.RectangleAfterPadding), true);
			}
			else
			{
				bool flag = this._staminaErrorRenderValue > 0f;
				if (flag)
				{
					bool flag2 = this._staminaErrorRenderValue <= this._progressBarErrorThreshold;
					if (flag2)
					{
						this._staminaErrorRenderValue = 0f;
					}
					else
					{
						this._staminaErrorRenderValue = MathHelper.Lerp(this._staminaErrorRenderValue, 0f, deltaTime * this.GetAnimationSpeed());
					}
					byte a = (byte)(MathHelper.Min(255f, 255f * this._staminaErrorRenderValue) * this.GetOpacityTargetValue());
					this._staminaBarError.Background.Color = UInt32Color.FromRGBA(byte.MaxValue, byte.MaxValue, byte.MaxValue, a);
					this._staminaBarError.Visible = true;
					this._staminaBarError.Layout(new Rectangle?(this._staminaBarError.Parent.RectangleAfterPadding), true);
				}
				else
				{
					bool visible = this._staminaBarError.Visible;
					if (visible)
					{
						this._staminaBarError.Visible = false;
					}
				}
			}
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x000BFAFC File Offset: 0x000BDCFC
		private float GetOpacityTargetValue()
		{
			float result;
			switch (this.AlertType)
			{
			case AlertType.Error:
				result = 1f;
				break;
			case AlertType.Low:
				result = this._lowAlertOutlineOpacityTarget;
				break;
			case AlertType.Overdrawn:
				result = 1f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x000BFB4C File Offset: 0x000BDD4C
		private float GetAnimationSpeed()
		{
			float result;
			switch (this.AlertType)
			{
			case AlertType.Error:
				result = this._errorAnimationSpeed;
				break;
			case AlertType.Low:
				result = this._lowAlertAnimationSpeed;
				break;
			case AlertType.Overdrawn:
				result = this._overdrawnAlertAnimationSpeed;
				break;
			default:
				result = 10f;
				break;
			}
			return result;
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x000BFBA0 File Offset: 0x000BDDA0
		private void UpdateProgressBarValues(float deltaTime)
		{
			bool flag = this._staminaRenderValue != this._staminaRenderValueTarget;
			if (flag)
			{
				this._staminaRenderValue = MathHelper.Lerp(this._staminaRenderValue, this._staminaRenderValueTarget, deltaTime * this._progressBarAnimationSpeed);
				this._staminaBarProgress.Value = this._staminaRenderValue;
				this._staminaBarProgress.Layout(null, true);
			}
			bool flag2 = this._depletionTimer > 0f;
			if (flag2)
			{
				this._depletionTimer -= deltaTime;
			}
			else
			{
				bool flag3 = this._staminaBarProgressDrain.Value != this._staminaBarProgress.Value;
				if (flag3)
				{
					this._staminaBarProgressDrain.Value = MathHelper.Lerp(this._staminaBarProgressDrain.Value, this._staminaBarProgress.Value, deltaTime * this._drainProgressBarAnimationSpeed);
					this._staminaBarProgressDrain.Layout(null, true);
				}
			}
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x000BFC94 File Offset: 0x000BDE94
		public void OnStatChanged(ClientEntityStatValue staminaEntityStat, float? nullablePreviousValue)
		{
			bool flag = staminaEntityStat.Max <= 0f;
			if (!flag)
			{
				bool flag2 = nullablePreviousValue == null;
				if (flag2)
				{
					nullablePreviousValue = new float?(this._staminaBarProgress.Value = (this._staminaRenderValue = (this._staminaRenderValueTarget = staminaEntityStat.Value)));
					this._staminaBarProgress.Layout(null, true);
				}
				float num = nullablePreviousValue.Value;
				float num2 = num - staminaEntityStat.Value;
				bool flag3 = staminaEntityStat.Value < num;
				if (flag3)
				{
					this._depletionTimer = this._drainProgressTime;
					this._staminaBarProgressDrain.Value = ((num2 > 0f && num2 < this._minimumDrainValue) ? staminaEntityStat.Value : num) / staminaEntityStat.Max;
					this._staminaBarProgressDrain.Layout(null, true);
				}
				this._isOutOfStamina = (staminaEntityStat.Value <= staminaEntityStat.Min);
				this.UpdateLowAlert(staminaEntityStat);
				this.UpdateOverdrawnAlert(staminaEntityStat);
				this.UpdateStaminaBarProgress(staminaEntityStat);
				this.UpdateStaminaPanelVisibility(staminaEntityStat, num);
				this.UpdateStaminaDebugInfo(staminaEntityStat, num);
			}
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x000BFDC8 File Offset: 0x000BDFC8
		private void UpdateLowAlert(ClientEntityStatValue staminaEntityStat)
		{
			Settings settings = this.InGameView.InGame.Instance.App.Settings;
			float num = (float)settings.StaminaLowAlertPercent / 100f;
			float num2 = staminaEntityStat.Value / staminaEntityStat.Max;
			this._lowAlertOutlineOpacityTarget = MathHelper.Clamp(MathHelper.ConvertToNewRange(num2, 0f, num, 1f, this._lowAlertOutlineOpacityStart), this._lowAlertOutlineOpacityStart, 1f);
			bool flag = !this._isLowAlert && num2 <= num && num2 > 0f;
			if (flag)
			{
				this._isLowAlert = true;
				this._lowAlertPlaybackId = this.PlaySound("STAMINA_LOW_ALERT");
			}
			else
			{
				bool flag2 = this._isLowAlert && num2 > num;
				if (flag2)
				{
					this._isLowAlert = false;
					this._lowAlertPlaybackId = this.StopSound(this._lowAlertPlaybackId);
				}
			}
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x000BFEA4 File Offset: 0x000BE0A4
		private void UpdateOverdrawnAlert(ClientEntityStatValue staminaEntityStat)
		{
			bool flag = !this._isErrorActive && !this._isOverdrawnAlert && staminaEntityStat.Value < 0f;
			if (flag)
			{
				this._isOverdrawnAlert = true;
				this.StopSound(this._overdrawnAlertPlaybackId);
				this._overdrawnAlertPlaybackId = this.PlaySound("STAMINA_EXHAUSTED_ALERT");
			}
			else
			{
				bool flag2 = this._isErrorActive || (this._isOverdrawnAlert && staminaEntityStat.Value >= 0f);
				if (flag2)
				{
					this._isOverdrawnAlert = false;
					this._overdrawnAlertPlaybackId = this.StopSound(this._overdrawnAlertPlaybackId);
				}
			}
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x000BFF44 File Offset: 0x000BE144
		private void UpdateStaminaBarProgress(ClientEntityStatValue staminaEntityStat)
		{
			this._staminaRenderValueTarget = staminaEntityStat.Value / staminaEntityStat.Max;
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x000BFF5C File Offset: 0x000BE15C
		private void UpdateStaminaPanelVisibility(ClientEntityStatValue staminaEntityStat, float previousValue)
		{
			bool flag = staminaEntityStat.Value == staminaEntityStat.Max || previousValue == staminaEntityStat.Max;
			if (flag)
			{
				this.InGameView.UpdateStaminaPanelVisibility(true);
			}
			else
			{
				bool isMounted = base.IsMounted;
				if (isMounted)
				{
					base.Layout(null, true);
				}
			}
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x000BFFB8 File Offset: 0x000BE1B8
		private void UpdateStaminaDebugInfo(ClientEntityStatValue staminaEntityStat, float previousValue)
		{
			bool staminaDebugInfo = this.InGameView.InGame.Instance.App.Settings.StaminaDebugInfo;
			if (staminaDebugInfo)
			{
				double num = (double)staminaEntityStat.Value;
				double num2 = (double)(staminaEntityStat.Value - previousValue);
				this._debugStaminaValue.Text = num.ToString("0.00");
				this._debugStaminaRegenRate.Text = num2.ToString("0.00");
				this._debugStaminaAlertState.Text = this.AlertType.ToString();
				this._debugStaminaInfo.Visible = true;
				this._debugStaminaInfo.Layout(new Rectangle?(this._debugStaminaInfo.Parent.RectangleAfterPadding), true);
			}
			else
			{
				bool visible = this._debugStaminaInfo.Visible;
				if (visible)
				{
					this._debugStaminaInfo.Visible = false;
					this._debugStaminaInfo.Layout(null, true);
				}
			}
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x000C00B8 File Offset: 0x000BE2B8
		public void ResetState()
		{
			this._staminaBarProgress.Value = (this._staminaRenderValue = (this._staminaRenderValueTarget = 0f));
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x000C00E8 File Offset: 0x000BE2E8
		public void OnEffectAdded(int effectIndex)
		{
			Dictionary<string, int> entityEffectIndicesByIds = this.InGameView.InGame.Instance.EntityStoreModule.EntityEffectIndicesByIds;
			int num;
			bool flag = entityEffectIndicesByIds.TryGetValue("Stamina_Broken", out num) && effectIndex == num;
			if (flag)
			{
				this.SetDepletion(true);
			}
			else
			{
				int num2;
				bool flag2 = entityEffectIndicesByIds.TryGetValue("Stamina_Error_State", out num2) && effectIndex == num2;
				if (flag2)
				{
					this._isErrorActive = true;
					this.PlaySound("STAMINA_ERROR_STATE");
				}
			}
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x000C0168 File Offset: 0x000BE368
		public void OnEffectRemoved(int effectIndex)
		{
			Dictionary<string, int> entityEffectIndicesByIds = this.InGameView.InGame.Instance.EntityStoreModule.EntityEffectIndicesByIds;
			int num;
			bool flag = entityEffectIndicesByIds.TryGetValue("Stamina_Broken", out num) && effectIndex == num;
			if (flag)
			{
				this.SetDepletion(false);
			}
			else
			{
				int num2;
				bool flag2 = entityEffectIndicesByIds.TryGetValue("Stamina_Error_State", out num2) && effectIndex == num2;
				if (flag2)
				{
					this._isErrorActive = false;
				}
			}
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x000C01DC File Offset: 0x000BE3DC
		private void SetDepletion(bool depleted)
		{
			this._staminaBar.Background = (depleted ? this._staminaBarBackgroundDepleted : this._staminaBarBackground);
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.InGameView.UpdateStaminaPanelVisibility(true);
			}
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x000C0220 File Offset: 0x000BE420
		private int PlaySound(string soundEventId)
		{
			uint soundEventIndex;
			bool flag = this.Interface.App.Engine.Audio.ResourceManager.WwiseEventIds.TryGetValue(soundEventId, out soundEventIndex);
			int result;
			if (flag)
			{
				result = this.InGameView.InGame.Instance.AudioModule.PlayLocalSoundEvent(soundEventIndex);
			}
			else
			{
				StaminaPanelComponent.Logger.Warn("Could not load sound: {0}", soundEventId);
				result = -1;
			}
			return result;
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x000C0290 File Offset: 0x000BE490
		private int StopSound(int playbackId)
		{
			bool flag = playbackId != -1;
			if (flag)
			{
				this.InGameView.InGame.Instance.AudioModule.ActionOnEvent(playbackId, 0);
			}
			return -1;
		}

		// Token: 0x04001F69 RID: 8041
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04001F6A RID: 8042
		public const string StaminaDepletedEffectId = "Stamina_Broken";

		// Token: 0x04001F6B RID: 8043
		public const string StaminaErrorEffectId = "Stamina_Error_State";

		// Token: 0x04001F6C RID: 8044
		private const string StaminaErrorWWiseId = "STAMINA_ERROR_STATE";

		// Token: 0x04001F6D RID: 8045
		private const string StaminaLowAlertWWiseId = "STAMINA_LOW_ALERT";

		// Token: 0x04001F6E RID: 8046
		private const string StaminaOverdrawnAlertWWiseId = "STAMINA_EXHAUSTED_ALERT";

		// Token: 0x04001F6F RID: 8047
		private const float DEFAULT_ANIMATION_SPEED = 10f;

		// Token: 0x04001F70 RID: 8048
		private const int NO_PLAYBACK_ID = -1;

		// Token: 0x04001F71 RID: 8049
		public readonly InGameView InGameView;

		// Token: 0x04001F72 RID: 8050
		private Group _staminaBar;

		// Token: 0x04001F73 RID: 8051
		private ProgressBar _staminaBarProgress;

		// Token: 0x04001F74 RID: 8052
		private ProgressBar _staminaBarProgressDrain;

		// Token: 0x04001F75 RID: 8053
		private PatchStyle _staminaBarBackground;

		// Token: 0x04001F76 RID: 8054
		private PatchStyle _staminaBarBackgroundDepleted;

		// Token: 0x04001F77 RID: 8055
		private PatchStyle _staminaBarBackgroundError;

		// Token: 0x04001F78 RID: 8056
		private PatchStyle _staminaBarBackgroundErrorOutline;

		// Token: 0x04001F79 RID: 8057
		private PatchStyle _staminaBarBackgroundOverdrawn;

		// Token: 0x04001F7A RID: 8058
		private PatchStyle _staminaBarBackgroundOverdrawnOutline;

		// Token: 0x04001F7B RID: 8059
		private Group _staminaBarError;

		// Token: 0x04001F7C RID: 8060
		private Group _staminaBarErrorOutline;

		// Token: 0x04001F7D RID: 8061
		private Group _debugStaminaInfo;

		// Token: 0x04001F7E RID: 8062
		private Label _debugStaminaValue;

		// Token: 0x04001F7F RID: 8063
		private Label _debugStaminaRegenRate;

		// Token: 0x04001F80 RID: 8064
		private Label _debugStaminaAlertState;

		// Token: 0x04001F81 RID: 8065
		private float _flashOutlineRenderValue;

		// Token: 0x04001F82 RID: 8066
		private float _staminaErrorRenderValue;

		// Token: 0x04001F83 RID: 8067
		private float _staminaRenderValue;

		// Token: 0x04001F84 RID: 8068
		private float _staminaRenderValueTarget;

		// Token: 0x04001F85 RID: 8069
		private float _depletionTimer;

		// Token: 0x04001F86 RID: 8070
		private float _flashTimer;

		// Token: 0x04001F87 RID: 8071
		private bool _isErrorActive;

		// Token: 0x04001F88 RID: 8072
		private float _progressBarAnimationSpeed;

		// Token: 0x04001F89 RID: 8073
		private float _drainProgressBarAnimationSpeed;

		// Token: 0x04001F8A RID: 8074
		private float _drainProgressTime;

		// Token: 0x04001F8B RID: 8075
		private float _errorAnimationSpeed;

		// Token: 0x04001F8C RID: 8076
		private float _progressBarErrorThreshold;

		// Token: 0x04001F8D RID: 8077
		private float _defaultErrorFlashTimer;

		// Token: 0x04001F8E RID: 8078
		private float _defaultStaminaErrorRenderValue;

		// Token: 0x04001F8F RID: 8079
		private float _minimumDrainValue;

		// Token: 0x04001F90 RID: 8080
		private bool _isLowAlert;

		// Token: 0x04001F91 RID: 8081
		private int _lowAlertPlaybackId = -1;

		// Token: 0x04001F92 RID: 8082
		private float _lowAlertOutlineOpacityStart;

		// Token: 0x04001F93 RID: 8083
		private float _lowAlertOutlineOpacityTarget;

		// Token: 0x04001F94 RID: 8084
		private bool _isOutOfStamina;

		// Token: 0x04001F95 RID: 8085
		private float _lowAlertAnimationSpeed;

		// Token: 0x04001F96 RID: 8086
		private float _lowAlertFlashTimer;

		// Token: 0x04001F97 RID: 8087
		private bool _isOverdrawnAlert;

		// Token: 0x04001F98 RID: 8088
		private int _overdrawnAlertPlaybackId;

		// Token: 0x04001F99 RID: 8089
		private float _overdrawnAlertFlashTimer;

		// Token: 0x04001F9A RID: 8090
		private float _overdrawnAlertAnimationSpeed;
	}
}
