using System;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Entities;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.None
{
	// Token: 0x02000B2D RID: 2861
	internal class EffectConditionInteraction : SimpleInstantInteraction
	{
		// Token: 0x06005906 RID: 22790 RVA: 0x001B4160 File Offset: 0x001B2360
		public EffectConditionInteraction(int id, Interaction interaction) : base(id, interaction)
		{
		}

		// Token: 0x06005907 RID: 22791 RVA: 0x001B416C File Offset: 0x001B236C
		protected override void FirstRun(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			Entity entity;
			bool flag = ClientInteraction.TryGetEntity(gameInstance, context, this.Interaction.EntityTarget, out entity);
			if (flag)
			{
				for (int i = 0; i < this.Interaction.EntityEffects.Length; i++)
				{
					Interaction.Match match_ = this.Interaction.Match_;
					Interaction.Match match = match_;
					if (match != null)
					{
						if (match == 1)
						{
							bool flag2 = entity.HasEffect(this.Interaction.EntityEffects[i]);
							if (flag2)
							{
								context.State.State = 3;
								break;
							}
						}
					}
					else
					{
						bool flag3 = !entity.HasEffect(this.Interaction.EntityEffects[i]);
						if (flag3)
						{
							context.State.State = 3;
							break;
						}
					}
				}
			}
		}
	}
}
