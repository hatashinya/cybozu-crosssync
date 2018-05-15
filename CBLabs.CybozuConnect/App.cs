using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Xml;

namespace CBLabs.CybozuConnect
{
    public class App
    {
        public enum CybozuType { Unknown, Office, Garoon, GaroonCloud };

        protected string cybozuUrl;
        protected string cybozuUsername;
        protected string cybozuPassword;
        protected CybozuType cybozuType = CybozuType.Unknown;

        protected User user;

        public Base Base;
        public Schedule Schedule;

        public XmlDocument LastXmlDoc;

        public static class ErrorCode
        {
            public const string GaroonLicenseError = "GRN_CBPAPI_63006";
            public const string OfficeLicenseError = "19107";
        }

        public App(string url)
        {
            if (url.IndexOf("/ag.") >= 0)
            {
                this.cybozuType = CybozuType.Office;
            }
            else if (url.IndexOf("/grn.") >= 0)
            {
                this.cybozuType = CybozuType.Garoon;
            }
            else if (url.IndexOf(".cybozu.com/g/") >= 0 || url.IndexOf(".cybozu.cn/g/") >= 0)
            {
                this.cybozuType = CybozuType.GaroonCloud;
            }
            else
            {
                throw new CybozuException("Cannot use API to the specified URL.");
            }

            int queryPos = url.IndexOf('?');
            this.cybozuUrl = (queryPos >= 0) ? url.Substring(0, queryPos) : url;
        }

        public App(string url, string username, string password)
            : this(url)
        {
            Auth(username, password);
        }

        public User User
        {
            get
            {
                return this.user;
            }
        }

        public string UserId
        {
            get
            {
                return this.user.ID;
            }
        }

        public void Auth(string username, string password)
        {
            this.cybozuUsername = username;
            this.cybozuPassword = password;

            ListDictionary login_name = new ListDictionary();
            login_name["innerValue"] = this.cybozuUsername;
            ListDictionary parameters = new ListDictionary();
            parameters["login_name"] = login_name;
            XmlElement resultNode = Query("Base", "BaseGetUsersByLoginName", parameters);

            XmlNode userNode = resultNode.SelectSingleNode("//user");
            if (userNode == null)
            {
                throw new CybozuException("Fail to auth.");
            }

            this.user = new User(userNode);
        }

        public void ClearAuth()
        {
            this.cybozuUsername = null;
            this.cybozuPassword = null;
            this.user = null;
        }

        public XmlElement Query(string service, string method, ListDictionary parameters)
        {
            if (method.IndexOf(service + "Get") != 0 && method.IndexOf(service + "Search") != 0)
            {
                throw new CybozuException("Cannot execute the updating methods by calling Query() method.");
            }

            return Exec(service, method, parameters);
        }

        public XmlElement Exec(string service, string method, ListDictionary parameters)
        {
            DateTime time = DateTime.Now;
            string created = Utility.FormatISO8601(time);
            string expires = Utility.FormatISO8601(time.AddSeconds(1));

            StringBuilder parametersXml = new StringBuilder();
            MakeParametersXml(parametersXml, "parameters", parameters);

            method = Utility.XmlEscape(method);

            StringBuilder requestXml = new StringBuilder();
            requestXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            requestXml.Append("<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            requestXml.Append("<soap:Header>");
            requestXml.Append(string.Format("<Action soap:mustUnderstand=\"1\" xmlns=\"http://schemas.xmlsoap.org/ws/2003/03/addressing\">{0}</Action>", method));
            requestXml.Append(string.Format("<Timestamp soap:mustUnderstande=\"1\" xmlns=\"http://schemas.xmlsoap.org/ws/2002/07/utility\"><Created>{0}</Created><Expires>{1}</Expires></Timestamp>", created, expires));
            requestXml.Append("<Security xmlns:wsu=\"http://schemas.xmlsoap.org/ws/2002/07/utility\" soap:mustUnderstand=\"1\" xmlns=\"http://schemas.xmlsoap.org/ws/2002/12/secext\">");
            requestXml.Append(string.Format("<UsernameToken><Username>{0}</Username><Password>{1}</Password></UsernameToken>", Utility.XmlEscape(cybozuUsername), Utility.XmlEscape(cybozuPassword)));
            requestXml.Append("</Security></soap:Header><soap:Body>");
            requestXml.Append(string.Format("<{0} xmlns=\"http://wsdl.cybozu.co.jp/base/2008\">{1}</{0}></soap:Body></soap:Envelope>", method, parametersXml));

            string url;
            switch (this.cybozuType)
            {
                case CybozuType.Office:
                    url = string.Format("{0}?page=PApi{1}", this.cybozuUrl, service);
                    break;
                case CybozuType.Garoon:
                    url = string.Format("{0}/cbpapi/{1}/api", this.cybozuUrl, service.ToLowerInvariant());
                    break;
                case CybozuType.GaroonCloud:
                    url = string.Format("{0}cbpapi/{1}/api.csp?", this.cybozuUrl, service.ToLowerInvariant());
                    break;
                default:
                    throw new CybozuException("Unexpected");
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = string.Format("application/soap+xml; charset=utf-8; action=\"{0}\"", method);
            string requestXmlString = requestXml.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(requestXmlString);
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new CybozuException("Network error.");
            }

            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream, Encoding.UTF8);
            string responseText = responseReader.ReadToEnd();
            response.Close();
            responseReader.Close();

            this.LastXmlDoc = new XmlDocument();
            try
            {
                this.LastXmlDoc.LoadXml(responseText);
            }
            catch (XmlException)
            {
                if (responseText.StartsWith("<?xml"))
                {
                    responseText = Regex.Replace(responseText, "[\x00-\x08\x0b-\x0c\x0e-\x1f\x7f]", ""); // delete control codes
                    try
                    {
                        this.LastXmlDoc.LoadXml(responseText);
                    }
                    catch (XmlException)
                    {
                        throw new CybozuException("Unexpected error.");
                    }
                }
                else if (responseText.IndexOf(ErrorCode.GaroonLicenseError) >= 0)
                {
                    throw new CybozuException("License error.", ErrorCode.GaroonLicenseError);
                }
                else
                {
                    throw new CybozuException("Unexpected error.");
                }
            }
            XmlElement root = this.LastXmlDoc.DocumentElement;
            XmlElement codeNode = root.SelectSingleNode("//code") as XmlElement;
            XmlElement diagNode = root.SelectSingleNode("//diagnosis") as XmlElement;
            if (codeNode != null && diagNode != null)
            {
                throw new CybozuException(diagNode.InnerText, codeNode.InnerText);
            }
            XmlElement returnsNode = root.SelectSingleNode("//returns") as XmlElement;
            if (returnsNode == null)
            {
                // there are APIs that have no response.
                //throw new CybozuException("Fail to get results.");
            }

            return returnsNode;
        }

        public XmlElement QueryItems(string service, string methodOfGetVersions, string methodOfGetById, string idName, string itemName)
        {
            ListDictionary parameters = new ListDictionary();
            ArrayList idArray = new ArrayList();
            parameters[idName] = idArray;

            // get item versions
            XmlElement resultElement = Query(service, methodOfGetVersions, null);

            XmlNodeList idNodeList = resultElement.SelectNodes("//" + itemName);
            foreach (XmlNode idNode in idNodeList)
            {
                XmlAttribute idAttr = idNode.Attributes["id"];
                if (idAttr != null)
                {
                    ListDictionary idParam = new ListDictionary();
                    idParam["innerValue"] = idAttr.Value;
                    idArray.Add(idParam);
                }
            }

            return Query(service, methodOfGetById, parameters);
        }

        protected void MakeParametersXml(StringBuilder sb, string name, ListDictionary child)
        {
            if (child == null || child.Count == 0)
            {
                sb.Append(string.Format("<{0} />", name));
                return;
            }

            bool attributeEnded = false;

            sb.Append(string.Format("<{0}", name));
            if (name == "parameters")
            {
                sb.Append(" xmlns=\"\"");
            }

            foreach (DictionaryEntry entry in child)
            {
                if (entry.Value == null) continue;

                string key = entry.Key as string;
                if (string.IsNullOrEmpty(key)) continue;

                if (entry.Value is ListDictionary)
                {
                    if (!attributeEnded)
                    {
                        attributeEnded = true;
                        sb.Append(">");
                    }
                    MakeParametersXml(sb, key, entry.Value as ListDictionary);
                }
                else if (entry.Value is ArrayList)
                {
                    if (!attributeEnded)
                    {
                        attributeEnded = true;
                        sb.Append(">");
                    }
                    foreach (object item in (entry.Value as ArrayList))
                    {
                        if (!(item is ListDictionary)) continue;
                        MakeParametersXml(sb, key, item as ListDictionary);
                    }
                }
                else if (key == "innerValue")
                {
                    if (!attributeEnded)
                    {
                        attributeEnded = true;
                        sb.Append(">");
                    }
                    sb.Append(Utility.HtmlEscape(entry.Value.ToString()));
                    break;
                }
                else if (!attributeEnded)
                {
                    if (entry.Value is string)
                    {
                        if ((entry.Value as string).IndexOf("\n") >= 0)
                        {
                            sb.Append(string.Format(" {0}=\"{1}\"", key, Utility.XmlAttributeEscape(entry.Value.ToString())));
                        }
                        else
                        {
                            sb.Append(string.Format(" {0}=\"{1}\"", key, Utility.HtmlEscape(entry.Value.ToString())));
                        }
                    }
                    else if (entry.Value is int || entry.Value is double)
                    {
                        sb.Append(string.Format(" {0}=\"{1}\"", key, entry.Value));
                    }
                    else if (entry.Value is bool)
                    {
                        sb.Append(string.Format(" {0}=\"{1}\"", key, entry.Value.ToString().ToLowerInvariant()));
                    }
                }
            }

            if (!attributeEnded)
            {
                sb.Append(">");
            }
            sb.Append(string.Format("</{0}>", name));
        }
    }
}
