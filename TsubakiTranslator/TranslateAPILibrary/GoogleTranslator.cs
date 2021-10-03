﻿using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace TsubakiTranslator.TranslateAPILibrary
{
    public class GoogleTranslator : ITranslator
    {
        private readonly string name = "谷歌";
        public string Name { get => name; }

        public string SourceLanguage { get; set; }

        public string Translate(string sourceText)
        {
            string desLang = "zh-CN"; 

            string bodyString = $"q={HttpUtility.UrlEncode(sourceText)}&sl={SourceLanguage}&tl={desLang}&client=at&dt=t";

            HttpContent content = new StringContent(bodyString);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            string url = @"https://translate.google.cn/translate_a/single";

            //string googleTransUrl = "https://translate.google.cn/translate_a/single?client=gtx&dt=t&sl=" + SourceLanguage + "&tl=" + desLang + "&tk=" + tk + "&q=" + HttpUtility.UrlEncode(sourceText);

            HttpClient client = CommonFunction.Client;

            try
            {
                HttpResponseMessage response = client.PostAsync(url, content).GetAwaiter().GetResult();//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的
                string responseBody = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Regex reg = new Regex(@"\[\[\[""(.*?)""\,""");
                Match match = reg.Match(responseBody);

                string result = match.Groups[1].Value;
                return result;
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                return ex.Message;
            }
            catch (System.Threading.Tasks.TaskCanceledException ex)
            {
                return ex.Message;
            }
        }

        public void TranslatorInit(string param1, string param2)
        {

        }


    }
}
