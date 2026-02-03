using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.Interaction
{
	// Token: 0x0200093A RID: 2362
	internal class InteractionEntry
	{
		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x060048B4 RID: 18612 RVA: 0x0011960B File Offset: 0x0011780B
		// (set) Token: 0x060048B5 RID: 18613 RVA: 0x00119614 File Offset: 0x00117814
		public InteractionSyncData ServerState
		{
			get
			{
				return this._serverState;
			}
			set
			{
				this._serverState = value;
				bool flag = this._serverState != null && (this._serverState.OperationCounter != this.State.OperationCounter || this._serverState.RootInteraction != this.State.RootInteraction);
				if (flag)
				{
					throw new Exception(string.Format("{0}: Client/Server desync {1} != {2}, {3} != {4}", new object[]
					{
						this.Index,
						this.State.OperationCounter,
						this._serverState.OperationCounter,
						this.State.RootInteraction,
						this._serverState.RootInteraction
					}));
				}
			}
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x001196E0 File Offset: 0x001178E0
		public InteractionEntry(int index, int operationCounter, int rootInteraction)
		{
			this.Index = index;
			this.State.OperationCounter = operationCounter;
			this.State.RootInteraction = rootInteraction;
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x00119760 File Offset: 0x00117960
		public int NextForkId()
		{
			int nextForkId = this._nextForkId;
			this._nextForkId = nextForkId + 1;
			return nextForkId;
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x00119784 File Offset: 0x00117984
		public int NextPredictedForkId()
		{
			int num = this._nextPredictedForkId - 1;
			this._nextPredictedForkId = num;
			return num;
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x001197A7 File Offset: 0x001179A7
		public void SetTimestamp(float shift)
		{
			this.Time.Start();
			this.TimeOffset = shift;
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x001197C0 File Offset: 0x001179C0
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}, {6}: {7}, {8}: {9}, {10}: {11}, {12}: {13}", new object[]
			{
				"Time",
				this.Time.Elapsed.TotalSeconds,
				"InteractionMetaStore",
				this.InteractionMetaStore,
				"State",
				this.State,
				"ServerState",
				this.ServerState,
				"WaitingForSyncData",
				this.WaitingForSyncData.Elapsed.TotalSeconds,
				"WaitingForServerFinished",
				this.WaitingForServerFinished.Elapsed.TotalSeconds,
				"WaitingForClientFinished",
				this.WaitingForClientFinished.Elapsed.TotalSeconds
			});
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x001198A8 File Offset: 0x00117AA8
		public int GetClientDataHashCode()
		{
			float progress = this.State.Progress;
			this.State.Progress = (float)((int)progress);
			int num = this.State.GetHashCode();
			bool flag = this.State.ForkCounts != null;
			if (flag)
			{
				foreach (KeyValuePair<InteractionType, int> keyValuePair in this.State.ForkCounts)
				{
					num = (num * 397 ^ keyValuePair.Key.GetHashCode());
					num = (num * 397 ^ keyValuePair.Value.GetHashCode());
				}
			}
			this.State.Progress = progress;
			return num;
		}

		// Token: 0x040024C1 RID: 9409
		public readonly int Index;

		// Token: 0x040024C2 RID: 9410
		public readonly InteractionMetaStore InteractionMetaStore = new InteractionMetaStore();

		// Token: 0x040024C3 RID: 9411
		public readonly Stopwatch Time = new Stopwatch();

		// Token: 0x040024C4 RID: 9412
		public float TimeOffset;

		// Token: 0x040024C5 RID: 9413
		public readonly InteractionSyncData State = new InteractionSyncData
		{
			State = 4
		};

		// Token: 0x040024C6 RID: 9414
		private InteractionSyncData _serverState;

		// Token: 0x040024C7 RID: 9415
		public readonly Stopwatch WaitingForSyncData = new Stopwatch();

		// Token: 0x040024C8 RID: 9416
		public readonly Stopwatch WaitingForServerFinished = new Stopwatch();

		// Token: 0x040024C9 RID: 9417
		public readonly Stopwatch WaitingForClientFinished = new Stopwatch();

		// Token: 0x040024CA RID: 9418
		private int _nextForkId;

		// Token: 0x040024CB RID: 9419
		private int _nextPredictedForkId;

		// Token: 0x040024CC RID: 9420
		public InteractionChain.CallState EnteredCallState;
	}
}
