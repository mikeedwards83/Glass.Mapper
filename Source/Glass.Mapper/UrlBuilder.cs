﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glass.Mapper
{
    public class UrlBuilder
    {
        private readonly string _url;
        public List<KeyValuePair<string, string>> QueryString { get; private set; }

        public string HtmlQuerySeparator { get; set; }
        public string QuerySeparator { get; set; }

        public UrlBuilder(string url)
        {
            QuerySeparator = "&";
            HtmlQuerySeparator = "&amp;";
            _url = url;
            QueryString = new List<KeyValuePair<string, string>>();
            ProcessQueryString(_url, QueryString);
        }

        protected void ProcessQueryString(string url, List<KeyValuePair<string, string>> queryString)
        {
            if (!url.HasValue() || url.IndexOf("?", StringComparison.Ordinal) <= -1)
            {
                return;
            }

            var urlQuery = Utilities.SplitOnFirstOccurance(url, '?');
            var query = urlQuery[1];

            if (!query.HasValue())
            {
                return;
            }

            query = query.Replace(HtmlQuerySeparator, "&");
            var pairs = query.Split('&', StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in pairs)
            {
                string[] keyValue = Utilities.SplitOnFirstOccurance(pair, '='); 
                if (keyValue.Length == 2)
                {
                    QueryString.Add(new KeyValuePair<string, string>(keyValue[0] ?? string.Empty,
                        keyValue[1] ?? string.Empty));
                }
            }
        }

     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString">A query string containing multiple pairs, e.g. ?key1=value1&key2=value2</param>
        public virtual void AddToQueryString(string queryString)
        {
            if (queryString.IndexOf("?") < 0)
            {
                queryString = "?" + queryString;
            }
            ProcessQueryString(queryString, QueryString);
        }

        public virtual void AddToQueryString(string key, string value)
        {
            if (key.HasValue() && value.HasValue())
            {
                QueryString.Add(new KeyValuePair<string, string>(key, value));
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (_url.HasValue())
            {
                var urlFirst = _url.Split('?')[0];
                sb.Append(urlFirst);
            }
            if (QueryString.Count > 0)
            {
                sb.Append("?");
            }
            foreach (var keyValuePair in QueryString)
            {
                if (keyValuePair.Key.HasValue())
                {
                    if (keyValuePair.Value.HasValue())
                    {
                        sb.AppendFormat("{0}={1}{2}", keyValuePair.Key ?? string.Empty, keyValuePair.Value ?? string.Empty, QuerySeparator);
                    }
                    else
                    {
                        sb.AppendFormat("{0}{1}", keyValuePair.Key ?? string.Empty, QuerySeparator);
                    }
                }
            }

            if (QueryString.Count > 0)
            {
                sb.Remove(sb.Length - QuerySeparator.Length, QuerySeparator.Length);
            }


            return sb.ToString();
        }
    }
}