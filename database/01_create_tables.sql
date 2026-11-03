-- Criação do banco de dados (execute esta linha separadamente no Workbench antes de rodar o resto)
CREATE DATABASE IF NOT EXISTS retrogames CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE retrogames;

-- 1. Tabelas de Domínio (Chaves Fixas, sem AUTO_INCREMENT)
CREATE TABLE tb_condicao_jogo (
    co_condicao_jogo INT NOT NULL,
    ds_condicao_jogo VARCHAR(50) NOT NULL UNIQUE,
    PRIMARY KEY (co_condicao_jogo)
);

CREATE TABLE tb_perfil (
    co_perfil INT NOT NULL,
    ds_perfil VARCHAR(50) NOT NULL UNIQUE,
    PRIMARY KEY (co_perfil)
);

-- 2. Tabela de Plataforma
CREATE TABLE tb_plataforma (
    co_seq_plataforma INT NOT NULL AUTO_INCREMENT,
    no_plataforma VARCHAR(100) NOT NULL UNIQUE,
    ds_fabricante VARCHAR(100) NOT NULL,
    nu_ano_lancamento INT NOT NULL,
    PRIMARY KEY (co_seq_plataforma)
);

-- 3. Tabela de Gênero
CREATE TABLE tb_genero (
    co_seq_genero INT NOT NULL AUTO_INCREMENT,
    no_genero VARCHAR(100) NOT NULL UNIQUE,
    PRIMARY KEY (co_seq_genero)
);

-- 4. Tabela Principal de Jogo
CREATE TABLE tb_jogo (
    co_seq_jogo INT NOT NULL AUTO_INCREMENT,
    no_jogo VARCHAR(150) NOT NULL,
    ds_jogo TEXT NULL, -- Única coluna que aceita NULL conforme a especificação
    nu_ano_lancamento INT NOT NULL,
    vl_preco DECIMAL(10,2) NOT NULL,
    qt_estoque INT NOT NULL,
    st_ativo BOOLEAN NOT NULL DEFAULT TRUE,
    co_plataforma INT NOT NULL,
    co_condicao_jogo INT NOT NULL,
    dt_cadastro DATETIME NOT NULL,
    PRIMARY KEY (co_seq_jogo),
    INDEX idx_no_jogo (no_jogo),
    CONSTRAINT fk_jogo_plataforma FOREIGN KEY (co_plataforma) REFERENCES tb_plataforma (co_seq_plataforma),
    CONSTRAINT fk_jogo_condicao FOREIGN KEY (co_condicao_jogo) REFERENCES tb_condicao_jogo (co_condicao_jogo)
);

-- 5. Tabela Associativa Jogo-Gênero
CREATE TABLE tb_jogo_genero (
    co_seq_jogo INT NOT NULL,
    co_seq_genero INT NOT NULL,
    PRIMARY KEY (co_seq_jogo, co_seq_genero),
    CONSTRAINT fk_jg_jogo FOREIGN KEY (co_seq_jogo) REFERENCES tb_jogo (co_seq_jogo),
    CONSTRAINT fk_jg_genero FOREIGN KEY (co_seq_genero) REFERENCES tb_genero (co_seq_genero)
);

-- 6. Tabela de Usuário
CREATE TABLE tb_usuario (
    co_seq_usuario INT NOT NULL AUTO_INCREMENT,
    no_usuario VARCHAR(100) NOT NULL,
    ds_email VARCHAR(150) NOT NULL UNIQUE,
    ds_senha_hash VARCHAR(255) NOT NULL,
    co_perfil INT NOT NULL,
    PRIMARY KEY (co_seq_usuario),
    CONSTRAINT fk_usuario_perfil FOREIGN KEY (co_perfil) REFERENCES tb_perfil (co_perfil)
);