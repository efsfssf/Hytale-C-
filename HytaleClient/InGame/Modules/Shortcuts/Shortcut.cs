using System;
using Newtonsoft.Json.Linq;

namespace HytaleClient.InGame.Modules.Shortcuts
{
	// Token: 0x02000902 RID: 2306
	internal abstract class Shortcut
	{
		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x0600453D RID: 17725 RVA: 0x000F1457 File Offset: 0x000EF657
		// (set) Token: 0x0600453E RID: 17726 RVA: 0x000F145F File Offset: 0x000EF65F
		public string Name { get; protected set; }

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x0600453F RID: 17727 RVA: 0x000F1468 File Offset: 0x000EF668
		// (set) Token: 0x06004540 RID: 17728 RVA: 0x000F1470 File Offset: 0x000EF670
		public string Command { get; private set; }

		// Token: 0x06004541 RID: 17729 RVA: 0x000F1479 File Offset: 0x000EF679
		public Shortcut(string name, string command)
		{
			this.Name = name;
			this.Command = command;
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x000F1494 File Offset: 0x000EF694
		public JObject ToJsonObject()
		{
			JObject jobject = new JObject();
			jobject.Add("name", JToken.FromObject(this.Name));
			jobject.Add("command", JToken.FromObject(this.Command));
			return jobject;
		}
	}
}
