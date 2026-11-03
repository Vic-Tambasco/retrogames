USE retrogames;

-- 1. Condições (Chaves Fixas)
INSERT INTO tb_condicao_jogo (co_condicao_jogo, ds_condicao_jogo) VALUES
(1, 'Novo'),
(2, 'Usado'),
(3, 'Recondicionado');

-- 2. Perfis (Chaves Fixas)
INSERT INTO tb_perfil (co_perfil, ds_perfil) VALUES
(1, 'Administrador'),
(2, 'Cliente');

-- 3. Plataformas (6 itens)
INSERT INTO tb_plataforma (no_plataforma, ds_fabricante, nu_ano_lancamento) VALUES
('Super Nintendo', 'Nintendo', 1990),
('Mega Drive', 'Sega', 1988),
('PlayStation 1', 'Sony', 1994),
('Nintendo 64', 'Nintendo', 1996),
('Master System', 'Sega', 1985),
('Atari 2600', 'Atari', 1977);

-- 4. Gêneros (8 itens)
INSERT INTO tb_genero (no_genero) VALUES
('Ação'),
('Aventura'),
('RPG'),
('Corrida'),
('Luta'),
('Plataforma'),
('Tiro'),
('Esporte');

-- 5. Jogos (20 itens)
-- Note que usei subqueries para pegar os IDs de plataforma e condição, e definimos dt_cadastro como NOW()
INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, co_plataforma, co_condicao_jogo, dt_cadastro) VALUES
-- Super Nintendo
('Super Mario World', 'A clássica aventura do Mario no SNES.', 1990, 150.00, 5, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Super Nintendo'), 2, NOW()),
('The Legend of Zelda: A Link to the Past', 'Um dos maiores clássicos do SNES.', 1991, 200.00, 3, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Super Nintendo'), 2, NOW()),
('Donkey Kong Country', 'Gráficos revolucionários para a época.', 1994, 120.00, 0, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Super Nintendo'), 2, NOW()), -- Estoque zero
('Chrono Trigger', 'Um dos melhores RPGs de todos os tempos.', 1995, 350.00, 2, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Super Nintendo'), 1, NOW()), -- Novo

-- Mega Drive
('Sonic the Hedgehog 2', 'A velocidade do ouriço azul.', 1992, 80.00, 10, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Mega Drive'), 2, NOW()),
('Streets of Rage 2', 'O melhor beat ''em up do Mega Drive.', 1992, 110.00, 4, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Mega Drive'), 2, NOW()),
('Gunstar Heroes', 'Ação frenética e tiroteio.', 1993, 180.00, 1, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Mega Drive'), 3, NOW()), -- Recondicionado

-- PlayStation 1
('Final Fantasy VII', 'O RPG que definiu uma geração.', 1997, 250.00, 6, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'PlayStation 1'), 2, NOW()),
('Metal Gear Solid', 'Espionagem tática e narrativa cinematográfica.', 1998, 150.00, 3, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'PlayStation 1'), 2, NOW()),
('Crash Bandicoot', 'Plataforma 3D clássico.', 1996, 90.00, 8, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'PlayStation 1'), 2, NOW()),
('Resident Evil 2', 'O terror sobrevivência no auge.', 1998, 200.00, 0, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'PlayStation 1'), 2, NOW()), -- Estoque zero
('Castlevania: Symphony of the Night', 'Um dos maiores jogos 2D já feitos.', 1997, 300.00, 1, FALSE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'PlayStation 1'), 2, NOW()), -- Inativo

-- Nintendo 64
('Super Mario 64', 'A revolução do 3D nos videogames.', 1996, 180.00, 5, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Nintendo 64'), 2, NOW()),
('The Legend of Zelda: Ocarina of Time', 'Considerado por muitos o melhor jogo da história.', 1998, 280.00, 2, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Nintendo 64'), 2, NOW()),
('GoldenEye 007', 'O FPS que popularizou o multiplayer em consoles.', 1997, 100.00, 4, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Nintendo 64'), 2, NOW()),
('Mario Kart 64', 'Diversão garantida nas pistas.', 1996, 130.00, 7, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Nintendo 64'), 2, NOW()),

-- Master System
('Alex Kidd in Miracle World', 'O mascote original do Master System.', 1986, 60.00, 3, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Master System'), 2, NOW()),
('Wonder Boy III: The Dragon''s Trap', 'Aventura e exploração.', 1989, 120.00, 2, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Master System'), 2, NOW()),

-- Atari 2600
('Pitfall!', 'O clássico de plataforma do Atari.', 1982, 40.00, 10, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Atari 2600'), 2, NOW()),
('Space Invaders', 'O jogo que popularizou os arcades.', 1980, 35.00, 15, TRUE, (SELECT co_seq_plataforma FROM tb_plataforma WHERE no_plataforma = 'Atari 2600'), 2, NOW());