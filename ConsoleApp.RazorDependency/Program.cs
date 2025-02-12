﻿using ConsoleApp.RazorDependency.Models;
using ConsoleApp.RazorDependency.Views;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp.RazorDependency;

internal class Program
{
    static async Task Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();


        await using var htmlRenderer = new HtmlRenderer(serviceProvider, loggerFactory);

        var people = new List<Person>()
        {
            new Person("dave"),
            new Person("steve")
        };

        var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var dictionary = new Dictionary<string, object?>
            {
                { "Message", "Hello from the Render Message component!" },
                { "People", people}
            };

            var parameters = ParameterView.FromDictionary(dictionary);
            var output = await htmlRenderer.RenderComponentAsync<RenderMessage>(parameters);

            return output.ToHtmlString();
        });

        Console.WriteLine(html);
        Console.ReadLine();
    }
}