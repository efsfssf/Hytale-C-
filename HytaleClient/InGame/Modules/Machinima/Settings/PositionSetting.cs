using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Machinima.Settings
{
	// Token: 0x0200091D RID: 2333
	internal class PositionSetting : KeyframeSetting<Vector3>
	{
		// Token: 0x06004728 RID: 18216 RVA: 0x0010CFF9 File Offset: 0x0010B1F9
		public PositionSetting(Vector3 position) : base("Position", position)
		{
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x0010D00C File Offset: 0x0010B20C
		public override IKeyframeSetting Clone()
		{
			return new PositionSetting(base.Value);
		}

		// Token: 0x040023C7 RID: 9159
		public const string KEY_NAME = "Position";

		// Token: 0x040023C8 RID: 9160
		public static KeyframeSettingType KeyframeType;
	}
}
