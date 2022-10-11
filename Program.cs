using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Dynamic;
using System.IO;
using System.Management;

namespace ShawkanyDb
{
    public class Program
    {
       
        public static void Main(string[] args)
        {

            string bios = "";

            var mbs = new ManagementObjectSearcher("Select SerialNumber From Win32_BIOS");
            var mbslist = mbs.Get();

            foreach (var item in mbslist)
            {
                bios = item["SerialNumber"].ToString();
            }

            if (bios == "7LBCQW1")
             //if (bios == "FT2M3G2")
            {
                BuildWebHost(args).Run();
            }

            //var configs = new ConfigurationBuilder().AddJsonFile("bundleconfig.json",optional:false).Build();
            //var file = configs.GetSection("sourceData").Value;

            //if (true)
            //{
            //    var date = DateTime.Now.Date;
            //    var until = Convert.ToDateTime("4/8/2022");
            //    if (date > until)
            //    {
            //        var appfile = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "bundleconfig.json");
            //        var json = File.ReadAllText(appfile);

            //        var jsonfile = new JsonSerializerSettings();

            //        jsonfile.Converters.Add(new ExpandoObjectConverter());
            //        jsonfile.Converters.Add(new StringEnumConverter());

            //        dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonfile);

            //        config.sourceData = false;

            //        var newjson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonfile);

            //        File.WriteAllText(appfile, newjson);

            //    }
            //    BuildWebHost(args).Run();

            //}

        }

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();


    }
}
