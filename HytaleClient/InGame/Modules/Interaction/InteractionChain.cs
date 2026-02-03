using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HytaleClient.Data.ClientInteraction;
using HytaleClient.Data.Items;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Interaction
{
	// Token: 0x02000938 RID: 2360
	internal class InteractionChain : InteractionModule.ChainSyncStorage
	{
		// Token: 0x170011A7 RID: 4519
		// (get) Token: 0x0600486D RID: 18541 RVA: 0x00117E12 File Offset: 0x00116012
		// (set) Token: 0x0600486E RID: 18542 RVA: 0x00117E1A File Offset: 0x0011601A
		public InteractionState ServerState { get; set; } = 4;

		// Token: 0x0600486F RID: 18543 RVA: 0x00117E24 File Offset: 0x00116024
		public InteractionChain(InteractionType type, InteractionContext context, InteractionChainData chainData, ClientRootInteraction rootInteraction, int initialSlot, ClientItemStack initialItem, Action onCompletion) : this(null, null, type, context, chainData, rootInteraction, initialSlot, initialItem, onCompletion)
		{
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x00117E48 File Offset: 0x00116048
		public InteractionChain(ForkedChainId forkedChainId, ForkedChainId baseForkedChainId, InteractionType type, InteractionContext context, InteractionChainData chainData, ClientRootInteraction rootInteraction, int initialSlot, ClientItemStack initialItem, Action onCompletion)
		{
			this.Type = type;
			this.ChainData = chainData;
			this.OnCompletion = onCompletion;
			this.ForkedChainId = forkedChainId;
			this.BaseForkedChainId = baseForkedChainId;
			this.RootInteraction = rootInteraction;
			this.InitialRootInteraction = rootInteraction;
			this.Context = context;
			this.InitialSlot = initialSlot;
			this.InitialItem = initialItem;
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x00117F44 File Offset: 0x00116144
		public void UpdateClientState(GameInstance gameInstance, InteractionModule.ClickType clickType)
		{
			bool flag = this.ClientState != 4;
			if (!flag)
			{
				bool flag2 = this.OperationCounter >= this.RootInteraction.Operations.Length;
				if (flag2)
				{
					this.ClientState = this.FinalState;
				}
				else
				{
					InteractionEntry interactionEntry;
					bool flag3 = !this.GetInteraction(this.OperationIndex, out interactionEntry);
					if (!flag3)
					{
						InteractionState state = interactionEntry.State.State;
						InteractionState interactionState = state;
						if (interactionState != null && interactionState != 4)
						{
							this.ClientState = 3;
						}
						else
						{
							this.ClientState = 4;
						}
					}
				}
			}
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x00117FD8 File Offset: 0x001161D8
		public void RemoveForksForEntry(InteractionModule module, int entry)
		{
			bool flag = this.ForkedChains.Count == 0;
			if (!flag)
			{
				List<ulong> list = Enumerable.ToList<ulong>(Enumerable.Select<KeyValuePair<ulong, InteractionChain>, ulong>(Enumerable.Where<KeyValuePair<ulong, InteractionChain>>(this.ForkedChains, (KeyValuePair<ulong, InteractionChain> e) => (int)(e.Key >> 32) == entry && e.Value.Predicted), (KeyValuePair<ulong, InteractionChain> e) => e.Key));
				foreach (ulong key in list)
				{
					module.RevertChain(this.ForkedChains[key], 0);
					this.ForkedChains.Remove(key);
				}
			}
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x001180B0 File Offset: 0x001162B0
		public void NextOperationIndex()
		{
			this.OperationIndex++;
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x001180C4 File Offset: 0x001162C4
		public bool FindForkedChain(GameInstance gameInstance, ForkedChainId chainId, InteractionChainData data, out InteractionChain ret)
		{
			ulong key = InteractionChain.ForkedIdToIndex(chainId);
			ulong num;
			bool flag = this._forkedChainsMap.TryGetValue(key, out num);
			if (flag)
			{
				key = num;
			}
			bool flag2 = this.ForkedChains.TryGetValue(key, out ret) || data == null;
			bool result;
			if (flag2)
			{
				result = (ret != null);
			}
			else
			{
				InteractionEntry interactionEntry;
				bool flag3 = !this.GetInteraction(chainId.EntryIndex, out interactionEntry);
				if (flag3)
				{
					result = false;
				}
				else
				{
					int rootInteraction = interactionEntry.State.RootInteraction;
					int operationCounter = interactionEntry.State.OperationCounter;
					ClientRootInteraction clientRootInteraction = gameInstance.InteractionModule.RootInteractions[rootInteraction];
					ClientRootInteraction.Operation operation = clientRootInteraction.Operations[operationCounter];
					ClientRootInteraction.InteractionWrapper interactionWrapper = operation as ClientRootInteraction.InteractionWrapper;
					bool flag4 = interactionWrapper != null;
					if (flag4)
					{
						ClientInteraction interaction = interactionWrapper.GetInteraction(gameInstance.InteractionModule);
						this.Context.InitEntry(this, interactionEntry, gameInstance);
						ret = interaction.MapForkChain(this.Context, data);
						this.Context.DeinitEntry(this, interactionEntry, gameInstance);
						bool flag5 = ret != null;
						if (flag5)
						{
							this._forkedChainsMap.Add(key, InteractionChain.ForkedIdToIndex(ret.BaseForkedChainId));
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x001181F8 File Offset: 0x001163F8
		public bool GetForkedChain(ForkedChainId chainId, out InteractionChain ret)
		{
			ulong key = InteractionChain.ForkedIdToIndex(chainId);
			ulong num;
			bool flag = this._forkedChainsMap.TryGetValue(key, out num);
			if (flag)
			{
				key = num;
			}
			return this.ForkedChains.TryGetValue(key, out ret);
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x00118234 File Offset: 0x00116434
		public bool RemoveTempForkedChain(ForkedChainId chainId, out InteractionChain.TempChain ret)
		{
			ulong key = InteractionChain.ForkedIdToIndex(chainId);
			ulong num;
			bool flag = this._forkedChainsMap.TryGetValue(key, out num);
			if (flag)
			{
				key = num;
			}
			bool flag2 = this._tempForkedChainData.TryGetValue(key, out ret);
			return flag2 && this._tempForkedChainData.Remove(key);
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x00118288 File Offset: 0x00116488
		public InteractionChain.TempChain GetTempForkedChain(ForkedChainId chainId)
		{
			ulong key = InteractionChain.ForkedIdToIndex(chainId);
			InteractionChain.TempChain tempChain;
			bool flag = !this._tempForkedChainData.TryGetValue(key, out tempChain);
			if (flag)
			{
				tempChain = new InteractionChain.TempChain();
				this._tempForkedChainData.Add(key, tempChain);
			}
			return tempChain;
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x001182CD File Offset: 0x001164CD
		internal void PutForkedChain(ForkedChainId chainId, InteractionChain val)
		{
			this.NewForks.Add(val);
			this.ForkedChains.Add(InteractionChain.ForkedIdToIndex(chainId), val);
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x001182F0 File Offset: 0x001164F0
		internal void RemoveForkedChain(InteractionModule module, ForkedChainId chainId)
		{
			ulong key = InteractionChain.ForkedIdToIndex(chainId);
			InteractionChain interactionChain;
			bool flag = this.ForkedChains.TryGetValue(key, out interactionChain);
			if (flag)
			{
				module.RevertChain(interactionChain, 0);
				interactionChain.ClientState = 3;
				interactionChain.ServerState = 3;
			}
			this.ForkedChains.Remove(key);
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x00118340 File Offset: 0x00116540
		public bool GetInteraction(int index, out InteractionEntry ret)
		{
			index -= this._operationIndexOffset;
			bool flag = index < 0 || index >= this._interactions.Count;
			bool result;
			if (flag)
			{
				ret = null;
				result = false;
			}
			else
			{
				ret = this._interactions[index];
				result = (ret != null);
			}
			return result;
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x00118394 File Offset: 0x00116594
		public InteractionEntry GetOrCreateInteractionEntry(int index)
		{
			int num = index - this._operationIndexOffset;
			InteractionEntry interactionEntry = (num < this._interactions.Count) ? this._interactions[num] : null;
			bool flag = interactionEntry == null;
			if (flag)
			{
				bool flag2 = num != this._interactions.Count;
				if (flag2)
				{
					throw new Exception(string.Format("Trying to add interaction entry at a weird location: {0} {1}", num, this._interactions.Count));
				}
				interactionEntry = new InteractionEntry(index, this.OperationCounter, this.RootInteraction.Index);
				this._interactions.Add(interactionEntry);
			}
			return interactionEntry;
		}

		// Token: 0x0600487C RID: 18556 RVA: 0x0011843C File Offset: 0x0011663C
		public void ShiftInteractionEntryOffset(int amount)
		{
			this._interactions.Clear();
			bool flag = this._operationIndexOffset > 0;
			if (flag)
			{
				this._operationIndexOffset -= amount;
			}
		}

		// Token: 0x0600487D RID: 18557 RVA: 0x00118474 File Offset: 0x00116674
		public void RemoveInteractionEntry(int index)
		{
			int num = index - this._operationIndexOffset;
			bool flag = num != 0;
			if (flag)
			{
				throw new Exception("Trying to remove out of order");
			}
			this.PreviousInteractionEntry = this._interactions[num];
			this._interactions.RemoveAt(num);
			this._operationIndexOffset++;
		}

		// Token: 0x0600487E RID: 18558 RVA: 0x001184CC File Offset: 0x001166CC
		public void PutInteractionSyncData(int index, InteractionSyncData data)
		{
			index -= this._tempSyncDataOffset;
			bool flag = index < this._tempSyncData.Count;
			if (flag)
			{
				this._tempSyncData[index] = data;
			}
			else
			{
				bool flag2 = index == this._tempSyncData.Count;
				if (!flag2)
				{
					throw new Exception(string.Format("Temp sync data send out of order: {0} {1}", index, this._tempSyncData.Count));
				}
				this._tempSyncData.Add(data);
			}
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x00118554 File Offset: 0x00116754
		public void SyncFork(GameInstance gameInstance, SyncInteractionChain packet)
		{
			ForkedChainId forkedId = packet.ForkedId;
			while (forkedId.ForkedId != null)
			{
				forkedId = forkedId.ForkedId;
			}
			InteractionChain chain;
			bool flag = this.FindForkedChain(gameInstance, forkedId, packet.Data, out chain);
			if (flag)
			{
				gameInstance.InteractionModule.Sync(chain, packet);
			}
			else
			{
				bool flag2 = packet.OverrideRootInteraction != int.MinValue && packet.ForkedId != null;
				bool flag3 = flag2;
				if (flag3)
				{
					gameInstance.InteractionModule.Handle(packet);
				}
				else
				{
					InteractionChain.TempChain tempForkedChain = this.GetTempForkedChain(forkedId);
					gameInstance.InteractionModule.Sync(tempForkedChain, packet);
				}
			}
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x001185F4 File Offset: 0x001167F4
		public InteractionSyncData RemoveInteractionSyncData(int index)
		{
			index -= this._tempSyncDataOffset;
			bool flag = index != 0;
			InteractionSyncData result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = this._tempSyncData.Count == 0;
				if (flag2)
				{
					result = null;
				}
				else
				{
					InteractionSyncData interactionSyncData = this._tempSyncData[index];
					bool flag3 = interactionSyncData != null;
					if (flag3)
					{
						this._tempSyncData.RemoveAt(index);
						this._tempSyncDataOffset++;
					}
					result = interactionSyncData;
				}
			}
			return result;
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x00118668 File Offset: 0x00116868
		public InteractionSyncData GetInteractionSyncData(int index)
		{
			index -= this._tempSyncDataOffset;
			bool flag = index != 0;
			InteractionSyncData result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = this._tempSyncData.Count == 0;
				if (flag2)
				{
					result = null;
				}
				else
				{
					result = this._tempSyncData[index];
				}
			}
			return result;
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x06004882 RID: 18562 RVA: 0x001186B2 File Offset: 0x001168B2
		public bool HasTempSyncData
		{
			get
			{
				return this._tempSyncData.Count > 0;
			}
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x001186C4 File Offset: 0x001168C4
		public void UpdateSyncPosition(int index)
		{
			bool flag = this._tempSyncDataOffset == index;
			if (flag)
			{
				this._tempSyncDataOffset = index + 1;
			}
			else
			{
				bool flag2 = index > this._tempSyncDataOffset;
				if (flag2)
				{
					throw new Exception(string.Format("Temp sync data send out of order: {0} {1}", index, this._tempSyncData.Count));
				}
			}
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x00118720 File Offset: 0x00116920
		public void CopyTempFrom(InteractionChain.TempChain tempChain)
		{
			this.ServerState = tempChain.ServerState;
			this._tempSyncData.AddRange(tempChain.TempSyncData);
			foreach (KeyValuePair<ulong, InteractionChain.TempChain> keyValuePair in tempChain.TempForkedChainData)
			{
				this._tempForkedChainData.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x001187A8 File Offset: 0x001169A8
		public int GetCallDepth()
		{
			return this._callStack.Count;
		}

		// Token: 0x06004886 RID: 18566 RVA: 0x001187C8 File Offset: 0x001169C8
		public void PushRoot(ClientRootInteraction nextInteraction)
		{
			InteractionChain.CallState callState = new InteractionChain.CallState(this.RootInteraction, this.OperationCounter);
			this._callStack.Add(callState);
			this._interactions[this.OperationIndex - this._operationIndexOffset].EnteredCallState = callState;
			this.OperationCounter = 0;
			this.RootInteraction = nextInteraction;
		}

		// Token: 0x06004887 RID: 18567 RVA: 0x00118824 File Offset: 0x00116A24
		public void PopRoot()
		{
			InteractionChain.CallState callState = this._callStack[this._callStack.Count - 1];
			this._callStack.RemoveAt(this._callStack.Count - 1);
			this.RootInteraction = callState.RootInteraction;
			this.OperationCounter = callState.OperationCounter + 1;
		}

		// Token: 0x06004888 RID: 18568 RVA: 0x00118880 File Offset: 0x00116A80
		public bool ConsumeFirstRun()
		{
			bool firstRun = this._firstRun;
			this._firstRun = false;
			return firstRun;
		}

		// Token: 0x06004889 RID: 18569 RVA: 0x001188A4 File Offset: 0x00116AA4
		public bool ConsumeDesync()
		{
			bool desync = this.Desync;
			this.Desync = false;
			return desync;
		}

		// Token: 0x0600488A RID: 18570 RVA: 0x001188C8 File Offset: 0x00116AC8
		public void ClearIncompleteSyncData()
		{
			int num = this._tempSyncData.FindIndex((InteractionSyncData v) => v.State == 4);
			bool flag = num != -1;
			if (flag)
			{
				this._tempSyncData.RemoveRange(num, this._tempSyncData.Count - num);
			}
		}

		// Token: 0x0600488B RID: 18571 RVA: 0x00118928 File Offset: 0x00116B28
		public void ClearSyncData()
		{
			this._tempSyncData.Clear();
		}

		// Token: 0x0600488C RID: 18572 RVA: 0x00118938 File Offset: 0x00116B38
		internal int LowestInteractionIndex()
		{
			return Enumerable.Min(Enumerable.Select<InteractionEntry, int>(this._interactions, (InteractionEntry v) => v.Index));
		}

		// Token: 0x0600488D RID: 18573 RVA: 0x00118979 File Offset: 0x00116B79
		public void ClearInteractions()
		{
			this._interactions.Clear();
			this._operationIndexOffset = 0;
		}

		// Token: 0x0600488E RID: 18574 RVA: 0x00118990 File Offset: 0x00116B90
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}, {10}: {11}", new object[]
			{
				"Time",
				this.Time.Elapsed.TotalSeconds,
				"ChainData",
				this.ChainData,
				"WaitingForServerFinished",
				this.WaitingForServerFinished.Elapsed.TotalSeconds,
				"TotalSeconds",
				this.WaitingForClientFinished,
				"ClientState",
				this.ClientState,
				"ServerState",
				this.ServerState
			});
		}

		// Token: 0x0600488F RID: 18575 RVA: 0x00118A4C File Offset: 0x00116C4C
		public static ulong ForkedIdToIndex(ForkedChainId chainId)
		{
			return (ulong)((long)chainId.EntryIndex << 32 | ((long)chainId.SubIndex & (long)((ulong)-1)));
		}

		// Token: 0x0400248D RID: 9357
		public readonly InteractionType Type;

		// Token: 0x0400248E RID: 9358
		public readonly InteractionChainData ChainData;

		// Token: 0x0400248F RID: 9359
		public int ChainId;

		// Token: 0x04002490 RID: 9360
		public readonly ForkedChainId ForkedChainId;

		// Token: 0x04002491 RID: 9361
		public readonly ForkedChainId BaseForkedChainId;

		// Token: 0x04002492 RID: 9362
		public bool Predicted;

		// Token: 0x04002493 RID: 9363
		public bool Desync;

		// Token: 0x04002494 RID: 9364
		public readonly InteractionContext Context;

		// Token: 0x04002495 RID: 9365
		public int ServerCompleteIndex;

		// Token: 0x04002496 RID: 9366
		public readonly Dictionary<ulong, InteractionChain> ForkedChains = new Dictionary<ulong, InteractionChain>();

		// Token: 0x04002497 RID: 9367
		private readonly Dictionary<ulong, InteractionChain.TempChain> _tempForkedChainData = new Dictionary<ulong, InteractionChain.TempChain>();

		// Token: 0x04002498 RID: 9368
		private readonly Dictionary<ulong, ulong> _forkedChainsMap = new Dictionary<ulong, ulong>();

		// Token: 0x04002499 RID: 9369
		public readonly List<InteractionChain> NewForks = new List<InteractionChain>();

		// Token: 0x0400249A RID: 9370
		public ClientRootInteraction InitialRootInteraction;

		// Token: 0x0400249B RID: 9371
		public ClientRootInteraction RootInteraction;

		// Token: 0x0400249C RID: 9372
		public int OperationCounter;

		// Token: 0x0400249D RID: 9373
		private readonly List<InteractionChain.CallState> _callStack = new List<InteractionChain.CallState>();

		// Token: 0x0400249E RID: 9374
		public bool ServerCancelled = false;

		// Token: 0x0400249F RID: 9375
		public int OperationIndex;

		// Token: 0x040024A0 RID: 9376
		private int _operationIndexOffset;

		// Token: 0x040024A1 RID: 9377
		private readonly List<InteractionEntry> _interactions = new List<InteractionEntry>();

		// Token: 0x040024A2 RID: 9378
		private readonly List<InteractionSyncData> _tempSyncData = new List<InteractionSyncData>();

		// Token: 0x040024A3 RID: 9379
		public bool ServerAck = false;

		// Token: 0x040024A4 RID: 9380
		public InteractionEntry PreviousInteractionEntry;

		// Token: 0x040024A5 RID: 9381
		private int _tempSyncDataOffset;

		// Token: 0x040024A6 RID: 9382
		public readonly Stopwatch Time = new Stopwatch();

		// Token: 0x040024A7 RID: 9383
		public readonly Stopwatch WaitingForServerFinished = new Stopwatch();

		// Token: 0x040024A8 RID: 9384
		public readonly Stopwatch WaitingForClientFinished = new Stopwatch();

		// Token: 0x040024A9 RID: 9385
		public InteractionState ClientState = 4;

		// Token: 0x040024AB RID: 9387
		public InteractionState FinalState = 0;

		// Token: 0x040024AC RID: 9388
		public Action OnCompletion;

		// Token: 0x040024AD RID: 9389
		public readonly int InitialSlot;

		// Token: 0x040024AE RID: 9390
		public readonly ClientItemStack InitialItem;

		// Token: 0x040024AF RID: 9391
		public bool SentInitialState;

		// Token: 0x040024B0 RID: 9392
		public float TimeShift;

		// Token: 0x040024B1 RID: 9393
		private bool _firstRun = true;

		// Token: 0x040024B2 RID: 9394
		internal bool SkipChainOnClick;

		// Token: 0x02000E17 RID: 3607
		public class CallState
		{
			// Token: 0x060066D7 RID: 26327 RVA: 0x002160C4 File Offset: 0x002142C4
			public CallState(ClientRootInteraction rootInteraction, int operationCounter)
			{
				this.RootInteraction = rootInteraction;
				this.OperationCounter = operationCounter;
			}

			// Token: 0x04004521 RID: 17697
			public readonly ClientRootInteraction RootInteraction;

			// Token: 0x04004522 RID: 17698
			public readonly int OperationCounter;
		}

		// Token: 0x02000E18 RID: 3608
		public class TempChain : InteractionModule.ChainSyncStorage
		{
			// Token: 0x1700145C RID: 5212
			// (get) Token: 0x060066D8 RID: 26328 RVA: 0x002160DC File Offset: 0x002142DC
			// (set) Token: 0x060066D9 RID: 26329 RVA: 0x002160E4 File Offset: 0x002142E4
			public InteractionState ServerState { get; set; } = 4;

			// Token: 0x060066DA RID: 26330 RVA: 0x002160F0 File Offset: 0x002142F0
			public InteractionChain.TempChain GetTempForkedChain(ForkedChainId chainId)
			{
				ulong key = InteractionChain.ForkedIdToIndex(chainId);
				InteractionChain.TempChain tempChain;
				bool flag = !this.TempForkedChainData.TryGetValue(key, out tempChain);
				if (flag)
				{
					tempChain = new InteractionChain.TempChain();
					this.TempForkedChainData.Add(key, tempChain);
				}
				return tempChain;
			}

			// Token: 0x060066DB RID: 26331 RVA: 0x00216138 File Offset: 0x00214338
			public void PutInteractionSyncData(int index, InteractionSyncData data)
			{
				bool flag = index < this.TempSyncData.Count;
				if (flag)
				{
					this.TempSyncData[index] = data;
				}
				else
				{
					bool flag2 = index == this.TempSyncData.Count;
					if (!flag2)
					{
						throw new Exception(string.Format("Temp sync data send out of order: {0} {1}", index, this.TempSyncData.Count));
					}
					this.TempSyncData.Add(data);
				}
			}

			// Token: 0x060066DC RID: 26332 RVA: 0x002161B4 File Offset: 0x002143B4
			public void SyncFork(GameInstance gameInstance, SyncInteractionChain packet)
			{
				ForkedChainId forkedId = packet.ForkedId;
				while (forkedId.ForkedId != null)
				{
					forkedId = forkedId.ForkedId;
				}
				InteractionChain.TempChain tempForkedChain = this.GetTempForkedChain(forkedId);
				gameInstance.InteractionModule.Sync(tempForkedChain, packet);
			}

			// Token: 0x04004523 RID: 17699
			public readonly Dictionary<ulong, InteractionChain.TempChain> TempForkedChainData = new Dictionary<ulong, InteractionChain.TempChain>();

			// Token: 0x04004524 RID: 17700
			public readonly List<InteractionSyncData> TempSyncData = new List<InteractionSyncData>();
		}
	}
}
