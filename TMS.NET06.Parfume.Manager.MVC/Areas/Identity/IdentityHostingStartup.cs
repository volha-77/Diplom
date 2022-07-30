using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMS.NET06.Parfume.Manager.MVC.Data;

[assembly: HostingStartup(typeof(TMS.NET06.Parfume.Manager.MVC.Areas.Identity.IdentityHostingStartup))]
namespace TMS.NET06.Parfume.Manager.MVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}