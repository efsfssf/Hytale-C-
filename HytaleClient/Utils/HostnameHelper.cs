using System;
using System.Globalization;
using System.Net;
using System.Text;

namespace HytaleClient.Utils
{
	// Token: 0x020007C2 RID: 1986
	internal static class HostnameHelper
	{
		// Token: 0x06003372 RID: 13170 RVA: 0x0004F83C File Offset: 0x0004DA3C
		public static bool TryParseHostname(string uri, int defaultPort, out string host, out int port, out string error)
		{
			uri = uri.Trim();
			bool flag = uri.Length <= 1;
			bool result;
			if (flag)
			{
				host = null;
				port = -1;
				error = "The hostname is too short.";
				result = false;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = string.Empty;
				bool flag2 = false;
				foreach (char c in uri)
				{
					bool flag3 = string.IsNullOrEmpty(text) && c == ':';
					if (flag3)
					{
						text = stringBuilder.ToString();
						flag2 = true;
					}
					stringBuilder.Append(c);
					bool flag4 = !flag2 || c != ']';
					if (!flag4)
					{
						break;
					}
				}
				bool flag5 = flag2;
				if (flag5)
				{
					IPAddress ipaddress;
					bool flag6 = IPAddress.TryParse(stringBuilder.ToString(), ref ipaddress) && ipaddress.AddressFamily == 23;
					if (flag6)
					{
						host = "[" + ipaddress.ToString().Split(new char[]
						{
							'%'
						})[0] + "]";
						uri = uri.Substring(stringBuilder.Length);
					}
					else
					{
						host = text.ToLowerInvariant();
						uri = uri.Substring(host.Length);
					}
				}
				else
				{
					host = stringBuilder.ToString().ToLowerInvariant();
					uri = uri.Substring(host.Length);
				}
				bool flag7 = string.IsNullOrEmpty(host) || Uri.CheckHostName(host) == UriHostNameType.Unknown;
				if (flag7)
				{
					port = -1;
					error = "The hostname could not be parsed.";
					result = false;
				}
				else
				{
					bool flag8 = uri.Length != 0 && uri[0] == ':';
					if (flag8)
					{
						uri = uri.Substring(1);
						stringBuilder = new StringBuilder();
						foreach (char c2 in uri)
						{
							bool flag9 = !char.IsDigit(c2);
							if (flag9)
							{
								port = -1;
								error = "Invalid port specified.";
								return false;
							}
							stringBuilder.Append(c2);
						}
						bool flag10 = stringBuilder.Length != 0;
						if (flag10)
						{
							uint num;
							bool flag11 = !uint.TryParse(stringBuilder.ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out num) || num > 65535U;
							if (flag11)
							{
								port = -1;
								error = "Invalid port number.";
								return false;
							}
							port = (int)num;
						}
						else
						{
							port = defaultPort;
						}
					}
					else
					{
						port = defaultPort;
					}
					error = null;
					result = true;
				}
			}
			return result;
		}
	}
}
