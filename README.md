# Moosic - Music Discovery and Library Management

## Project Overview

Moosic is a web-based application that enables users to discover new music, manage their music library, and provide ratings/reviews based on their preferences. The application integrates with the Spotify API to allow users to explore and organize songs, and offers personalized experiences through music discovery, library management, and rating features.

### Features

- **User Authentication**: Secure login and registration for personalized experiences.
- **Search and Discover Music**: Search for songs and discover new tracks through the Spotify API integration.
- **Add Music to Library**: Add songs to a personalized music library, keeping track of favorite tracks.
- **Rate and Review Music**: Users can rate and review songs they discover, creating an interactive and tailored experience.
- **Library Management**: View, filter, and manage your personal music library by song details, genre, and ratings.

## Technologies Used

- **ASP.NET Core MVC**: Framework for building the web application and separating concerns between frontend and backend.
- **Spotify API**: For discovering and fetching music data.
- **Entity Framework Core**: For managing database interactions and models.
- **SQL Server**: For storing user data and music information.
- **NUnit**: For testing the application’s functionalities.
- **Bootstrap**: For responsive and modern UI design.

## Project Structure

The project follows a structured approach with directories for models, views, controllers, services, and static assets. Below is a brief overview of the key components:

### Directory Breakdown

- **Controllers**: Contains controllers like `AccountController`, `HomeController`, and `MusicController` for handling user authentication, music search, and library management.
- **Migrations**: Contains migration files that manage database schema changes.
- **Models**: Includes the core models like `Music`, `User`, and `LibraryItem` which handle the logic of the application.
- **Views**: Contains Razor Views for various parts of the app, including user authentication views, dashboard, and music search results.
- **Services**: Contains services like `ApiService` and `SpotifyAuthService` that handle external API calls and authentication with Spotify.
- **obj**: Contains build-related files and intermediate files.
- **Properties**: Contains application settings such as `launchSettings.json` for environment-specific settings.

## Use Cases

1. **User Authentication (Login/Register)**: Secure user login, registration, and session management.
2. **Search for Music**: Search and display results from the Spotify API based on user queries.
3. **Add Music to Library**: Add searched music to a user's personal library.
4. **Rate Music**: Users can rate songs they've added to their library.
5. **View and Manage Library**: Users can view their library, filter by genre, and manage their saved music.
