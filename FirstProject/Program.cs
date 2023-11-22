using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

// ReSharper disable UseFormatSpecifierInInterpolation

namespace FirstProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvPath = @"C:\Users\Admin\Documents\Programming\Knowledge\C#\Ling\CSharpCourse\FirstProject\googleplaystore1.csv";
            var googleApps = LoadGoogleAps(csvPath);

            //Display(googleApps);
            GetData(googleApps);

        }

        static void GetData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedApps = googleApps.Where(app => app.Rating> 4.6);
            var highRatedBeautyApps = highRatedApps.Where(app => (app.Category == Category.BEAUTY));

            //var firstHighRatedBeautyApps = highRatedBeautyApps.First(app => app.Reviews < 50);  //exception because value dont exist
            var firstHighRatedBeautyApps = highRatedBeautyApps.FirstOrDefault(app => app.Reviews > 0);

            //var singleHighRatedBeautyApps = highRatedBeautyApps.Single(app => app.Reviews > 0);//if is more then one element then return exeption, use only when is one element
            var singleHighRatedBeautyApps = highRatedBeautyApps.SingleOrDefault(app => app.Reviews == 0);

            var lastHighRatedBeautyApps = highRatedBeautyApps.LastOrDefault(app => app.Reviews > 100);//work like first but last element
            Display(lastHighRatedBeautyApps);
        }

        static void Display(IEnumerable<GoogleApp> googleApps)
        {
            foreach (var googleApp in googleApps)
            {
                Console.WriteLine(googleApp);
            }

        }
        static void Display(GoogleApp googleApp)
        {
            Console.WriteLine(googleApp);
        }

        static List<GoogleApp> LoadGoogleAps(string csvPath)
        {
            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<GoogleAppMap>();
                var records = csv.GetRecords<GoogleApp>().ToList();
                return records;
            }

        }

    }

    
}


