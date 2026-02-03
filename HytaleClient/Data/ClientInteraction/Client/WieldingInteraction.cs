using System;
using System.Collections.Generic;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B4D RID: 2893
	internal class WieldingInteraction : ChargingInteraction
	{
		// Token: 0x06005994 RID: 22932 RVA: 0x001BA93D File Offset: 0x001B8B3D
		public WieldingInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005995 RID: 22933 RVA: 0x001BA94C File Offset: 0x001B8B4C
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			PlayerEntity localPlayer = gameInstance.LocalPlayer;
			bool hasStaminaDepletedEffect = localPlayer.HasStaminaDepletedEffect;
			if (hasStaminaDepletedEffect)
			{
				gameInstance.App.Interface.InGameView.Wielding = false;
				gameInstance.App.Interface.InGameView.UpdateStaminaPanelVisibility(true);
				gameInstance.App.Interface.InGameView.ReticleComponent.ResetClientEvent();
				localPlayer.ActionCameraSettings = null;
				localPlayer.ModelRenderer.SetCameraNodes(localPlayer.CameraSettings);
				context.State.State = 3;
				context.State.ChargeValue = 0f;
				bool flag = context.Labels != null;
				if (flag)
				{
					ClientRootInteraction.Label[] labels = context.Labels;
					Dictionary<float, int> chargedNext = this.Interaction.ChargedNext;
					context.Jump(labels[(chargedNext != null) ? chargedNext.Count : 0]);
				}
			}
			else
			{
				base.Tick0(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
				if (firstRun)
				{
					gameInstance.App.Interface.InGameView.Wielding = true;
					gameInstance.App.Interface.InGameView.UpdateStaminaPanelVisibility(true);
					gameInstance.App.Interface.InGameView.ReticleComponent.OnClientEvent(1, context.HeldItem.Id);
					CameraSettings cameraSettings = new CameraSettings(localPlayer.CameraSettings);
					for (int i = 0; i < cameraSettings.Yaw.TargetNodes.Length; i++)
					{
						cameraSettings.Yaw.TargetNodes[i] = 4;
					}
					localPlayer.ActionCameraSettings = cameraSettings;
					localPlayer.ModelRenderer.SetCameraNodes(cameraSettings);
				}
				else
				{
					bool flag2 = context.State.State == 0;
					if (flag2)
					{
						gameInstance.App.Interface.InGameView.Wielding = false;
						gameInstance.App.Interface.InGameView.UpdateStaminaPanelVisibility(true);
						gameInstance.App.Interface.InGameView.ReticleComponent.ResetClientEvent();
						localPlayer.ActionCameraSettings = null;
						localPlayer.ModelRenderer.SetCameraNodes(localPlayer.CameraSettings);
					}
				}
			}
		}
	}
}
