-- 6. Associação Jogo-Gênero
-- Usando subqueries para garantir que os IDs batam perfeitamente.

-- Super Mario World (Plataforma, Aventura)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Super Mario World'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Plataforma')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Super Mario World'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Aventura'));

-- Zelda: A Link to the Past (Aventura, RPG)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'The Legend of Zelda: A Link to the Past'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Aventura')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'The Legend of Zelda: A Link to the Past'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'RPG'));

-- Donkey Kong Country (Plataforma)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Donkey Kong Country'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Plataforma'));

-- Chrono Trigger (RPG, Aventura)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Chrono Trigger'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'RPG')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Chrono Trigger'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Aventura'));

-- Sonic 2 (Plataforma, Ação)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Sonic the Hedgehog 2'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Plataforma')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Sonic the Hedgehog 2'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Ação'));

-- Streets of Rage 2 (Luta, Ação)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Streets of Rage 2'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Luta')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Streets of Rage 2'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Ação'));

-- Gunstar Heroes (Ação, Tiro)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Gunstar Heroes'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Ação')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Gunstar Heroes'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Tiro'));

-- Final Fantasy VII (RPG)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Final Fantasy VII'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'RPG'));

-- Metal Gear Solid (Ação, Aventura)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Metal Gear Solid'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Ação')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Metal Gear Solid'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Aventura'));

-- Crash Bandicoot (Plataforma)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Crash Bandicoot'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Plataforma'));

-- Resident Evil 2 (Ação, Aventura)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Resident Evil 2'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Ação')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Resident Evil 2'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Aventura'));

-- Castlevania: SOTN (Ação, Aventura, RPG)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Castlevania: Symphony of the Night'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Ação')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Castlevania: Symphony of the Night'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Aventura')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Castlevania: Symphony of the Night'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'RPG'));

-- Super Mario 64 (Plataforma, Aventura)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Super Mario 64'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Plataforma')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Super Mario 64'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Aventura'));

-- Zelda: Ocarina of Time (Aventura, RPG)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'The Legend of Zelda: Ocarina of Time'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Aventura')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'The Legend of Zelda: Ocarina of Time'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'RPG'));

-- GoldenEye 007 (Tiro, Ação)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'GoldenEye 007'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Tiro')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'GoldenEye 007'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Ação'));

-- Mario Kart 64 (Corrida)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Mario Kart 64'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Corrida'));

-- Alex Kidd (Plataforma)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Alex Kidd in Miracle World'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Plataforma'));

-- Wonder Boy III (Aventura, Plataforma)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Wonder Boy III: The Dragon''s Trap'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Aventura')),
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Wonder Boy III: The Dragon''s Trap'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Plataforma'));

-- Pitfall! (Plataforma)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Pitfall!'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Plataforma'));

-- Space Invaders (Tiro)
INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES
((SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Space Invaders'), (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'Tiro'));