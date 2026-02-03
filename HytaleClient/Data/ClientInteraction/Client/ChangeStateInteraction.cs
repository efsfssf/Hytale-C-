using System;
using System.Collections.Generic;
using HytaleClient.Data.Map;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B3C RID: 2876
	internal class ChangeStateInteraction : SimpleBlockInteraction
	{
		// Token: 0x0600593E RID: 22846 RVA: 0x001B5FCE File Offset: 0x001B41CE
		public ChangeStateInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x0600593F RID: 22847 RVA: 0x001B5FDC File Offset: 0x001B41DC
		protected override void InteractWithBlock(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context, BlockPosition targetBlockHit, int blockId)
		{
			ClientBlockType clientBlockType = gameInstance.MapModule.ClientBlockTypes[blockId];
			string text = null;
			Dictionary<int, string> statesReverse = clientBlockType.StatesReverse;
			if (statesReverse != null)
			{
				statesReverse.TryGetValue(blockId, out text);
			}
			text = (text ?? "default");
			string key;
			bool flag = this.Interaction.StateChanges.TryGetValue(text, out key) && clientBlockType.States != null;
			if (flag)
			{
				int blockId2;
				bool flag2 = clientBlockType.States.TryGetValue(key, out blockId2);
				if (flag2)
				{
					gameInstance.MapModule.SetClientBlock(targetBlockHit.X, targetBlockHit.Y, targetBlockHit.Z, blockId2);
					return;
				}
			}
			context.State.State = 3;
		}
	}
}
