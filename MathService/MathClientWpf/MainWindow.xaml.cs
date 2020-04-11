using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathClientWpf
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int iValue;
            Int32.TryParse(Valor.Text, out iValue);
            MathService.MathClient client = new MathService.MathClient();

            bool result = client.Prime(iValue);

            if (result == true)
            {
                Resultado.Content = "Es primo";
            }
            else
            {
                Resultado.Content = "No es primo";
            }
        }

        private void ButtonTupla_Click(object sender, RoutedEventArgs e)
        {
            string iName = TuplaNombre.Text;
            string iData = TuplaNumeros.Text;
            string[] words = iData.Split(' ');
            double[] data = new double[words.Length];
            for (int i=0; i<words.Length; i++)
            {
                data[i] = Double.Parse(words[i]);
            }
            
            MathService.MathClient client = new MathService.MathClient();
            MathService.Tuple add = new MathService.Tuple();
            add.Name = iName;
            add.Data = data;
            MathService.Tuple result = new MathService.Tuple();
            result = client.AddTuple(add);
            ResultadoTupla.Content = result.Name + ": " +Convert.ToString(result.Data[0]);
        }

        private void BotonEqLi_Click(object sender, RoutedEventArgs e)
        {
            double[][] A = new double[1][];
            A[0] = new double[1];
            A[0][0] = Double.Parse(A00.Text);
            A[0][1] = Double.Parse(A01.Text);
            A[1][0] = Double.Parse(A10.Text);
            A[1][1] = Double.Parse(A11.Text);
            double[] B = new double[] { Double.Parse(B0.Text), Double.Parse(B1.Text)};

            MathService.MathClient client = new MathService.MathClient();
            B = client.EqSys(A, 2, B);
        }
    }
}
