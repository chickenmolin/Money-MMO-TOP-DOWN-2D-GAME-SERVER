  -- Tạo database
  CREATE DATABASE IF NOT EXISTS mmorpg CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
  USE mmorpg;

  -- Xóa bảng nếu tồn tại
  DROP TABLE IF EXISTS players;

  -- Tạo bảng players
  CREATE TABLE players (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    password VARCHAR(50) NOT NULL,
    points INT NOT NULL DEFAULT 0,
    role VARCHAR(10) NOT NULL DEFAULT 'client'
  );

  -- Thêm dữ liệu mẫu
  INSERT INTO players (username, password, points, role) VALUES
  ('player1','123456',1000,'host'),       -- Được quyền Host + Server
  ('player2','654321',100,'client'),      -- Chỉ Client
  ('player3','abc123',200,'client'),      -- Chỉ Client
  ('player_server','server123',0,'server'); -- Chỉ Server
