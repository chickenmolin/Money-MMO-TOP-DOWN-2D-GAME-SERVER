// server.js
const express = require('express');
const cors = require('cors');
const bodyParser = require('body-parser');
const http = require('http');
const { Server } = require('socket.io');
const pool = require('./config/db'); // lấy kết nối DB

const authRoutes = require('./routes/auth');
const playerRoutes = require('./routes/player');
const combatRoutes = require('./routes/combat');

const app = express();
app.use(cors());
app.use(bodyParser.json());

// REST API
app.use('/auth', authRoutes);
app.use('/player', playerRoutes);
app.use('/combat', combatRoutes);

const server = http.createServer(app);
const io = new Server(server, {
    cors: { origin: "*", methods: ["GET", "POST"] }
});

let onlinePlayers = {};

io.on('connection', (socket) => {
    console.log(`🔌 Player connected: ${socket.id}`);

    // Player join
    socket.on('join', async (playerData) => {
        if (!playerData?.username) {
            console.log("⚠️ Player join request missing username");
            return;
        }

        try {
            // Lấy thông tin từ DB
            const [rows] = await pool.query(
                "SELECT username, points, role FROM players WHERE username = ?",
                [playerData.username]
            );

            if (rows.length > 0) {
                const player = rows[0];
                onlinePlayers[socket.id] = {
                    username: player.username,
                    points: player.points,
                    role: player.role,
                    x: Math.random() * 5,
                    y: Math.random() * 5
                };

                console.log(`📥 Player joined: ${player.username} | Points: ${player.points} | Role: ${player.role}`);
            } else {
                // Nếu không tìm thấy trong DB, dùng tạm dữ liệu client gửi
                onlinePlayers[socket.id] = {
                    username: playerData.username,
                    points: playerData.points || 0,
                    role: "client",
                    x: Math.random() * 5,
                    y: Math.random() * 5
                };
                console.log(`📥 Player joined (not in DB): ${playerData.username}`);
            }

            io.emit('updatePlayers', JSON.stringify(onlinePlayers));
        } catch (err) {
            console.error("❌ Error fetching player from DB:", err);
        }
    });

    // Cập nhật điểm
    socket.on('updateScore', (data) => {
        if (onlinePlayers[socket.id]) {
            onlinePlayers[socket.id].points = data.points;
            console.log(`🏆 ${onlinePlayers[socket.id].username} score updated: ${data.points}`);
            io.emit('updatePlayers', JSON.stringify(onlinePlayers));
        }
    });

    // Cập nhật vị trí
    socket.on('updatePosition', (pos) => {
        if (onlinePlayers[socket.id]) {
            onlinePlayers[socket.id].x = pos.x;
            onlinePlayers[socket.id].y = pos.y;
            io.emit('updatePlayers', JSON.stringify(onlinePlayers));
        }
    });

    // Player thoát
    socket.on('disconnect', () => {
        if (onlinePlayers[socket.id]) {
            console.log(`❌ Player disconnected: ${onlinePlayers[socket.id].username}`);
            delete onlinePlayers[socket.id];
            io.emit('updatePlayers', JSON.stringify(onlinePlayers));
        } else {
            console.log(`❌ Unknown socket disconnected: ${socket.id}`);
        }
    });
});

const PORT = 3000;
server.listen(PORT, () => {
    console.log(`✅ Server running on http://localhost:${PORT}`);
});
