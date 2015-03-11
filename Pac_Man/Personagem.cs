﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pac_Man
{

    public enum Direccao
    {
        Cima,
        Baixo,
        Esquerda,
        Direita
    }

    public class Personagem
    {
        private Texture2D textura;
        /// <summary>
        /// Textura utilizada por esta personagem
        /// </summary>
        public Texture2D Textura
        {
            get { return textura; }
        }

        private Vector2 posicao;
        /// <summary>
        /// Posição da personagem no mundo
        /// </summary>
        public Vector2 Posicao
        {
            get { return posicao; }
            set { posicao = value; }
        }

        private float rotacao;
        /// <summary>
        /// Rotação da personagem
        /// </summary>
        public float Rotacao
        {
            get { return rotacao; }
            set { rotacao = value; }
        }

        private float velocidade;
        /// <summary>
        /// Velocidade a que a personagem se move
        /// </summary>
        public float Velocidade
        {
            get { return velocidade; }
            set { velocidade = value; }
        }
        
        

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="content">Instância de ContentManager</param>
        /// <param name="assetName">Nome da textura desta personagem</param>
        public Personagem(ContentManager content, string assetName)
        {
            this.Rotacao = 0f;
            this.velocidade = 0.05f;
            this.Posicao = new Vector2(2, 1);
            this.textura = content.Load<Texture2D>(assetName);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void moverPacMan(Direccao direccao)
        {
            switch (direccao)
            {
                case Direccao.Cima:
                    this.posicao.Y -= Velocidade;
                    break;
                case Direccao.Baixo:
                    this.posicao.Y += Velocidade;
                    break;
                case Direccao.Esquerda:
                    this.posicao.X -= Velocidade;
                    break;
                case Direccao.Direita:
                    this.posicao.X += Velocidade;
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Textura, new Vector2(Posicao.X * 30, Posicao.Y * 30 + 5f), Color.White);
        }

        /// <summary>
        /// Destruir a textura
        /// </summary>
        public void Dispose()
        {
            this.textura.Dispose();
        }

    }
}