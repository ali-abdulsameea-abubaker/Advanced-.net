using MovieTrackerRazor.Models;

namespace MovieTrackerRazor.Data;

public static class SeedData
{
    public static void Initialize(MovieTrackerRazorContext context)
    {
        context.Genre.AddRange(
            new Genre { GenreDescription = "Action" },
            new Genre { GenreDescription = "Comedy" },
            new Genre { GenreDescription = "Drama" }
        );
        context.Movie.AddRange(
            new Movie
            {
                Title = "12 Angry Men",
                DateSeen = DateTime.Now.AddDays(-550).Date,
                GenreId = 3,
                Rating = 7,
                ImageFile = "12angrymen.png"
            },
            new Movie
            {
                Title = "Back to the Future",
                DateSeen = DateTime.Now.AddDays(-450).Date,
                GenreId = 1,
                Rating = 8,
                ImageFile = "backtofuture.png"
            },
            new Movie
            {
                Title = "Men in Black",
                DateSeen = DateTime.Now.AddDays(-350).Date,
                GenreId = 1,
                Rating = 7,
                ImageFile = "meninblack.png"
            },
            new Movie
            {
                Title = "The Dark Knight",
                DateSeen = DateTime.Now.AddDays(-250).Date,
                GenreId = 1,
                Rating = 9,
                ImageFile = "darkknight.png"
            },
            new Movie
            {
                Title = "3 Idiots",
                DateSeen = DateTime.Now.AddDays(-150).Date,
                GenreId = 2,
                Rating = 8,
                ImageFile = "3idiots.png"
            }
        );
        context.SaveChanges();
    }
}