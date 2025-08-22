const express = require('express');
const router = express.Router();
const db = require('../config/db');

// Lấy thông tin player theo username
router.get('/:username', async (req, res) => {
    try {
        const { username } = req.params;
        console.log("[SERVER] GET points for:", username);

        const [rows] = await db.query(
            'SELECT id, username, points FROM players WHERE username = ?',
            [username]
        );

        console.log("[SERVER] DB result:", rows);

        if (rows.length === 0) {
            return res.status(404).json({ message: 'Không tìm thấy player' });
        }

        // Trả JSON đầy đủ
res.json({
    id: rows[0].id,
    username: rows[0].username,
    points: rows[0].points,
    role: rows[0].role // ← thêm trường role
});

    } catch (err) {
        console.error("[SERVER] Lỗi truy vấn DB:", err);
        res.status(500).json({ error: 'Lỗi server' });
    }
});

// Cập nhật điểm player
router.put('/:username', async (req, res) => {
    try {
        const { username } = req.params;
        const { points } = req.body;

        if (typeof points !== 'number') {
            return res.status(400).json({ message: 'Giá trị points phải là số' });
        }

        // Lấy điểm hiện tại của player
        const [current] = await db.query(
            'SELECT points FROM players WHERE username = ?',
            [username]
        );

        if (current.length === 0) {
            return res.status(404).json({ message: 'Không tìm thấy player' });
        }

        const oldPoints = current[0].points;

        // Cập nhật điểm mới
        const [result] = await db.query(
            'UPDATE players SET points = ? WHERE username = ?',
            [points, username]
        );

        if (result.affectedRows === 0) {
            return res.status(404).json({ message: 'Không tìm thấy player' });
        }

        // Nếu điểm thay đổi thì log ra
        if (oldPoints !== points) {
            console.log(`[SERVER] ⚠️ Player ${username}: điểm thay đổi từ ${oldPoints} → ${points}`);
        }

        res.json({ message: 'Cập nhật điểm thành công', points });

    } catch (err) {
        console.error("[SERVER] Lỗi cập nhật DB:", err);
        res.status(500).json({ error: 'Lỗi server' });
    }
});


module.exports = router;
