using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Entities.Projectile;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B46 RID: 2886
	internal class ProjectileInteraction : SimpleInstantInteraction
	{
		// Token: 0x06005980 RID: 22912 RVA: 0x001BA096 File Offset: 0x001B8296
		public ProjectileInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005981 RID: 22913 RVA: 0x001BA0A4 File Offset: 0x001B82A4
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			Entity entity = context.Entity;
			Vector3 position = entity.Position;
			position.Y += entity.EyeOffset;
			context.State.AttackerPos = position.ToPositionPacket();
			context.State.AttackerRot = entity.LookOrientation.ToDirectionPacket();
			Guid guid = Guid.NewGuid();
			context.State.GeneratedUUID = guid;
			Quaternion rotation = Quaternion.CreateFromYawPitchRoll(entity.LookOrientation.Yaw, entity.LookOrientation.Pitch, 0f);
			Vector3 direction = Vector3.Transform(Vector3.Forward, rotation);
			context.InstanceStore.Projectile = PredictedProjectile.Spawn(guid, gameInstance, this.Interaction.ProjectileConfig_, gameInstance.LocalPlayer, position, direction);
		}

		// Token: 0x06005982 RID: 22914 RVA: 0x001BA164 File Offset: 0x001B8364
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			base.Revert0(gameInstance, type, context);
			bool flag = context.InstanceStore.Projectile != null;
			if (flag)
			{
				int toDespawn = context.InstanceStore.Projectile.NetworkId;
				gameInstance.Engine.RunOnMainThread(context.InstanceStore.Projectile, delegate
				{
					gameInstance.EntityStoreModule.Despawn(toDespawn);
				}, true, false);
			}
		}
	}
}
