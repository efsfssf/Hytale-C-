using System;
using System.Linq;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B37 RID: 2871
	internal class ApplyForceInteraction : SimpleInteraction
	{
		// Token: 0x0600592A RID: 22826 RVA: 0x001B4ECC File Offset: 0x001B30CC
		public ApplyForceInteraction(int id, Interaction interaction) : base(id, interaction)
		{
			bool flag = interaction.VerticalClamp != null;
			if (flag)
			{
				this._verticalClamp = new FloatRange?(new FloatRange(interaction.VerticalClamp.InclusiveMin, interaction.VerticalClamp.InclusiveMax));
			}
			this._forces = Enumerable.ToArray<ApplyForceInteraction.ClientForce>(Enumerable.Select<AppliedForce, ApplyForceInteraction.ClientForce>(this.Interaction.Forces, (AppliedForce v) => new ApplyForceInteraction.ClientForce(v)));
		}

		// Token: 0x0600592B RID: 22827 RVA: 0x001B4F54 File Offset: 0x001B3154
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = firstRun || (this.Interaction.Duration > 0f && time < this.Interaction.Duration);
			if (flag)
			{
				gameInstance.CharacterControllerModule.MovementController.RaycastDistance = this.Interaction.RaycastDistance;
				gameInstance.CharacterControllerModule.MovementController.RaycastHeightOffset = this.Interaction.RaycastHeightOffset;
				gameInstance.CharacterControllerModule.MovementController.RaycastMode = this.Interaction.RaycastMode_;
				Quaternion quaternion = Quaternion.CreateFromAxisAngle(Vector3.Up, gameInstance.LocalPlayer.LookOrientation.Y);
				float num = gameInstance.LocalPlayer.LookOrientation.X;
				num = ((this._verticalClamp != null) ? this._verticalClamp.GetValueOrDefault().Clamp(num) : num);
				Quaternion quaternion2 = Quaternion.CreateFromAxisAngle(Vector3.Right, num);
				ChangeVelocityType changeType = this.Interaction.ChangeVelocityType_;
				Vector3 vector = gameInstance.LocalPlayer.Position;
				for (int i = 0; i < this._forces.Length; i++)
				{
					ApplyForceInteraction.ClientForce clientForce = this._forces[i];
					Vector3 vector2 = clientForce.Direction;
					bool adjustVertical = clientForce.AdjustVertical;
					if (adjustVertical)
					{
						Vector3.Transform(ref vector2, ref quaternion2, out vector2);
					}
					vector2 *= clientForce.Force;
					Vector3.Transform(ref vector2, ref quaternion, out vector2);
					bool debugDisplay = ApplyForceInteraction.DebugDisplay;
					if (debugDisplay)
					{
						gameInstance.DebugDisplayModule.AddForce(vector, vector2, new Vector3(0f, (float)(i + 1) * (1f / (float)this._forces.Length), 0f), 15f, true);
					}
					gameInstance.CharacterControllerModule.MovementController.VelocityChange(vector2.X, vector2.Y, vector2.Z, changeType, this.Interaction.VelocityConfig_);
					changeType = 0;
					vector += vector2;
				}
				context.State.State = 4;
				gameInstance.CharacterControllerModule.MovementController.ApplyMarioFallForce = false;
			}
			else
			{
				bool flag2 = time >= this.Interaction.GroundCheckDelay;
				bool flag3 = flag2 && this.Interaction.WaitForGround && gameInstance.CharacterControllerModule.MovementController.MovementStates.IsOnGround;
				bool flag4 = time >= this.Interaction.CollisionCheckDelay;
				bool flag5 = flag4 && this.Interaction.WaitForCollision && gameInstance.CharacterControllerModule.MovementController.MovementStates.IsEntityCollided;
				bool flag6 = (this.Interaction.RunTime <= 0f && !this.Interaction.WaitForCollision && !this.Interaction.WaitForGround) || (this.Interaction.RunTime > 0f && time >= this.Interaction.RunTime);
				context.State.ApplyForceState_ = 0;
				bool flag7 = flag3;
				if (flag7)
				{
					context.State.ApplyForceState_ = 1;
					context.State.State = 0;
					context.Jump(context.Labels[1]);
				}
				else
				{
					bool flag8 = flag5;
					if (flag8)
					{
						context.State.ApplyForceState_ = 2;
						context.State.State = 0;
						context.Jump(context.Labels[2]);
					}
					else
					{
						bool flag9 = flag6;
						if (flag9)
						{
							context.State.ApplyForceState_ = 3;
							context.State.State = 0;
							context.Jump(context.Labels[0]);
						}
						else
						{
							context.State.State = 4;
						}
					}
				}
				base.Tick0(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
			}
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x001B5328 File Offset: 0x001B3528
		public override void Handle(GameInstance gameInstance, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			base.Handle(gameInstance, firstRun, time, type, context);
			bool flag = context.State.State != 4;
			if (flag)
			{
				gameInstance.CharacterControllerModule.MovementController.RaycastDistance = 0f;
				gameInstance.CharacterControllerModule.MovementController.RaycastHeightOffset = 0f;
				gameInstance.CharacterControllerModule.MovementController.RaycastMode = 0;
			}
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x001B5398 File Offset: 0x001B3598
		public override void Compile(InteractionModule module, ClientRootInteraction.OperationsBuilder builder)
		{
			ClientRootInteraction.Label[] array = new ClientRootInteraction.Label[3];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = builder.CreateUnresolvedLabel();
			}
			builder.AddOperation(this.Id, array);
			ClientRootInteraction.Label label = builder.CreateUnresolvedLabel();
			ApplyForceInteraction.Resolve(module, builder, this.Interaction.Next, array[0], label);
			ApplyForceInteraction.Resolve(module, builder, this.Interaction.GroundNext, array[1], label);
			ApplyForceInteraction.Resolve(module, builder, this.Interaction.CollisionNext, array[2], label);
			builder.ResolveLabel(label);
		}

		// Token: 0x0600592E RID: 22830 RVA: 0x001B542C File Offset: 0x001B362C
		public static void Resolve(InteractionModule module, ClientRootInteraction.OperationsBuilder builder, int id, ClientRootInteraction.Label label, ClientRootInteraction.Label endLabel)
		{
			builder.ResolveLabel(label);
			bool flag = id != int.MinValue;
			if (flag)
			{
				ClientInteraction clientInteraction = module.Interactions[id];
				clientInteraction.Compile(module, builder);
			}
			builder.Jump(endLabel);
		}

		// Token: 0x04003756 RID: 14166
		public static bool DebugDisplay;

		// Token: 0x04003757 RID: 14167
		private readonly FloatRange? _verticalClamp;

		// Token: 0x04003758 RID: 14168
		private readonly ApplyForceInteraction.ClientForce[] _forces;

		// Token: 0x04003759 RID: 14169
		private const int NextLabelIndex = 0;

		// Token: 0x0400375A RID: 14170
		private const int GroundLabelIndex = 1;

		// Token: 0x0400375B RID: 14171
		private const int CollisionLabelIndex = 2;

		// Token: 0x02000F36 RID: 3894
		private class ClientForce
		{
			// Token: 0x06006891 RID: 26769 RVA: 0x0021BFFC File Offset: 0x0021A1FC
			public ClientForce(AppliedForce force)
			{
				this.Direction = new Vector3(force.Direction.X, force.Direction.Y, force.Direction.Z);
				this.AdjustVertical = force.AdjustVertical;
				this.Force = force.Force;
			}

			// Token: 0x04004A71 RID: 19057
			public readonly Vector3 Direction;

			// Token: 0x04004A72 RID: 19058
			public readonly bool AdjustVertical;

			// Token: 0x04004A73 RID: 19059
			public readonly float Force;
		}
	}
}
