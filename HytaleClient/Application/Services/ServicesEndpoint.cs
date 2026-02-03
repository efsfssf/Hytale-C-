using System;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BFC RID: 3068
	public class ServicesEndpoint
	{
		// Token: 0x060061BA RID: 25018 RVA: 0x002021EF File Offset: 0x002003EF
		public ServicesEndpoint(string host, int port, bool secure)
		{
			this.Host = host;
			this.Port = port;
			this.Secure = secure;
		}

		// Token: 0x060061BB RID: 25019 RVA: 0x00202210 File Offset: 0x00200410
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}", new object[]
			{
				"Host",
				this.Host,
				"Port",
				this.Port,
				"Secure",
				this.Secure
			});
		}

		// Token: 0x060061BC RID: 25020 RVA: 0x00202270 File Offset: 0x00200470
		public static ServicesEndpoint Parse(string input)
		{
			ServicesEndpoint result;
			if (!(input == "Development"))
			{
				if (!(input == "Production"))
				{
					throw new Exception("Failed to parse services endpoint '" + input + "'");
				}
				result = ServicesEndpoint.Production;
			}
			else
			{
				result = ServicesEndpoint.Development;
			}
			return result;
		}

		// Token: 0x04003CD1 RID: 15569
		public static readonly ServicesEndpoint Development = new ServicesEndpoint("ash-dev.services.hytale.com", 443, true);

		// Token: 0x04003CD2 RID: 15570
		public static readonly ServicesEndpoint Production = new ServicesEndpoint("services.hytale.com", 443, true);

		// Token: 0x04003CD3 RID: 15571
		public static readonly ServicesEndpoint Default = ServicesEndpoint.Production;

		// Token: 0x04003CD4 RID: 15572
		public readonly string Host;

		// Token: 0x04003CD5 RID: 15573
		public readonly int Port;

		// Token: 0x04003CD6 RID: 15574
		public readonly bool Secure;
	}
}
