using System;
using HytaleClient.Audio;
using HytaleClient.Data.Map;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Math;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B44 RID: 2884
	internal class PickBlockInteraction : SimpleBlockInteraction
	{
		// Token: 0x06005960 RID: 22880 RVA: 0x001B7DC6 File Offset: 0x001B5FC6
		public PickBlockInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005961 RID: 22881 RVA: 0x001B7DD4 File Offset: 0x001B5FD4
		protected override void InteractWithBlock(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context, BlockPosition targetBlockHit, int blockId)
		{
			bool flag = gameInstance.GameMode == 1;
			if (flag)
			{
				bool hasActiveTool = gameInstance.BuilderToolsModule.HasActiveTool;
				if (hasActiveTool)
				{
					gameInstance.BuilderToolsModule.OnPickBlockInteraction();
				}
				else
				{
					ClientBlockType blockType = gameInstance.MapModule.ClientBlockTypes[blockId];
					gameInstance.Engine.RunOnMainThread(gameInstance, delegate
					{
						gameInstance.InventoryModule.AddAndSelectHotbarItem(blockType.Item);
					}, true, false);
					int value;
					bool flag2 = gameInstance.ServerSettings.BlockSoundSets[blockType.BlockSoundSetIndex].SoundEventIndices.TryGetValue(6, out value);
					if (flag2)
					{
						uint networkWwiseId = ResourceManager.GetNetworkWwiseId(value);
						gameInstance.AudioModule.PlaySoundEvent(networkWwiseId, new Vector3((float)targetBlockHit.X + 0.5f, (float)targetBlockHit.Y + 0.5f, (float)targetBlockHit.Z + 0.5f), Vector3.Zero);
					}
				}
			}
		}
	}
}
