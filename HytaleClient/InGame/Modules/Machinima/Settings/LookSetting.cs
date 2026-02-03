using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Machinima.Settings
{
	// Token: 0x0200091C RID: 2332
	internal class LookSetting : KeyframeSetting<Vector3>
	{
		// Token: 0x06004725 RID: 18213 RVA: 0x0010CFC1 File Offset: 0x0010B1C1
		public LookSetting(Vector3 position) : base("Look", position)
		{
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x0010CFD4 File Offset: 0x0010B1D4
		public override IKeyframeSetting Clone()
		{
			return new LookSetting(base.Value);
		}

		// Token: 0x040023C5 RID: 9157
		public const string KEY_NAME = "Look";

		// Token: 0x040023C6 RID: 9158
		public static KeyframeSettingType KeyframeType = KeyframeSettingType.Look;
	}
}
