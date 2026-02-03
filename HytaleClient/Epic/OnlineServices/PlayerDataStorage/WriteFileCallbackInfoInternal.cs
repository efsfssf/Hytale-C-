using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.PlayerDataStorage
{
	// Token: 0x02000327 RID: 807
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct WriteFileCallbackInfoInternal : ICallbackInfoInternal, IGettable<WriteFileCallbackInfo>, ISettable<WriteFileCallbackInfo>, IDisposable
	{
		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x0600160F RID: 5647 RVA: 0x000201B4 File Offset: 0x0001E3B4
		// (set) Token: 0x06001610 RID: 5648 RVA: 0x000201CC File Offset: 0x0001E3CC
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

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001611 RID: 5649 RVA: 0x000201D8 File Offset: 0x0001E3D8
		// (set) Token: 0x06001612 RID: 5650 RVA: 0x000201F9 File Offset: 0x0001E3F9
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

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06001613 RID: 5651 RVA: 0x0002020C File Offset: 0x0001E40C
		public IntPtr ClientDataAddress
		{
			get
			{
				return this.m_ClientData;
			}
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x06001614 RID: 5652 RVA: 0x00020224 File Offset: 0x0001E424
		// (set) Token: 0x06001615 RID: 5653 RVA: 0x00020245 File Offset: 0x0001E445
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

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001616 RID: 5654 RVA: 0x00020258 File Offset: 0x0001E458
		// (set) Token: 0x06001617 RID: 5655 RVA: 0x00020279 File Offset: 0x0001E479
		public Utf8String Filename
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Filename, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Filename);
			}
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x00020289 File Offset: 0x0001E489
		public void Set(ref WriteFileCallbackInfo other)
		{
			this.ResultCode = other.ResultCode;
			this.ClientData = other.ClientData;
			this.LocalUserId = other.LocalUserId;
			this.Filename = other.Filename;
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x000202C0 File Offset: 0x0001E4C0
		public void Set(ref WriteFileCallbackInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.ResultCode = other.Value.ResultCode;
				this.ClientData = other.Value.ClientData;
				this.LocalUserId = other.Value.LocalUserId;
				this.Filename = other.Value.Filename;
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x0002032E File Offset: 0x0001E52E
		public void Dispose()
		{
			Helper.Dispose(ref this.m_ClientData);
			Helper.Dispose(ref this.m_LocalUserId);
			Helper.Dispose(ref this.m_Filename);
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x00020355 File Offset: 0x0001E555
		public void Get(out WriteFileCallbackInfo output)
		{
			output = default(WriteFileCallbackInfo);
			output.Set(ref this);
		}

		// Token: 0x0400099A RID: 2458
		private Result m_ResultCode;

		// Token: 0x0400099B RID: 2459
		private IntPtr m_ClientData;

		// Token: 0x0400099C RID: 2460
		private IntPtr m_LocalUserId;

		// Token: 0x0400099D RID: 2461
		private IntPtr m_Filename;
	}
}
