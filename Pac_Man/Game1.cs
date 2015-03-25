﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Pac_Man.Animations;
using Microsoft.Xna.Framework.Audio;
using Pac_Man.Animations;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Pac_Man
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bloco;
        Texture2D paredeFerro;
        Texture2D comida;
        Texture2D sem_comida;
        
        List<Personagem> fantasmas;
        List<Personagem> pacmans;
        SoundEffect somComer;
        SoundEffect somExplosao;
        SoundEffect somAviso;
        SoundEffectInstance instanceSomComer;

        SpriteFont myFont;
        Texture2D dummyTexture;
        
        float ultimoMovimento = 0f;
        float time;
        int gametime;
        float contador;
        bool bombaLargada = false;
        public float tempoExpulão;
        int numerodeBombasimplantadas=0;
        bool proximaBombaPac1;
        bool proximaBombaPac2;
        Texture2D bomba;

        Texture2D portal_saida;
        Texture2D portal_entrada;

        bool doisJogadores;

        /*
         * 0 - Caminho / Comida
         * 1 - Parede
         * 2 - ???
         * 3 - sasa dos fantasmas
         * 4 - Portal de saida
         * 5 - Portal de entrada
         * 6 - Bomba
        */
        
        KeyboardState teclado;
        byte[,] mapa ={{3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
                        {3,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,3},
                        {3,0,1,0,0,1,1,0,1,1,0,1,0,1,1,0,0,1,0,3},
                        {3,0,1,0,0,1,1,0,1,1,0,1,0,1,1,0,0,1,0,3},
                        {3,0,1,1,0,1,1,0,0,0,0,1,0,1,1,0,1,1,0,3},
                        {3,0,0,0,0,0,0,0,1,1,0,1,0,0,0,0,0,0,0,3},
                        {3,0,1,1,1,1,1,0,1,1,0,1,0,1,1,1,1,1,0,3},
                        {3,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,3},
                        {3,1,1,0,1,0,1,1,1,1,1,1,1,1,0,1,0,1,1,3},
                        {0,0,0,0,1,0,2,2,2,2,2,2,2,1,0,1,0,0,0,0},
                        {3,1,1,0,1,0,1,1,1,1,1,1,1,1,0,1,0,1,1,3},
                        {3,0,0,0,1,0,0,0,0,0,0,0,0,0,0,1,0,0,0,3},
                        {3,0,1,1,1,1,1,0,1,0,1,1,0,1,1,1,1,1,0,3},
                        {3,0,0,0,0,0,0,0,1,0,1,1,0,0,0,0,0,0,0,3},
                        {3,0,1,1,0,1,1,0,1,0,1,1,0,1,1,0,1,1,0,3},
                        {3,0,1,0,0,1,1,0,1,0,0,0,0,1,1,0,0,1,0,3},
                        {3,0,1,0,0,0,0,0,1,0,1,1,0,0,0,0,0,1,0,3},
                        {3,0,1,1,0,1,1,0,1,0,1,1,0,1,1,0,1,1,0,3},
                        {3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3},
                        {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3}};

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = true;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            teclado = new KeyboardState();

            fantasmas = new List<Personagem>();
            pacmans = new List<Personagem>();
            doisJogadores = false;

            SpriteAnimationManager.Initialize();
            
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bloco = Content.Load<Texture2D>("parede");
            paredeFerro = Content.Load<Texture2D>("parede2");
            
            Personagem pac = new Personagem(Content, "pac2", TipoPersonagem.Player, mapa, Color.Yellow, 0);
            pacmans.Add(pac);
            
            Personagem fantasma = new Personagem(Content, "ghost", TipoPersonagem.NPC, mapa, Color.Green, 1).teleportTo(new Vector2(9, 7));
            fantasma.Velocidade = 0.5f;
            fantasmas.Add(fantasma);

            Personagem fantasma2 = new Personagem(Content, "ghost", TipoPersonagem.NPC, mapa, Color.Red, 6).teleportTo(new Vector2(9, 9));
            fantasma2.Velocidade = 0.5f;
            fantasmas.Add(fantasma2);

            Personagem fantasma3 = new Personagem(Content, "ghost", TipoPersonagem.NPC, mapa, Color.Blue, 4).teleportTo(new Vector2(9, 11));
            fantasma3.Velocidade = 0.5f;
            fantasmas.Add(fantasma3);
            
            comida = Content.Load<Texture2D>("comida");
            sem_comida = Content.Load<Texture2D>("sem_comida");
            bomba = Content.Load<Texture2D>("Bomb");
            portal_saida = Content.Load<Texture2D>("portal_saida");
            portal_entrada = Content.Load<Texture2D>("portal_entrada");


            dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });

            myFont = Content.Load<SpriteFont>("MyFont");
            //som
            somComer = Content.Load<SoundEffect>("som\\pacmanComer");
            somAviso = Content.Load<SoundEffect>("som\\avisodaBomba");
            somExplosao=Content.Load<SoundEffect>("som\\explosao");
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            bloco.Dispose();
            paredeFerro.Dispose();
            foreach (Personagem pacman in pacmans)
            {
                pacman.Dispose();
            }
            comida.Dispose();
            sem_comida.Dispose();
            bomba.Dispose();
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            ultimoMovimento += (float)gameTime.ElapsedGameTime.TotalSeconds;
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            teclado = Keyboard.GetState();
            gametime = (int)time;
            contador += 0.5f;
            
            
            if (ultimoMovimento > 0.09f)
            {

                updateInput();
                
                if (bombaLargada == true) 
                {
                    tempoExpulão += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }


                foreach (Personagem pacman in pacmans)
                {
                    pacman.Update(gameTime, pacmans, mapa, fantasmas,tempoExpulão, Content);
                    
                    pacman.UpdateBombs(tempoExpulão, mapa,gameTime, Content);
                    
                }
                
                foreach (Personagem fantasma in fantasmas)
                {
                    fantasma.Update(gameTime, pacmans, mapa, fantasmas,tempoExpulão, Content);
                }

                comer();
                bombaLargada = false;
               
                
                ultimoMovimento = 0;
                tempoExpulão = 0;
                
                
            }

            //atualizar explosões
            SpriteAnimationManager.Update(gameTime);

            base.Update(gameTime);

        }

        private void updateInput()
        {
            foreach (Personagem pacman in pacmans)
            {
                if (pacman == pacmans[0])
                {

                    //Verificar se há um 2º jogador para entrar
                    if ((teclado.IsKeyDown(Keys.Up)
                        || teclado.IsKeyDown(Keys.Down)
                        || teclado.IsKeyDown(Keys.Left)
                        || teclado.IsKeyDown(Keys.Right)
                        || teclado.IsKeyDown(Keys.Insert)
                        || teclado.IsKeyDown(Keys.Delete))
                        && !doisJogadores)
                    {
                        doisJogadores = true;
                    }

                    #region Player 1
                    if (teclado.IsKeyDown(Keys.W) &&
                            !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X, pacman.Posicao.Y - 1))
                            && teclado.IsKeyUp(Keys.S)
                            && teclado.IsKeyUp(Keys.A)
                            && teclado.IsKeyUp(Keys.D))
                    {
                        pacman.moverPacMan(Direccao.Cima);
                    }
                    if (teclado.IsKeyDown(Keys.A) &&
                        !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X - 1, pacman.Posicao.Y))
                        && teclado.IsKeyUp(Keys.S)
                        && teclado.IsKeyUp(Keys.W)
                        && teclado.IsKeyUp(Keys.D))
                    {
                        pacman.moverPacMan(Direccao.Esquerda);
                    }
                    if (teclado.IsKeyDown(Keys.D) &&
                        !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X + 1, pacman.Posicao.Y))
                        && teclado.IsKeyUp(Keys.S)
                        && teclado.IsKeyUp(Keys.A)
                        && teclado.IsKeyUp(Keys.W))
                    {
                        pacman.moverPacMan(Direccao.Direita);
                    }
                    if (teclado.IsKeyDown(Keys.S) &&
                        !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X, pacman.Posicao.Y + 1))
                        && teclado.IsKeyUp(Keys.W)
                        && teclado.IsKeyUp(Keys.A)
                        && teclado.IsKeyUp(Keys.D))
                    {
                        pacman.moverPacMan(Direccao.Baixo);
                    }
                    if (teclado.IsKeyDown(Keys.Space))
                    {
                        if (Utils.existePortal(mapa, 4) && !Utils.existePortal(mapa, 5) && Utils.posicaoPortalSaida(mapa).X != (int)pacman.Posicao.X && Utils.posicaoPortalSaida(mapa).Y != (int)pacman.Posicao.Y)
                        {
                            //Já existe portal de saída, vamos colocar um portal de entrada
                            mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 5;
                        }
                        else if (!Utils.existePortal(mapa, 4))
                        {
                            //Ainda não existe portal de saida, vamos colocar portal de saida
                            mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 4;
                        }
                    }
                    if (teclado.IsKeyDown(Keys.B)&&proximaBombaPac1==true)
                    {
                        if (pacmans[0].Score > 50 && numerodeBombasimplantadas == 0)
                        {

                            // posição da bomba passa a ser igual à posição do pac neste instante de tempo!!
                            //Bomba bomb=new Bomba(Content, "Bomb", Color.White, pacman.Posicao);
                            //bombas.Add(bomb);
                            //PosiçãoBomba = new Vector2(pacman.Posicao.X, pacman.Posicao.Y);
                            //mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 6;
                            pacmans[0].Score=pacmans[0].insereBomba(pacmans[0].Score);
                            bombaLargada = true;
                            proximaBombaPac2 = false;
                            //numerodeBombasimplantadas = 1;

                        }
                    }
                    if (teclado.IsKeyUp(Keys.B)) 
                    {
                        proximaBombaPac1 = true;
                    }
                    #endregion
                }
                else
                {
                    #region Player 2

                    if (doisJogadores)
                    {
                        if (teclado.IsKeyDown(Keys.Up) &&
                            !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X, pacman.Posicao.Y - 1))
                            && teclado.IsKeyUp(Keys.Down)
                            && teclado.IsKeyUp(Keys.Left)
                            && teclado.IsKeyUp(Keys.Right))
                        {
                            pacman.moverPacMan(Direccao.Cima);
                        }
                        if (teclado.IsKeyDown(Keys.Left) &&
                            !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X - 1, pacman.Posicao.Y))
                            && teclado.IsKeyUp(Keys.Down)
                            && teclado.IsKeyUp(Keys.Up)
                            && teclado.IsKeyUp(Keys.Right))
                        {
                            pacman.moverPacMan(Direccao.Esquerda);
                        }
                        if (teclado.IsKeyDown(Keys.Right) &&
                            !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X + 1, pacman.Posicao.Y))
                            && teclado.IsKeyUp(Keys.Down)
                            && teclado.IsKeyUp(Keys.Left)
                            && teclado.IsKeyUp(Keys.Up))
                        {
                            pacman.moverPacMan(Direccao.Direita);
                        }
                        if (teclado.IsKeyDown(Keys.Down) &&
                            !Colisoes.paredeEncontrada(mapa, new Vector2(pacman.Posicao.X, pacman.Posicao.Y + 1))
                            && teclado.IsKeyUp(Keys.Up)
                            && teclado.IsKeyUp(Keys.Left)
                            && teclado.IsKeyUp(Keys.Right))
                        {
                            pacman.moverPacMan(Direccao.Baixo);
                        }
                        if (teclado.IsKeyDown(Keys.Insert))
                        {
                            if (Utils.existePortal(mapa, 4) && !Utils.existePortal(mapa, 5) && Utils.posicaoPortalSaida(mapa).X != (int)pacman.Posicao.X && Utils.posicaoPortalSaida(mapa).Y != (int)pacman.Posicao.Y)
                            {
                                //Já existe portal de saída, vamos colocar um portal de entrada
                                mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 5;
                            }
                            else if (!Utils.existePortal(mapa, 4) && !Utils.existePortal(mapa, 5))
                            {
                                //Ainda não existe portal de saida, vamos colocar portal de saida
                                mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 4;
                            }
                        }
                        if (teclado.IsKeyDown(Keys.Delete)&& proximaBombaPac2==true)
                        {
                           if (pacmans[1].Score > 20 && numerodeBombasimplantadas == 0)
                            {

                            // posição da bomba passa a ser igual à posição do pac neste instante de tempo!!
                            //Bomba bomb=new Bomba(Content, "Bomb", Color.White, pacman.Posicao);
                            //bombas.Add(bomb);
                            //PosiçãoBomba = new Vector2(pacman.Posicao.X, pacman.Posicao.Y);
                            //mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 6;
                            pacmans[1].Score=pacmans[1].insereBomba(pacmans[1].Score);
                            bombaLargada = true;
                            proximaBombaPac2 = false;
                            //numerodeBombasimplantadas = 1;

                            }

                            
                        }
                        if (teclado.IsKeyUp(Keys.Delete))
                        {
                            proximaBombaPac2 = true;
                        }
                    #endregion
                    }
                }
            }
            if (doisJogadores && pacmans.Count == 1)
            {
                criarSegundoJogador();
            }
        }

        
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            //desenhar o mapa
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    switch (mapa[x, y])
                    {
                        case 0:
                            spriteBatch.Draw(comida, new Vector2((x * 30) + 12, (y * 30) + 12), Color.White);
                            break;
                        case 1:
                            spriteBatch.Draw(bloco, new Vector2(x * 30, y * 30), Color.White);
                            break;
                        case 3:
                            spriteBatch.Draw(paredeFerro, new Vector2(x * 30, y * 30), Color.White);
                            break;

                        case 4:
                            spriteBatch.Draw(portal_saida, new Vector2(x * 30, y * 30), Color.White);
                            break;
                        case 5:
                            spriteBatch.Draw(portal_entrada, new Vector2(x * 30, y * 30), Color.White);
                            break;
                        case 6:
                            
                            if (contador >= 3f)
                            {
                                spriteBatch.Draw(bomba, new Vector2(x * 30, y * 30), Color.White);
                                contador = 0f;
                                somAviso.Play();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        

            foreach (Personagem pacman in pacmans)
            {
                pacman.Draw(spriteBatch, gameTime,mapa);
            }

            foreach (Personagem fantasma in fantasmas)
            {
                fantasma.Draw(spriteBatch, gameTime,mapa);
            }

            //Desenhar explosões
            SpriteAnimationManager.Draw(spriteBatch);

            //desenhar texto e mostrar pontuaçao
            spriteBatch.DrawString(myFont, "Score", new Vector2(650, 10), Color.Yellow);
            spriteBatch.DrawString(myFont, pacmans[0].Score+"", new Vector2(680, 50), Color.Yellow);
            
            if(pacmans.Count==2)
            {
                spriteBatch.DrawString(myFont, "Score", new Vector2(650, 100), Color.Pink);
                spriteBatch.DrawString(myFont, pacmans[1].Score + "", new Vector2(680, 150), Color.Pink);
            }
            
            spriteBatch.DrawString(myFont, "Game Time", new Vector2(620, 200), Color.Yellow);
            spriteBatch.DrawString(myFont, gametime + "sec", new Vector2(680, 250), Color.Yellow);

            

            spriteBatch.End();

            base.Draw(gameTime);

        }

        private void criarSegundoJogador()
        {
            Personagem pac = new Personagem(Content, "pac2", TipoPersonagem.Player, mapa, Color.Pink, 0).teleportTo(new Vector2(11, 5));
            pacmans.Add(pac);
        }
        

        //metodo para eliminar comida apos pacman passar por cima
        private void comer()
        {
            foreach (Personagem pacman in pacmans)
            {
                
                if (mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] == 0)
                {
                    mapa[(int)pacman.Posicao.X, (int)pacman.Posicao.Y] = 2;
                       pacman.Score += 10;
                    //som comer
                       somComer.Play();
                }
            }
        }  
       
    }
    
}
