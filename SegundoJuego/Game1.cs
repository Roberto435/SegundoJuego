using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SegundoJuego
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D naveTexture;
        private Texture2D bombaTexture;
        private Vector2 navePosition;
        private Random _random;
        private bool _juegoTerminado;
        private SpriteFont fuente;
        private int _puntuacion;
        private Texture2D disparoTexture;
        private List<Vector2> disparos = new List<Vector2>();
        private List<Vector2> rocas = new List<Vector2>();
        private float disparoSpeed = 10f;
        private float bombaSpeed = 2f;

        // Variables para el tiempo de recarga
        private float tiempoRecarga = 0.7f; // Tiempo de recarga en segundos
        private float tiempoDesdeUltimoDisparo = 0f; // Tiempo transcurrido desde el último disparo
        private int cantidadRocas = 5; // Cantidad inicial de rocas

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _random = new Random();
            _juegoTerminado = false;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            naveTexture = Content.Load<Texture2D>("Nave");
            bombaTexture = Content.Load<Texture2D>("Roca");
            disparoTexture = Content.Load<Texture2D>("Disparo");
            fuente = Content.Load<SpriteFont>("MiFuente");
            navePosition = new Vector2(400, 400); // Posición inicial de la nave
            _puntuacion = 0;

            // Inicializar rocas
            for (int i = 0; i < cantidadRocas; i++)
            {
                AgregarRoca();
            }
        }

        private void AgregarRoca()
        {
            rocas.Add(new Vector2(_random.Next(0, _graphics.PreferredBackBufferWidth - bombaTexture.Width),
                                   _random.Next(-100, -50)));
        }

        protected override void Update(GameTime gameTime)
        {
            if (_juegoTerminado)
            {
                // Si el juego ha terminado, comprobar si se presiona la tecla "R" para reiniciar
                var teclado = Keyboard.GetState();
                if (teclado.IsKeyDown(Keys.R))
                {
                    ReiniciarJuego();
                }
                return; // Salir del método Update si el juego ha terminado
            }

            // Actualizar el tiempo desde el último disparo
            tiempoDesdeUltimoDisparo += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Control de la nave
            var tecladoEstado = Keyboard.GetState();
            if (tecladoEstado.IsKeyDown(Keys.Left))
                navePosition.X -= 5;
            if (tecladoEstado.IsKeyDown(Keys.Right))
                navePosition.X += 5;

            // Asegúrate de que la nave no salga de la pantalla
            navePosition.X = MathHelper.Clamp(navePosition.X, 0, _graphics.PreferredBackBufferWidth - naveTexture.Width);

            if (tecladoEstado.IsKeyDown(Keys.Space) && tiempoDesdeUltimoDisparo >= tiempoRecarga)
            {
                // Agregar un nuevo disparo
                disparos.Add(new Vector2(navePosition.X + naveTexture.Width / 2 - disparoTexture.Width / 2, navePosition.Y - 10));
                tiempoDesdeUltimoDisparo = 0f; // Reiniciar el temporizador
            }

            // Actualizar disparos
            for (int i = disparos.Count - 1; i >= 0; i--)
            {
                disparos[i] = new Vector2(disparos[i].X, disparos[i].Y - disparoSpeed);
                // Eliminar disparos que salen de la pantalla
                if (disparos[i].Y < 0)
                {
                    disparos.RemoveAt(i);
                }
            }

            // Actualizar rocas
            for (int i = rocas.Count - 1; i >= 0; i--)
            {
                rocas[i] = new Vector2(rocas[i].X, rocas[i].Y + bombaSpeed);

                // Verificar colisión con la nave
                if (rocas[i].X < navePosition.X + naveTexture.Width &&
                    rocas[i].X + bombaTexture.Width > navePosition.X &&
                    rocas[i].Y < navePosition.Y + naveTexture.Height &&
                    rocas[i].Y + bombaTexture.Height > navePosition.Y)
                {
                    _juegoTerminado = true; // Terminar el juego si hay colisión
                }

                // Reiniciar roca si sale de la pantalla
                if (rocas[i].Y > _graphics.PreferredBackBufferHeight)
                {
                    rocas.RemoveAt(i);
                    AgregarRoca(); // Agregar una nueva roca cuando una roca sale de la pantalla
                }
            }

            // Verificar colisiones entre disparos y rocas
            for (int i = disparos.Count - 1; i >= 0; i--)
            {
                for (int j = rocas.Count - 1; j >= 0; j--)
                {
                    // Comprobar colisión
                    if (disparos[i].X < rocas[j].X + bombaTexture.Width &&
                        disparos[i].X + disparoTexture.Width > rocas[j].X &&
                        disparos[i].Y < rocas[j].Y + bombaTexture.Height &&
                        disparos[i].Y + disparoTexture.Height > rocas[j].Y)
                    {
                        // Colisión detectada, eliminar el disparo y la roca
                        disparos.RemoveAt(i);
                        rocas.RemoveAt(j);
                        _puntuacion += 10; // Aumentar puntuación
                        AgregarRoca(); // Agregar una nueva roca al eliminar una
                        break; // Salir del bucle de rocas
                    }
                }
            }

            base.Update(gameTime);
        }

        private void ReiniciarJuego()
        {
            // Reiniciar variables del juego
            _juegoTerminado = false;
            _puntuacion = 0;
            navePosition = new Vector2(400, 400);
            disparos.Clear();
            rocas.Clear();

            // Inicializar rocas
            for (int i = 0; i < cantidadRocas; i++)
            {
                AgregarRoca();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            // Dibujar nave
            _spriteBatch.Draw(naveTexture, navePosition, Color.White);

            // Dibujar disparos
            foreach (var disparo in disparos)
            {
                _spriteBatch.Draw(disparoTexture, disparo, Color.White);
            }

            // Dibujar rocas
            foreach (var roca in rocas)
            {
                _spriteBatch.Draw(bombaTexture, roca, Color.White);
            }

            // Mostrar menú de juego terminado
            if (_juegoTerminado)
            {
                string mensaje = "Puntuacion = " + _puntuacion+ "\nPresiona 'R' para reiniciar";
                Vector2 tamañoTexto = fuente.MeasureString(mensaje);
                _spriteBatch.DrawString(fuente, mensaje, new Vector2((_graphics.PreferredBackBufferWidth - tamañoTexto.X) / 2,
                                                                       (_graphics.PreferredBackBufferHeight - tamañoTexto.Y) / 2), Color.Black);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

