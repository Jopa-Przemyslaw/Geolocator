using Geolocator.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Geolocator.Data.Helpers;

public class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder app, bool isProd)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
        }
    }

    private static void SeedData(AppDbContext? context, bool isProd)
    {
        if (isProd)
        {
            Console.WriteLine("--> Attempting to apply migrations...");
            try
            {
                context?.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }

        if (context != null && !context.IpInfos.Any())
        {
            Console.Write("--> Seeding Data...");

            context.IpInfos.AddRange(
                new IpInfo
                {
                    Ip = "31.62.171.98", City = "Poznań", Country = "PL",
                    Hostname = "public-gprs525345.centertel.pl", Org = "AS5617 Orange Polska Spolka Akcyjna",
                    Postal = "61-101", Region = "Greater Poland", Loc = "52.4069,16.9299"
                },
                new IpInfo
                {
                    Ip = "68.87.41.40", City = "Philadelphia", Country = "US",
                    Hostname = "comcast.com", Org = "AS7922 Comcast Cable Communications, LLC",
                    Postal = "19099", Region = "Pennsylvania", Loc = "39.9524,-75.1636"
                },
                new IpInfo
                {
                    Ip = "8.8.4.4", City = "Mountain View", Country = "US",
                    Hostname = "dns.google", Org = "AS15169 Google LLC",
                    Postal = "94043", Region = "California", Loc = "53.1333,23.1643"
                }
            );
            context.SaveChanges();

            Console.WriteLine(" Done.");
        }
        else
        {
            Console.WriteLine("--> We already have data.");
        }
    }
}