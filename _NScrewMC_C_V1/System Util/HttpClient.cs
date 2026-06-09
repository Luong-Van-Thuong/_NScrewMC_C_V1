using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _NScrewMC_C_V1.System_Util
{
    public class HttpRestClient
    {
        private readonly HttpClient _client;
        public HttpRestClient()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// HTTP GET 요청 (비동기)
        /// </summary>
        public async Task<string> GetAsync(string url)
        {
            var response = await _client.GetAsync(url);
            return await HandleResponse(response);
        }

        /// <summary>
        /// HTTP POST 요청 (비동기)
        /// </summary>
        public async Task<string> PostAsync(string url, string jsonBody)
        {
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(url, content);
            return await HandleResponse(response);
        }

        /// <summary>
        /// HTTP PUT 요청 (비동기)
        /// </summary>
        public async Task<string> PutAsync(string url, string jsonBody)
        {
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(url, content);
            return await HandleResponse(response);
        }

        /// <summary>
        /// HTTP DELETE 요청 (비동기)
        /// </summary>
        public async Task<string> DeleteAsync(string url)
        {
            var response = await _client.DeleteAsync(url);
            return await HandleResponse(response);
        }

        private async Task<string> HandleResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else
                return $"Error: {response.StatusCode}";
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        /// <summary>
        /// getPrevInspInfo 요청을 전송하는 함수
        /// </summary>
        public Task<string> SendPrevInspInfo(string prodcMagtNo)
        {
            // 싱글톤에서 현재 선택된 사이트명 가져오기
            string siteName = "SEVT";//SingletonManager.instance.SiteName;
            // SiteList에서 해당 사이트 정보 찾기
            SiteInfo selectedSite = SiteList.Find(s => s.Name.Equals(siteName, StringComparison.OrdinalIgnoreCase));
            if (selectedSite == null)
            {
                return Task.FromResult("Error: SiteNotFound");
            }
            var reqRoot = new
            {
                com_samsung_gmes2_qm_json_vo_QmSubInspForJsonSVO = new
                {
                    qmSubInspForJson01DVO = new
                    {
                        fctCode = selectedSite.FctCode,
                        plantCode = selectedSite.PlantCode,
                        inspTopCode = "TOPD80",//SingletonManager.instance.ProcessCode,
                        prodcMagtNo = prodcMagtNo
                    },
                    anyframeDVO = new
                    {
                        appName = "com.samsung.gmes2.qm.json.app.QmSubInspForJsonApp",
                        methodName = "getPrevInspInfo",
                        inputSVOName = "com.samsung.gmes2.qm.json.vo.QmSubInspForJsonSVO",
                        pageNo = "0",
                        pageRowCount = "0"
                    }
                }
            };

            string json = JsonConvert.SerializeObject(reqRoot, Formatting.Indented);

            // Task를 반환 (호출부에서 Task를 저장/관리)
            return Task.Run(async () =>
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, selectedSite.Url))
                {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.Add("j_username", "809a13fe7c33a882fdcf4c88087896035241c8c705d491bb211e2c8c1e055db2");
                    request.Headers.Add("j_password", "23d72bc66d630909dd4be3c806b994d97bb9e600e05765480a90133754682931");

                    var response = await _client.SendAsync(request).ConfigureAwait(false);
                    return await HandleResponse(response).ConfigureAwait(false);
                }
            });
        }
        public string GetRsltCode(string responseJson)
        {
            try
            {
                var root = JObject.Parse(responseJson);
                var rsltCode = root["com_samsung_gmes2_qm_json_vo_QmSubInspForJsonSVO"]?["qmSubInspForJson02DVO"]?["rsltCode"]?.ToString();
                return rsltCode ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        public string GetErrorCode(string responseJson)
        {
            try
            {
                var root = JObject.Parse(responseJson);
                var rsltCode = root["com_samsung_gmes2_qm_json_vo_QmSubInspForJsonSVO"]?["qmSubInspForJson02DVO"]?["errCode"]?.ToString();
                return rsltCode ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        public Task<string> SendSaveInspInfo(int index)
        {
            // 현재 시간 (yyyyMMddHHmmss)
            string inspDt = DateTime.Now.ToString("yyyyMMddHHmmss");

            // 싱글톤에서 현재 선택된 사이트명, 공정코드 등 가져오기
            string siteName = "SEVT";
            SiteInfo selectedSite = SiteList.Find(s => s.Name.Equals(siteName, StringComparison.OrdinalIgnoreCase));
            if (selectedSite == null)
                return Task.FromResult("Error: SiteNotFound");

            string fctCode = selectedSite.FctCode;
            string plantCode = selectedSite.PlantCode;
            string inspTopCode = "TOPD80";

            // JSON 구조 생성
            var reqRoot = new
            {
                com_samsung_gmes2_qm_json_vo_QmSubInspForJsonSVO = new
                {
                    qmSubInspForJson01DVO = new
                    {
                        fctCode = fctCode,
                        plantCode = plantCode,
                        inspTopCode = inspTopCode,
                        prodcMagtNo = InforManager.Instance.BarcodePort,
                        rsltCode = MSystem.m_pTrsScrew[index].m_bMesResult == 1 ? "PASS" : "FAIL",
                        bcrIp = GetLocalIPv4(),
                        jigNo = index,
                        inspDt = inspDt
                    },
                    anyframeDVO = new
                    {
                        appName = "com.samsung.gmes2.qm.json.app.QmSubInspForJsonApp",
                        methodName = "saveInspInfo",
                        inputSVOName = "com.samsung.gmes2.qm.json.vo.QmSubInspForJsonSVO",
                        pageNo = "0",
                        pageRowCount = "0"
                    }
                }
            };

            string json = JsonConvert.SerializeObject(reqRoot, Formatting.Indented);

            // Task 반환 (POST 전송)
            return Task.Run(async () =>
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, selectedSite.Url))
                {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.Add("j_username", "809a13fe7c33a882fdcf4c88087896035241c8c705d491bb211e2c8c1e055db2");
                    request.Headers.Add("j_password", "23d72bc66d630909dd4be3c806b994d97bb9e600e05765480a90133754682931");

                    var response = await _client.SendAsync(request).ConfigureAwait(false);
                    return await HandleResponse(response).ConfigureAwait(false);
                }
            });
        }
        public readonly List<SiteInfo> SiteList = new List<SiteInfo>
        {
            new SiteInfo { Name = "GUMI",    Url = "http://168.219.108.30:90/gmes2/gmes2If.do",        FctCode = "C100E", PlantCode = "P104" },
            new SiteInfo { Name = "TSTC",    Url = "http://tstcmes.sec.samsung.net/gmes2/gmes2If.do",  FctCode = "C6F0A", PlantCode = "P625" },
            new SiteInfo { Name = "SEHZ",    Url = "http://sehzmes.sec.samsung.net/gmes2/gmes2If.do",  FctCode = "C670A", PlantCode = "P648" },
            new SiteInfo { Name = "SEV",     Url = "http://107.107.161.44:90/gmes2/gmes2If.do",        FctCode = "C5H0A", PlantCode = "P518" },
            new SiteInfo { Name = "SEVT",    Url = "http://107.114.35.44:90/gmes2/gmes2If.do",         FctCode = "C5H2A", PlantCode = "P520" },
            new SiteInfo { Name = "SIEL",    Url = "http://sieln-mes.sec.samsung.net/gmes2/gmes2If.do",FctCode = "C550A", PlantCode = "P538" },
            new SiteInfo { Name = "SEIN",    Url = "http://seinmes.sec.samsung.net/gmes2/gmes2If.do",  FctCode = "C570A", PlantCode = "P529" },
            new SiteInfo { Name = "SEDA_C",  Url = "http://sedac-mes.sec.samsung.net/gmes2/gmes2If.do",FctCode = "C820B", PlantCode = "P811" },
            new SiteInfo { Name = "SEDA_M",  Url = "http://sedam-mes.sec.samsung.net/gmes2/gmes2If.do",FctCode = "C820A", PlantCode = "P81J" },
            new SiteInfo { Name = "TEST",    Url = "http://10.40.40.101/gmes2/gmes2If.do",             FctCode = "C5H0A", PlantCode = "P518" }
        };

        public string GetLocalIPv4()
        {
            string localIP = string.Empty;
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                localIP = host.AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
                    ?.ToString() ?? string.Empty;
            }
            catch
            {
                localIP = string.Empty;
            }
            return localIP;
        }
    }

    public class SiteInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string FctCode { get; set; }
        public string PlantCode { get; set; }
    }
}
