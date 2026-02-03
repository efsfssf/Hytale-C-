using System;

namespace HytaleClient.Graphics.Programs
{
	// Token: 0x02000A52 RID: 2642
	public struct Attrib
	{
		// Token: 0x06005433 RID: 21555 RVA: 0x001825B6 File Offset: 0x001807B6
		public Attrib(uint index, string name)
		{
			this.Index = index;
			this.Name = name;
		}

		// Token: 0x04002F13 RID: 12051
		public readonly uint Index;

		// Token: 0x04002F14 RID: 12052
		public readonly string Name;
	}
}
