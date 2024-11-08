using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyPlayer.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Path { get; set; } 

        public ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
        public ICollection<FavoriteSong> FavoriteSong { get; set; } = new List<FavoriteSong>();
    }
}