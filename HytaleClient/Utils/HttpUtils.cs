using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Linq;
using NLog;

namespace HytaleClient.Utils
{
	// Token: 0x020007C3 RID: 1987
	public static class HttpUtils
	{
		// Token: 0x06003373 RID: 13171 RVA: 0x0004FAAC File Offset: 0x0004DCAC
		public static void RequestJson(HttpWebRequest req, JObject postData, Action<JObject, Exception, HttpWebResponse> callback)
		{
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				try
				{
					JObject arg;
					Exception arg2;
					HttpWebResponse arg3;
					HttpUtils.RequestJsonSync(req, postData, out arg, out arg2, out arg3);
					callback(arg, arg2, arg3);
				}
				catch (Exception value)
				{
					HttpUtils.Logger.Error<Exception>(value);
				}
			});
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x0004FAE8 File Offset: 0x0004DCE8
		public static void RequestJson(string url, string method, JObject postData, Action<JObject, Exception, HttpWebResponse> callback)
		{
			ThreadPool.QueueUserWorkItem(delegate(object _)
			{
				try
				{
					JObject arg;
					Exception arg2;
					HttpWebResponse arg3;
					HttpUtils.RequestJsonSync(url, method, postData, out arg, out arg2, out arg3);
					callback(arg, arg2, arg3);
				}
				catch (Exception value)
				{
					HttpUtils.Logger.Error<Exception>(value);
				}
			});
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x0004FB2A File Offset: 0x0004DD2A
		public static void RequestJson(string url, Action<JObject, Exception, HttpWebResponse> callback)
		{
			HttpUtils.RequestJson(url, "GET", null, callback);
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x0004FB3C File Offset: 0x0004DD3C
		public static bool IsHttpStatusCode(Exception ex, HttpStatusCode statusCode)
		{
			bool flag = ex is WebException;
			bool result;
			if (flag)
			{
				HttpWebResponse httpWebResponse = (HttpWebResponse)((WebException)ex).Response;
				result = (httpWebResponse != null && httpWebResponse.StatusCode == statusCode);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x0004FB80 File Offset: 0x0004DD80
		private static void RequestJsonSync(HttpWebRequest req, JObject postData, out JObject responseData, out Exception exception, out HttpWebResponse res)
		{
			bool flag = req.Method != "GET" && postData != null;
			if (flag)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(postData.ToString());
				req.ContentLength = (long)bytes.Length;
				try
				{
					using (Stream requestStream = req.GetRequestStream())
					{
						requestStream.Write(bytes, 0, bytes.Length);
					}
				}
				catch (Exception ex)
				{
					responseData = null;
					exception = ex;
					res = null;
					return;
				}
			}
			HttpWebResponse httpWebResponse;
			JObject jobject;
			try
			{
				httpWebResponse = (HttpWebResponse)req.GetResponse();
				jobject = JObject.Parse(Encoding.UTF8.GetString(httpWebResponse.GetResponseStream().ReadAllBytes()));
			}
			catch (WebException ex2)
			{
				responseData = null;
				exception = ex2;
				res = (HttpWebResponse)ex2.Response;
				return;
			}
			catch (Exception ex3)
			{
				responseData = null;
				exception = ex3;
				res = null;
				return;
			}
			responseData = jobject;
			exception = null;
			res = httpWebResponse;
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x0004FC98 File Offset: 0x0004DE98
		private static void RequestJsonSync(string url, string method, JObject postData, out JObject responseData, out Exception exception, out HttpWebResponse res)
		{
			HttpUtils.RequestJsonSync(HttpUtils.CreateRequest(url, method), postData, out responseData, out exception, out res);
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x0004FCB0 File Offset: 0x0004DEB0
		public static HttpWebRequest CreateRequest(string url, string method = "GET")
		{
			HttpWebRequest httpWebRequest = WebRequest.CreateHttp(url);
			httpWebRequest.Method = method;
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Timeout = 15000;
			return httpWebRequest;
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x0004FCEC File Offset: 0x0004DEEC
		public static string BuildQueryString(NameValueCollection parameters)
		{
			StringBuilder stringBuilder = new StringBuilder(parameters.Count * 4 - 1);
			foreach (string text in parameters.AllKeys)
			{
				foreach (string value in parameters.GetValues(text))
				{
					bool flag = stringBuilder.Length != 0;
					if (flag)
					{
						stringBuilder.Append('&');
					}
					stringBuilder.Append(WebUtility.UrlEncode(text));
					stringBuilder.Append("=");
					stringBuilder.Append(WebUtility.UrlEncode(value));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400171D RID: 5917
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	}
}
