using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Mods
{
	// Token: 0x02000330 RID: 816
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct EnumerateModsCallbackInfoInternal : ICallbackInfoInternal, IGettable<EnumerateModsCallbackInfo>, ISettable<EnumerateModsCallbackInfo>, IDisposable
	{
		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06001658 RID: 5720 RVA: 0x0002095C File Offset: 0x0001EB5C
		// (set) Token: 0x06001659 RID: 5721 RVA: 0x00020974 File Offset: 0x0001EB74
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

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x0600165A RID: 5722 RVA: 0x00020980 File Offset: 0x0001EB80
		// (set) Token: 0x0600165B RID: 5723 RVA: 0x000209A1 File Offset: 0x0001EBA1
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

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x0600165C RID: 5724 RVA: 0x000209B4 File Offset: 0x0001EBB4
		// (set) Token: 0x0600165D RID: 5725 RVA: 0x000209D5 File Offset: 0x0001EBD5
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

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x0600165E RID: 5726 RVA: 0x000209E8 File Offset: 0x0001EBE8
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x0600165F RID: 5727 RVA: 0x00020A00 File Offset: 0x0001EC00
		// (set) Token: 0x06001660 RID: 5728 RVA: 0x00020A18 File Offset: 0x0001EC18
		public ModEnumerationType Type
		{
			get
			{
				return this.m_Type;
			}
			set
			{
				this.m_Type = value;
			}
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x00020A22 File Offset: 0x0001EC22
		public void Set(ref EnumerateModsCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.LocalUserId = other.LocalUserId;
			this.ClientData = other.ClientData;
			this.Type = other.Type;
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00020A5C File Offset: 0x0001EC5C
		public void Set(ref EnumerateModsCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.LocalUserId = other.Value.LocalUserId;
				this.ClientData = other.Value.ClientData;
				this.Type = other.Value.Type;
			}
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00020ACA File Offset: 0x0001ECCA
		public void Dispose()
		{
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_ClientData);
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00020AE5 File Offset: 0x0001ECE5
		public void Get(out EnumerateModsCallbackInfo output)
		{
			output = default(EnumerateModsCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x040009C1 RID: 2497
		private Result m_ResultCode;

		// Token: 0x040009C2 RID: 2498
		private IntPtr m_LocalUserId;

		// Token: 0x040009C3 RID: 2499
		private IntPtr m_ClientData;

		// Token: 0x040009C4 RID: 2500
		private ModEnumerationType m_Type;
	}
}
