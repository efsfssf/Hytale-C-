using System;
using System.Collections.Generic;
using System.Diagnostics;
using HytaleClient.Utils;

namespace HytaleClient.Application.Services.Api
{
	// Token: 0x02000C00 RID: 3072
	internal class Server
	{
		// Token: 0x06006206 RID: 25094 RVA: 0x00204FD0 File Offset: 0x002031D0
		protected bool Equals(Server other)
		{
			return this.IsLan == other.IsLan && string.Equals(this.Name, other.Name) && string.Equals(this.Host, other.Host);
		}

		// Token: 0x06006207 RID: 25095 RVA: 0x00205018 File Offset: 0x00203218
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this == obj;
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = obj.GetType() != base.GetType();
					result = (!flag3 && this.Equals((Server)obj));
				}
			}
			return result;
		}

		// Token: 0x06006208 RID: 25096 RVA: 0x00205068 File Offset: 0x00203268
		public override int GetHashCode()
		{
			int num = this.IsLan.GetHashCode();
			int num2 = num * 397;
			string name = this.Name;
			num = (num2 ^ ((name != null) ? name.GetHashCode() : 0));
			int num3 = num * 397;
			string host = this.Host;
			return num3 ^ ((host != null) ? host.GetHashCode() : 0);
		}

		// Token: 0x04003CEC RID: 15596
		private static readonly Comparison<Server> NameComparison = (Server s1, Server s2) => string.Compare(s1.Name, s2.Name, StringComparison.OrdinalIgnoreCase);

		// Token: 0x04003CED RID: 15597
		private static readonly Comparison<Server> RatingComparison = (Server s1, Server s2) => string.Compare("", "", StringComparison.OrdinalIgnoreCase);

		// Token: 0x04003CEE RID: 15598
		private static readonly Comparison<Server> OnlinePlayersComparison = (Server s1, Server s2) => s2.OnlinePlayers.CompareTo(s1.OnlinePlayers);

		// Token: 0x04003CEF RID: 15599
		public static readonly Comparison<Server> NameSort = ComparisonUtils.Compose<Server>(new Comparison<Server>[]
		{
			Server.NameComparison,
			Server.OnlinePlayersComparison,
			Server.RatingComparison
		});

		// Token: 0x04003CF0 RID: 15600
		public static readonly Comparison<Server> RatingSort = ComparisonUtils.Compose<Server>(new Comparison<Server>[]
		{
			Server.RatingComparison,
			Server.OnlinePlayersComparison,
			Server.NameComparison
		});

		// Token: 0x04003CF1 RID: 15601
		public static readonly Comparison<Server> OnlinePlayersSort = ComparisonUtils.Compose<Server>(new Comparison<Server>[]
		{
			Server.OnlinePlayersComparison,
			Server.NameComparison,
			Server.RatingComparison
		});

		// Token: 0x04003CF2 RID: 15602
		public bool IsLan;

		// Token: 0x04003CF3 RID: 15603
		public Guid UUID;

		// Token: 0x04003CF4 RID: 15604
		public string Name;

		// Token: 0x04003CF5 RID: 15605
		public string Description;

		// Token: 0x04003CF6 RID: 15606
		public string ImageUrl;

		// Token: 0x04003CF7 RID: 15607
		public string Version;

		// Token: 0x04003CF8 RID: 15608
		public List<string> Tags;

		// Token: 0x04003CF9 RID: 15609
		public List<string> Languages;

		// Token: 0x04003CFA RID: 15610
		public string Host;

		// Token: 0x04003CFB RID: 15611
		public int OnlinePlayers;

		// Token: 0x04003CFC RID: 15612
		public int MaxPlayers;

		// Token: 0x04003CFD RID: 15613
		public int Ping;

		// Token: 0x04003CFE RID: 15614
		public bool IsOnline;

		// Token: 0x04003CFF RID: 15615
		public bool IsFavorite;

		// Token: 0x04003D00 RID: 15616
		public Stopwatch Updated = Stopwatch.StartNew();
	}
}
