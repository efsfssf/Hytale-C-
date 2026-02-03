using System;
using HytaleClient.InGame.Modules.Machinima.Track;
using HytaleClient.Protocol;
using Newtonsoft.Json;

namespace HytaleClient.InGame.Modules.Machinima.Events
{
	// Token: 0x02000921 RID: 2337
	internal class CommandEvent : KeyframeEvent
	{
		// Token: 0x06004733 RID: 18227 RVA: 0x0010D1E2 File Offset: 0x0010B3E2
		public CommandEvent(string command)
		{
			this.Command = command;
			base.AllowDuplicates = true;
			base.Initialized = true;
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x0010D204 File Offset: 0x0010B404
		public override void Execute(GameInstance gameInstance, SceneTrack track)
		{
			bool flag = this.Command.StartsWith(".");
			if (flag)
			{
				gameInstance.ExecuteCommand(this.Command);
			}
			else
			{
				gameInstance.Connection.SendPacket(new ChatMessage(this.Command));
			}
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x0010D24C File Offset: 0x0010B44C
		public override string ToString()
		{
			return string.Format("#{0} - CommandEvent [Command: '{1}']", this.Id, this.Command);
		}

		// Token: 0x040023CE RID: 9166
		[JsonProperty("Command")]
		public readonly string Command;
	}
}
