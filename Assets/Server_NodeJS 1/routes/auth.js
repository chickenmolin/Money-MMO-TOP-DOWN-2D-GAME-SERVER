// routes/auth.js
const express = require('express');
const router = express.Router();
const playerModel = require('../models/playerModel');

router.post('/login', async (req, res) => {
    const { username, password } = req.body;
    const player = await playerModel.getPlayerByUsername(username);

    // Kiểm tra tài khoản
    if (!player || player.password !== password) {
        return res.status(401).json({ message: 'Sai tài khoản hoặc mật khẩu' });
    }

    // Chỉ gửi dữ liệu cần thiết sang client
    const playerData = {
        username: player.username,
        points: player.points,
        role: player.role
    };

    res.json({
        message: 'Đăng nhập thành công',
        player: playerData
    });
});

module.exports = router;
