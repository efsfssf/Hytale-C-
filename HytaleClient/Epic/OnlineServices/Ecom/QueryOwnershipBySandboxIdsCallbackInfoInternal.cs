using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200055E RID: 1374
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct QueryOwnershipBySandboxIdsCallbackInfoInternal : ICallbackInfoInternal, IGettable<QueryOwnershipBySandboxIdsCallbackInfo>, ISettable<QueryOwnershipBySandboxIdsCallbackInfo>, IDisposable
	{
		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x060023CF RID: 9167 RVA: 0x00034A70 File Offset: 0x00032C70
		// (set) Token: 0x060023D0 RID: 9168 RVA: 0x00034A88 File Offset: 0x00032C88
		public Result ResultCode
		{
			get
			{
				return this.m_ResultCode;
			}
			set
			{
				this.m_ResultCode = value;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x060023D1 RID: 9169 RVA: 0x00034A94 File Offset: 0x00032C94
		// (set) Token: 0x060023D2 RID: 9170 RVA: 0x00034AB5 File Offset: 0x00032CB5
		public object ClientData
		{
			get
			{
				object result;
				Helper.Get(this.m_ClientData, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_ClientData);
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x060023D3 RID: 9171 RVA: 0x00034AC8 File Offset: 0x00032CC8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x060023D4 RID: 9172 RVA: 0x00034AE0 File Offset: 0x00032CE0
		// (set) Token: 0x060023D5 RID: 9173 RVA: 0x00034B01 File Offset: 0x00032D01
		public EpicAccountId LocalUserId
		{
			get
			{
				EpicAccountId result;
				Helper.Get<EpicAccountId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x00034B14 File Offset: 0x00032D14
		// (set) Token: 0x060023D7 RID: 9175 RVA: 0x00034B3B File Offset: 0x00032D3B
		public SandboxIdItemOwnership[] SandboxIdItemOwnerships
		{
			get
			{
				SandboxIdItemOwnership[] result;
				Helper.Get<SandboxIdItemOwnershipInternal, SandboxIdItemOwnership>(this.m_SandboxIdItemOwnerships, out result, this.m_SandboxIdItemOwnershipsCount);
				return result;
			}
			set
			{
				Helper.Set<SandboxIdItemOwnership, SandboxIdItemOwnershipInternal>(ref value, ref this.m_SandboxIdItemOwnerships, out this.m_SandboxIdItemOwnershipsCount);
			}
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x00034B52 File Offset: 0x00032D52
		public void Set(ref QueryOwnershipBySandboxIdsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.SandboxIdItemOwnerships = other.SandboxIdItemOwnerships;
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x00034B8C File Offset: 0x00032D8C
		public void Set(ref QueryOwnershipBySandboxIdsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.SandboxIdItemOwnerships = other.Value.SandboxIdItemOwnerships;
			}
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x00034BFA File Offset: 0x00032DFA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_SandboxIdItemOwnerships);
		}

		// Token: 0x060023DB RID: 9179 RVA: 0x00034C21 File Offset: 0x00032E21
		public void Get(out QueryOwnershipBySandboxIdsCallbackInfo output)
		{
			output = default(QueryOwnershipBySandboxIdsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04000FB2 RID: 4018
		private Result m_ResultCode;

		// Token: 0x04000FB3 RID: 4019
		private IntPtr m_ClientData;

		// Token: 0x04000FB4 RID: 4020
		private IntPtr m_LocalUserId;

		// Token: 0x04000FB5 RID: 4021
		private IntPtr m_SandboxIdItemOwnerships;

		// Token: 0x04000FB6 RID: 4022
		private uint m_SandboxIdItemOwnershipsCount;
	}
}
