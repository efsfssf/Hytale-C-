using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.CustomInvites
{
	// Token: 0x020005BB RID: 1467
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct SendCustomNativeInviteRequestedCallbackInfoInternal : ICallbackInfoInternal, IGettable<SendCustomNativeInviteRequestedCallbackInfo>, ISettable<SendCustomNativeInviteRequestedCallbackInfo>, IDisposable
	{
		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06002621 RID: 9761 RVA: 0x00038008 File Offset: 0x00036208
		// (set) Token: 0x06002622 RID: 9762 RVA: 0x00038029 File Offset: 0x00036229
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

		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x06002623 RID: 9763 RVA: 0x0003803C File Offset: 0x0003623C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06002624 RID: 9764 RVA: 0x00038054 File Offset: 0x00036254
		// (set) Token: 0x06002625 RID: 9765 RVA: 0x0003806C File Offset: 0x0003626C
		public ulong UiEventId
		{
			get
			{
				return this.m_UiEventId;
			}
			set
			{
				this.m_UiEventId = value;
			}
		}

		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x06002626 RID: 9766 RVA: 0x00038078 File Offset: 0x00036278
		// (set) Token: 0x06002627 RID: 9767 RVA: 0x00038099 File Offset: 0x00036299
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

		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x06002628 RID: 9768 RVA: 0x000380AC File Offset: 0x000362AC
		// (set) Token: 0x06002629 RID: 9769 RVA: 0x000380CD File Offset: 0x000362CD
		public Utf8String TargetNativeAccountType
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TargetNativeAccountType, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetNativeAccountType);
			}
		}

		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x0600262A RID: 9770 RVA: 0x000380E0 File Offset: 0x000362E0
		// (set) Token: 0x0600262B RID: 9771 RVA: 0x00038101 File Offset: 0x00036301
		public Utf8String TargetUserNativeAccountId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_TargetUserNativeAccountId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_TargetUserNativeAccountId);
			}
		}

		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x0600262C RID: 9772 RVA: 0x00038114 File Offset: 0x00036314
		// (set) Token: 0x0600262D RID: 9773 RVA: 0x00038135 File Offset: 0x00036335
		public Utf8String InviteId
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_InviteId, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_InviteId);
			}
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x00038148 File Offset: 0x00036348
		public void Set(ref SendCustomNativeInviteRequestedCallbackInfo other)
		{
			this.ClientData = other.ClientData;
			this.UiEventId = other.UiEventId;
			this.LocalUserId = other.LocalUserId;
			this.TargetNativeAccountType = other.TargetNativeAccountType;
			this.TargetUserNativeAccountId = other.TargetUserNativeAccountId;
			this.InviteId = other.InviteId;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000381A4 File Offset: 0x000363A4
		public void Set(ref SendCustomNativeInviteRequestedCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ClientData = other.Value.ClientData;
				this.UiEventId = other.Value.UiEventId;
				this.LocalUserId = other.Value.LocalUserId;
				this.TargetNativeAccountType = other.Value.TargetNativeAccountType;
				this.TargetUserNativeAccountId = other.Value.TargetUserNativeAccountId;
				this.InviteId = other.Value.InviteId;
			}
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x0003823F File Offset: 0x0003643F
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_TargetNativeAccountType);
			Helper.Dispose(ref this.m_TargetUserNativeAccountId);
			Helper.Dispose(ref this.m_InviteId);
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x0003827E File Offset: 0x0003647E
		public void Get(out SendCustomNativeInviteRequestedCallbackInfo output)
		{
			output = default(SendCustomNativeInviteRequestedCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x04001081 RID: 4225
		private IntPtr m_ClientData;

		// Token: 0x04001082 RID: 4226
		private ulong m_UiEventId;

		// Token: 0x04001083 RID: 4227
		private IntPtr m_LocalUserId;

		// Token: 0x04001084 RID: 4228
		private IntPtr m_TargetNativeAccountType;

		// Token: 0x04001085 RID: 4229
		private IntPtr m_TargetUserNativeAccountId;

		// Token: 0x04001086 RID: 4230
		private IntPtr m_InviteId;
	}
}
