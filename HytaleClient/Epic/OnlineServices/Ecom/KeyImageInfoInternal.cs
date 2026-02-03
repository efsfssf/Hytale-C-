using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Ecom
{
	// Token: 0x0200053F RID: 1343
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	internal struct KeyImageInfoInternal : IGettable<KeyImageInfo>, ISettable<KeyImageInfo>, IDisposable
	{
		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x0600231B RID: 8987 RVA: 0x00033F58 File Offset: 0x00032158
		// (set) Token: 0x0600231C RID: 8988 RVA: 0x00033F79 File Offset: 0x00032179
		public Utf8String Type
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Type, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Type);
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x0600231D RID: 8989 RVA: 0x00033F8C File Offset: 0x0003218C
		// (set) Token: 0x0600231E RID: 8990 RVA: 0x00033FAD File Offset: 0x000321AD
		public Utf8String Url
		{
			get
			{
				Utf8String result;
				Helper.Get(this.m_Url, out result);
				return result;
			}
			set
			{
				Helper.Set(value, ref this.m_Url);
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x0600231F RID: 8991 RVA: 0x00033FC0 File Offset: 0x000321C0
		// (set) Token: 0x06002320 RID: 8992 RVA: 0x00033FD8 File Offset: 0x000321D8
		public uint Width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06002321 RID: 8993 RVA: 0x00033FE4 File Offset: 0x000321E4
		// (set) Token: 0x06002322 RID: 8994 RVA: 0x00033FFC File Offset: 0x000321FC
		public uint Height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		// Token: 0x06002323 RID: 8995 RVA: 0x00034006 File Offset: 0x00032206
		public void Set(ref KeyImageInfo other)
		{
			this.m_ApiVersion = 1;
			this.Type = other.Type;
			this.Url = other.Url;
			this.Width = other.Width;
			this.Height = other.Height;
		}

		// Token: 0x06002324 RID: 8996 RVA: 0x00034044 File Offset: 0x00032244
		public void Set(ref KeyImageInfo? other)
		{
			bool flag = other != null;
			if (flag)
			{
				this.m_ApiVersion = 1;
				this.Type = other.Value.Type;
				this.Url = other.Value.Url;
				this.Width = other.Value.Width;
				this.Height = other.Value.Height;
			}
		}

		// Token: 0x06002325 RID: 8997 RVA: 0x000340B9 File Offset: 0x000322B9
		public void Dispose()
		{
			Helper.Dispose(ref this.m_Type);
			Helper.Dispose(ref this.m_Url);
		}

		// Token: 0x06002326 RID: 8998 RVA: 0x000340D4 File Offset: 0x000322D4
		public void Get(out KeyImageInfo output)
		{
			output = default(KeyImageInfo);
			output.Set(ref this);
		}

		// Token: 0x04000F7D RID: 3965
		private int m_ApiVersion;

		// Token: 0x04000F7E RID: 3966
		private IntPtr m_Type;

		// Token: 0x04000F7F RID: 3967
		private IntPtr m_Url;

		// Token: 0x04000F80 RID: 3968
		private uint m_Width;

		// Token: 0x04000F81 RID: 3969
		private uint m_Height;
	}
}
