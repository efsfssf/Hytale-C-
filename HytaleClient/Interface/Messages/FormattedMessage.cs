using System;
using System.Collections.Generic;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Utils;

namespace HytaleClient.Interface.Messages
{
	// Token: 0x02000815 RID: 2069
	[UIMarkupData]
	public class FormattedMessage
	{
		// Token: 0x06003961 RID: 14689 RVA: 0x0007AF14 File Offset: 0x00079114
		public static FormattedMessage FromMessageId(string messageId, Dictionary<string, object> messageParams = null)
		{
			return new FormattedMessage
			{
				MessageId = messageId,
				Params = messageParams
			};
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x0007AF3C File Offset: 0x0007913C
		public static bool TryParseFromBson(sbyte[] bytes, out FormattedMessage message)
		{
			bool result;
			try
			{
				message = BsonHelper.ObjectFromBson<FormattedMessage>(bytes);
				result = true;
			}
			catch (Exception)
			{
				message = null;
				result = false;
			}
			return result;
		}

		// Token: 0x04001946 RID: 6470
		public List<FormattedMessage> Children;

		// Token: 0x04001947 RID: 6471
		public string RawText;

		// Token: 0x04001948 RID: 6472
		public string MessageId;

		// Token: 0x04001949 RID: 6473
		public Dictionary<string, object> Params;

		// Token: 0x0400194A RID: 6474
		public Dictionary<string, FormattedMessage> MessageParams;

		// Token: 0x0400194B RID: 6475
		public string Color;

		// Token: 0x0400194C RID: 6476
		public bool? Bold;

		// Token: 0x0400194D RID: 6477
		public bool? Italic;

		// Token: 0x0400194E RID: 6478
		public bool? Underlined;

		// Token: 0x0400194F RID: 6479
		public string Link;

		// Token: 0x04001950 RID: 6480
		public bool MarkupEnabled;
	}
}
