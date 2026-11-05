-- ==========================================
-- ETAPA 5: Consultas do Catálogo e Testes de Integridade
-- ==========================================

-- 1. Catálogo: jogos ativos com o nome da plataforma e a descrição da condição, ordenados por nome.
SELECT 
    j.no_jogo,
    p.no_plataforma,
    c.ds_condicao_jogo
FROM tb_jogo j
JOIN tb_plataforma p ON j.co_plataforma = p.co_seq_plataforma
JOIN tb_condicao_jogo c ON j.co_condicao_jogo = c.co_condicao_jogo
WHERE j.st_ativo = TRUE
ORDER BY j.no_jogo;

-- 2. Gêneros de um jogo informado.
SELECT g.no_genero
FROM tb_jogo_genero jg
JOIN tb_genero g ON jg.co_seq_genero = g.co_seq_genero
WHERE jg.co_seq_jogo = (SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Chrono Trigger');

-- 3. Jogos de um gênero informado.
SELECT j.no_jogo
FROM tb_jogo j
JOIN tb_jogo_genero jg ON j.co_seq_jogo = jg.co_seq_jogo
JOIN tb_genero g ON jg.co_seq_genero = g.co_seq_genero
WHERE g.no_genero = 'Aventura'
ORDER BY j.no_jogo;

-- 4. Quantidade de jogos por plataforma, incluindo as plataformas sem jogos.
SELECT 
    p.no_plataforma, 
    COUNT(j.co_seq_jogo) AS qt_jogos
FROM tb_plataforma p
LEFT JOIN tb_jogo j ON p.co_seq_plataforma = j.co_plataforma
GROUP BY p.no_plataforma
ORDER BY p.no_plataforma;

-- 5. Busca por parte do nome do jogo, com 5 itens por página.
SELECT no_jogo, vl_preco
FROM tb_jogo
WHERE no_jogo LIKE '%Mario%' AND st_ativo = TRUE
ORDER BY no_jogo
LIMIT 5 OFFSET 0;

-- ==========================================
-- TESTES DE INTEGRIDADE (Anotar o resultado)
-- ==========================================

-- Teste 1: Inserir jogo com co_plataforma inexistente (999).
-- Resultado: ERROR 1452 (23000): Cannot add or update a child row: a foreign key constraint fails 

-- Teste 2: Inserir plataforma duplicada ('Super Nintendo').
-- Resultado: ERROR 1062 (23000): Duplicate entry 'Super Nintendo' for key 'no_plataforma'

-- Teste 3: Excluir plataforma 'Super Nintendo' que tem jogos.
-- Resultado: ERROR 1451 (23000): Cannot delete or update a parent row: a foreign key constraint fails

-- Teste 4: Inserir jogo com no_jogo nulo.
-- Resultado: ERROR 1048 (23000): Column 'no_jogo' cannot be null

-- Teste 5: Inserir jogo com vl_preco = 0.
-- Resultado: Query OK, 1 row affected (0,009 sec). Como o preço não era menor que 0, e sim 0, foi inserido.
