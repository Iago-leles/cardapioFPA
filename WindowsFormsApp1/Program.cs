using System;
using System.Windows.Forms;

class Program
{
    static int k;
    static int n;
    static int m;
    static int[,] dishes;
    static TextBox[] costBoxes;
    static TextBox[] profitBoxes;

    static void Main(string[] args)
    {
        Application.Run(new MainForm());
    }

    static void CalculateMenu()
    {
        int[,] dp = new int[k + 1, m + 1];
        int[,] lastDish = new int[k + 1, m + 1];

        for (int i = 1; i <= n; i++)
        {
            int cost = int.Parse(costBoxes[i - 1].Text);
            int profit = int.Parse(profitBoxes[i - 1].Text);

            dishes[i, 0] = cost;
            dishes[i, 1] = profit;
        }

        for (int i = 1; i <= k; i++)
        {
            for (int j = 0; j <= m; j++)
            {
                dp[i, j] = dp[i - 1, j];
                lastDish[i, j] = -1;

                for (int dish = 1; dish <= n; dish++)
                {
                    int cost = dishes[dish, 0];
                    int profit = dishes[dish, 1];

                    if (cost <= j && lastDish[i - 1, j - cost] != dish)
                    {
                        dp[i, j] = Math.Max(dp[i, j], dp[i - 1, j - cost] + profit);

                        if (dp[i, j] == dp[i - 1, j - cost] + profit)
                        {
                            lastDish[i, j] = dish;
                        }
                    }
                }
            }
        }

        double maxProfit = dp[k, m];
        string result = $"Lucro total R${maxProfit:F1}\n";

        int[] menu = new int[k];
        int remainingBudget = m;
        int currentDay = k;

        
        while (currentDay > 0 && remainingBudget > 0)
        {
            int dish = lastDish[currentDay, remainingBudget];
            menu[currentDay - 1] = dish;
            remainingBudget -= dishes[dish, 0];
            currentDay--;
        }

        for (int i = 0; i < k; i++)
        {
            result += $"Dia {i + 1}: Prato {menu[i]}\n";
        }


        MessageBox.Show(result, "Resultado");
    }

    public class MainForm : Form
    {
        private Label daysLabel;
        private TextBox daysTextBox;
        private Label dishesLabel;
        private TextBox dishesTextBox;
        private Label budgetLabel;
        private TextBox budgetTextBox;
        private Button submitButton;
        private Label[] costLabels;
        private Label[] profitLabels;
        private Button calculateButton;

        public MainForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            daysLabel = new Label
            {
                Text = "Número de dias:",
                Location = new System.Drawing.Point(10, 10),
                AutoSize = true
            };

            daysTextBox = new TextBox
            {
                Location = new System.Drawing.Point(150, 10)
            };

            dishesLabel = new Label
            {
                Text = "Número de pratos:",
                Location = new System.Drawing.Point(10, 40),
                AutoSize = true
            };

            dishesTextBox = new TextBox
            {
                Location = new System.Drawing.Point(150, 40)
            };

            budgetLabel = new Label
            {
                Text = "Orçamento:",
                Location = new System.Drawing.Point(10, 70),
                AutoSize = true
            };

            budgetTextBox = new TextBox
            {
                Location = new System.Drawing.Point(150, 70)
            };

            submitButton = new Button
            {
                Text = "Enviar",
                Location = new System.Drawing.Point(200, 100),
                DialogResult = DialogResult.OK
            };

            submitButton.Click += SubmitButton_Click;

            Controls.Add(daysLabel);
            Controls.Add(daysTextBox);
            Controls.Add(dishesLabel);
            Controls.Add(dishesTextBox);
            Controls.Add(budgetLabel);
            Controls.Add(budgetTextBox);
            Controls.Add(submitButton);
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            k = int.Parse(daysTextBox.Text);
            n = int.Parse(dishesTextBox.Text);
            m = int.Parse(budgetTextBox.Text);

            Controls.Clear();

            costLabels = new Label[n];
            profitLabels = new Label[n];
            costBoxes = new TextBox[n];
            profitBoxes = new TextBox[n];

            for (int i = 0; i < n; i++)
            {
                costLabels[i] = new Label
                {
                    Text = $"Custo do prato {i + 1}:",
                    Location = new System.Drawing.Point(10, 10 + 60 * i),
                    AutoSize = true
                };

                profitLabels[i] = new Label
                {
                    Text = $"Lucro do prato {i + 1}:",
                    Location = new System.Drawing.Point(10, 40 + 60 * i),
                    AutoSize = true
                };

                costBoxes[i] = new TextBox
                {
                    Location = new System.Drawing.Point(150, 10 + 60 * i)
                };

                profitBoxes[i] = new TextBox
                {
                    Location = new System.Drawing.Point(150, 40 + 60 * i)
                };

                Controls.Add(costLabels[i]);
                Controls.Add(profitLabels[i]);
                Controls.Add(costBoxes[i]);
                Controls.Add(profitBoxes[i]);
            }

            calculateButton = new Button
            {
                Text = "Calcular",
                Location = new System.Drawing.Point(200, 10 + 60 * n),
                DialogResult = DialogResult.OK
            };

            calculateButton.Click += CalculateButton_Click;

            Controls.Add(calculateButton);
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            dishes = new int[n + 1, 2];

            CalculateMenu();
        }
    }
}
