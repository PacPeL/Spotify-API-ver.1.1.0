using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spotify_API.Context;
using Spotify_API.Models;

namespace Spotify_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MusicController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/music
        [HttpGet]
        public async Task<IActionResult> GetSongs()
        {
            var songs = await _context.Songs.ToListAsync();
            return Ok(songs);
        }

        // GET: api/music/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null) return NotFound();
            return Ok(song);
        }

        // POST: api/music/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadSong([FromForm] SongUploadDto dto)
        {
            try
            {
                if (dto.Audio == null || dto.Image == null)
                    return BadRequest("Audio and image files are required.");

                using var audioStream = new MemoryStream();
                await dto.Audio.CopyToAsync(audioStream);
                string audioBase64 = Convert.ToBase64String(audioStream.ToArray());

                using var imageStream = new MemoryStream();
                await dto.Image.CopyToAsync(imageStream);
                string imageBase64 = Convert.ToBase64String(imageStream.ToArray());

                var song = new Song
                {
                    Title = dto.Title,
                    Artist = dto.Artist,
                    Album = dto.Album,
                    AudioBase64 = audioBase64,
                    ImageBase64 = imageBase64
                };

                _context.Songs.Add(song);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetSong), new { id = song.Id }, song);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

                // PUT: api/music/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromBody] SongUpdateDto dto)
        {
            try
            {
                var song = await _context.Songs.FindAsync(id);
                if (song == null) return NotFound("Canción no encontrada.");

                // Actualizar campos si se proporcionan
                song.Title = dto.Title ?? song.Title;
                song.Artist = dto.Artist ?? song.Artist;
                song.Album = dto.Album ?? song.Album;

                await _context.SaveChangesAsync();

                return Ok(song);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/music/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSong(int id)
        {
            try
            {
                var song = await _context.Songs.FindAsync(id);
                if (song == null) return NotFound("Canción no encontrada.");

                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();

                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DTO para actualizar canción
        public class SongUpdateDto
        {
            public string Title { get; set; }
            public string Artist { get; set; }
            public string Album { get; set; }
        }


        // DTO para subida de archivos
        public class SongUploadDto
        {
            public string Title { get; set; }
            public string Artist { get; set; }
            public string Album { get; set; }
            public IFormFile Audio { get; set; }
            public IFormFile Image { get; set; }
        }
    }
}
