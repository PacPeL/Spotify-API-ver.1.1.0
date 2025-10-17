// app.js - Conexión a tu API en https://localhost:7000
const apiUrl = "http://localhost:7000/api/music";


async function fetchSongs() {
    try {
        const response = await fetch(apiUrl);

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const songs = await response.json();

        if (!songs || songs.length === 0) {
            document.getElementById("songs-container").innerHTML = "<p>No hay canciones disponibles.</p>";
            return;
        }

        displaySongs(songs);

    } catch (error) {
        console.error("Error al cargar canciones:", error);
        document.getElementById("songs-container").innerHTML = "<p>Error cargando canciones desde la API.</p>";
    }
}

function displaySongs(songs) {
    const container = document.getElementById("songs-container");
    container.innerHTML = ""; // Limpiar contenedor

    songs.forEach(song => {
        const card = document.createElement("div");
        card.className = "song-card";

        // Imagen del álbum
        const img = document.createElement("img");
        img.src = `data:image/jpeg;base64,${song.imageBase64}`;
        img.alt = `${song.title} cover`;
        card.appendChild(img);

        // Título
        const title = document.createElement("h3");
        title.textContent = song.title;
        card.appendChild(title);

        // Artista
        const artist = document.createElement("p");
        artist.textContent = song.artist;
        card.appendChild(artist);

        // Reproductor de audio
        const audio = document.createElement("audio");
        audio.controls = true;
        audio.src = `data:audio/mpeg;base64,${song.audioBase64}`;
        card.appendChild(audio);

        container.appendChild(card);
    });
}

// Cargar canciones al iniciar
fetchSongs();
