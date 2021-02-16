using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Plantilla.NetCoreSwaggerBasicAuth.Extensions
{
    public static  class Utils
    {
        public static string log_name = "Application";

        public const int _ErrorRetry = 5;

        private static Random random = new Random();

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int maxItems)
        {
            return items.Select((item, inx) => new { item, inx })
                        .GroupBy(x => x.inx / maxItems)
                        .Select(g => g.Select(x => x.item));
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Encode64AndEncrypt(this string value)
        {
            return Base64Encode(Encryption.ToEncrypt(value));
        }
        public static string Decode64AndDecrypt(this string value)
        {
            string res = Base64Decode(value);

            return Encryption.ToDecrypt(res);
        }

        public static string EncodeAndEncrypt(this string value)
        {
            return System.Web.HttpUtility.UrlEncode(Encryption.ToEncrypt(value));
        }

        public static string EncodeAndDecrypt(this string value)
        {
            string res = System.Web.HttpUtility.UrlDecode(System.Web.HttpUtility.UrlEncode(value));

            return Encryption.ToDecrypt(res);
        }

        public static decimal ToDecimal(this object data)
        {
            try
            {
                System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();

                customCulture.NumberFormat.NumberDecimalSeparator = ".";

                System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

                return Convert.ToDecimal(data.ToString().Trim());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string ToDatetime(this object data)
        {
            try
            {
                var date = Convert.ToDateTime(data);

                if (date.Year < 1900)
                {
                    return "1900-01-01";
                }

                return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            catch (Exception)
            {
                return "1900-01-01";
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string ToMoneyFormat(decimal value)
        {
            Thread.CurrentThread.CurrentCulture
                = Thread.CurrentThread.CurrentUICulture
                  = new CultureInfo("en-US");

            return value.ToString("C", CultureInfo.CurrentCulture).Replace("$", "");
        }

        public static string SerializeObject<T>(T model)
        {
            var indented = Newtonsoft.Json.Formatting.Indented;

            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            return JsonConvert.SerializeObject(model, indented, settings);
        }

        public static T DeserializeObject<T>(string modelJson)
        {
            return JsonConvert.DeserializeObject<T>(modelJson, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
            });
        }

        public static IEnumerable<T> Execute_Query<T>(string query, string connectionString = "")
        {
            IEnumerable<T> items = null;

            int Contador = 0;

            while (Contador <= _ErrorRetry)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        items = connection.Query<T>(query);
                    }

                    Contador = _ErrorRetry + 1;
                }
                catch (Exception ex)
                {
                    if (Contador == _ErrorRetry)
                    {
                        throw ex;
                    }

                    Contador++;
                }
            }

            return items;
        }

        public static IEnumerable<T> Execute_Query<T>(string spName, Dictionary<string, string> dParameters, string connectionString = "")
        {
            var dbArgs = new DynamicParameters();

            int Contador = 0;

            while (Contador <= _ErrorRetry)
            {
                try
                {
                    foreach (var pair in dParameters)
                    {
                        dbArgs.Add(pair.Key, pair.Value);
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        return (connection.Query<T>(spName, new DynamicParameters(dbArgs), commandType: CommandType.StoredProcedure));
                    }

                }
                catch (Exception ex)
                {                    
                    using (EventLog eventLog = new EventLog(log_name))
                    {
                        eventLog.Source = log_name;

                        eventLog.WriteEntry( System.Reflection.MethodBase.GetCurrentMethod().Name + ex.Message + ex.StackTrace, EventLogEntryType.Error, 101, 1);
                    }

                    if (Contador == _ErrorRetry)
                    {
                        throw ex;
                    }

                    Contador++;
                }
            }

            return null;

        }

        public static string CleanParameter(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value.Trim();
        }

        public static string CleanDecimalParameter(this decimal value)
        {
            return value.ToString().Replace(",", "");
        }

        public static string ToDatetimeServer(this DateTime dateValue)
        {
            DateTime date = Convert.ToDateTime(dateValue);

            try
            {
                return $"{date.Year}-{date.Month}-{date.Day} {date.Hour}:{date.Minute}:{date.Second}.{date.Millisecond}";
            }
            catch (Exception)
            {
                return $"{date.Year}-{date.Day}-{date.Month} {date.Hour}:{date.Minute}:{date.Second}.{date.Millisecond}";
            }

        }

        public static string ToXmlSerialize<T>(this T obj)
        {
            try
            {
                string xmlString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(typeof(T));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xs.Serialize(xmlTextWriter, obj);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                xmlString = UTF8ByteArrayToString(memoryStream.ToArray());

                try
                {
                    xmlString = xmlString.Substring(1);

                    if (!xmlString.StartsWith("<"))
                    {
                        xmlString = "<" + xmlString;
                    }
                }
                catch { }

                return xmlString;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T ToXmlDeserialize<T>(this string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));

            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xml));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (T)xs.Deserialize(memoryStream);
        }

        private static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private static Byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        public static string Transform_Xslt_Xml(string xmlContent, string XsltContent)
        {             
            string output = String.Empty;
            using (StringReader srt = new StringReader(XsltContent))
            using (StringReader sri = new StringReader(xmlContent))
            {
                using (XmlReader xrt = XmlReader.Create(srt))
                using (XmlReader xri = XmlReader.Create(sri))
                {
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(xrt);
                    using (StringWriter sw = new StringWriter())
                    using (XmlWriter xwo = XmlWriter.Create(sw, xslt.OutputSettings))
                    {
                        xslt.Transform(xri, xwo);
                        output = sw.ToString();
                    }
                }
            }

            return output;

        }

    }
}
