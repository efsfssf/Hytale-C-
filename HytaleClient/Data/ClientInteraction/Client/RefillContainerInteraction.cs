using System;
using System.Collections.Generic;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Collision;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B47 RID: 2887
	internal class RefillContainerInteraction : SimpleInstantInteraction
	{
		// Token: 0x06005983 RID: 22915 RVA: 0x001BA1F4 File Offset: 0x001B83F4
		public RefillContainerInteraction(int id, Interaction interaction) : base(id, interaction)
		{
			this.refillBlocks = new HashSet<int>(interaction.RefillBlocks);
		}

		// Token: 0x06005984 RID: 22916 RVA: 0x001BA214 File Offset: 0x001B8414
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			Ray lookRay = gameInstance.CameraModule.GetLookRay();
			CollisionModule.BlockRaycastOptions @default = CollisionModule.BlockRaycastOptions.Default;
			@default.Block.BlockWhitelist = this.refillBlocks;
			CollisionModule.BlockResult default2 = CollisionModule.BlockResult.Default;
			bool flag = gameInstance.CollisionModule.FindTargetBlock(ref lookRay, ref @default, ref default2);
			if (flag)
			{
				bool flag2 = !this.refillBlocks.Contains(default2.BlockId);
				if (!flag2)
				{
					context.State.BlockPosition_ = new BlockPosition(default2.Block.X, default2.Block.Y, default2.Block.Z);
				}
			}
		}

		// Token: 0x04003775 RID: 14197
		private readonly HashSet<int> refillBlocks;
	}
}
