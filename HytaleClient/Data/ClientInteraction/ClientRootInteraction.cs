using System;
using System.Collections.Generic;
using HytaleClient.InGame;
using HytaleClient.InGame.Modules.Interaction;
using HytaleClient.Protocol;
using NLog;

namespace HytaleClient.Data.ClientInteraction
{
	// Token: 0x02000B12 RID: 2834
	internal class ClientRootInteraction
	{
		// Token: 0x060058AF RID: 22703 RVA: 0x001B1CC0 File Offset: 0x001AFEC0
		public ClientRootInteraction(int index, RootInteraction root)
		{
			this.Id = root.Id;
			this.Index = index;
			this.RootInteraction = root;
			this.Rules = new ClientInteraction.ClientInteractionRules(root.Rules);
			this.Tags = ((root.Tags != null) ? new HashSet<int>(root.Tags) : new HashSet<int>());
		}

		// Token: 0x060058B0 RID: 22704 RVA: 0x001B1D20 File Offset: 0x001AFF20
		public void Build(InteractionModule module)
		{
			bool flag = module.Interactions == null;
			if (!flag)
			{
				ClientRootInteraction.OperationsBuilder operationsBuilder = new ClientRootInteraction.OperationsBuilder();
				foreach (int num in this.RootInteraction.Interactions)
				{
					bool flag2 = num == int.MinValue;
					if (flag2)
					{
						ClientRootInteraction.Logger.Error(string.Format("Root interaction {0} contains an undefined interaction.", this.Index));
						return;
					}
					ClientInteraction clientInteraction = module.Interactions[num];
					clientInteraction.Compile(module, operationsBuilder);
				}
				this.Operations = operationsBuilder.Build();
			}
		}

		// Token: 0x04003727 RID: 14119
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04003728 RID: 14120
		public readonly string Id;

		// Token: 0x04003729 RID: 14121
		public readonly int Index;

		// Token: 0x0400372A RID: 14122
		public readonly RootInteraction RootInteraction;

		// Token: 0x0400372B RID: 14123
		public ClientRootInteraction.Operation[] Operations;

		// Token: 0x0400372C RID: 14124
		public readonly ClientInteraction.ClientInteractionRules Rules;

		// Token: 0x0400372D RID: 14125
		public readonly HashSet<int> Tags;

		// Token: 0x02000F27 RID: 3879
		public class OperationsBuilder
		{
			// Token: 0x0600684C RID: 26700 RVA: 0x0021ABB4 File Offset: 0x00218DB4
			public ClientRootInteraction.Label CreateLabel()
			{
				return new ClientRootInteraction.Label(this._operationList.Count);
			}

			// Token: 0x0600684D RID: 26701 RVA: 0x0021ABD8 File Offset: 0x00218DD8
			public ClientRootInteraction.Label CreateUnresolvedLabel()
			{
				return new ClientRootInteraction.Label(int.MinValue);
			}

			// Token: 0x0600684E RID: 26702 RVA: 0x0021ABF4 File Offset: 0x00218DF4
			public void ResolveLabel(ClientRootInteraction.Label label)
			{
				label.Index = this._operationList.Count;
			}

			// Token: 0x0600684F RID: 26703 RVA: 0x0021AC08 File Offset: 0x00218E08
			public void Jump(ClientRootInteraction.Label target)
			{
				this._operationList.Add(new ClientRootInteraction.JumpOperation(target));
			}

			// Token: 0x06006850 RID: 26704 RVA: 0x0021AC1D File Offset: 0x00218E1D
			public void AddOperation(int index)
			{
				this._operationList.Add(new ClientRootInteraction.DefaultOperation(index));
			}

			// Token: 0x06006851 RID: 26705 RVA: 0x0021AC32 File Offset: 0x00218E32
			public void AddOperation(int index, params ClientRootInteraction.Label[] labels)
			{
				this._operationList.Add(new ClientRootInteraction.LabelOperation(new ClientRootInteraction.DefaultOperation(index), labels));
			}

			// Token: 0x06006852 RID: 26706 RVA: 0x0021AC50 File Offset: 0x00218E50
			public ClientRootInteraction.Operation[] Build()
			{
				return this._operationList.ToArray();
			}

			// Token: 0x04004A4D RID: 19021
			private List<ClientRootInteraction.Operation> _operationList = new List<ClientRootInteraction.Operation>();
		}

		// Token: 0x02000F28 RID: 3880
		public class Label
		{
			// Token: 0x06006854 RID: 26708 RVA: 0x0021AC81 File Offset: 0x00218E81
			public Label(int index)
			{
				this.Index = index;
			}

			// Token: 0x06006855 RID: 26709 RVA: 0x0021AC94 File Offset: 0x00218E94
			public override string ToString()
			{
				return string.Format("{0}: {1}", "Index", this.Index);
			}

			// Token: 0x04004A4E RID: 19022
			public int Index;
		}

		// Token: 0x02000F29 RID: 3881
		public interface Operation
		{
			// Token: 0x06006856 RID: 26710
			Interaction.WaitForDataFrom GetWaitForDataFrom(GameInstance gameInstance);

			// Token: 0x06006857 RID: 26711
			void Tick(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context);

			// Token: 0x06006858 RID: 26712
			void Handle(GameInstance gameInstance, bool firstRun, float time, InteractionType type, InteractionContext context);

			// Token: 0x06006859 RID: 26713
			bool TryGetRules(GameInstance gameInstance, out ClientInteraction.ClientInteractionRules rules, out HashSet<int> tags);

			// Token: 0x0600685A RID: 26714
			void Revert(GameInstance gameInstance, InteractionType type, InteractionContext context);

			// Token: 0x0600685B RID: 26715
			void MatchServer(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context);
		}

		// Token: 0x02000F2A RID: 3882
		public interface InteractionWrapper : ClientRootInteraction.Operation
		{
			// Token: 0x0600685C RID: 26716
			ClientInteraction GetInteraction(InteractionModule module);
		}

		// Token: 0x02000F2B RID: 3883
		public class JumpOperation : ClientRootInteraction.Operation
		{
			// Token: 0x0600685D RID: 26717 RVA: 0x0021ACC0 File Offset: 0x00218EC0
			public JumpOperation(ClientRootInteraction.Label target)
			{
				this.Target = target;
			}

			// Token: 0x0600685E RID: 26718 RVA: 0x0021ACD4 File Offset: 0x00218ED4
			public Interaction.WaitForDataFrom GetWaitForDataFrom(GameInstance gameInstance)
			{
				return 2;
			}

			// Token: 0x0600685F RID: 26719 RVA: 0x0021ACE8 File Offset: 0x00218EE8
			public bool TryGetRules(GameInstance gameInstance, out ClientInteraction.ClientInteractionRules rules, out HashSet<int> tags)
			{
				rules = null;
				tags = null;
				return false;
			}

			// Token: 0x06006860 RID: 26720 RVA: 0x0021AD01 File Offset: 0x00218F01
			public void Tick(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
			{
				context.OperationCounter = this.Target.Index;
				context.State.State = 0;
			}

			// Token: 0x06006861 RID: 26721 RVA: 0x0021AD24 File Offset: 0x00218F24
			public void Handle(GameInstance gameInstance, bool firstRun, float time, InteractionType type, InteractionContext context)
			{
			}

			// Token: 0x06006862 RID: 26722 RVA: 0x0021AD27 File Offset: 0x00218F27
			public void Revert(GameInstance gameInstance, InteractionType type, InteractionContext context)
			{
			}

			// Token: 0x06006863 RID: 26723 RVA: 0x0021AD2A File Offset: 0x00218F2A
			public void MatchServer(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
			{
				context.OperationCounter = this.Target.Index;
				context.State.State = 0;
			}

			// Token: 0x06006864 RID: 26724 RVA: 0x0021AD50 File Offset: 0x00218F50
			public override string ToString()
			{
				return string.Format("{0}: {1}", "Target", this.Target);
			}

			// Token: 0x04004A4F RID: 19023
			public readonly ClientRootInteraction.Label Target;
		}

		// Token: 0x02000F2C RID: 3884
		public class LabelOperation : ClientRootInteraction.InteractionWrapper, ClientRootInteraction.Operation
		{
			// Token: 0x06006865 RID: 26725 RVA: 0x0021AD77 File Offset: 0x00218F77
			public LabelOperation(ClientRootInteraction.InteractionWrapper inner, ClientRootInteraction.Label[] labels)
			{
				this.Inner = inner;
				this.Labels = labels;
			}

			// Token: 0x06006866 RID: 26726 RVA: 0x0021AD90 File Offset: 0x00218F90
			public Interaction.WaitForDataFrom GetWaitForDataFrom(GameInstance gameInstance)
			{
				return this.Inner.GetWaitForDataFrom(gameInstance);
			}

			// Token: 0x06006867 RID: 26727 RVA: 0x0021ADAE File Offset: 0x00218FAE
			public void Tick(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
			{
				context.Labels = this.Labels;
				this.Inner.Tick(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
			}

			// Token: 0x06006868 RID: 26728 RVA: 0x0021ADD8 File Offset: 0x00218FD8
			public bool TryGetRules(GameInstance gameInstance, out ClientInteraction.ClientInteractionRules rules, out HashSet<int> tags)
			{
				return this.Inner.TryGetRules(gameInstance, out rules, out tags);
			}

			// Token: 0x06006869 RID: 26729 RVA: 0x0021ADF8 File Offset: 0x00218FF8
			public void Handle(GameInstance gameInstance, bool firstRun, float time, InteractionType type, InteractionContext context)
			{
				context.Labels = this.Labels;
				this.Inner.Handle(gameInstance, firstRun, time, type, context);
			}

			// Token: 0x0600686A RID: 26730 RVA: 0x0021AE1B File Offset: 0x0021901B
			public void Revert(GameInstance gameInstance, InteractionType type, InteractionContext context)
			{
				context.Labels = this.Labels;
				this.Inner.Revert(gameInstance, type, context);
			}

			// Token: 0x0600686B RID: 26731 RVA: 0x0021AE39 File Offset: 0x00219039
			public void MatchServer(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
			{
				context.Labels = this.Labels;
				this.Inner.MatchServer(gameInstance, clickType, hasAnyButtonClick, type, context);
			}

			// Token: 0x0600686C RID: 26732 RVA: 0x0021AE5C File Offset: 0x0021905C
			public ClientInteraction GetInteraction(InteractionModule module)
			{
				return this.Inner.GetInteraction(module);
			}

			// Token: 0x0600686D RID: 26733 RVA: 0x0021AE7C File Offset: 0x0021907C
			public override string ToString()
			{
				return string.Format("{0}: {1}, {2}: {3}", new object[]
				{
					"Inner",
					this.Inner,
					"Labels",
					this.Labels
				});
			}

			// Token: 0x04004A50 RID: 19024
			public readonly ClientRootInteraction.InteractionWrapper Inner;

			// Token: 0x04004A51 RID: 19025
			public readonly ClientRootInteraction.Label[] Labels;
		}

		// Token: 0x02000F2D RID: 3885
		public class DefaultOperation : ClientRootInteraction.InteractionWrapper, ClientRootInteraction.Operation
		{
			// Token: 0x0600686E RID: 26734 RVA: 0x0021AEC0 File Offset: 0x002190C0
			public DefaultOperation(int interactionId)
			{
				this.InteractionId = interactionId;
			}

			// Token: 0x0600686F RID: 26735 RVA: 0x0021AED4 File Offset: 0x002190D4
			public Interaction.WaitForDataFrom GetWaitForDataFrom(GameInstance gameInstance)
			{
				return gameInstance.InteractionModule.Interactions[this.InteractionId].Interaction.WaitForDataFrom_;
			}

			// Token: 0x06006870 RID: 26736 RVA: 0x0021AF04 File Offset: 0x00219104
			public bool TryGetRules(GameInstance gameInstance, out ClientInteraction.ClientInteractionRules rules, out HashSet<int> tags)
			{
				ClientInteraction clientInteraction = gameInstance.InteractionModule.Interactions[this.InteractionId];
				rules = clientInteraction.Rules;
				tags = clientInteraction.Tags;
				return true;
			}

			// Token: 0x06006871 RID: 26737 RVA: 0x0021AF3C File Offset: 0x0021913C
			public ClientInteraction GetInteraction(InteractionModule module)
			{
				return module.Interactions[this.InteractionId];
			}

			// Token: 0x06006872 RID: 26738 RVA: 0x0021AF5B File Offset: 0x0021915B
			public void Tick(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, bool firstRun, float time, InteractionType type, InteractionContext context)
			{
				gameInstance.InteractionModule.Interactions[this.InteractionId].Tick(gameInstance, clickType, hasAnyButtonClick, firstRun, time, type, context);
			}

			// Token: 0x06006873 RID: 26739 RVA: 0x0021AF81 File Offset: 0x00219181
			public void Handle(GameInstance gameInstance, bool firstRun, float time, InteractionType type, InteractionContext context)
			{
				gameInstance.InteractionModule.Interactions[this.InteractionId].Handle(gameInstance, firstRun, time, type, context);
			}

			// Token: 0x06006874 RID: 26740 RVA: 0x0021AFA3 File Offset: 0x002191A3
			public void Revert(GameInstance gameInstance, InteractionType type, InteractionContext context)
			{
				gameInstance.InteractionModule.Interactions[this.InteractionId].Revert(gameInstance, type, context);
			}

			// Token: 0x06006875 RID: 26741 RVA: 0x0021AFC1 File Offset: 0x002191C1
			public void MatchServer(GameInstance gameInstance, InteractionModule.ClickType clickType, bool hasAnyButtonClick, InteractionType type, InteractionContext context)
			{
				gameInstance.InteractionModule.Interactions[this.InteractionId].MatchServer(gameInstance, clickType, hasAnyButtonClick, type, context);
			}

			// Token: 0x06006876 RID: 26742 RVA: 0x0021AFE4 File Offset: 0x002191E4
			public override string ToString()
			{
				return string.Format("{0}: {1}", "InteractionId", this.InteractionId);
			}

			// Token: 0x04004A52 RID: 19026
			public readonly int InteractionId;
		}
	}
}
