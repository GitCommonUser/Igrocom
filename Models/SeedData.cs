using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Igrocom.Data;
using System;
using System.Linq;

namespace Igrocom.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        

        using (var context = new IgrocomContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<IgrocomContext>>()))
        {

            // Данный метод отвечает за заполнение таблицы Контента, если она пустая, иначе возвращаются текующие значения таблицы

            /*if (context.Content.Any())
            {
                return;   
            }
            context.Content.AddRange(
                new Content
                {
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateOnly.Parse("1989-2-12"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M
                },
                new Content
                {
                    Title = "Ghostbusters ",
                    ReleaseDate = DateOnly.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M
                },
                new Content
                {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateOnly.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Price = 9.99M
                },
                new Content
                {
                    Title = "Rio Bravo",
                    ReleaseDate = DateOnly.Parse("1959-4-15"),
                    Genre = "Western",
                    Price = 3.99M
                }
            );

            if (context.Game.Any())
            {
                return;   
            }
            context.Game.AddRange(
                new Game
                {
                    Title = "Minecraft",
                    ReleaseDate = DateOnly.Parse("2011-11-18"),
                    Genre = "Песочница",
                    Rating = 89,
                    Description = "Игра с открытым миром и бесконечными возможностями",
                    Image=""
                },
                new Game
                {
                    Title = "Counter Strike 2",
                    ReleaseDate = DateOnly.Parse("2012-8-21"),
                    Genre = "Шутер",
                    Rating = 75,
                    Description = "Самый известный шутер на планете",
                    Image=""
                },
                new Game
                {
                    Title = "Civilization V",
                    ReleaseDate = DateOnly.Parse("2010-11-21"),
                    Genre = "Стратегия",
                    Rating = 77,
                    Description = "Пошаговая стратегия с развитием от каменного века до современности",
                    Image=""
                }
            );*/
            if(!context.Game.Any()){
                context.Game.AddRange(
                    new Game
                    {
                        Title = "Minecraft",
                        ReleaseDate = DateOnly.Parse("2011-11-18"),
                        Genre = "Песочница",
                        Rating = 89,
                        Description = "Игра с открытым миром и бесконечными возможностями",
                        //Image=""
                    },
                    new Game
                    {
                        Title = "Counter Strike 2",
                        ReleaseDate = DateOnly.Parse("2012-8-21"),
                        Genre = "Шутер",
                        Rating = 75,
                        Description = "Самый известный шутер на планете",
                        //Image=""
                    },
                    new Game
                    {
                        Title = "Civilization V",
                        ReleaseDate = DateOnly.Parse("2010-11-21"),
                        Genre = "Стратегия",
                        Rating = 77,
                        Description = "Пошаговая стратегия с развитием от каменного века до современности",
                        //Image=""
                    }
                );
            }

            context.SaveChanges();
        }
    }
}