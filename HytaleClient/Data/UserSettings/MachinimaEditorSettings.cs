using System;

namespace HytaleClient.Data.UserSettings
{
	// Token: 0x02000AD5 RID: 2773
	public class MachinimaEditorSettings
	{
		// Token: 0x06005779 RID: 22393 RVA: 0x001A87FC File Offset: 0x001A69FC
		public MachinimaEditorSettings Clone()
		{
			return new MachinimaEditorSettings
			{
				NewKeyframeFrameOffset = this.NewKeyframeFrameOffset,
				AutosaveDelay = this.AutosaveDelay,
				CompressSaveFiles = this.CompressSaveFiles
			};
		}

		// Token: 0x0400356E RID: 13678
		public int NewKeyframeFrameOffset = 30;

		// Token: 0x0400356F RID: 13679
		public int AutosaveDelay = 30;

		// Token: 0x04003570 RID: 13680
		public bool CompressSaveFiles = false;
	}
}
