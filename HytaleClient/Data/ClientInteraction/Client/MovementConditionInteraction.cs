using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.CharacterController;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B43 RID: 2883
	internal class MovementConditionInteraction : SimpleInteraction
	{
		// Token: 0x0600595C RID: 22876 RVA: 0x001B79A1 File Offset: 0x001B5BA1
		public MovementConditionInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x0600595D RID: 22877 RVA: 0x001B79B0 File Offset: 0x001B5BB0
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			CharacterControllerModule characterControllerModule = gameInstance.CharacterControllerModule;
			bool flag = characterControllerModule.ForwardsTimestamp > characterControllerModule.BackwardsTimestamp;
			bool flag2 = characterControllerModule.BackwardsTimestamp > characterControllerModule.ForwardsTimestamp;
			bool flag3 = characterControllerModule.LeftTimestamp > characterControllerModule.RightTimestamp;
			bool flag4 = characterControllerModule.RightTimestamp > characterControllerModule.LeftTimestamp;
			InteractionSyncData.MovementDirection movementDirection_ = 0;
			bool flag5 = flag && flag3 && this.Interaction.ForwardLeft != int.MinValue;
			if (flag5)
			{
				movementDirection_ = 5;
			}
			else
			{
				bool flag6 = flag && flag4 && this.Interaction.ForwardRight != int.MinValue;
				if (flag6)
				{
					movementDirection_ = 6;
				}
				else
				{
					bool flag7 = flag2 && flag3 && this.Interaction.BackLeft != int.MinValue;
					if (flag7)
					{
						movementDirection_ = 7;
					}
					else
					{
						bool flag8 = flag2 && flag4 && this.Interaction.BackRight != int.MinValue;
						if (flag8)
						{
							movementDirection_ = 8;
						}
						else
						{
							bool flag9 = flag && this.Interaction.Forward != int.MinValue;
							if (flag9)
							{
								movementDirection_ = 1;
							}
							else
							{
								bool flag10 = flag2 && this.Interaction.Back != int.MinValue;
								if (flag10)
								{
									movementDirection_ = 2;
								}
								else
								{
									bool flag11 = flag3 && this.Interaction.Left != int.MinValue;
									if (flag11)
									{
										movementDirection_ = 3;
									}
									else
									{
										bool flag12 = flag4 && this.Interaction.Right != int.MinValue;
										if (flag12)
										{
											movementDirection_ = 4;
										}
									}
								}
							}
						}
					}
				}
			}
			context.State.MovementDirection_ = movementDirection_;
			context.State.State = 0;
			switch (movementDirection_)
			{
			case 0:
				context.Jump(context.Labels[0]);
				break;
			case 1:
				context.Jump(context.Labels[1]);
				break;
			case 2:
				context.Jump(context.Labels[2]);
				break;
			case 3:
				context.Jump(context.Labels[3]);
				break;
			case 4:
				context.Jump(context.Labels[4]);
				break;
			case 5:
				context.Jump(context.Labels[5]);
				break;
			case 6:
				context.Jump(context.Labels[6]);
				break;
			case 7:
				context.Jump(context.Labels[7]);
				break;
			case 8:
				context.Jump(context.Labels[8]);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0600595E RID: 22878 RVA: 0x001B7C64 File Offset: 0x001B5E64
		public override void Compile(InteractionModule module, ClientRootInteraction.OperationsBuilder builder)
		{
			ClientRootInteraction.Label[] array = new ClientRootInteraction.Label[9];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = builder.CreateUnresolvedLabel();
			}
			builder.AddOperation(this.Id, array);
			ClientRootInteraction.Label label = builder.CreateUnresolvedLabel();
			MovementConditionInteraction.Resolve(module, builder, this.Interaction.Failed, array[0], label);
			MovementConditionInteraction.Resolve(module, builder, this.Interaction.Forward, array[1], label);
			MovementConditionInteraction.Resolve(module, builder, this.Interaction.Back, array[2], label);
			MovementConditionInteraction.Resolve(module, builder, this.Interaction.Left, array[3], label);
			MovementConditionInteraction.Resolve(module, builder, this.Interaction.Right, array[4], label);
			MovementConditionInteraction.Resolve(module, builder, this.Interaction.ForwardLeft, array[5], label);
			MovementConditionInteraction.Resolve(module, builder, this.Interaction.ForwardRight, array[6], label);
			MovementConditionInteraction.Resolve(module, builder, this.Interaction.BackLeft, array[7], label);
			MovementConditionInteraction.Resolve(module, builder, this.Interaction.BackRight, array[8], label);
			builder.ResolveLabel(label);
		}

		// Token: 0x0600595F RID: 22879 RVA: 0x001B7D84 File Offset: 0x001B5F84
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
	}
}
