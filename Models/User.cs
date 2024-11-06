using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyPlayer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        public ICollection<FavoriteSong> FavoriteSongs { get; set; } = new List<FavoriteSong>();
    }
}
