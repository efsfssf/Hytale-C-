using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;

namespace HytaleClient.Data.ClientInteraction.Client
{
	// Token: 0x02000B3A RID: 2874
	internal class ChainingInteraction : ClientInteraction
	{
		// Token: 0x06005935 RID: 22837 RVA: 0x001B597C File Offset: 0x001B3B7C
		public ChainingInteraction(int id, Interaction interaction) : base(id, interaction)
		{
			bool flag = interaction.Flags != null;
			if (flag)
			{
				this._sortedFlagKags = Enumerable.ToArray<string>(interaction.Flags.Keys);
				Array.Sort<string>(this._sortedFlagKags);
				this._flagIndex = new Dictionary<string, int>();
				for (int i = 0; i < this._sortedFlagKags.Length; i++)
				{
					this._flagIndex.Add(this._sortedFlagKags[i], i);
				}
			}
		}

		// Token: 0x06005936 RID: 22838 RVA: 0x001B5A10 File Offset: 0x001B3C10
		protected override void Tick0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
		{
			bool flag = !firstRun;
			if (!flag)
			{
				int num = this._lastSequenceIndex;
				Stopwatch timeSinceLastAttack = this._timeSinceLastAttack;
				bool flag2 = this.Interaction.ChainId != null;
				if (flag2)
				{
					ChainingInteraction.ChainData chainData;
					bool flag3 = !ChainingInteraction.NamedSequenceData.TryGetValue(this.Interaction.ChainId, out chainData);
					if (flag3)
					{
						ChainingInteraction.NamedSequenceData.Add(this.Interaction.ChainId, chainData = new ChainingInteraction.ChainData());
					}
					num = chainData.LastSequenceIndex;
					timeSinceLastAttack = chainData.TimeSinceLastAttack;
				}
				bool flag4 = this.Interaction.ChainingAllowance > 0f && timeSinceLastAttack.Elapsed.TotalSeconds * (double)gameInstance.TimeDilationModifier > (double)this.Interaction.ChainingAllowance;
				if (flag4)
				{
					num = -1;
					bool flag5 = this.Interaction.ChainId != null;
					if (flag5)
					{
						ChainingInteraction.NamedSequenceData[this.Interaction.ChainId].CurrentFlag = null;
						ChainingInteraction.NamedSequenceData[this.Interaction.ChainId].Variable = null;
					}
				}
				bool flag6 = false;
				bool flag7 = this.Interaction.ChainId != null;
				if (flag7)
				{
					ChainingInteraction.ChainData chainData2 = ChainingInteraction.NamedSequenceData[this.Interaction.ChainId];
					int num2;
					bool flag8 = chainData2.CurrentFlag != null && this._flagIndex.TryGetValue(chainData2.CurrentFlag, out num2);
					if (flag8)
					{
						context.State.FlagIndex = num2;
						context.Jump(context.Labels[this.Interaction.ChainingNext.Length + num2]);
						flag6 = true;
					}
				}
				num++;
				bool flag9 = num >= this.Interaction.ChainingNext.Length;
				if (flag9)
				{
					num = 0;
				}
				bool flag10 = !flag6;
				if (flag10)
				{
					context.State.ChainingIndex = num;
					context.Jump(context.Labels[num]);
				}
				context.State.State = 0;
				timeSinceLastAttack.Restart();
				bool flag11 = this.Interaction.ChainId != null;
				if (flag11)
				{
					ChainingInteraction.ChainData chainData3 = ChainingInteraction.NamedSequenceData[this.Interaction.ChainId];
					chainData3.LastSequenceIndex = num;
					chainData3.CurrentFlag = null;
				}
				else
				{
					this._lastSequenceIndex = num;
				}
			}
		}

		// Token: 0x06005937 RID: 22839 RVA: 0x001B5C5C File Offset: 0x001B3E5C
		protected override void Revert0(GameInstance gameInstance, InteractionType type, InteractionContext context)
		{
			int num = this._lastSequenceIndex;
			bool flag = this.Interaction.ChainId != null;
			if (flag)
			{
				ChainingInteraction.ChainData chainData;
				bool flag2 = !ChainingInteraction.NamedSequenceData.TryGetValue(this.Interaction.ChainId, out chainData);
				if (flag2)
				{
					return;
				}
				num = chainData.LastSequenceIndex;
			}
			num--;
			bool flag3 = num < 0;
			if (flag3)
			{
				num = this.Interaction.ChainingNext.Length - 1;
			}
			bool flag4 = this.Interaction.ChainId != null;
			if (flag4)
			{
				ChainingInteraction.NamedSequenceData[this.Interaction.ChainId].LastSequenceIndex = num;
			}
		}

		// Token: 0x06005938 RID: 22840 RVA: 0x001B5CFC File Offset: 0x001B3EFC
		protected override void MatchServer0(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
		{
			bool flag = context.ServerData.State != 4;
			if (flag)
			{
				throw new Exception("Server in unexpected state");
			}
			this.Tick0(gameInstance, clickType, hasAnyButtonClick, true, 0f, type, context);
		}

		// Token: 0x06005939 RID: 22841 RVA: 0x001B5D40 File Offset: 0x001B3F40
		public override void Compile(InteractionModule module, ClientRootInteraction.OperationsBuilder builder)
		{
			int num = this.Interaction.ChainingNext.Length;
			string[] sortedFlagKags = this._sortedFlagKags;
			ClientRootInteraction.Label[] array = new ClientRootInteraction.Label[num + ((sortedFlagKags != null) ? sortedFlagKags.Length : 0)];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = builder.CreateUnresolvedLabel();
			}
			builder.AddOperation(this.Id, array);
			ClientRootInteraction.Label label = builder.CreateUnresolvedLabel();
			for (int j = 0; j < this.Interaction.ChainingNext.Length; j++)
			{
				builder.ResolveLabel(array[j]);
				ClientInteraction clientInteraction = module.Interactions[this.Interaction.ChainingNext[j]];
				clientInteraction.Compile(module, builder);
				builder.Jump(label);
			}
			bool flag = this._sortedFlagKags != null;
			if (flag)
			{
				for (int k = 0; k < this._sortedFlagKags.Length; k++)
				{
					string key = this._sortedFlagKags[k];
					builder.ResolveLabel(array[this.Interaction.ChainingNext.Length + k]);
					ClientInteraction clientInteraction2 = module.Interactions[this.Interaction.Flags[key]];
					clientInteraction2.Compile(module, builder);
					builder.Jump(label);
				}
			}
			builder.ResolveLabel(label);
		}

		// Token: 0x0400375C RID: 14172
		private readonly Stopwatch _timeSinceLastAttack = new Stopwatch();

		// Token: 0x0400375D RID: 14173
		private int _lastSequenceIndex = -1;

		// Token: 0x0400375E RID: 14174
		public static Dictionary<string, ChainingInteraction.ChainData> NamedSequenceData = new Dictionary<string, ChainingInteraction.ChainData>();

		// Token: 0x0400375F RID: 14175
		private string[] _sortedFlagKags;

		// Token: 0x04003760 RID: 14176
		private Dictionary<string, int> _flagIndex;

		// Token: 0x02000F38 RID: 3896
		public class ChainData
		{
			// Token: 0x04004A76 RID: 19062
			public readonly Stopwatch TimeSinceLastAttack = new Stopwatch();

			// Token: 0x04004A77 RID: 19063
			public int LastSequenceIndex = -1;

			// Token: 0x04004A78 RID: 19064
			public string CurrentFlag;

			// Token: 0x04004A79 RID: 19065
			public string Variable;
		}
	}
}
