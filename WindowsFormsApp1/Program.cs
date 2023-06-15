using System;
using System.Windows.Forms;

class Program
{
    static int k; //(dias)
    static int n; //(pratos)
    static int m; //(orçamento)
    static int[,] pratos;
    static TextBox[] custoBox;
    static TextBox[] lucroBox;

    static void Main(string[] args) // o programa inicia aqui
    {
        Application.Run(new MainForm()); // ele chama esse método para iniciar a aplicação 
    }                                       //e exibe a janela do formulário principal

    static void CalculateMenu()
    {
        int[,] lucroMaxParc = new int[k + 1, m + 1]; // é criada essa matriz para armazenar os lucros parciais do calculo
        int[,] ultPrato = new int[k + 1, m + 1]; // é criada essa outra matriz para rastrear qual prato foi escolhido em cada dia para atingir
                                                 // o lucro máximo

        for (int i = 1; i <= n; i++) // aqui é feito um loop para preencher a matriz 'pratos' com os valores de custo e lucro inseridos pelo usuário
        {
            int custo = int.Parse(custoBox[i - 1].Text); // pega o valor do input custo e adiciona a variavel custo
            int lucro = int.Parse(lucroBox[i - 1].Text);// pega o valor do input lucro e adiciona a variavel lucro

            pratos[i, 0] = custo;   // adiciona o valor do custo na linha do prato n
            pratos[i, 1] = lucro;   // adiciona o valor de lucro na linha do prato n
        }

        for (int i = 1; i <= k; i++) // esse loop percorre cada dia do menu (de 1 a k)
        {
            for (int j = 0; j <= m; j++)            // itera sobre os valores de orçamento disponíveis para cada dia do menu
            {                                        // aqui estamos verificando todas as opções de orçamento possíveis para cada dia.

                lucroMaxParc[i, j] = lucroMaxParc[i - 1, j]; // atribui o valor do lucro do dia anterior para o dia atual.
                                                         // Isso ocorre porque estamos considerando a possibilidade de não escolher
                                                         // nenhum prato adicional para o dia atual e, portanto, o lucro será o mesmo do dia anterior.
                ultPrato[i, j] = -1;    //atribui o valor -1 à matriz ultPrato, indicando que nenhum prato foi
                                        //escolhido até o momento para o dia e orçamento atual.

                for (int prato = 1; prato <= n; prato++) // itera sobre os pratos disponiveis
                {
                    int custo = pratos[prato, 0]; // obtem o custo do prato atual do array pratos
                    int lucro = pratos[prato, 1]; // obtem o lucro do prato atual do array pratos

                    if (custo <= j && ultPrato[i - 1, j - custo] != prato) // verifica se é possível escolher o prato atual com o orçamento disponivel
                    {                                                       // e se ele não foi escolhido no dia anterior com o mesmo orçamento
                        lucroMaxParc[i, j] = Math.Max(lucroMaxParc[i, j], lucroMaxParc[i - 1, j - custo] + lucro);

                        if (lucroMaxParc[i, j] == lucroMaxParc[i - 1, j - custo] + lucro)
                        {
                            ultPrato[i, j] = prato;
                        }
                    }

                }
            }
        }

        double maxProfit = lucroMaxParc[k, m];
        string resultado = $"Lucro total R${maxProfit:F1}\n";

        int[] menu = new int[k];
        int orcamentoRestante = m;
        int diaAtual = k;

        
        while (diaAtual > 0 && orcamentoRestante > 0)
        {
            int dish = ultPrato[diaAtual, orcamentoRestante];
            menu[diaAtual - 1] = dish;
            orcamentoRestante -= pratos[dish, 0];
            diaAtual--;
        }

        for (int i = 0; i < k; i++)
        {
            resultado += $"Dia {i + 1}: Prato {menu[i]}\n";
        }


        MessageBox.Show(resultado, "Resultado");
    }

    public class MainForm : Form
    {
        private Label diasLabel;
        private TextBox diasTextBox;
        private Label pratosLabel;
        private TextBox pratosTextBox;
        private Label orcamentoLabel;
        private TextBox orcamentoTextBox;
        private Button submitButton;
        private Label[] custoLabels;
        private Label[] lucroLabels;
        private Button calculateButton;

        public MainForm() // esse componente chama outra função
        {
            InitializeComponents();
        }

        private void InitializeComponents() // aqui é feito a construção da interface 
        {
            diasLabel = new Label
            {
                Text = "Número de dias:",
                Location = new System.Drawing.Point(10, 10),
                AutoSize = true
            };

            diasTextBox = new TextBox
            {
                Location = new System.Drawing.Point(150, 10)
            };

            pratosLabel = new Label
            {
                Text = "Número de pratos:",
                Location = new System.Drawing.Point(10, 40),
                AutoSize = true
            };

            pratosTextBox = new TextBox
            {
                Location = new System.Drawing.Point(150, 40)
            };

            orcamentoLabel = new Label
            {
                Text = "Orçamento:",
                Location = new System.Drawing.Point(10, 70),
                AutoSize = true
            };

            orcamentoTextBox = new TextBox
            {
                Location = new System.Drawing.Point(150, 70)
            };

            submitButton = new Button
            {
                Text = "Enviar",
                Location = new System.Drawing.Point(200, 100),
                DialogResult = DialogResult.OK
            };

            submitButton.Click += SubmitButton_Click;  // É acionado o método SubmitButton_Click

            Controls.Add(diasLabel);
            Controls.Add(diasTextBox);
            Controls.Add(pratosLabel);
            Controls.Add(pratosTextBox);
            Controls.Add(orcamentoLabel);
            Controls.Add(orcamentoTextBox);
            Controls.Add(submitButton);

            //Essas linhas de código (Controls.Add) estão configurando a interface de entrada de dados
            //para o usuário, permitindo que ele insira o número de dias, o número de pratos e
            //o orçamento. O botão de envio será usado para continuar com o
            //cálculo com base nessas entradas.
        }

        private void SubmitButton_Click(object sender, EventArgs e) //esses parametros são necessarios para lidar com eventos de clique no botão
        {
            // abaixo os valores inseridos nos campos de texto são obtidos e armazenados nas
            // variáveis k(dias), n(pratos) e m(orçamento)

            k = int.Parse(diasTextBox.Text);
            n = int.Parse(pratosTextBox.Text);
            m = int.Parse(orcamentoTextBox.Text);
                
            
            Controls.Clear(); //os controles da interface são removidas para ser 
                                // adicionado os próximos necessários

            // são criado quatro arrays para para armazenar as labels e
            // os inputs dos custos e lucros de cada prato

            custoLabels = new Label[n];
            lucroLabels = new Label[n];
            custoBox = new TextBox[n];
            lucroBox = new TextBox[n];

            for (int i = 0; i < n; i++) // renderiza de acordo com a quantidade de pratos que o usuário informou
            {                   
                custoLabels[i] = new Label
                {
                    Text = $"Custo do prato {i + 1}:",
                    Location = new System.Drawing.Point(10, 10 + 60 * i),
                    AutoSize = true
                };

                lucroLabels[i] = new Label
                {
                    Text = $"Lucro do prato {i + 1}:",
                    Location = new System.Drawing.Point(10, 40 + 60 * i),
                    AutoSize = true
                };

                custoBox[i] = new TextBox
                {
                    Location = new System.Drawing.Point(150, 10 + 60 * i)
                };

                lucroBox[i] = new TextBox
                {
                    Location = new System.Drawing.Point(150, 40 + 60 * i)
                };

                Controls.Add(custoLabels[i]);
                Controls.Add(lucroLabels[i]);
                Controls.Add(custoBox[i]);
                Controls.Add(lucroBox[i]);

                // aqui segue a mesma lógica dos anteriores, é necessário para permitir que o
                // usuário interaja e insira os valores desejados
            }

            calculateButton = new Button
            {
                Text = "Calcular",
                Location = new System.Drawing.Point(200, 10 + 60 * n),
                DialogResult = DialogResult.OK
            };

            calculateButton.Click += CalculateButton_Click; // o método CalculateButton_Click é chamado quando houver o
                                                            // clique do usuário no botão

            Controls.Add(calculateButton);
        }

        private void CalculateButton_Click(object sender, EventArgs e) //esses parametros são necessarios para lidar com eventos de clique no botão
        {
            pratos = new int[n + 1, 2]; // aqui é criada uma matriz bidimensional com n + 1 linhas e duas colunas,
                                        // para armazenar os custos e lucros dos pratos

            CalculateMenu();    // chama o método CalculateMenu
        }
    }
}
