using System;
using Newtonsoft.Json;

namespace HytaleClient.Utils
{
	// Token: 0x020007CE RID: 1998
	public struct ScreenResolution
	{
		// Token: 0x06003427 RID: 13351 RVA: 0x0005381B File Offset: 0x00051A1B
		public ScreenResolution(int width, int height, bool fullscreen)
		{
			this.Width = width;
			this.Height = height;
			this.Fullscreen = fullscreen;
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x00053834 File Offset: 0x00051A34
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x00053858 File Offset: 0x00051A58
		public static ScreenResolution FromString(string value)
		{
			return JsonConvert.DeserializeObject<ScreenResolution>(value);
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x00053870 File Offset: 0x00051A70
		public bool Equals(ScreenResolution other)
		{
			return this.Width == other.Width && this.Height == other.Height;
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000538A4 File Offset: 0x00051AA4
		public bool FitsIn(int screenWidth, int screenHeight)
		{
			return this.Width <= screenWidth && this.Height <= screenHeight;
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x000538D0 File Offset: 0x00051AD0
		public string GetOptionName(bool native)
		{
			string arg = native ? "*" : "";
			return string.Format("{0} x {1}{2}", this.Width, this.Height, arg);
		}

		// Token: 0x0400175E RID: 5982
		public int Width;

		// Token: 0x0400175F RID: 5983
		public int Height;

		// Token: 0x04001760 RID: 5984
		public bool Fullscreen;
	}
}
