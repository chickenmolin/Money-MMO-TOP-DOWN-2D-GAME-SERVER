// models/playerModel.js
const db = require('../config/db');

async function getPlayerByUsername(username) {
    const [rows] = await db.query('SELECT * FROM players WHERE username = ?', [username]);
    return rows[0];
}

async function updatePoints(username, points) {
    await db.query('UPDATE players SET points = ? WHERE username = ?', [points, username]);
}

module.exports = { getPlayerByUsername, updatePoints };