using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;
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
            //GetData(googleApps);
            //ProjectDate(googleApps);
            //DevideData(googleApps);
            //OrderData(googleApps);
            //DataSetOperation(googleApps);
            //DateVerification(googleApps);
            //GroupData(googleApps);
            //GroupDataOperation(googleApps);
        }

        //Join is one to one, GroupJoin is one to many, for example one person to many addresses
        private static void GroupDataOperation(IEnumerable<GoogleApp> googleApps)
        {
            var categoryGroup = googleApps
                .GroupBy( e => e.Category);
            
            foreach(var group in categoryGroup)
            {
                Console.WriteLine($"Category: {group.Key.ToString()}");//print category name
                var avarageReviews = group.Average( g=>g.Reviews );
                var minAvarageReviews = group.Min( g => g.Reviews );
                var maxAvarageReviews = group.Max( g => g.Reviews );
                var reviewsCount = group.Sum(g => g.Reviews);
                Console.WriteLine($"Min: {minAvarageReviews}, max: {maxAvarageReviews}, sum: {reviewsCount}");
            }
        }

        private static void GroupData(IEnumerable<GoogleApp> googleApps)
        {
            var categoryGroup = googleApps.GroupBy(g => g.Category);// return group where key is category, but it's not work like a dictionary/map
            var artAndDesignGroup = categoryGroup.First( g => g.Key == Category.ART_AND_DESIGN);

            //both metod below return the same collection of element
            var apps = artAndDesignGroup.Select(e => e);
            var apps2 = artAndDesignGroup.ToList();
            Console.WriteLine($"Displaing elements for group: {artAndDesignGroup.Key}");
            Display(apps);
        }

        private static void DateVerification(IEnumerable<GoogleApp> googleApps)
        {
            var allOperatorResult = googleApps.Where(a => a.Category == Category.WEATHER)
                .All(a => a.Reviews > 10);//condition is true for all value, return bool
            Console.WriteLine($"allOperatorResult: {allOperatorResult}");

            var anyOperatorResult = googleApps.Where(a=>a.Category == Category.WEATHER).Any( a => a.Reviews > 2_000_000);//nice method for write 2M
            Console.WriteLine($"anyOperatorResult: {anyOperatorResult}");
        }

        private static void DataSetOperation(IEnumerable<GoogleApp> googleApps)
        {
            var paidAppsCategories = googleApps.Where(a => a.Type == Type.Paid).Select(a => a.Category);
            //Console.WriteLine($"Paid apps categories: {string.Join(", ", paidAppsCategories)}");//print all category 

            paidAppsCategories = paidAppsCategories.Union(paidAppsCategories);//print all category but each category is print one time
            //Console.WriteLine(string.Join(", ", paidAppsCategories));

            //concatenate two set without repetition
            var setA = googleApps.Where(a => a.Rating > 4.7 && a.Type == Type.Paid && a.Reviews > 1000);
            var setB = googleApps.Where(a => a.Name.Contains("Pro") && a.Rating > 4.6 && a.Reviews >10000);

            var appsUnion = setA.Union(setB);//type of collections must be the same,
            //Console.WriteLine(string.Join(", ", appsUnion));//it not aligned
            //Display(appsUnion);
            
            var appsIntersect = setA.Intersect(setB);//return this value what is in both sets
            //Display(appsIntersect);

            var appsExcept = setA.Except(setB);//return value froma setA which are't in setB
            Display(appsExcept);
        }

        static void OrderData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedBeautyApps = googleApps.Where(app => (app.Rating > 4.6 && app.Category == Category.BEAUTY));
            Display(highRatedBeautyApps);

            Console.WriteLine("Sorted list:");
            //var sortedResults = highRatedBeautyApps.OrderBy(app => app.Rating);
            //var sortedDescResults = highRatedBeautyApps.OrderByDescending(app => app.Rating);
            var sortedResults = highRatedBeautyApps.OrderBy(app => app.Rating).ThenBy(app=>app.Name);


            Display(sortedResults);

            var name = sortedResults.Select(app => app.Name).ToList();
            Console.WriteLine(string.Join(", ", name));

        }

        static void DevideData(IEnumerable<GoogleApp> googleApps)
        {
            var highRatedBeautyApps = googleApps.Where(app => (app.Rating > 4.6 && app.Category == Category.BEAUTY));
            //var first5highRatedBeautyApps = highRatedBeautyApps.Take(5);
            //var last5highRatedBeautyApps = highRatedBeautyApps.TakeLast(5);

            //var first5highRatedBeautyApps = highRatedBeautyApps.TakeWhile(app => app.Reviews > 1000);//iterate while reviews is grader then 1000, not all records grader thern 1000
            //but from start to first elemetn lower then 100

            //Display(first5highRatedBeautyApps);
            //Display(last5highRatedBeautyApps);

            //var skipResults = highRatedBeautyApps.Skip(5);//start from 6th element
            //Display(skipResults);

            var skipWhileResults = highRatedBeautyApps.SkipWhile( app => app.Reviews > 1000);//return from first ement where condition lower then 1000 is equel true, 
            Console.WriteLine("Skipped result:");
            Display(skipWhileResults);

        }

        private static void ProjectDate(List<GoogleApp> googleApps)
        {
            var highRatedBeautyApps = googleApps.Where(app => (app.Rating > 4.6 && app.Category == Category.BEAUTY));
            var highRatedBeautyAppsNames = highRatedBeautyApps.Select(app => app.Name);//return only Name column

            //Display(highRatedBeautyApps);
            /*foreach(var name in highRatedBeautyAppsNames)
            {
                Console.WriteLine(name);
            }*/

            Console.WriteLine(string.Join(", ", highRatedBeautyAppsNames));

            //var dtos = highRatedBeautyApps.Select(app => new GoogleAppDto()
            //{
            //    Reviews = app.Reviews,
            //    Name = app.Name
            //});

            var dtos = highRatedBeautyApps.Select(app => new //anonymous type
            {
                Reviews = app.Reviews,
                Name = app.Name
            });

            /*foreach(var dto in dtos)
            {
                Console.WriteLine($"{dto.Reviews} {dto.Name}");
            }*/

            var generes = highRatedBeautyApps.SelectMany(app => app.Genres);//SelectMany because it is a list 
            Console.WriteLine(string.Join (":", generes));
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


