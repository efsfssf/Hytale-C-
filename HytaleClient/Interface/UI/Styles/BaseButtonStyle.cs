using System;

namespace HytaleClient.Interface.UI.Styles
{
	// Token: 0x0200082C RID: 2092
	public abstract class BaseButtonStyle<T> where T : class, new()
	{
		// Token: 0x04001A60 RID: 6752
		public T Default = Activator.CreateInstance<T>();

		// Token: 0x04001A61 RID: 6753
		public T Hovered;

		// Token: 0x04001A62 RID: 6754
		public T Pressed;

		// Token: 0x04001A63 RID: 6755
		public T Disabled;

		// Token: 0x04001A64 RID: 6756
		public ButtonSounds Sounds;
	}
}
