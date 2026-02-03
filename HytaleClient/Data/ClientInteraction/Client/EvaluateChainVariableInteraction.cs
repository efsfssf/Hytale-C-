using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B40 RID: 2880
	internal class EvaluateChainVariableInteraction : ClientInteraction
	{
		// Token: 0x0600594F RID: 22863 RVA: 0x001B73F0 File Offset: 0x001B55F0
		public EvaluateChainVariableInteraction(int id, Interaction interaction) : base(id, interaction)
		{
			bool flag = interaction.ChainVariableNext != null;
			if (flag)
			{
				this._sortedNextKeys = Enumerable.ToArray<string>(interaction.ChainVariableNext.Keys);
				Array.Sort<string>(this._sortedNextKeys);
				this._nextIndexes = new Dictionary<string, int>();
				for (int i = 0; i < this._sortedNextKeys.Length; i++)
				{
					this._nextIndexes.Add(this._sortedNextKeys[i], i);
				}
			}
		}

		// Token: 0x06005950 RID: 22864 RVA: 0x001B7474 File Offset: 0x001B5674
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = !firstRun || this.Interaction.ChainId == null;
			if (!flag)
			{
				ChainingInteraction.ChainData chainData;
				bool flag2 = ChainingInteraction.NamedSequenceData.TryGetValue(this.Interaction.ChainId, out chainData);
				if (flag2)
				{
					int num;
					bool flag3 = chainData.Variable != null && this._nextIndexes.TryGetValue(chainData.Variable, out num);
					if (flag3)
					{
						context.State.State = 0;
						context.State.ChainVariableNextIndex = num;
						context.Jump(context.Labels[num]);
						chainData.Variable = null;
						return;
					}
				}
				context.State.State = 3;
				context.Jump(context.Labels[this._sortedNextKeys.Length]);
			}
		}

		// Token: 0x06005951 RID: 22865 RVA: 0x001B753C File Offset: 0x001B573C
		public override void Compile(InteractionModule module, ClientRootInteraction.OperationsBuilder builder)
		{
			bool flag = this._sortedNextKeys != null;
			if (flag)
			{
				ClientRootInteraction.Label[] array = new ClientRootInteraction.Label[this._sortedNextKeys.Length + 1];
				ClientRootInteraction.Label label = builder.CreateUnresolvedLabel();
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = builder.CreateUnresolvedLabel();
				}
				builder.AddOperation(this.Id, array);
				builder.Jump(label);
				for (int j = 0; j < this._sortedNextKeys.Length; j++)
				{
					string key = this._sortedNextKeys[j];
					builder.ResolveLabel(array[j]);
					ClientInteraction clientInteraction = module.Interactions[this.Interaction.ChainVariableNext[key]];
					clientInteraction.Compile(module, builder);
					builder.Jump(label);
				}
				int num = this._sortedNextKeys.Length;
				builder.ResolveLabel(array[num]);
				bool flag2 = this.Interaction.Failed != int.MinValue;
				if (flag2)
				{
					ClientInteraction clientInteraction2 = module.Interactions[this.Interaction.Failed];
					clientInteraction2.Compile(module, builder);
				}
				builder.Jump(label);
				builder.ResolveLabel(label);
			}
		}

		// Token: 0x06005952 RID: 22866 RVA: 0x001B766D File Offset: 0x001B586D
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
		}

		// Token: 0x06005953 RID: 22867 RVA: 0x001B7670 File Offset: 0x001B5870
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			context.State.State = context.ServerData.State;
			InteractionState state = context.State.State;
			InteractionState interactionState = state;
			if (interactionState != null)
			{
				if (interactionState == 3)
				{
					context.Jump(context.Labels[this._sortedNextKeys.Length]);
				}
			}
			else
			{
				context.Jump(context.Labels[context.State.ChainVariableNextIndex]);
			}
		}

		// Token: 0x04003769 RID: 14185
		private string[] _sortedNextKeys;

		// Token: 0x0400376A RID: 14186
		private Dictionary<string, int> _nextIndexes;
	}
}
