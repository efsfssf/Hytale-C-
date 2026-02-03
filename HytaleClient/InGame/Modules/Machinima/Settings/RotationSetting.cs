using System;
using HytaleClient.Math;

namespace HytaleClient.InGame.Modules.Machinima.Settings
{
	// Token: 0x0200091E RID: 2334
	internal class RotationSetting : KeyframeSetting<Vector3>
	{
		// Token: 0x0600472A RID: 18218 RVA: 0x0010D029 File Offset: 0x0010B229
		public RotationSetting(Vector3 position) : base("Rotation", position)
		{
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x0010D03C File Offset: 0x0010B23C
		public override IKeyframeSetting Clone()
		{
			return new RotationSetting(base.Value);
		}

		// Token: 0x040023C9 RID: 9161
		public const string KEY_NAME = "Rotation";

		// Token: 0x040023CA RID: 9162
		public static KeyframeSettingType KeyframeType = KeyframeSettingType.Rotation;
	}
}
