using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005B7 RID: 1463
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendCustomInviteCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendCustomInviteCallbackInfo>, ISettable<SendCustomInviteCallbackInfo>, IDisposable
	{
		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x060025FD RID: 9725 RVA: 0x00037C98 File Offset: 0x00035E98
		// (set) Token: 0x060025FE RID: 9726 RVA: 0x00037CB0 File Offset: 0x00035EB0
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

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x060025FF RID: 9727 RVA: 0x00037CBC File Offset: 0x00035EBC
		// (set) Token: 0x06002600 RID: 9728 RVA: 0x00037CDD File Offset: 0x00035EDD
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

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06002601 RID: 9729 RVA: 0x00037CF0 File Offset: 0x00035EF0
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x06002602 RID: 9730 RVA: 0x00037D08 File Offset: 0x00035F08
		// (set) Token: 0x06002603 RID: 9731 RVA: 0x00037D29 File Offset: 0x00035F29
		public ProductUserId LocalUserId
		{
			get
			{
				ProductUserId result;
				Helper.Get<ProductUserId>(this.m_LocalUserId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_LocalUserId);
			}
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06002604 RID: 9732 RVA: 0x00037D3C File Offset: 0x00035F3C
		// (set) Token: 0x06002605 RID: 9733 RVA: 0x00037D63 File Offset: 0x00035F63
		public ProductUserId[] TargetUserIds
		{
			get
			{
				ProductUserId[] result;
				Helper.GetHandle<ProductUserId>(this.m_TargetUserIds, out result, this.m_TargetUserIdsCount);
				return result;
			}
			set
			{
				Helper.Set<ProductUserId>(value, ref this.m_TargetUserIds, out this.m_TargetUserIdsCount);
			}
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x00037D79 File Offset: 0x00035F79
		public void Set(ref SendCustomInviteCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.TargetUserIds = other.TargetUserIds;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x00037DB0 File Offset: 0x00035FB0
		public void Set(ref SendCustomInviteCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetUserIds = other.Value.TargetUserIds;
			}
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x00037E1E File Offset: 0x0003601E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetUserIds);
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x00037E45 File Offset: 0x00036045
		public void Get(out SendCustomInviteCallbackInfo output)
		{
			output = default(SendCustomInviteCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001070 RID: 4208
		private Result m_ResultCode;

		// Token: 0x04001071 RID: 4209
		private IntPtr m_ClientData;

		// Token: 0x04001072 RID: 4210
		private IntPtr m_LocalUserId;

		// Token: 0x04001073 RID: 4211
		private IntPtr m_TargetUserIds;

		// Token: 0x04001074 RID: 4212
		private uint m_TargetUserIdsCount;
	}
}
