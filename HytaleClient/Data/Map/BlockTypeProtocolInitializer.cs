using System;
using HytaleClient.Graphics;
using HytaleClient.Protocol;

namespace HytaleClient.Data.Map
{
	// Token: 0x02000ADE RID: 2782
	internal static class BlockTypeProtocolInitializer
	{
		// Token: 0x0600579A RID: 22426 RVA: 0x001A9980 File Offset: 0x001A7B80
		public static void ConvertShadingMode(BlockType.ShadingMode protocolShadingMode, out ShadingMode shadingMode)
		{
			switch (protocolShadingMode)
			{
			case 1:
				shadingMode = ShadingMode.Flat;
				return;
			case 2:
				shadingMode = ShadingMode.Fullbright;
				return;
			case 3:
				shadingMode = ShadingMode.Reflective;
				return;
			}
			shadingMode = ShadingMode.Standard;
		}
	}
}
