using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace t_rex_runnner_game
{
    public partial class Form1 : Form
    {
        //variaveis globais
        bool jumping = false; //se esta pulando ou n
        int jumpSpeed = 12; 
        int force = 12;
        int score = 0;
        int obstacleSpeed = 10;
        Random rand = new Random();
        int position; //posicao do obstaculo
        bool isGameOver = false;
        int bestScore = 0;

        public Form1()
        {
            InitializeComponent();
            gameReset(); //inicia o jogo
        }

        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            trex.Top += jumpSpeed; //trex vai pra cima

            txtScore.Text = "Pontos: " + score;

            //configuracoes do pulo
            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                jumpSpeed = 12;
            }

            //retorna com as configurações padrões
            if (trex.Top > 206 && jumping == false)
            {
                force = 12;
                trex.Top = 207;
                jumpSpeed = 0;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstaculo")
                {
                    x.Left -= obstacleSpeed; //movimento do obstaculo

                    if (x.Left < -100) //quando os cactos estiverem <<<<
                    {
                        //gera um cacto novo e adiciona o score
                        x.Left = this.ClientSize.Width + rand.Next(200, 500) + (x.Width * 15);
                        score++;
                    }

                    if (trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        trex.Image = Properties.Resources.dead;
                        SoundPlayer hit = new SoundPlayer(@"C:\Users\isaso\Documents\Ingrid\FATEC SP\4 sem\ILP580\t rex\imagens\Resources_hit.wav");
                        hit.Play();
                        txtScore.Text += " Pressione R para reiniciar o jogo!";
                        isGameOver = true;
                        if(bestScore < score)
                        {
                            bestScore = score;
                            txtBestScore.Text = "Melhor pontuação: " + bestScore;
                        }

                    }
                }
            }

            //niveis de dificuldade
            if (score > 5 && score<10) //+- facil
            {
                obstacleSpeed = 13;
            }
            if (score > 10 && score<20) //medio
            {
                obstacleSpeed = 15;
            }
            if (score > 20) //dificil
            {
                obstacleSpeed = 18;
            }


        }

        private void keyIsDown(object sender, KeyEventArgs e) //tecla pressionada
        {
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
                SoundPlayer jumpS = new SoundPlayer(@"C:\Users\isaso\Documents\Ingrid\FATEC SP\4 sem\ILP580\t rex\imagens\Resources_button-press.wav");
                jumpS.Play();
            }
        }

        private void keyIsUp(object sender, KeyEventArgs e) //tecla nao pressionada
        {
            if (jumping == true)
            {
                jumping = false;
            }

            if (e.KeyCode == Keys.R && isGameOver == true) //caso perca a partida e queira reiniciar
            {
                gameReset();
            }
        }

        private void gameReset()
        {
            jumping = false;
            force = 12;
            jumpSpeed = 0;
            score = 0;
            obstacleSpeed = 10;
            txtScore.Text = "Pontos: " + score;
            txtBestScore.Text = "Melhor pontuação: " + bestScore;
            trex.Image = Properties.Resources.running; //gif do t rex andando
            trex.Top = 207;//y location
            isGameOver = false;
            

            foreach (Control x in this.Controls)
            {
                //posicao aleatoria dos obstaculos
                if (x is PictureBox && (string)x.Tag == "obstaculo") 
                {
                    position = this.ClientSize.Width + rand.Next(500, 800) + (x.Width * 10);

                    x.Left = position;
                }
            }

            gameTimer.Start();
        }
    }
}
