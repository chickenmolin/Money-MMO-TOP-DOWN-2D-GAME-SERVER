// routes/combat.js
const express = require('express');
const router = express.Router();

router.post('/', (req, res) => {
    const { playerDamage, enemyDamage } = req.body;
    res.json({ message: 'Combat processed', result: { playerDamage, enemyDamage } });
});

module.exports = router;
