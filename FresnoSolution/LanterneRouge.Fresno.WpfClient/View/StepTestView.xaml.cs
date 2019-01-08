﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.View
{
    /// <summary>
    /// Interaction logic for StepTestView.xaml
    /// </summary>
    public partial class StepTestView : UserControl
    {
        public StepTestView()
        {
            InitializeComponent();
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }

        private void textBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var tb = (sender as TextBox);
            if (tb != null)
            {
                if (!tb.IsKeyboardFocusWithin)
                {
                    e.Handled = true;
                    tb.Focus();
                }
            }
        }
    }
}
