using System;

namespace Epic.OnlineServices.Lobby
{
	// Token: 0x02000373 RID: 883
	public struct Attribute
	{
		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x060017A3 RID: 6051 RVA: 0x00022814 File Offset: 0x00020A14
		// (set) Token: 0x060017A4 RID: 6052 RVA: 0x0002281C File Offset: 0x00020A1C
		public AttributeData? Data { get; set; }

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x060017A5 RID: 6053 RVA: 0x00022825 File Offset: 0x00020A25
		// (set) Token: 0x060017A6 RID: 6054 RVA: 0x0002282D File Offset: 0x00020A2D
		public LobbyAttributeVisibility Visibility { get; set; }

		// Token: 0x060017A7 RID: 6055 RVA: 0x00022836 File Offset: 0x00020A36
		internal void Set(ref AttributeInternal other)
		{
			this.Data = other.Data;
			this.Visibility = other.Visibility;
		}
	}
}
