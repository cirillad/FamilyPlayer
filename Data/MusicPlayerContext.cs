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

        public MusicPlayerContext()  { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"workstation id=FamilyPlayer.mssql.somee.com;packet size=4096;user id=Hattori1_SQLLogin_1;pwd=hz9gzpr8rm;data source=FamilyPlayer.mssql.somee.com;persist security info=False;initial catalog=FamilyPlayer;TrustServerCertificate=True");
        }
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
