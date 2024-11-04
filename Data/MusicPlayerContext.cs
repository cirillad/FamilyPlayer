using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FamilyPlayer.Models;

namespace FamilyPlayer.Data
{
    public class MusicPlayerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<FavoriteSong> FavoriteSongs { get; set; }

        public MusicPlayerContext(DbContextOptions<MusicPlayerContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Playlists)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteSongs)
                .WithOne(fs => fs.User)
                .HasForeignKey(fs => fs.UserId);

            modelBuilder.Entity<FavoriteSong>()
                .HasKey(fs => new { fs.UserId, fs.SongId });
            modelBuilder.Entity<Playlist>()
                .HasMany(p => p.Songs)
                .WithMany(s => s.Playlists)
                .UsingEntity(j => j.ToTable("PlaylistSongs"));
        }

        public void SeedData()
        {
            if (!Users.Any())
            {
                Users.Add(new User
                {
                    Username = "testUser",
                    Password = "hashedPassword",
                    Playlists = new List<Playlist>
                    {
                        new Playlist { Name = "My First Playlist" }
                    }
                });
                SaveChanges();
            }
        }
    }
}
