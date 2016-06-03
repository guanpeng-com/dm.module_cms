using Abp.Apps;
using Abp.Core.Enums;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Abp.Core.Utils
{
    public class PageUtils
    {
        public const char SeparatorChar = '/';

        public const string UNCLICKED_URL = "javascript:;";

        public static string ParseNavigationUrl(string url)
        {
            return ParseNavigationUrl(url, HttpContext.Current.Request.ApplicationPath);
        }

        public static string ParseNavigationUrl(string url, string domainUrl)
        {
            string retval = string.Empty;
            if (string.IsNullOrEmpty(url))
            {
                return retval;
            }
            if (url.StartsWith("~"))
            {
                retval = Combine(domainUrl, url.Substring(1));
            }
            else
            {
                retval = url;
            }
            return retval;
        }

        public static string AddProtocolToUrl(string url)
        {
            return AddProtocolToUrl(url, string.Empty);
        }

        /// <summary>
        /// 按照给定的host，添加Protocol
        /// Demo: 发送的邮件中，需要内容标题的链接为全连接，那么需要指定他的host
        /// </summary>
        /// <param name="url"></param>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string AddProtocolToUrl(string url, string host)
        {
            if (url == PageUtils.UNCLICKED_URL)
            {
                return url;
            }
            string retval = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                url = url.Trim();
                if (IsProtocolUrl(url))
                {
                    retval = url;
                }
                else
                {
                    if (string.IsNullOrEmpty(host))
                        retval = url.StartsWith("/") ? (PageUtils.GetScheme() + "://" + PageUtils.GetHost() + url) : (PageUtils.GetScheme() + "://" + url);
                    else
                        retval = url.StartsWith("/") ? (host.TrimEnd('/') + url) : (host + url);
                }
            }
            return retval;
        }

        public static string AddQuestionOrAndToUrl(string pageUrl)
        {
            string url = pageUrl;
            if (string.IsNullOrEmpty(url))
            {
                url = "?";
            }
            else
            {
                if (url.IndexOf('?') == -1)
                {
                    url = url + "?";
                }
                else if (!url.EndsWith("?"))
                {
                    url = url + "&";
                }
            }
            return url;
        }

        public static string RemovePortFromUrl(string url)
        {
            string retval = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                var regex = new Regex(@":\d+");
                retval = regex.Replace(url, "");
            }
            return retval;
        }

        public static string RemoveProtocolFromUrl(string url)
        {
            string retval = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                url = url.Trim();
                if (IsProtocolUrl(url))
                {
                    retval = url.Substring(url.IndexOf("://") + 3);
                }
                else
                {
                    retval = url;
                }
            }
            return retval;
        }

        public static bool IsProtocolUrl(string url)
        {
            bool retval = false;
            if (!string.IsNullOrEmpty(url))
            {
                if (url.IndexOf("://") != -1 || url.StartsWith("javascript:"))
                {
                    retval = true;
                }
            }
            return retval;
        }

        public static string GetAbsoluteUrl()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri;
        }

        public static string PathDifference(string path1, string path2, bool compareCase)
        {
            int num2 = -1;
            int num1 = 0;
            while ((num1 < path1.Length) && (num1 < path2.Length))
            {
                if ((path1[num1] != path2[num1]) && (compareCase || (char.ToLower(path1[num1], CultureInfo.InvariantCulture) != char.ToLower(path2[num1], CultureInfo.InvariantCulture))))
                {
                    break;
                }
                if (path1[num1] == '/')
                {
                    num2 = num1;
                }
                num1++;
            }
            if (num1 == 0)
            {
                return path2;
            }
            if ((num1 == path1.Length) && (num1 == path2.Length))
            {
                return string.Empty;
            }
            StringBuilder builder1 = new StringBuilder();
            while (num1 < path1.Length)
            {
                if (path1[num1] == '/')
                {
                    builder1.Append("../");
                }
                num1++;
            }
            return (builder1 + path2.Substring(num2 + 1));
        }

        /// <summary>
        /// 获取服务器根域名  
        /// </summary>
        /// <returns></returns>
        public static string GetMainDomain(string url)
        {
            if (string.IsNullOrEmpty(url)) return url;

            url = PageUtils.RemoveProtocolFromUrl(url.ToLower());
            if (url.IndexOf('/') != -1)
            {
                url = url.Substring(0, url.IndexOf('/'));
            }

            if (url.IndexOf('.') > 0)
            {
                string[] strArr = url.Split('.');
                string lastStr = strArr.GetValue(strArr.Length - 1).ToString();
                if (StringUtils.IsNumber(lastStr)) //如果最后一位是数字，那么说明是IP地址
                {
                    return url;
                }
                else //否则为域名
                {
                    string[] domainRules = ".com.cn|.net.cn|.org.cn|.gov.cn|.com|.net|.cn|.org|.cc|.me|.tel|.mobi|.asia|.biz|.info|.name|.tv|.hk|.公司|.中国|.网络".Split('|');
                    string findStr = string.Empty;
                    string replaceStr = string.Empty;
                    string returnStr = string.Empty;
                    for (int i = 0; i < domainRules.Length; i++)
                    {
                        if (url.EndsWith(domainRules[i].ToLower())) //如果最后有找到匹配项
                        {
                            findStr = domainRules[i].ToString(); //www.px915.COM
                            replaceStr = url.Replace(findStr, ""); //将匹配项替换为空，便于再次判断
                            if (replaceStr.IndexOf('.') > 0) //存在二级域名或者三级，比如：www.px915
                            {
                                string[] replaceArr = replaceStr.Split('.'); // www px915
                                returnStr = replaceArr.GetValue(replaceArr.Length - 1).ToString() + findStr;
                                return returnStr;
                            }
                            else //px915
                            {
                                returnStr = replaceStr + findStr; //连接起来输出为：px915.com
                                return returnStr;
                            };
                        }
                        else
                        { returnStr = url; }
                    }
                    return returnStr;
                }
            }
            else
            {
                return url;
            }
        }

        public static string GetHost()
        {
            string host = string.Empty;
            if (HttpContext.Current != null)
            {
                host = HttpContext.Current.Request.Headers["HOST"];
                if (string.IsNullOrEmpty(host))
                {
                    host = HttpContext.Current.Request.Url.Host;
                }
            }

            return (string.IsNullOrEmpty(host)) ? string.Empty : host.Trim().ToLower();
        }

        public static string GetScheme()
        {
            string scheme = string.Empty;
            if (HttpContext.Current != null)
            {
                scheme = HttpContext.Current.Request.Headers["SCHEME"];
                if (string.IsNullOrEmpty(scheme))
                {
                    scheme = HttpContext.Current.Request.Url.Scheme;
                }
            }

            return (string.IsNullOrEmpty(scheme)) ? "http" : scheme.Trim().ToLower();
        }

        public static NameValueCollection GetQueryString(string url)
        {
            NameValueCollection attributes = new NameValueCollection();
            if (!string.IsNullOrEmpty(url) && url.IndexOf("?") != -1)
            {
                string querystring = url.Substring(url.IndexOf("?") + 1);
                attributes = TranslateUtils.ToNameValueCollection(querystring);
            }
            return attributes;
        }

        public static string Combine(params string[] urls)
        {
            string retval = string.Empty;
            if (urls != null && urls.Length > 0)
            {
                retval = (urls[0] != null) ? urls[0].Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar) : string.Empty;
                for (int i = 1; i < urls.Length; i++)
                {
                    string url = (urls[i] != null) ? urls[i].Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar) : string.Empty;
                    retval = PageUtils.Combine(retval, url);
                }
            }
            return retval;
        }

        private static string Combine(string url1, string url2)
        {
            if ((url1 == null) || (url2 == null))
            {
                throw new ArgumentNullException((url1 == null) ? "url1" : "url2");
            }
            if (url2.Length == 0)
            {
                return url1;
            }
            if (url1.Length == 0)
            {
                return url2;
            }

            return (url1.TrimEnd(SeparatorChar) + SeparatorChar + url2.TrimStart(SeparatorChar));
        }

        public static string AddQueryString(string url, string queryStringKey, string queryStringValue)
        {
            NameValueCollection queryString = new NameValueCollection();
            queryString.Add(queryStringKey, queryStringValue);
            return AddQueryString(url, queryString);
        }

        public static string AddQueryString(string url, string queryString)
        {
            if (queryString == null || url == null)
                return url;

            queryString = queryString.TrimStart(new char[] { '?', '&' });

            if (url.IndexOf("?") == -1)
            {
                return string.Concat(url, "?", queryString);
            }
            else
            {
                if (url.EndsWith("?"))
                {
                    return string.Concat(url, queryString);
                }
                else
                {
                    return string.Concat(url, "&", queryString);
                }
            }
        }

        public static string AddQueryString(string url, NameValueCollection queryString)
        {
            if (queryString == null || url == null || queryString.Count == 0)
                return url;

            StringBuilder builder = new StringBuilder();
            foreach (string key in queryString.Keys)
            {
                builder.AppendFormat("&{0}={1}", key, HttpUtility.UrlEncode(queryString[key]));
            }
            if (url.IndexOf("?") == -1)
            {
                if (builder.Length > 0) builder.Remove(0, 1);
                return string.Concat(url, "?", builder.ToString());
            }
            else
            {
                if (url.EndsWith("?"))
                {
                    if (builder.Length > 0) builder.Remove(0, 1);
                }
                return string.Concat(url, builder.ToString());
            }
        }

        public static string RemoveQueryString(string url, string queryString)
        {
            if (queryString == null || url == null)
                return url;

            if (url.IndexOf("?") == -1 || url.EndsWith("?"))
            {
                return url;
            }
            else
            {
                NameValueCollection attributes = PageUtils.GetQueryString(url);
                attributes.Remove(queryString);
                url = url.Substring(0, url.IndexOf("?"));
                return PageUtils.AddQueryString(url, attributes);
            }
        }

        public static string GetIPAddress()
        {
            //取CDN用户真实IP的方法
            //当用户使用代理时，取到的是代理IP
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(result))
            {
                //可能有代理
                if (result.IndexOf(".") == -1)
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        result = result.Replace("  ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIP(temparyip[i]) && temparyip[i].Substring(0, 3) != "10." && temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                result = temparyip[i];
                            }
                        }
                        string[] str = result.Split(',');
                        if (str.Length > 0)
                            result = str[0].ToString().Trim();
                    }
                    else if (IsIP(result))
                        return result;
                }
            }

            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = HttpContext.Current.Request.UserHostAddress;
            if (string.IsNullOrEmpty(result))
                result = "127.0.0.1";


            return result;
        }

        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        public static string GetRefererUrl()
        {
            string url = HttpContext.Current.Request.ServerVariables["HTTP_REFERER"];
            return url;
        }

        public static string GetUrlWithReturnUrl(string pageUrl, string returnUrl)
        {
            string retval = pageUrl;
            returnUrl = string.Format("ReturnUrl={0}", returnUrl);
            if (pageUrl.IndexOf("?") != -1)
            {
                if (pageUrl.EndsWith("&"))
                {
                    retval += returnUrl;
                }
                else
                {
                    retval += "&" + returnUrl;
                }
            }
            else
            {
                retval += "?" + returnUrl;
            }
            return PageUtils.ParseNavigationUrl(retval, HttpContext.Current.Request.PhysicalApplicationPath);
        }

        public static string GetReturnUrl()
        {
            return GetReturnUrl(true);
        }

        public static string GetReturnUrl(bool toReferer)
        {
            string redirectUrl = string.Empty;
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["ReturnUrl"]))
            {
                redirectUrl = PageUtils.ParseNavigationUrl(HttpContext.Current.Request.QueryString["ReturnUrl"]);
            }
            else if (toReferer)
            {
                string referer = PageUtils.GetRefererUrl();
                if (!string.IsNullOrEmpty(referer))
                {
                    redirectUrl = referer;
                }
                else
                {
                    redirectUrl = PageUtils.GetHost();
                }
            }
            return redirectUrl;
        }

        public static string GetUrlByBaseUrl(string rawUrl, string baseUrl)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(rawUrl))
            {
                rawUrl = rawUrl.Trim().TrimEnd('#');
            }
            if (!string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = baseUrl.Trim();
            }
            if (!string.IsNullOrEmpty(rawUrl))
            {
                rawUrl = rawUrl.Trim();
                if (PageUtils.IsProtocolUrl(rawUrl))
                {
                    url = rawUrl;
                }
                else if (rawUrl.StartsWith("/"))
                {
                    string domain = GetUrlWithoutPathInfo(baseUrl);
                    url = domain + rawUrl;
                }
                else if (rawUrl.StartsWith("../"))
                {
                    int count = StringUtils.GetStartCount("../", rawUrl);
                    rawUrl = rawUrl.Remove(0, 3 * count);
                    baseUrl = GetUrlWithoutFileName(baseUrl).TrimEnd('/');
                    baseUrl = PageUtils.RemoveProtocolFromUrl(baseUrl);
                    for (int i = 0; i < count; i++)
                    {
                        int j = baseUrl.LastIndexOf('/');
                        if (j != -1)
                        {
                            baseUrl = StringUtils.Remove(baseUrl, j);
                        }
                        else
                        {
                            break;
                        }
                    }
                    url = PageUtils.Combine(PageUtils.AddProtocolToUrl(baseUrl), rawUrl);
                }
                else
                {
                    if (baseUrl.EndsWith("/"))
                    {
                        url = baseUrl + rawUrl;
                    }
                    else
                    {
                        string urlWithoutFileName = GetUrlWithoutFileName(baseUrl);
                        if (!urlWithoutFileName.EndsWith("/"))
                        {
                            urlWithoutFileName += "/";
                        }
                        url = urlWithoutFileName + rawUrl;
                    }
                }
            }
            return url;
        }

        /// <summary>
        /// 将Url地址的查询字符串去掉
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public static string GetUrlWithoutQueryString(string rawUrl)
        {
            string urlWithoutQueryString;
            if (rawUrl != null && rawUrl.IndexOf("?") != -1)
            {
                string queryString = rawUrl.Substring(rawUrl.IndexOf("?"));
                urlWithoutQueryString = rawUrl.Replace(queryString, "");
            }
            else
            {
                urlWithoutQueryString = rawUrl;
            }
            return urlWithoutQueryString;
        }

        /// <summary>
        /// 将Url地址域名后的字符去掉
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public static string GetUrlWithoutPathInfo(string rawUrl)
        {
            string urlWithoutPathInfo = string.Empty;
            if (rawUrl != null && rawUrl.Trim().Length > 0)
            {
                if (rawUrl.ToLower().StartsWith("http://"))
                {
                    urlWithoutPathInfo = rawUrl.Substring("http://".Length);
                }
                if (urlWithoutPathInfo.IndexOf("/") != -1)
                {
                    urlWithoutPathInfo = urlWithoutPathInfo.Substring(0, urlWithoutPathInfo.IndexOf("/"));
                }
                if (string.IsNullOrEmpty(urlWithoutPathInfo))
                {
                    urlWithoutPathInfo = rawUrl;
                }
                urlWithoutPathInfo = "http://" + urlWithoutPathInfo;
            }
            return urlWithoutPathInfo;
        }

        /// <summary>
        /// 将Url地址后的文件名称去掉
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public static string GetUrlWithoutFileName(string rawUrl)
        {
            string urlWithoutFileName = string.Empty;
            if (rawUrl != null && rawUrl.Trim().Length > 0)
            {
                if (rawUrl.ToLower().StartsWith("http://"))
                {
                    urlWithoutFileName = rawUrl.Substring("http://".Length);
                }
                if (urlWithoutFileName.IndexOf("/") != -1 && !urlWithoutFileName.EndsWith("/"))
                {
                    string regex = "/(?<filename>[^/]*\\.[^/]*)[^/]*$";
                    RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);
                    Regex reg = new Regex(regex, options);
                    Match match = reg.Match(urlWithoutFileName);
                    if (match.Success)
                    {
                        string fileName = match.Groups["filename"].Value;
                        urlWithoutFileName = urlWithoutFileName.Substring(0, urlWithoutFileName.LastIndexOf(fileName));
                    }
                }
                urlWithoutFileName = "http://" + urlWithoutFileName;
            }
            return urlWithoutFileName;
        }

        public static string GetUrlQueryString(string url)
        {
            string queryString = string.Empty;
            if (!string.IsNullOrEmpty(url) && url.IndexOf("?") != -1)
            {
                queryString = url.Substring(url.IndexOf("?") + 1);
            }
            return queryString;
        }

        public static string GetFileNameFromUrl(string rawUrl)
        {
            string fileName = string.Empty;
            if (!string.IsNullOrEmpty(rawUrl))
            {
                //if (rawUrl.ToLower().StartsWith("http://"))
                //{
                //    rawUrl = rawUrl.Substring("http://".Length);
                //}
                //if (rawUrl.IndexOf("?") != -1)
                //{
                //    int index = rawUrl.IndexOf("?");
                //    rawUrl = rawUrl.Remove(index, rawUrl.Length - index);
                //}
                rawUrl = PageUtils.RemoveProtocolFromUrl(rawUrl);
                rawUrl = PageUtils.GetUrlWithoutQueryString(rawUrl);
                if (rawUrl.IndexOf("/") != -1 && !rawUrl.EndsWith("/"))
                {
                    string regex = "/(?<filename>[^/]*\\.[^/]*)[^/]*$";
                    RegexOptions options = ((RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase);
                    Regex reg = new Regex(regex, options);
                    Match match = reg.Match(rawUrl);
                    if (match.Success)
                    {
                        fileName = match.Groups["filename"].Value;
                    }
                }
                else
                {
                    fileName = rawUrl;
                }
            }
            return fileName;
        }

        public static string GetExtensionFromUrl(string rawUrl)
        {
            string extension = string.Empty;
            if (!string.IsNullOrEmpty(rawUrl))
            {
                rawUrl = PageUtils.RemoveProtocolFromUrl(rawUrl);
                rawUrl = PageUtils.GetUrlWithoutQueryString(rawUrl);
                rawUrl = rawUrl.TrimEnd('/');
                if (rawUrl.IndexOf('/') != -1)
                {
                    rawUrl = rawUrl.Substring(rawUrl.LastIndexOf('/'));
                    if (rawUrl.IndexOf('.') != -1)
                    {
                        extension = rawUrl.Substring(rawUrl.LastIndexOf('.'));
                    }
                }
            }
            return extension;
        }

        public static string UrlEncode(string urlString)
        {
            if (urlString == null || urlString == "$4")
            {
                return string.Empty;
            }

            string newValue = urlString.Replace("\"", "'");
            newValue = System.Web.HttpUtility.UrlEncode(newValue);
            newValue = newValue.Replace("%2f", "/");
            return newValue;
        }

        public static string UrlEncode(string urlString, string encoding)
        {
            if (urlString == null || urlString == "$4")
            {
                return string.Empty;
            }

            string newValue = urlString.Replace("\"", "'");
            newValue = System.Web.HttpUtility.UrlEncode(newValue, System.Text.Encoding.GetEncoding(encoding));
            newValue = newValue.Replace("%2f", "/");
            return newValue;
        }

        public static string UrlEncode(string urlString, ECharset charset)
        {
            if (urlString == null || urlString == "$4")
            {
                return string.Empty;
            }

            string newValue = urlString.Replace("\"", "'");
            newValue = System.Web.HttpUtility.UrlEncode(newValue, ECharsetUtils.GetEncoding(charset));
            newValue = newValue.Replace("%2f", "/");
            return newValue;
        }

        public static string UrlDecode(string urlString, string encoding)
        {
            return System.Web.HttpUtility.UrlDecode(urlString, System.Text.Encoding.GetEncoding(encoding));
        }

        public static string UrlDecode(string urlString, ECharset charset)
        {
            return System.Web.HttpUtility.UrlDecode(urlString, ECharsetUtils.GetEncoding(charset));
        }

        public static string UrlDecode(string urlString)
        {
            return HttpContext.Current.Server.UrlDecode(urlString);
        }

        public static void Redirect(string url)
        {
            HttpContext.Current.Response.Redirect(url, true);
        }

        /// <summary>
        /// 获取文件相对路径，格式：AppDir + FilePath
        /// </summary>
        /// <param name="app"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetUrlWithAppDir(App app, string url)
        {
            if (url == null)
                return "";

            if (url.IndexOf("@") == 0)
            {
                url = url.TrimStart('@');
            }

            url = PathUtils.Combine(app.AppDir, url);

            return url;
        }

        /// <summary>
        /// 获取数据库文件相对路径, 去掉Url中的AppDir
        /// </summary>
        /// <param name="app"></param>
        /// <param name="url">包含AppDir的文件路径</param>
        /// <returns></returns>
        public static string GetUrlWithoutAppDir(App app, string url)
        {
            if (url == null)
                return "";
            if (url.IndexOf(app.AppDir) == 0)
            {
                url = url.Substring(app.AppDir.Length);
            }
            if (url.IndexOf("@") < 0)
            {
                url = PathUtils.Combine("@", url);
            }
            return url;
        }

    }
}
