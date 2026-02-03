using System;
using System.Runtime.Serialization;
using Utf8Json;
using Utf8Json.Resolvers;

namespace HytaleClient.Data.Boot
{
	// Token: 0x02000B5D RID: 2909
	[Obsolete]
	public class LegacyBootPayload
	{
		// Token: 0x1700136E RID: 4974
		// (get) Token: 0x060059E1 RID: 23009 RVA: 0x001BDC69 File Offset: 0x001BBE69
		// (set) Token: 0x060059E2 RID: 23010 RVA: 0x001BDC71 File Offset: 0x001BBE71
		[DataMember(Name = "javaExecutableLocation")]
		public string JavaExecutable { get; set; }

		// Token: 0x1700136F RID: 4975
		// (get) Token: 0x060059E3 RID: 23011 RVA: 0x001BDC7A File Offset: 0x001BBE7A
		// (set) Token: 0x060059E4 RID: 23012 RVA: 0x001BDC82 File Offset: 0x001BBE82
		[DataMember(Name = "serverPath")]
		public string ServerJar { get; set; }

		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x060059E5 RID: 23013 RVA: 0x001BDC8B File Offset: 0x001BBE8B
		// (set) Token: 0x060059E6 RID: 23014 RVA: 0x001BDC93 File Offset: 0x001BBE93
		[DataMember(Name = "assetsPath")]
		public string AssetsDirectory { get; set; }

		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x060059E7 RID: 23015 RVA: 0x001BDC9C File Offset: 0x001BBE9C
		// (set) Token: 0x060059E8 RID: 23016 RVA: 0x001BDCA4 File Offset: 0x001BBEA4
		[DataMember(Name = "customServerArguments")]
		public string CustomServerArguments { get; set; } = string.Empty;

		// Token: 0x060059E9 RID: 23017 RVA: 0x001BDCB0 File Offset: 0x001BBEB0
		public static LegacyBootPayload Parse(string json)
		{
			return JsonSerializer.Deserialize<LegacyBootPayload>(json, StandardResolver.CamelCase);
		}
	}
}
