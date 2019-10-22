using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Xml;

namespace SSConnectTest
{
    class Program
    {
        private static string _filePath = @"D:\shadowsocks-win\gui-config.json";
        static void Main(string[] args)
        {
            Console.Write("ss配置文件默认位置为 " + _filePath+ " ,若需要更换，请按下回车键并输入新路径后再回车确认。");
            string newPath = string.Empty;
            while (true)
            {
                var pressKey = Console.ReadKey();
                if (pressKey.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    newPath = Console.ReadLine();
                    break;
                }
            }

            var configs = OpenConfigFile(string.IsNullOrEmpty(newPath) ?  _filePath : newPath );
            PingTest(configs);
            Console.ReadKey();
        }

        private static List<SSConfig> OpenConfigFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("ss配置文件不存在！");
                return null;
            }

            try
            {
                var obj = (JObject)JsonConvert.DeserializeObject(File.ReadAllText(filePath));
                var result = JsonConvert.DeserializeObject<List<SSConfig>>(obj["configs"].ToString());
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static void PingTest(List<SSConfig> configs)
        {
            if (configs == null || !configs.Any())
            {
                Console.WriteLine("配置项为空！");
                return;
            }

            foreach (var ssConfig in configs)
            {
                Task.Run(() =>
                {
                    try
                    {
                        Ping ping = new Ping();
                        var count = 0;
                        var delay = 0l;
                        for (int i = 0; i < 5; i++)
                        {
                            var ret = ping.Send(ssConfig.Server);
                            switch (ret.Status)
                            {
                                case IPStatus.Success:
                                    count++;
                                    delay += ret.RoundtripTime;
                                    break;
                                default:
                                    break;
                            }
                        }

                        if (count == 5)
                        {
                            Console.WriteLine(ssConfig.Remarks + "  可用，延迟为 " + delay / 5 + " ms");
                        }
                        else
                        {
                            Console.WriteLine(ssConfig.Remarks + "  不可用");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ssConfig.Remarks + "  不可用");
                    }

                });
            }
        }

    }
}
